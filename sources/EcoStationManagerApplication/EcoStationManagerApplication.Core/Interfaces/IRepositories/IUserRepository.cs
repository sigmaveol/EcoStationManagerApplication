using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoStationManagerApplication.Models;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Get a user by username.
        /// </summary>
        Task<User> GetByUsernameAsync(string username);

        /// <summary>
        /// Get a user by email.
        /// </summary>
        Task<User> GetByEmailAsync(string email);

        /// <summary>
        /// Validate user credentials.
        /// </summary>
        Task<bool> ValidateCredentialsAsync(string username, string passwordHash);

        /// <summary>
        /// Update a user's password.
        /// </summary>
        Task<bool> UpdatePasswordAsync(int userId, string newPasswordHash);

        /// <summary>
        /// Deactivate a user account.
        /// </summary>
        Task<bool> DeactivateUserAsync(int userId);

        /// <summary>
        /// Activate a user account.
        /// </summary>
        Task<bool> ActivateUserAsync(int userId);

        /// <summary>
        /// Get all users by a specific role.
        /// </summary>
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
    }
}
