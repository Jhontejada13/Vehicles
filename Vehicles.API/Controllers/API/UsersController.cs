﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.API.Data;
using Vehicles.API.Data.Entities;
using Vehicles.API.Helpers;
using Vehicles.API.Models.Request;
using Vehicles.Common.Enums;
using Vehicles.Common.Models;

namespace Vehicles.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Se puede poner a nivel de método o a nivel de clase
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IBlobHelper _blobHelper;

        public UsersController(DataContext context, IUserHelper userHelper, IMailHelper mailHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(x => x.DocumentType)
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Where(x => x.UserType == UserType.User)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            User user = await _context.Users
                .Include(x => x.DocumentType)
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.VehicleType)
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.Brand)
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.VehiclePhotos)
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.Histories)
                .ThenInclude(x => x.Details)
                .ThenInclude(x => x.Procedure)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost()]
        public async Task<ActionResult<User>> PostUser(UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DocumentType documentType = await _context.DocumentTypes.FindAsync(request.DocumentTypeId);

            if (documentType == null)
            {
                return BadRequest("El tipo de documento no existe");
            }

            User user = await _userHelper.GetUserAsync(request.Email);

            if (user != null)
            {
                return BadRequest("Ya existe un usuario registrado con esa dirección de email");
            }

            Guid imageId = Guid.Empty;
            if (request.Image != null && request.Image.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.Image, "users");
            }

            user = new User
            {
                Address = request.Address,
                Document = request.Document,
                DocumentType = documentType,
                Email = request.Email,
                FirstName = request.FirstName,
                ImageId = imageId,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                UserType = UserType.User
            };

            await _userHelper.AddUserAsync(user, "123456");
            await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendEmail(request.Email, "Vehicles - Confirmación de cuenta", $"<h1>Vehicles - Confirmación de cuenta</h1>" +
                $"Para habilitar el usuario, " +
                $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DocumentType documentType = await _context.DocumentTypes.FindAsync(request.DocumentTypeId);
            if (documentType == null)
            {
                return BadRequest("El tipo de documento no existe");
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return BadRequest("No existe el usuario");
            }

            Guid imageId = user.ImageId;
            if (request.Image != null && request.Image.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.Image, "users");
            }

            user.Address = request.Address;
            user.Document = request.Document;
            user.DocumentType = documentType;
            user.FirstName = request.FirstName;
            user.ImageId = imageId;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            await _userHelper.UpdateUserAsync(user);
            return NoContent();
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            User user = await _context.Users
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.VehiclePhotos)
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.Histories)
                .ThenInclude(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user.ImageId != Guid.Empty)
            {
                await _blobHelper.DeleteBlobAsync(user.ImageId, "users");
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleType(int id, VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicleType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe este tipo de vehículo");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}