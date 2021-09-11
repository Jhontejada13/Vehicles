using System.ComponentModel.DataAnnotations;

namespace Vehicles.API.Data.Entities
{
    public class DocumentType
    {
        public int Id { get; set; }

        [Display(Name = "Procedimiento")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        public string Description { get; set; }
    }
}
