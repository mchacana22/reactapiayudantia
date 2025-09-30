using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaveMenorAPI.Models
{
    /// <summary>
    /// Representa el zarpe (salida del puerto) de una nave menor
    /// El zarpe es el permiso oficial para que una embarcación abandone el puerto
    /// </summary>
    public class Zarpe
    {
        /// <summary>
        /// Identificador único del zarpe
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ID de la nave que realiza el zarpe
        /// </summary>
        [Required(ErrorMessage = "La nave es obligatoria")]
        public int NaveMenorId { get; set; }

        /// <summary>
        /// Navegación a la nave que realiza el zarpe
        /// </summary>
        [ForeignKey("NaveMenorId")]
        public NaveMenor? NaveMenor { get; set; }

        /// <summary>
        /// Fecha y hora del zarpe
        /// </summary>
        [Required(ErrorMessage = "La fecha de zarpe es obligatoria")]
        public DateTime FechaZarpe { get; set; }

        /// <summary>
        /// Puerto de salida
        /// </summary>
        [Required(ErrorMessage = "El puerto de salida es obligatorio")]
        [StringLength(100)]
        public string PuertoSalida { get; set; } = string.Empty;

        /// <summary>
        /// Puerto de destino
        /// </summary>
        [Required(ErrorMessage = "El puerto de destino es obligatorio")]
        [StringLength(100)]
        public string PuertoDestino { get; set; } = string.Empty;

        /// <summary>
        /// Propósito del viaje (pesca, transporte, recreación, etc.)
        /// </summary>
        [StringLength(200)]
        public string? Proposito { get; set; }

        /// <summary>
        /// Nombre del capitán o patrón de la embarcación
        /// </summary>
        [StringLength(200)]
        public string? Capitan { get; set; }

        /// <summary>
        /// Número de tripulantes a bordo
        /// </summary>
        [Range(0, 1000, ErrorMessage = "El número de tripulantes debe estar entre 0 y 1000")]
        public int? NumeroTripulantes { get; set; }

        /// <summary>
        /// Autorizado por (nombre del oficial que autoriza el zarpe)
        /// </summary>
        [StringLength(200)]
        public string? AutorizadoPor { get; set; }

        /// <summary>
        /// Observaciones del zarpe
        /// </summary>
        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}
