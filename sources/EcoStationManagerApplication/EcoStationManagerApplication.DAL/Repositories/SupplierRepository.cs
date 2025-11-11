using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Suppliers", "supplier_id")
        {
        }

        public async Task<IEnumerable<Supplier>> SearchAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllAsync();

                var sql = @"
                    SELECT * FROM Suppliers 
                    WHERE name LIKE @Keyword OR contact_person LIKE @Keyword OR phone LIKE @Keyword OR email LIKE @Keyword
                    ORDER BY name";

                return await _databaseHelper.QueryAsync<Supplier>(sql, new { Keyword = $"%{keyword}%" });
            }
            catch (Exception ex)
            {
                _logger.Error($"SearchAsync error - Keyword: {keyword} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Supplier>> GetByContactPersonAsync(string contactPerson)
        {
            try
            {
                var sql = "SELECT * FROM Suppliers WHERE contact_person LIKE @ContactPerson ORDER BY name";
                return await _databaseHelper.QueryAsync<Supplier>(sql, new { ContactPerson = $"%{contactPerson}%" });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByContactPersonAsync error - ContactPerson: {contactPerson} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeSupplierId = null)
        {
            try
            {
                var sql = "SELECT 1 FROM Suppliers WHERE email = @Email";
                var parameters = new DynamicParameters();
                parameters.Add("Email", email);

                if (excludeSupplierId.HasValue)
                {
                    sql += " AND supplier_id != @ExcludeSupplierId";
                    parameters.Add("ExcludeSupplierId", excludeSupplierId.Value);
                }

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(sql, parameters);
                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsEmailExistsAsync error - Email: {email}, ExcludeId: {excludeSupplierId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsPhoneExistsAsync(string phone, int? excludeSupplierId = null)
        {
            try
            {
                var sql = "SELECT 1 FROM Suppliers WHERE phone = @Phone";
                var parameters = new DynamicParameters();
                parameters.Add("Phone", phone);

                if (excludeSupplierId.HasValue)
                {
                    sql += " AND supplier_id != @ExcludeSupplierId";
                    parameters.Add("ExcludeSupplierId", excludeSupplierId.Value);
                }

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(sql, parameters);
                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsPhoneExistsAsync error - Phone: {phone}, ExcludeId: {excludeSupplierId} - {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalSuppliersCountAsync()
        {
            try
            {
                var sql = "SELECT COUNT(*) FROM Suppliers";
                return await _databaseHelper.ExecuteScalarAsync<int>(sql);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalSuppliersCountAsync error - {ex.Message}");
                throw;
            }
        }

    }


}
