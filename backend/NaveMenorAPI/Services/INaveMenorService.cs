using NaveMenorAPI.Models;

namespace NaveMenorAPI.Services
{
    /// <summary>
    /// Interfaz que define el contrato para el servicio de negocio de NaveMenor
    /// La capa de servicio contiene la lógica de negocio y validaciones
    /// Se coloca entre el controlador y el repositorio
    /// </summary>
    public interface INaveMenorService
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
        /// Crea una nueva nave menor con validaciones de negocio
        /// </summary>
        /// <param name="naveMenor">Objeto NaveMenor a crear</param>
        /// <returns>La nave menor creada con su ID asignado</returns>
        /// <exception cref="InvalidOperationException">Si la matrícula ya existe</exception>
        Task<NaveMenor> CreateAsync(NaveMenor naveMenor);

        /// <summary>
        /// Actualiza una nave menor existente con validaciones de negocio
        /// </summary>
        /// <param name="id">ID de la nave a actualizar</param>
        /// <param name="naveMenor">Objeto NaveMenor con los datos actualizados</param>
        /// <returns>La nave menor actualizada</returns>
        /// <exception cref="InvalidOperationException">Si la nave no existe o la matrícula está duplicada</exception>
        Task<NaveMenor> UpdateAsync(int id, NaveMenor naveMenor);

        /// <summary>
        /// Elimina una nave menor (eliminación lógica)
        /// </summary>
        /// <param name="id">ID de la nave menor a eliminar</param>
        /// <returns>True si se eliminó correctamente, False si no existe</returns>
        Task<bool> DeleteAsync(int id);
    }
}
