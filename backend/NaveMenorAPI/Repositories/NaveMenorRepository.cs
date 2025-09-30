using Microsoft.EntityFrameworkCore;
using NaveMenorAPI.Data;
using NaveMenorAPI.Models;

namespace NaveMenorAPI.Repositories
{
    /// <summary>
    /// Implementación del repositorio de NaveMenor usando Entity Framework Core
    /// Encapsula todas las operaciones de acceso a datos para la entidad NaveMenor
    /// </summary>
    public class NaveMenorRepository : INaveMenorRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos mediante inyección de dependencias
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        public NaveMenorRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene una nave menor por su ID, incluyendo datos del propietario
        /// </summary>
        public async Task<NaveMenor?> GetByIdAsync(int id)
        {
            // Include carga de forma "eager" (anticipada) los datos relacionados del propietario
            // Esto evita el problema N+1 de consultas múltiples
            return await _context.NavesMenores
                .Include(n => n.Propietario)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        /// <summary>
        /// Obtiene todas las naves menores activas con sus propietarios
        /// </summary>
        public async Task<IEnumerable<NaveMenor>> GetAllAsync()
        {
            // Filtra solo las naves activas y carga los datos del propietario
            return await _context.NavesMenores
                .Include(n => n.Propietario)
                .Where(n => n.Activa)
                .OrderBy(n => n.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Crea una nueva nave menor en la base de datos
        /// </summary>
        public async Task<NaveMenor> CreateAsync(NaveMenor naveMenor)
        {
            // Establece la fecha de registro actual
            naveMenor.FechaRegistro = DateTime.UtcNow;
            naveMenor.Activa = true;

            // Agrega la entidad al contexto
            _context.NavesMenores.Add(naveMenor);
            
            // Guarda los cambios en la base de datos
            // SaveChangesAsync devuelve el número de registros afectados
            await _context.SaveChangesAsync();

            // Carga el propietario relacionado después de guardar
            await _context.Entry(naveMenor)
                .Reference(n => n.Propietario)
                .LoadAsync();

            return naveMenor;
        }

        /// <summary>
        /// Actualiza una nave menor existente
        /// </summary>
        public async Task<NaveMenor> UpdateAsync(NaveMenor naveMenor)
        {
            // Marca la entidad como modificada
            _context.Entry(naveMenor).State = EntityState.Modified;
            
            // Guarda los cambios
            await _context.SaveChangesAsync();

            // Recarga el propietario
            await _context.Entry(naveMenor)
                .Reference(n => n.Propietario)
                .LoadAsync();

            return naveMenor;
        }

        /// <summary>
        /// Realiza una eliminación lógica de la nave (marca como inactiva)
        /// No elimina físicamente el registro para mantener la integridad referencial
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var naveMenor = await _context.NavesMenores.FindAsync(id);
            
            if (naveMenor == null)
            {
                return false;
            }

            // Eliminación lógica: solo marca como inactiva
            naveMenor.Activa = false;
            
            await _context.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// Verifica si ya existe una nave con la matrícula especificada
        /// Útil para validar duplicados antes de crear o actualizar
        /// </summary>
        public async Task<bool> ExisteMatriculaAsync(string matricula)
        {
            return await _context.NavesMenores
                .AnyAsync(n => n.Matricula == matricula);
        }

        /// <summary>
        /// Busca una nave menor por su matrícula
        /// </summary>
        public async Task<NaveMenor?> GetByMatriculaAsync(string matricula)
        {
            return await _context.NavesMenores
                .Include(n => n.Propietario)
                .FirstOrDefaultAsync(n => n.Matricula == matricula);
        }
    }
}
