# Configuración de Base de Datos

Esta guía explica cómo configurar la base de datos SQL Server para la API de Naves Menores.

## Requisitos Previos

- SQL Server 2018 o superior (puede ser Express, Developer o Enterprise)
- .NET 9 SDK instalado
- Entity Framework Core Tools instalado

## Instalación de EF Core Tools

Si no tienes instaladas las herramientas de Entity Framework Core, ejecuta:

```bash
dotnet tool install --global dotnet-ef
```

O actualiza a la última versión:

```bash
dotnet tool update --global dotnet-ef
```

Verifica la instalación:

```bash
dotnet ef --version
```

## Paso 1: Configurar la Cadena de Conexión

### Opción A: Windows con SQL Server Express (Windows Authentication)

Edita `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NaveMenorDB_Dev;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Opción B: SQL Server con Autenticación de Usuario

Edita `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NaveMenorDB_Dev;User Id=sa;Password=TuPassword;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Opción C: Docker SQL Server

Si usas SQL Server en Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

Cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=NaveMenorDB_Dev;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

## Paso 2: Crear las Migraciones

Desde el directorio del proyecto (`NaveMenorAPI`), ejecuta:

```bash
cd backend/NaveMenorAPI
dotnet ef migrations add InitialCreate
```

Este comando creará:
- Una carpeta `Migrations/` con los archivos de migración
- Un archivo con el snapshot del modelo actual

## Paso 3: Aplicar las Migraciones a la Base de Datos

```bash
dotnet ef database update
```

Este comando:
- Creará la base de datos si no existe
- Creará todas las tablas según el modelo definido
- Aplicará restricciones, índices y relaciones

## Paso 4: Verificar la Base de Datos

Puedes conectarte a SQL Server con SQL Server Management Studio (SSMS) o Azure Data Studio para verificar que se crearon las tablas:

- `NavesMenores`
- `Propietarios`
- `Zarpes`
- `Recaladas`
- `Inspecciones`
- `Usuarios`
- `LogsOperacion`
- `__EFMigrationsHistory` (tabla de control de migraciones)

## Paso 5: Insertar Datos de Prueba (Opcional)

Puedes crear un script SQL para insertar datos de prueba:

```sql
-- Insertar un propietario de prueba
INSERT INTO Propietarios (Rut, Nombre, Direccion, Telefono, Email, FechaRegistro, Activo)
VALUES ('12.345.678-9', 'Juan Pérez', 'Av. Principal 123, Valparaíso', '+56912345678', 'juan.perez@example.com', GETDATE(), 1);

-- Insertar una nave de prueba
INSERT INTO NavesMenores (Nombre, Matricula, Tipo, Eslora, Manga, Puntal, Material, AnioConstruccion, PuertoBase, CapacidadTripulacion, PropietarioId, FechaRegistro, Activa, Observaciones)
VALUES ('La Gaviota', 'VAL-12345', 'Pesquero', 12.5, 4.2, 2.8, 'Fibra de vidrio', 2020, 'Valparaíso', 6, 1, GETDATE(), 1, 'En buen estado');

-- Insertar un usuario de prueba (contraseña: "admin123" - deberías hashearla en producción)
INSERT INTO Usuarios (NombreUsuario, PasswordHash, NombreCompleto, Email, Rol, Activo, FechaCreacion)
VALUES ('admin', 'admin123', 'Administrador del Sistema', 'admin@navesmenores.cl', 'Admin', 1, GETDATE());
```

O puedes usar un archivo SQL más completo que puedes ejecutar con:

```bash
sqlcmd -S localhost -d NaveMenorDB_Dev -i datos_prueba.sql
```

## Comandos Útiles de Entity Framework Core

### Ver las migraciones aplicadas

```bash
dotnet ef migrations list
```

### Revertir la última migración

```bash
dotnet ef database update <nombre-migracion-anterior>
```

### Eliminar la última migración (si aún no se aplicó)

```bash
dotnet ef migrations remove
```

### Generar script SQL de las migraciones

```bash
dotnet ef migrations script -o migration.sql
```

### Eliminar la base de datos completamente

```bash
dotnet ef database drop
```

Luego puedes volver a crearla con:

```bash
dotnet ef database update
```

## Actualización del Modelo

Cuando modifiques las entidades (Models), necesitas crear una nueva migración:

1. Modifica la clase del modelo
2. Crea la migración:
   ```bash
   dotnet ef migrations add NombreDeLaMigracion
   ```
3. Revisa el archivo de migración generado
4. Aplica la migración:
   ```bash
   dotnet ef database update
   ```

## Resolución de Problemas

### Error: "A network-related or instance-specific error"

- Verifica que SQL Server esté ejecutándose
- Verifica que el nombre del servidor sea correcto
- Verifica que el puerto 1433 esté abierto
- En Windows, verifica que SQL Server Browser esté ejecutándose

### Error: "Login failed for user"

- Verifica las credenciales en la cadena de conexión
- Verifica que el usuario tenga permisos en SQL Server
- Si usas Windows Authentication, asegúrate de usar `Integrated Security=true`

### Error: "Cannot attach the file as database"

- Puede haber permisos insuficientes en el directorio
- Intenta especificar una ruta completa en la cadena de conexión
- O usa una base de datos en el servidor en lugar de un archivo

### Migración con conflictos

Si hay errores durante la migración:

1. Revisa el mensaje de error
2. Elimina la migración problemática:
   ```bash
   dotnet ef migrations remove
   ```
3. Corrige el modelo
4. Crea una nueva migración

## Seguridad

⚠️ **IMPORTANTE**: En producción:

1. **Nunca comitear cadenas de conexión con contraseñas reales**
2. Usar variables de entorno o Azure Key Vault
3. Usar usuarios de base de datos con permisos mínimos necesarios
4. Habilitar cifrado en las conexiones (TLS/SSL)
5. Hashear las contraseñas de usuarios (usar BCrypt, PBKDF2, etc.)

## Ejemplo de Variables de Entorno

En producción, usa variables de entorno:

```bash
# Linux/Mac
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=NaveMenorDB;..."

# Windows PowerShell
$env:ConnectionStrings__DefaultConnection="Server=prod-server;Database=NaveMenorDB;..."
```

Y lee desde `Program.cs`:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
```

## Backup y Restauración

### Crear un backup

```sql
BACKUP DATABASE NaveMenorDB_Dev 
TO DISK = 'C:\Backups\NaveMenorDB.bak'
WITH FORMAT;
```

### Restaurar desde backup

```sql
RESTORE DATABASE NaveMenorDB_Dev 
FROM DISK = 'C:\Backups\NaveMenorDB.bak'
WITH REPLACE;
```

## Azure SQL Database

Si despliegas en Azure, la cadena de conexión será diferente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:tuservidor.database.windows.net,1433;Database=NaveMenorDB;User ID=tuusuario@tuservidor;Password=tupassword;Encrypt=true;Connection Timeout=30;"
  }
}
```

Las migraciones se aplican igual:

```bash
dotnet ef database update
```
