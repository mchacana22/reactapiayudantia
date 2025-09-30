using NaveMenorAPI.Models;

namespace NaveMenorAPI.Repositories
{
    /// <summary>
    /// Interfaz que define el contrato para el repositorio de NaveMenor
    /// El patrón Repository abstrae el acceso a datos de la lógica de negocio
    /// Esto facilita el testing y el cambio de tecnología de acceso a datos en el futuro
    /// </summary>
    public interface INaveMenorRepository
    {
        /// <summary>
        /// Obtiene una nave menor por su ID
        /// </summary>
        /// <param name="id">ID de la nave menor</param>
        /// <returns>La nave menor si existe, null si no</returns>
        Task<NaveMenor?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las naves menores activas
        /// </summary>
        /// <returns>Lista de naves menores activas</returns>
        Task<IEnumerable<NaveMenor>> GetAllAsync();

        /// <summary>
        /// Crea una nueva nave menor en la base de datos
        /// </summary>
        /// <param name="naveMenor">Objeto NaveMenor a crear</param>
        /// <returns>La nave menor creada con su ID asignado</returns>
        Task<NaveMenor> CreateAsync(NaveMenor naveMenor);

        /// <summary>
        /// Actualiza una nave menor existente
        /// </summary>
        /// <param name="naveMenor">Objeto NaveMenor con los datos actualizados</param>
        /// <returns>La nave menor actualizada</returns>
        Task<NaveMenor> UpdateAsync(NaveMenor naveMenor);

        /// <summary>
        /// Elimina una nave menor (eliminación lógica, marca como inactiva)
        /// </summary>
        /// <param name="id">ID de la nave menor a eliminar</param>
        /// <returns>True si se eliminó correctamente, False si no</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Verifica si existe una nave con la matrícula especificada
        /// </summary>
        /// <param name="matricula">Matrícula a verificar</param>
        /// <returns>True si existe, False si no</returns>
        Task<bool> ExisteMatriculaAsync(string matricula);

        /// <summary>
        /// Busca una nave menor por su matrícula
        /// </summary>
        /// <param name="matricula">Matrícula de la nave</param>
        /// <returns>La nave menor si existe, null si no</returns>
        Task<NaveMenor?> GetByMatriculaAsync(string matricula);
    }
}
