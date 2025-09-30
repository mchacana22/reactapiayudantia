using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaveMenorAPI.Models
{
    /// <summary>
    /// Representa una nave menor conforme a la legislación chilena.
    /// Una nave menor es aquella embarcación de pequeño tamaño utilizada para actividades marítimas.
    /// </summary>
    public class NaveMenor
    {
        /// <summary>
        /// Identificador único de la nave menor
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la nave menor (obligatorio)
        /// </summary>
        [Required(ErrorMessage = "El nombre de la nave es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Matrícula o número de registro único de la nave (obligatorio)
        /// Debe ser único en el sistema
        /// </summary>
        [Required(ErrorMessage = "La matrícula es obligatoria")]
        [StringLength(50, ErrorMessage = "La matrícula no puede exceder 50 caracteres")]
        public string Matricula { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de nave (ejemplo: Lancha, Bote, Yate, Pesquero, etc.)
        /// </summary>
        [StringLength(50)]
        public string? Tipo { get; set; }

        /// <summary>
        /// Eslora (longitud) de la nave en metros
        /// </summary>
        [Range(0.1, 100.0, ErrorMessage = "La eslora debe estar entre 0.1 y 100 metros")]
        public decimal Eslora { get; set; }

        /// <summary>
        /// Manga (anchura) de la nave en metros
        /// </summary>
        [Range(0.1, 50.0, ErrorMessage = "La manga debe estar entre 0.1 y 50 metros")]
        public decimal Manga { get; set; }

        /// <summary>
        /// Puntal (altura) de la nave en metros
        /// </summary>
        [Range(0.1, 30.0, ErrorMessage = "El puntal debe estar entre 0.1 y 30 metros")]
        public decimal Puntal { get; set; }

        /// <summary>
        /// Material de construcción de la nave (Madera, Fibra de vidrio, Aluminio, etc.)
        /// </summary>
        [StringLength(50)]
        public string? Material { get; set; }

        /// <summary>
        /// Año de construcción de la nave
        /// </summary>
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? AnioConstruccion { get; set; }

        /// <summary>
        /// Puerto base donde está registrada la nave
        /// </summary>
        [StringLength(100)]
        public string? PuertoBase { get; set; }

        /// <summary>
        /// Capacidad de tripulación (número de personas)
        /// </summary>
        [Range(0, 1000, ErrorMessage = "La capacidad de tripulación debe estar entre 0 y 1000")]
        public int? CapacidadTripulacion { get; set; }

        /// <summary>
        /// ID del propietario de la nave (clave foránea)
        /// </summary>
        [Required(ErrorMessage = "El propietario es obligatorio")]
        public int PropietarioId { get; set; }

        /// <summary>
        /// Navegación al propietario de la nave
        /// </summary>
        [ForeignKey("PropietarioId")]
        public Propietario? Propietario { get; set; }

        /// <summary>
        /// Fecha de registro de la nave en el sistema
        /// </summary>
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indica si la nave está activa o dada de baja
        /// </summary>
        public bool Activa { get; set; } = true;

        /// <summary>
        /// Observaciones adicionales sobre la nave
        /// </summary>
        [StringLength(500)]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Navegación a las inspecciones realizadas a esta nave
        /// </summary>
        public ICollection<Inspeccion>? Inspecciones { get; set; }

        /// <summary>
        /// Navegación a los zarpes (salidas) de esta nave
        /// </summary>
        public ICollection<Zarpe>? Zarpes { get; set; }

        /// <summary>
        /// Navegación a las recaladas (llegadas) de esta nave
        /// </summary>
        public ICollection<Recalada>? Recaladas { get; set; }
    }
}
