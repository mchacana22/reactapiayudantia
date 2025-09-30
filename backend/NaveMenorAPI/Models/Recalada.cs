using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaveMenorAPI.Models
{
    /// <summary>
    /// Representa la recalada (llegada al puerto) de una nave menor
    /// La recalada es el registro oficial de la llegada de una embarcación al puerto
    /// </summary>
    public class Recalada
    {
        /// <summary>
        /// Identificador único de la recalada
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ID de la nave que realiza la recalada
        /// </summary>
        [Required(ErrorMessage = "La nave es obligatoria")]
        public int NaveMenorId { get; set; }

        /// <summary>
        /// Navegación a la nave que realiza la recalada
        /// </summary>
        [ForeignKey("NaveMenorId")]
        public NaveMenor? NaveMenor { get; set; }

        /// <summary>
        /// Fecha y hora de la recalada
        /// </summary>
        [Required(ErrorMessage = "La fecha de recalada es obligatoria")]
        public DateTime FechaRecalada { get; set; }

        /// <summary>
        /// Puerto de llegada
        /// </summary>
        [Required(ErrorMessage = "El puerto de llegada es obligatorio")]
        [StringLength(100)]
        public string PuertoLlegada { get; set; } = string.Empty;

        /// <summary>
        /// Puerto de origen (de donde viene)
        /// </summary>
        [Required(ErrorMessage = "El puerto de origen es obligatorio")]
        [StringLength(100)]
        public string PuertoOrigen { get; set; } = string.Empty;

        /// <summary>
        /// Resultado del viaje (éxito, incidente, etc.)
        /// </summary>
        [StringLength(200)]
        public string? ResultadoViaje { get; set; }

        /// <summary>
        /// Nombre del capitán o patrón de la embarcación
        /// </summary>
        [StringLength(200)]
        public string? Capitan { get; set; }

        /// <summary>
        /// Número de tripulantes que llegan
        /// </summary>
        [Range(0, 1000, ErrorMessage = "El número de tripulantes debe estar entre 0 y 1000")]
        public int? NumeroTripulantes { get; set; }

        /// <summary>
        /// Registrado por (nombre del oficial que registra la recalada)
        /// </summary>
        [StringLength(200)]
        public string? RegistradoPor { get; set; }

        /// <summary>
        /// Observaciones de la recalada
        /// </summary>
        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}
