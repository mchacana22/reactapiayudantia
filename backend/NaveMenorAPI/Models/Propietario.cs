using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaveMenorAPI.Models
{
    /// <summary>
    /// Representa al propietario de una o varias naves menores
    /// </summary>
    public class Propietario
    {
        /// <summary>
        /// Identificador único del propietario
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// RUT del propietario (Rol Único Tributario - identificación chilena)
        /// </summary>
        [Required(ErrorMessage = "El RUT es obligatorio")]
        [StringLength(12, ErrorMessage = "El RUT no puede exceder 12 caracteres")]
        public string Rut { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del propietario
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 200 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Dirección del propietario
        /// </summary>
        [StringLength(300)]
        public string? Direccion { get; set; }

        /// <summary>
        /// Teléfono de contacto del propietario
        /// </summary>
        [StringLength(20)]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string? Telefono { get; set; }

        /// <summary>
        /// Correo electrónico del propietario
        /// </summary>
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public string? Email { get; set; }

        /// <summary>
        /// Fecha de registro del propietario en el sistema
        /// </summary>
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indica si el propietario está activo en el sistema
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Navegación a las naves propiedad de este propietario
        /// </summary>
        public ICollection<NaveMenor>? Naves { get; set; }
    }
}
