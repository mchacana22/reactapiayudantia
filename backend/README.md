# API de Registro de Naves Menores

API REST desarrollada en ASP.NET Core para el registro y gestión de naves menores conforme a la legislación chilena.

## Características

- ✅ API REST con ASP.NET Core (.NET 9)
- ✅ Entity Framework Core para acceso a datos
- ✅ SQL Server 2018+ como base de datos
- ✅ Autenticación JWT (JSON Web Tokens)
- ✅ Documentación con Swagger/OpenAPI
- ✅ Patrón Repository para acceso a datos
- ✅ Capa de servicio para lógica de negocio
- ✅ Middleware personalizado para logging y manejo de errores
- ✅ Validaciones de datos con Data Annotations
- ✅ Inyección de dependencias

## Estructura del Proyecto

```
NaveMenorAPI/
├── Controllers/        # Controladores API REST
│   └── NavesController.cs
├── Models/            # Modelos de datos (entidades)
│   ├── NaveMenor.cs
│   ├── Propietario.cs
│   ├── Zarpe.cs
│   ├── Recalada.cs
│   ├── Inspeccion.cs
│   ├── Usuario.cs
│   └── LogOperacion.cs
├── Data/              # Contexto de base de datos
│   └── ApplicationDbContext.cs
├── Repositories/      # Capa de acceso a datos
│   ├── INaveMenorRepository.cs
│   └── NaveMenorRepository.cs
├── Services/          # Capa de lógica de negocio
│   ├── INaveMenorService.cs
│   └── NaveMenorService.cs
├── Middleware/        # Middleware personalizado
│   ├── ErrorHandlingMiddleware.cs
│   └── RequestLoggingMiddleware.cs
├── Program.cs         # Punto de entrada y configuración
└── appsettings.json   # Configuración de la aplicación
```

## Modelos de Datos

### NaveMenor
Representa una nave menor con sus características técnicas:
- Nombre, matrícula, tipo
- Dimensiones: eslora, manga, puntal
- Material de construcción
- Puerto base, capacidad de tripulación
- Relación con propietario

### Propietario
Dueño de una o varias naves:
- RUT, nombre, dirección
- Datos de contacto (teléfono, email)

### Zarpe
Registro de salida del puerto:
- Fecha de zarpe
- Puerto de salida y destino
- Propósito del viaje, capitán, tripulantes

### Recalada
Registro de llegada al puerto:
- Fecha de recalada
- Puerto de llegada y origen
- Resultado del viaje

### Inspeccion
Inspección técnica de la nave:
- Fecha y tipo de inspección
- Inspector, resultado
- Deficiencias encontradas

### Usuario
Usuario del sistema con credenciales:
- Nombre de usuario, contraseña (hasheada)
- Rol (Admin, Inspector, Operador)

### LogOperacion
Registro de auditoría:
- Operaciones realizadas (CRUD)
- Usuario, fecha/hora, IP
- Resultado de la operación

## Endpoints API

### Naves Menores

```
GET    /api/naves          # Obtener todas las naves activas
GET    /api/naves/{id}     # Obtener una nave por ID
POST   /api/naves          # Crear nueva nave
PUT    /api/naves/{id}     # Actualizar nave existente
DELETE /api/naves/{id}     # Eliminar nave (lógica)
```

### Ejemplo de Petición POST

```json
POST /api/naves
Content-Type: application/json

{
  "nombre": "La Gaviota",
  "matricula": "VAL-12345",
  "tipo": "Pesquero",
  "eslora": 12.5,
  "manga": 4.2,
  "puntal": 2.8,
  "material": "Fibra de vidrio",
  "anioConstruccion": 2020,
  "puertoBase": "Valparaíso",
  "capacidadTripulacion": 6,
  "propietarioId": 1,
  "observaciones": "En buen estado"
}
```

### Ejemplo de Respuesta 201 Created

```json
{
  "id": 1,
  "nombre": "La Gaviota",
  "matricula": "VAL-12345",
  "tipo": "Pesquero",
  "eslora": 12.5,
  "manga": 4.2,
  "puntal": 2.8,
  "material": "Fibra de vidrio",
  "anioConstruccion": 2020,
  "puertoBase": "Valparaíso",
  "capacidadTripulacion": 6,
  "propietarioId": 1,
  "propietario": {
    "id": 1,
    "rut": "12.345.678-9",
    "nombre": "Juan Pérez",
    "email": "juan@example.com"
  },
  "fechaRegistro": "2025-01-13T10:30:00Z",
  "activa": true,
  "observaciones": "En buen estado"
}
```

## Configuración

### Requisitos Previos

- .NET 9 SDK
- SQL Server 2018 o superior
- Visual Studio 2022 / VS Code / Rider (opcional)

### Configuración de Base de Datos

1. Actualizar la cadena de conexión en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NaveMenorDB;User Id=sa;Password=TuPassword;TrustServerCertificate=True"
  }
}
```

2. Crear las migraciones de Entity Framework:

```bash
dotnet ef migrations add InitialCreate
```

3. Aplicar las migraciones a la base de datos:

```bash
dotnet ef database update
```

### Configuración de JWT

En `appsettings.json`, configurar las claves JWT:

```json
{
  "JwtSettings": {
    "SecretKey": "TuClaveSecreta-Minimo32Caracteres",
    "Issuer": "NaveMenorAPI",
    "Audience": "NaveMenorAPIClients",
    "ExpirationMinutes": 60
  }
}
```

**IMPORTANTE**: En producción, usar secretos seguros y nunca comitear claves en el código fuente.

## Ejecución

### Desarrollo

```bash
cd NaveMenorAPI
dotnet run
```

La API estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

### Producción

```bash
dotnet publish -c Release -o ./publish
cd publish
dotnet NaveMenorAPI.dll
```

## Documentación con Swagger

Una vez ejecutada la aplicación, accede a Swagger UI en:
```
https://localhost:5001/swagger
```

Swagger proporciona:
- Documentación interactiva de todos los endpoints
- Posibilidad de probar los endpoints directamente
- Esquemas de los modelos de datos
- Códigos de respuesta HTTP

## Validaciones

El sistema incluye validaciones en múltiples niveles:

1. **Validaciones de Datos (Data Annotations)**:
   - Campos requeridos
   - Longitud de cadenas
   - Rangos numéricos
   - Formatos (email, teléfono)

2. **Validaciones de Negocio (Service Layer)**:
   - Matrícula única
   - Dimensiones coherentes (manga < eslora)
   - Verificación de existencia de entidades relacionadas

3. **Validaciones de API (Controllers)**:
   - Validación de ModelState
   - Verificación de concordancia de IDs

## Middleware

### ErrorHandlingMiddleware
Captura excepciones no manejadas y las convierte en respuestas HTTP apropiadas:
- `InvalidOperationException` → 400 Bad Request
- `UnauthorizedAccessException` → 401 Unauthorized
- Otras excepciones → 500 Internal Server Error

### RequestLoggingMiddleware
Registra todas las peticiones HTTP con:
- Método HTTP, ruta, query string
- IP de origen
- Código de respuesta
- Tiempo de procesamiento

## Arquitectura

El proyecto sigue los principios de **Clean Architecture** y **SOLID**:

1. **Separación de Responsabilidades**:
   - Controllers: Manejan peticiones HTTP
   - Services: Contienen lógica de negocio
   - Repositories: Acceso a datos
   - Models: Entidades de dominio

2. **Inversión de Dependencias**:
   - Todo se inyecta mediante interfaces
   - Facilita testing y mantenimiento

3. **Responsabilidad Única**:
   - Cada clase tiene una responsabilidad bien definida

## Logging

El sistema utiliza el framework de logging de ASP.NET Core:
- Logs de información para operaciones normales
- Logs de advertencia para situaciones anómalas
- Logs de error para excepciones

Los logs se pueden configurar en `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "NaveMenorAPI": "Debug"
    }
  }
}
```

## Seguridad

- ✅ Autenticación JWT
- ✅ HTTPS obligatorio en producción
- ✅ Validación de entrada en todos los endpoints
- ✅ Eliminación lógica (no se pierden datos)
- ✅ Logs de auditoría
- ✅ Contraseñas hasheadas (nunca en texto plano)
- ✅ CORS configurado
- ✅ SQL Injection prevenido con EF Core

## Próximos Pasos

Para completar la implementación:

1. Implementar endpoints para las demás entidades (Propietarios, Zarpes, Recaladas, Inspecciones)
2. Crear endpoint de login que genere tokens JWT
3. Agregar autorización por roles a los endpoints
4. Implementar paginación en las consultas
5. Agregar filtros y búsquedas
6. Crear tests unitarios e integración
7. Configurar CI/CD
8. Dockerizar la aplicación

## Licencia

Este proyecto es un ejemplo educativo para el registro de naves menores.
