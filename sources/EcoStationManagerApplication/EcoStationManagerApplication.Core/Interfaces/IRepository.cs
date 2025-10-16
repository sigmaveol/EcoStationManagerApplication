using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{

    /// <summary>
    /// Generic interface cho Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T:class
    {
        /// <summary>
        /// Get entity by primary key
        /// </summary>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Get all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get only active entities (for tables with is_active field)
        /// </summary>
        Task<IEnumerable<T>> GetActiveAsync();

        /// <summary>
        /// Create new entity
        /// </summary>
        /// <returns>ID of created entity</returns>
        Task<int> CreateAsync(T entity);

        /// <summary>
        /// Update existing entity
        /// </summary>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Permanent delete
        /// </summary>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Soft delete (set is_active = false)
        /// </summary>
        Task<bool> SoftDeleteAsync(int id);

        /// <summary>
        /// Check if entity exists
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Get total count of entities
        /// </summary>
        Task<int> GetCountAsync();

    }
}
