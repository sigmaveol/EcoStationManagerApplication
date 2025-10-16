using EcoStationManagerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces.IServices
{
    public interface IUserService
    {
        // Logic đặc thù của User
        Task<User> LoginAsync(string username, string password);
        Task<bool> ResetPasswordAsync(int userId);
        Task<bool> AssignRoleAsync(int userId, int roleId);
    }
}
