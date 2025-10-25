using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoStationManagerApplication.Models;

namespace EcoStationManagerApplication.Core.Interfaces
{
    /// <summary>
    /// Role repository interface extending the generic IRepository.
    /// Provides extra methods for role and permission operations.
    /// </summary>
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// Get a role by its name.
        /// </summary>
        Task<Role> GetByNameAsync(string name);

        /// <summary>
        /// Get all roles including their permissions.
        /// </summary>
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();

        /// <summary>
        /// Update the permissions of a specific role.
        /// </summary>
        /// <param name="roleId">The ID of the role.</param>
        /// <param name="permissionsJson">Permissions data in JSON format.</param>
        Task<bool> UpdatePermissionsAsync(int roleId, string permissionsJson);
    }
}
