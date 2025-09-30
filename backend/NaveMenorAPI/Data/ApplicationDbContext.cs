using Microsoft.EntityFrameworkCore;
using NaveMenorAPI.Models;

namespace NaveMenorAPI.Data
{
    /// <summary>
    /// Contexto de base de datos para Entity Framework Core
    /// Gestiona todas las entidades y sus relaciones con SQL Server
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor que recibe las opciones de configuración del contexto
        /// </summary>
        /// <param name="options">Opciones de configuración del DbContext</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets representan las tablas en la base de datos

        /// <summary>
        /// Tabla de Naves Menores
        /// </summary>
        public DbSet<NaveMenor> NavesMenores { get; set; }

        /// <summary>
        /// Tabla de Propietarios
        /// </summary>
        public DbSet<Propietario> Propietarios { get; set; }

        /// <summary>
        /// Tabla de Zarpes (salidas de puerto)
        /// </summary>
        public DbSet<Zarpe> Zarpes { get; set; }

        /// <summary>
        /// Tabla de Recaladas (llegadas a puerto)
        /// </summary>
        public DbSet<Recalada> Recaladas { get; set; }

        /// <summary>
        /// Tabla de Inspecciones
        /// </summary>
        public DbSet<Inspeccion> Inspecciones { get; set; }

        /// <summary>
        /// Tabla de Usuarios del sistema
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// Tabla de Logs de Operación
        /// </summary>
        public DbSet<LogOperacion> LogsOperacion { get; set; }

        /// <summary>
        /// Configuración del modelo de base de datos
        /// Aquí se definen relaciones, índices, restricciones, etc.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de NaveMenor
            modelBuilder.Entity<NaveMenor>(entity =>
            {
                // Índice único en Matrícula (no puede haber dos naves con la misma matrícula)
                entity.HasIndex(e => e.Matricula).IsUnique();
                
                // Configuración de propiedades decimales con precisión específica
                entity.Property(e => e.Eslora).HasPrecision(10, 2);
                entity.Property(e => e.Manga).HasPrecision(10, 2);
                entity.Property(e => e.Puntal).HasPrecision(10, 2);

                // Relación con Propietario (una nave tiene un propietario, un propietario puede tener muchas naves)
                entity.HasOne(e => e.Propietario)
                    .WithMany(p => p.Naves)
                    .HasForeignKey(e => e.PropietarioId)
                    .OnDelete(DeleteBehavior.Restrict); // No permite eliminar un propietario si tiene naves
            });

            // Configuración de Propietario
            modelBuilder.Entity<Propietario>(entity =>
            {
                // Índice único en RUT (no puede haber dos propietarios con el mismo RUT)
                entity.HasIndex(e => e.Rut).IsUnique();
            });

            // Configuración de Zarpe
            modelBuilder.Entity<Zarpe>(entity =>
            {
                // Relación con NaveMenor
                entity.HasOne(e => e.NaveMenor)
                    .WithMany(n => n.Zarpes)
                    .HasForeignKey(e => e.NaveMenorId)
                    .OnDelete(DeleteBehavior.Restrict); // No permite eliminar una nave si tiene zarpes
            });

            // Configuración de Recalada
            modelBuilder.Entity<Recalada>(entity =>
            {
                // Relación con NaveMenor
                entity.HasOne(e => e.NaveMenor)
                    .WithMany(n => n.Recaladas)
                    .HasForeignKey(e => e.NaveMenorId)
                    .OnDelete(DeleteBehavior.Restrict); // No permite eliminar una nave si tiene recaladas
            });

            // Configuración de Inspeccion
            modelBuilder.Entity<Inspeccion>(entity =>
            {
                // Relación con NaveMenor
                entity.HasOne(e => e.NaveMenor)
                    .WithMany(n => n.Inspecciones)
                    .HasForeignKey(e => e.NaveMenorId)
                    .OnDelete(DeleteBehavior.Restrict); // No permite eliminar una nave si tiene inspecciones
            });

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Índice único en NombreUsuario (no puede haber dos usuarios con el mismo nombre)
                entity.HasIndex(e => e.NombreUsuario).IsUnique();
                
                // Índice único en Email
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de LogOperacion
            modelBuilder.Entity<LogOperacion>(entity =>
            {
                // Índice en FechaHora para mejorar búsquedas por fecha
                entity.HasIndex(e => e.FechaHora);
            });
        }
    }
}
