using Microsoft.AspNetCore.Mvc;
using NaveMenorAPI.Models;
using NaveMenorAPI.Services;

namespace NaveMenorAPI.Controllers
{
    /// <summary>
    /// Controlador API REST para gestión de Naves Menores
    /// Expone endpoints HTTP para operaciones CRUD
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NavesController : ControllerBase
    {
        private readonly INaveMenorService _service;
        private readonly ILogger<NavesController> _logger;

        /// <summary>
        /// Constructor que recibe el servicio y logger mediante inyección de dependencias
        /// </summary>
        /// <param name="service">Servicio de NaveMenor</param>
        /// <param name="logger">Logger para registrar eventos</param>
        public NavesController(
            INaveMenorService service,
            ILogger<NavesController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todas las naves menores activas
        /// </summary>
        /// <returns>Lista de naves menores</returns>
        /// <response code="200">Retorna la lista de naves menores</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NaveMenor>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<NaveMenor>>> GetAll()
        {
            try
            {
                _logger.LogInformation("GET /api/naves - Obteniendo todas las naves");
                
                var naves = await _service.GetAllAsync();
                
                return Ok(naves);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las naves");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una nave menor específica por su ID
        /// </summary>
        /// <param name="id">ID de la nave menor</param>
        /// <returns>La nave menor solicitada</returns>
        /// <response code="200">Retorna la nave menor</response>
        /// <response code="404">Si la nave no existe</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NaveMenor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NaveMenor>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("GET /api/naves/{Id} - Obteniendo nave por ID", id);
                
                var nave = await _service.GetByIdAsync(id);
                
                if (nave == null)
                {
                    return NotFound(new { message = $"No se encontró la nave con ID {id}" });
                }
                
                return Ok(nave);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener nave con ID: {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva nave menor
        /// </summary>
        /// <param name="naveMenor">Datos de la nave menor a crear</param>
        /// <returns>La nave menor creada</returns>
        /// <response code="201">Nave creada exitosamente</response>
        /// <response code="400">Si los datos son inválidos o la matrícula ya existe</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(NaveMenor), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NaveMenor>> Create([FromBody] NaveMenor naveMenor)
        {
            try
            {
                _logger.LogInformation("POST /api/naves - Creando nueva nave con matrícula: {Matricula}", naveMenor.Matricula);
                
                // ModelState valida automáticamente las anotaciones de datos (Required, StringLength, etc.)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var naveCreada = await _service.CreateAsync(naveMenor);
                
                // CreatedAtAction devuelve status 201 con la URI del recurso creado en el header Location
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = naveCreada.Id },
                    naveCreada);
            }
            catch (InvalidOperationException ex)
            {
                // Errores de validación de negocio (ej: matrícula duplicada)
                _logger.LogWarning(ex, "Error de validación al crear nave");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nave");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una nave menor existente
        /// </summary>
        /// <param name="id">ID de la nave a actualizar</param>
        /// <param name="naveMenor">Datos actualizados de la nave</param>
        /// <returns>La nave menor actualizada</returns>
        /// <response code="200">Nave actualizada exitosamente</response>
        /// <response code="400">Si los datos son inválidos o el ID no coincide</response>
        /// <response code="404">Si la nave no existe</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NaveMenor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NaveMenor>> Update(int id, [FromBody] NaveMenor naveMenor)
        {
            try
            {
                _logger.LogInformation("PUT /api/naves/{Id} - Actualizando nave", id);
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validar que el ID de la ruta coincida con el ID del objeto (buena práctica REST)
                if (id != naveMenor.Id && naveMenor.Id != 0)
                {
                    return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto" });
                }

                var naveActualizada = await _service.UpdateAsync(id, naveMenor);
                
                return Ok(naveActualizada);
            }
            catch (InvalidOperationException ex)
            {
                // Puede ser que no exista la nave o que haya un error de validación de negocio
                _logger.LogWarning(ex, "Error de validación al actualizar nave con ID: {Id}", id);
                
                if (ex.Message.Contains("No existe"))
                {
                    return NotFound(new { message = ex.Message });
                }
                
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar nave con ID: {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una nave menor (eliminación lógica)
        /// </summary>
        /// <param name="id">ID de la nave a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="204">Nave eliminada exitosamente</response>
        /// <response code="404">Si la nave no existe</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("DELETE /api/naves/{Id} - Eliminando nave", id);
                
                var resultado = await _service.DeleteAsync(id);
                
                if (!resultado)
                {
                    return NotFound(new { message = $"No se encontró la nave con ID {id}" });
                }
                
                // NoContent (204) indica que la operación fue exitosa pero no hay contenido que retornar
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar nave con ID: {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}
