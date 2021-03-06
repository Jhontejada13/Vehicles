using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vehicles.Common.Enums;

namespace Vehicles.API.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        public string LastName { get; set; }

        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        public DocumentType DocumentType { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        public string Document { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://vehiclesapijhontd.azurewebsites.net/images/no-image.png"
            : $"https://vehiclesjhon.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName => $"{FirstName} {LastName}";

        //public ICollection<User> Users { get; set; }

        public ICollection<History> Histories { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

        [Display(Name = "# Vehículos")]
        public int VehiclesCount => Vehicles == null ? 0 : Vehicles.Count;

    }
}
