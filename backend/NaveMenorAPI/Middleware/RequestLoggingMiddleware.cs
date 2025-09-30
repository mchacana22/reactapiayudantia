namespace NaveMenorAPI.Middleware
{
    /// <summary>
    /// Middleware para logging de peticiones HTTP
    /// Registra información básica de cada petición para auditoría y diagnóstico
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        /// <summary>
        /// Constructor del middleware
        /// </summary>
        /// <param name="next">Siguiente middleware en la pipeline</param>
        /// <param name="logger">Logger para registrar información</param>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
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
            // Capturar información de la petición
            var method = context.Request.Method;
            var path = context.Request.Path;
            var queryString = context.Request.QueryString;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            // Registrar el inicio de la petición
            _logger.LogInformation(
                "Petición entrante: {Method} {Path}{QueryString} desde IP: {IpAddress}",
                method, path, queryString, ipAddress);

            // Medir el tiempo de respuesta
            var startTime = DateTime.UtcNow;

            try
            {
                // Continuar con el siguiente middleware
                await _next(context);

                // Calcular el tiempo transcurrido
                var elapsedTime = DateTime.UtcNow - startTime;

                // Registrar la respuesta
                _logger.LogInformation(
                    "Respuesta: {Method} {Path} - Status: {StatusCode} - Tiempo: {ElapsedMs}ms",
                    method, path, context.Response.StatusCode, elapsedTime.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                // En caso de error, también registrar
                var elapsedTime = DateTime.UtcNow - startTime;
                
                _logger.LogError(
                    ex,
                    "Error en petición: {Method} {Path} - Tiempo: {ElapsedMs}ms",
                    method, path, elapsedTime.TotalMilliseconds);

                // Re-lanzar la excepción para que sea manejada por ErrorHandlingMiddleware
                throw;
            }
        }
    }

    /// <summary>
    /// Extensión para facilitar el registro del middleware
    /// </summary>
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
