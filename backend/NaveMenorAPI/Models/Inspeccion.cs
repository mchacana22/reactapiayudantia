using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaveMenorAPI.Models
{
    /// <summary>
    /// Representa una inspección realizada a una nave menor
    /// Las inspecciones son revisiones periódicas de seguridad y operatividad
    /// </summary>
    public class Inspeccion
    {
        /// <summary>
        /// Identificador único de la inspección
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ID de la nave inspeccionada
        /// </summary>
        [Required(ErrorMessage = "La nave es obligatoria")]
        public int NaveMenorId { get; set; }

        /// <summary>
        /// Navegación a la nave inspeccionada
        /// </summary>
        [ForeignKey("NaveMenorId")]
        public NaveMenor? NaveMenor { get; set; }

        /// <summary>
        /// Fecha de la inspección
        /// </summary>
        [Required(ErrorMessage = "La fecha de inspección es obligatoria")]
        public DateTime FechaInspeccion { get; set; }

        /// <summary>
        /// Tipo de inspección (Anual, Extraordinaria, Seguridad, etc.)
        /// </summary>
        [Required(ErrorMessage = "El tipo de inspección es obligatorio")]
        [StringLength(100)]
        public string TipoInspeccion { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del inspector que realizó la inspección
        /// </summary>
        [Required(ErrorMessage = "El inspector es obligatorio")]
        [StringLength(200)]
        public string Inspector { get; set; } = string.Empty;

        /// <summary>
        /// Resultado de la inspección (Aprobada, Rechazada, Aprobada con observaciones, etc.)
        /// </summary>
        [Required(ErrorMessage = "El resultado es obligatorio")]
        [StringLength(50)]
        public string Resultado { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de vencimiento de la inspección (próxima inspección requerida)
        /// </summary>
        public DateTime? FechaVencimiento { get; set; }

        /// <summary>
        /// Observaciones detalladas de la inspección
        /// </summary>
        [StringLength(1000)]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Deficiencias encontradas durante la inspección
        /// </summary>
        [StringLength(1000)]
        public string? Deficiencias { get; set; }
    }
}
