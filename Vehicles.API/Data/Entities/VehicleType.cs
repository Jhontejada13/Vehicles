﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicles.API.Data.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }

        [Display(Name ="Tipo de vehículo")]
        [MaxLength(50, ErrorMessage ="El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage ="El Campo {0} es obligatorio")]
        public string Description { get; set; }

    }
}