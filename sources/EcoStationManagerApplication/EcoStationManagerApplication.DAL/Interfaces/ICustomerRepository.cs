using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByPhoneAsync(string phone);

        Task<IEnumerable<Customer>> SearchAsync(string keyword);

        Task<IEnumerable<Customer>> GetByRankAsync(string rank);

        Task<bool> UpdatePointsAsync(int customerId, int pointsToAdd);

        Task<bool> UpdateRankAsync(int customerId, string newRank);

        Task<bool> ToggleActiveAsync(int customerId, bool isActive);

        Task<IEnumerable<Customer>> GetTopCustomersAsync(int limit = 10);

        Task<(IEnumerable<Customer> Customers, int TotalCount)> GetPagedCustomersAsync(
            int pageNumber, int pageSize, string searchKeyword = null, string rank = null
        );
    }
}
