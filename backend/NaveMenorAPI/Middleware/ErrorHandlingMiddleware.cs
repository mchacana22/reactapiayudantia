using System.Net;
using System.Text.Json;

namespace NaveMenorAPI.Middleware
{
    /// <summary>
    /// Middleware para manejo global de errores
    /// Captura excepciones no manejadas y las convierte en respuestas HTTP apropiadas
    /// Esto centraliza el manejo de errores y evita que se expongan detalles técnicos al cliente
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Constructor del middleware
        /// </summary>
        /// <param name="next">Siguiente middleware en la pipeline</param>
        /// <param name="logger">Logger para registrar errores</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Método que se ejecuta en cada petición
        /// </summary>
        /// <param name="context">Contexto HTTP de la petición</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continuar con el siguiente middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, la manejamos aquí
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Maneja la excepción y genera una respuesta HTTP apropiada
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Registrar el error con todos sus detalles
            _logger.LogError(exception, "Excepción no manejada: {Message}", exception.Message);

            // Preparar la respuesta
            context.Response.ContentType = "application/json";
            
            var response = new ErrorResponse
            {
                Message = "Ha ocurrido un error interno en el servidor",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            // Personalizar el mensaje según el tipo de excepción
            switch (exception)
            {
                case InvalidOperationException invalidOpEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = invalidOpEx.Message;
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "Acceso no autorizado";
                    break;

                case ArgumentException argEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = argEx.Message;
                    break;

                default:
                    // Para otros errores, mantener el mensaje genérico
                    // En producción, no exponemos detalles técnicos
                    break;
            }

            context.Response.StatusCode = response.StatusCode;

            // Serializar la respuesta a JSON
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }

    /// <summary>
    /// Clase para estructurar las respuestas de error
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Mensaje de error
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Código de estado HTTP
        /// </summary>
        public int StatusCode { get; set; }
    }

    /// <summary>
    /// Extensión para facilitar el registro del middleware
    /// </summary>
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
