using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NaveMenorAPI.Data;
using NaveMenorAPI.Repositories;
using NaveMenorAPI.Services;
using NaveMenorAPI.Middleware;

// ============================================================================
// Configuración del builder de la aplicación ASP.NET Core
// ============================================================================

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// 1. CONFIGURACIÓN DE BASE DE DATOS CON ENTITY FRAMEWORK CORE
// ============================================================================

// Configurar la cadena de conexión a SQL Server
// En producción, esta información debe estar en variables de entorno o Azure Key Vault
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // La cadena de conexión se lee desde appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// ============================================================================
// 2. CONFIGURACIÓN DE AUTENTICACIÓN JWT
// ============================================================================

// Configurar autenticación basada en JWT (JSON Web Tokens)
// JWT es un estándar para transmitir información de forma segura entre partes
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "ClaveSecretaParaDesarrollo-CambiarEnProduccion-MinimoDe32Caracteres";

builder.Services.AddAuthentication(options =>
{
    // Esquema por defecto es JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "NaveMenorAPI",
        ValidAudience = jwtSettings["Audience"] ?? "NaveMenorAPIClients",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero // Elimina el tiempo de tolerancia por defecto
    };
});

// ============================================================================
// 3. CONFIGURACIÓN DE CONTROLADORES
// ============================================================================

// Agregar controladores al contenedor de servicios
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configurar el serializador JSON para ignorar ciclos de referencia
        // Esto es importante cuando hay relaciones bidireccionales entre entidades
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// ============================================================================
// 4. CONFIGURACIÓN DE SWAGGER/OPENAPI PARA DOCUMENTACIÓN
// ============================================================================

// Swagger genera documentación interactiva de la API
// Permite probar endpoints directamente desde el navegador
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Registro de Naves Menores",
        Version = "v1",
        Description = "API REST para el registro y gestión de naves menores conforme a la legislación chilena",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Autoridad Marítima",
            Email = "contacto@navesmenores.cl"
        }
    });

    // Configurar Swagger para usar JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. Ingrese 'Bearer' [espacio] y luego su token.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ============================================================================
// 5. CONFIGURACIÓN DE CORS (Cross-Origin Resource Sharing)
// ============================================================================

// CORS permite que aplicaciones frontend de otros dominios accedan a la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // En desarrollo, permitir cualquier origen
        // En producción, especificar los dominios permitidos
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ============================================================================
// 6. INYECCIÓN DE DEPENDENCIAS - Repositorios y Servicios
// ============================================================================

// Registrar repositorios con ciclo de vida Scoped (una instancia por petición HTTP)
builder.Services.AddScoped<INaveMenorRepository, NaveMenorRepository>();

// Registrar servicios de negocio
builder.Services.AddScoped<INaveMenorService, NaveMenorService>();

// ============================================================================
// Construcción de la aplicación
// ============================================================================

var app = builder.Build();

// ============================================================================
// 7. CONFIGURACIÓN DEL PIPELINE DE MIDDLEWARE
// El orden de los middleware es importante y afecta el procesamiento de peticiones
// ============================================================================

// Middleware personalizado para logging de peticiones
app.UseRequestLogging();

// Middleware personalizado para manejo global de errores
app.UseErrorHandling();

// Habilitar Swagger en desarrollo
// Swagger UI estará disponible en /swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Naves Menores v1");
        c.RoutePrefix = "swagger"; // Swagger UI en /swagger
    });
}

// Redirigir HTTP a HTTPS (importante para seguridad en producción)
app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowFrontend");

// Habilitar autenticación
app.UseAuthentication();

// Habilitar autorización
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// ============================================================================
// Mensaje de inicio
// ============================================================================

app.Logger.LogInformation("Iniciando API de Naves Menores...");
app.Logger.LogInformation("Documentación Swagger disponible en: /swagger");

// Ejecutar la aplicación
app.Run();
