using NaveMenorAPI.Models;

namespace NaveMenorAPI.Data
{
    /// <summary>
    /// Clase para poblar la base de datos con datos iniciales de prueba
    /// Útil para desarrollo y testing
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Inicializa la base de datos con datos de prueba si está vacía
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        public static void Initialize(ApplicationDbContext context)
        {
            // Asegurar que la base de datos está creada
            context.Database.EnsureCreated();

            // Si ya hay datos, no hacer nada
            if (context.Propietarios.Any())
            {
                return; // La base de datos ya tiene datos
            }

            // Crear propietarios de prueba
            var propietarios = new[]
            {
                new Propietario
                {
                    Rut = "12.345.678-9",
                    Nombre = "Juan Pérez García",
                    Direccion = "Av. Errázuriz 123, Valparaíso",
                    Telefono = "+56912345678",
                    Email = "juan.perez@example.com",
                    FechaRegistro = DateTime.UtcNow,
                    Activo = true
                },
                new Propietario
                {
                    Rut = "23.456.789-0",
                    Nombre = "María González Silva",
                    Direccion = "Calle Prat 456, Coquimbo",
                    Telefono = "+56923456789",
                    Email = "maria.gonzalez@example.com",
                    FechaRegistro = DateTime.UtcNow,
                    Activo = true
                },
                new Propietario
                {
                    Rut = "34.567.890-1",
                    Nombre = "Pedro Rodríguez López",
                    Direccion = "Av. Pedro Montt 789, Talcahuano",
                    Telefono = "+56934567890",
                    Email = "pedro.rodriguez@example.com",
                    FechaRegistro = DateTime.UtcNow,
                    Activo = true
                }
            };

            context.Propietarios.AddRange(propietarios);
            context.SaveChanges();

            // Crear naves menores de prueba
            var naves = new[]
            {
                new NaveMenor
                {
                    Nombre = "La Gaviota",
                    Matricula = "VAL-12345",
                    Tipo = "Pesquero",
                    Eslora = 12.5m,
                    Manga = 4.2m,
                    Puntal = 2.8m,
                    Material = "Fibra de vidrio",
                    AnioConstruccion = 2020,
                    PuertoBase = "Valparaíso",
                    CapacidadTripulacion = 6,
                    PropietarioId = propietarios[0].Id,
                    FechaRegistro = DateTime.UtcNow,
                    Activa = true,
                    Observaciones = "Equipada con GPS y radio VHF"
                },
                new NaveMenor
                {
                    Nombre = "El Delfín",
                    Matricula = "COQ-98765",
                    Tipo = "Recreación",
                    Eslora = 8.3m,
                    Manga = 2.9m,
                    Puntal = 1.8m,
                    Material = "Aluminio",
                    AnioConstruccion = 2022,
                    PuertoBase = "Coquimbo",
                    CapacidadTripulacion = 4,
                    PropietarioId = propietarios[1].Id,
                    FechaRegistro = DateTime.UtcNow,
                    Activa = true,
                    Observaciones = "Uso recreativo familiar"
                },
                new NaveMenor
                {
                    Nombre = "Mar Azul",
                    Matricula = "TAL-55555",
                    Tipo = "Transporte",
                    Eslora = 15.0m,
                    Manga = 5.5m,
                    Puntal = 3.2m,
                    Material = "Acero",
                    AnioConstruccion = 2018,
                    PuertoBase = "Talcahuano",
                    CapacidadTripulacion = 8,
                    PropietarioId = propietarios[2].Id,
                    FechaRegistro = DateTime.UtcNow,
                    Activa = true,
                    Observaciones = "Transporte de pasajeros entre islas"
                },
                new NaveMenor
                {
                    Nombre = "Estrella del Mar",
                    Matricula = "VAL-24680",
                    Tipo = "Pesquero",
                    Eslora = 10.8m,
                    Manga = 3.8m,
                    Puntal = 2.5m,
                    Material = "Madera",
                    AnioConstruccion = 2015,
                    PuertoBase = "Valparaíso",
                    CapacidadTripulacion = 5,
                    PropietarioId = propietarios[0].Id,
                    FechaRegistro = DateTime.UtcNow,
                    Activa = true,
                    Observaciones = "Pesca artesanal"
                }
            };

            context.NavesMenores.AddRange(naves);
            context.SaveChanges();

            // Crear usuarios de prueba
            // NOTA: En producción, las contraseñas deben estar hasheadas con BCrypt u otro algoritmo seguro
            var usuarios = new[]
            {
                new Usuario
                {
                    NombreUsuario = "admin",
                    PasswordHash = "admin123", // En producción, usar BCrypt.HashPassword()
                    NombreCompleto = "Administrador del Sistema",
                    Email = "admin@navesmenores.cl",
                    Rol = "Admin",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                },
                new Usuario
                {
                    NombreUsuario = "inspector",
                    PasswordHash = "inspector123", // En producción, usar BCrypt.HashPassword()
                    NombreCompleto = "Inspector Marítimo",
                    Email = "inspector@navesmenores.cl",
                    Rol = "Inspector",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                },
                new Usuario
                {
                    NombreUsuario = "operador",
                    PasswordHash = "operador123", // En producción, usar BCrypt.HashPassword()
                    NombreCompleto = "Operador de Puerto",
                    Email = "operador@navesmenores.cl",
                    Rol = "Operador",
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                }
            };

            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            // Crear algunas inspecciones de prueba
            var inspecciones = new[]
            {
                new Inspeccion
                {
                    NaveMenorId = naves[0].Id,
                    FechaInspeccion = DateTime.UtcNow.AddMonths(-2),
                    TipoInspeccion = "Anual",
                    Inspector = "Carlos Fuentes",
                    Resultado = "Aprobada",
                    FechaVencimiento = DateTime.UtcNow.AddMonths(10),
                    Observaciones = "Todos los equipos de seguridad en buen estado"
                },
                new Inspeccion
                {
                    NaveMenorId = naves[1].Id,
                    FechaInspeccion = DateTime.UtcNow.AddMonths(-1),
                    TipoInspeccion = "Seguridad",
                    Inspector = "Ana Martínez",
                    Resultado = "Aprobada con observaciones",
                    FechaVencimiento = DateTime.UtcNow.AddMonths(11),
                    Observaciones = "Reemplazar chaleco salvavidas dañado",
                    Deficiencias = "1 chaleco salvavidas con desgaste"
                }
            };

            context.Inspecciones.AddRange(inspecciones);
            context.SaveChanges();

            // Crear algunos zarpes de prueba
            var zarpes = new[]
            {
                new Zarpe
                {
                    NaveMenorId = naves[0].Id,
                    FechaZarpe = DateTime.UtcNow.AddDays(-5),
                    PuertoSalida = "Valparaíso",
                    PuertoDestino = "Quintero",
                    Proposito = "Pesca",
                    Capitan = "Juan Pérez García",
                    NumeroTripulantes = 5,
                    AutorizadoPor = "Capitán de Puerto",
                    Observaciones = "Condiciones meteorológicas favorables"
                },
                new Zarpe
                {
                    NaveMenorId = naves[2].Id,
                    FechaZarpe = DateTime.UtcNow.AddDays(-3),
                    PuertoSalida = "Talcahuano",
                    PuertoDestino = "Isla Santa María",
                    Proposito = "Transporte de pasajeros",
                    Capitan = "Pedro Rodríguez López",
                    NumeroTripulantes = 3,
                    AutorizadoPor = "Oficial de Puerto",
                    Observaciones = "Transporte regular"
                }
            };

            context.Zarpes.AddRange(zarpes);
            context.SaveChanges();

            // Crear algunas recaladas de prueba
            var recaladas = new[]
            {
                new Recalada
                {
                    NaveMenorId = naves[0].Id,
                    FechaRecalada = DateTime.UtcNow.AddDays(-4),
                    PuertoLlegada = "Valparaíso",
                    PuertoOrigen = "Quintero",
                    ResultadoViaje = "Exitoso",
                    Capitan = "Juan Pérez García",
                    NumeroTripulantes = 5,
                    RegistradoPor = "Oficial de Puerto",
                    Observaciones = "Pesca exitosa, sin incidentes"
                }
            };

            context.Recaladas.AddRange(recaladas);
            context.SaveChanges();
        }
    }
}
