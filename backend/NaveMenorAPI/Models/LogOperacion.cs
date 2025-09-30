using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaveMenorAPI.Models
{
    /// <summary>
    /// Representa un registro de log de las operaciones realizadas en el sistema
    /// Útil para auditoría y seguimiento de actividades
    /// </summary>
    public class LogOperacion
    {
        /// <summary>
        /// Identificador único del log
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Fecha y hora de la operación
        /// </summary>
        [Required]
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Usuario que realizó la operación
        /// </summary>
        [StringLength(50)]
        public string? Usuario { get; set; }

        /// <summary>
        /// Tipo de operación (Create, Read, Update, Delete)
        /// </summary>
        [Required(ErrorMessage = "El tipo de operación es obligatorio")]
        [StringLength(20)]
        public string TipoOperacion { get; set; } = string.Empty;

        /// <summary>
        /// Entidad afectada (NaveMenor, Propietario, etc.)
        /// </summary>
        [Required(ErrorMessage = "La entidad es obligatoria")]
        [StringLength(50)]
        public string Entidad { get; set; } = string.Empty;

        /// <summary>
        /// ID del registro afectado
        /// </summary>
        public int? RegistroId { get; set; }

        /// <summary>
        /// Descripción detallada de la operación
        /// </summary>
        [StringLength(500)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Dirección IP desde donde se realizó la operación
        /// </summary>
        [StringLength(45)] // IPv6 puede tener hasta 45 caracteres
        public string? DireccionIP { get; set; }

        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        public bool Exitosa { get; set; } = true;

        /// <summary>
        /// Mensaje de error en caso de que la operación haya fallado
        /// </summary>
        [StringLength(1000)]
        public string? MensajeError { get; set; }
    }
}
