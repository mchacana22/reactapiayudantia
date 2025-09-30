using NaveMenorAPI.Models;
using NaveMenorAPI.Repositories;

namespace NaveMenorAPI.Services
{
    /// <summary>
    /// Implementación del servicio de negocio para NaveMenor
    /// Contiene la lógica de validación y reglas de negocio
    /// </summary>
    public class NaveMenorService : INaveMenorService
    {
        private readonly INaveMenorRepository _repository;
        private readonly ILogger<NaveMenorService> _logger;

        /// <summary>
        /// Constructor que recibe el repositorio y logger mediante inyección de dependencias
        /// </summary>
        /// <param name="repository">Repositorio de NaveMenor</param>
        /// <param name="logger">Logger para registrar eventos y errores</param>
        public NaveMenorService(
            INaveMenorRepository repository,
            ILogger<NaveMenorService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene una nave menor por su ID
        /// </summary>
        public async Task<NaveMenor?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo nave menor con ID: {Id}", id);
            
            try
            {
                var nave = await _repository.GetByIdAsync(id);
                
                if (nave == null)
                {
                    _logger.LogWarning("No se encontró nave menor con ID: {Id}", id);
                }
                
                return nave;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener nave menor con ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas las naves menores activas
        /// </summary>
        public async Task<IEnumerable<NaveMenor>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todas las naves menores activas");
            
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las naves menores");
                throw;
            }
        }

        /// <summary>
        /// Crea una nueva nave menor con validaciones de negocio
        /// </summary>
        public async Task<NaveMenor> CreateAsync(NaveMenor naveMenor)
        {
            _logger.LogInformation("Creando nueva nave menor con matrícula: {Matricula}", naveMenor.Matricula);
            
            try
            {
                // Validación de negocio: verificar que la matrícula no exista
                var existeMatricula = await _repository.ExisteMatriculaAsync(naveMenor.Matricula);
                
                if (existeMatricula)
                {
                    _logger.LogWarning("Intento de crear nave con matrícula duplicada: {Matricula}", naveMenor.Matricula);
                    throw new InvalidOperationException($"Ya existe una nave con la matrícula {naveMenor.Matricula}");
                }

                // Validaciones adicionales de negocio
                ValidarDimensionesNave(naveMenor);

                var naveCreada = await _repository.CreateAsync(naveMenor);
                
                _logger.LogInformation("Nave menor creada exitosamente con ID: {Id}", naveCreada.Id);
                
                return naveCreada;
            }
            catch (InvalidOperationException)
            {
                throw; // Re-lanzar excepciones de validación de negocio
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nave menor");
                throw;
            }
        }

        /// <summary>
        /// Actualiza una nave menor existente con validaciones de negocio
        /// </summary>
        public async Task<NaveMenor> UpdateAsync(int id, NaveMenor naveMenor)
        {
            _logger.LogInformation("Actualizando nave menor con ID: {Id}", id);
            
            try
            {
                // Verificar que la nave existe
                var naveExistente = await _repository.GetByIdAsync(id);
                
                if (naveExistente == null)
                {
                    _logger.LogWarning("Intento de actualizar nave inexistente con ID: {Id}", id);
                    throw new InvalidOperationException($"No existe una nave con el ID {id}");
                }

                // Si se cambió la matrícula, verificar que no esté duplicada
                if (naveExistente.Matricula != naveMenor.Matricula)
                {
                    var existeMatricula = await _repository.ExisteMatriculaAsync(naveMenor.Matricula);
                    
                    if (existeMatricula)
                    {
                        _logger.LogWarning("Intento de actualizar con matrícula duplicada: {Matricula}", naveMenor.Matricula);
                        throw new InvalidOperationException($"Ya existe otra nave con la matrícula {naveMenor.Matricula}");
                    }
                }

                // Validaciones adicionales de negocio
                ValidarDimensionesNave(naveMenor);

                // Asegurar que el ID sea el correcto
                naveMenor.Id = id;

                var naveActualizada = await _repository.UpdateAsync(naveMenor);
                
                _logger.LogInformation("Nave menor actualizada exitosamente con ID: {Id}", id);
                
                return naveActualizada;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar nave menor con ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Elimina una nave menor (eliminación lógica)
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando nave menor con ID: {Id}", id);
            
            try
            {
                var resultado = await _repository.DeleteAsync(id);
                
                if (resultado)
                {
                    _logger.LogInformation("Nave menor eliminada exitosamente con ID: {Id}", id);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar nave menor con ID: {Id}", id);
                }
                
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar nave menor con ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Valida que las dimensiones de la nave sean coherentes
        /// Regla de negocio: la manga (ancho) no puede ser mayor que la eslora (largo)
        /// </summary>
        private void ValidarDimensionesNave(NaveMenor naveMenor)
        {
            if (naveMenor.Manga > naveMenor.Eslora)
            {
                throw new InvalidOperationException("La manga (ancho) no puede ser mayor que la eslora (largo) de la nave");
            }

            if (naveMenor.Puntal > naveMenor.Eslora)
            {
                throw new InvalidOperationException("El puntal (altura) no puede ser mayor que la eslora (largo) de la nave");
            }
        }
    }
}
