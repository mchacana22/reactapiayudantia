using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaveMenorAPI.Models
{
    /// <summary>
    /// Representa un usuario del sistema con credenciales de acceso
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de usuario para login (único)
        /// </summary>
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña hasheada del usuario (no almacenar en texto plano)
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(200)]
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del usuario
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Rol del usuario (Admin, Inspector, Operador, etc.)
        /// </summary>
        [Required(ErrorMessage = "El rol es obligatorio")]
        [StringLength(50)]
        public string Rol { get; set; } = "Operador";

        /// <summary>
        /// Indica si el usuario está activo en el sistema
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Fecha de creación del usuario
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha del último acceso del usuario
        /// </summary>
        public DateTime? UltimoAcceso { get; set; }
    }
}
