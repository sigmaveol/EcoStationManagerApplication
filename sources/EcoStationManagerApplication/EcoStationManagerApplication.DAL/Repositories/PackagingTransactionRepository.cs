using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class PackagingTransactionRepository : BaseRepository<PackagingTransaction>, IPackagingTransactionRepository
    {
        private readonly IPackagingInventoryRepository _packagingInventoryRepository;

        public PackagingTransactionRepository(
            IDatabaseHelper databaseHelper,
            IPackagingInventoryRepository packagingInventoryRepository)
            : base(databaseHelper, "PackagingTransactions", "transaction_id")
        {
            _packagingInventoryRepository = packagingInventoryRepository;
        }

        public async Task<IEnumerable<PackagingTransaction>> GetByPackagingAsync(int packagingId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<PackagingTransaction>(
                    PackagingTransactionQueries.GetByPackaging,
                    new { PackagingId = packagingId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByPackagingAsync error - PackagingId: {packagingId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PackagingTransaction>> GetByCustomerAsync(int customerId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("CustomerId", customerId);

                string sql = PackagingTransactionQueries.GetByCustomer;
                if (fromDate.HasValue)
                {
                    sql += " AND pt.created_date >= @FromDate";
                    parameters.Add("FromDate", fromDate);
                }
                if (toDate.HasValue)
                {
                    sql += " AND pt.created_date <= @ToDate";
                    parameters.Add("ToDate", toDate);
                }

                sql += " ORDER BY pt.created_date DESC";

                var data = await _databaseHelper.QueryAsync<PackagingTransaction>(
                    sql,
                    parameters
                );

                var list = data.ToList();
                foreach (var t in list)
                {
                    if (t.Type == PackagingTransactionType.RETURN)
                    {
                        t.Quantity = -Math.Abs(t.Quantity);
                        t.RefundAmount = -Math.Abs(t.RefundAmount);
                        t.DepositPrice = 0m;
                    }
                    else if (t.Type == PackagingTransactionType.ISSUE)
                    {
                        t.Quantity = Math.Abs(t.Quantity);
                        t.DepositPrice = Math.Abs(t.DepositPrice);
                        t.RefundAmount = 0m;
                    }
                }

                var packagingIds = list.Select(x => x.PackagingId).Distinct().ToList();
                var customerIds = list.Where(x => x.CustomerId.HasValue).Select(x => x.CustomerId.Value).Distinct().ToList();
                var userIds = list.Where(x => x.UserId.HasValue).Select(x => x.UserId.Value).Distinct().ToList();

                if (packagingIds.Any())
                {
                    var packagings = await _databaseHelper.QueryAsync<Packaging>(
                        "SELECT * FROM Packaging WHERE packaging_id IN @Ids",
                        new { Ids = packagingIds }
                    );
                    var pDict = packagings.ToDictionary(p => p.PackagingId);
                    foreach (var t in list)
                    {
                        if (pDict.TryGetValue(t.PackagingId, out var p))
                            t.Packaging = p;
                    }
                }

                if (customerIds.Any())
                {
                    var customers = await _databaseHelper.QueryAsync<Customer>(
                        "SELECT * FROM Customers WHERE customer_id IN @Ids",
                        new { Ids = customerIds }
                    );
                    var cDict = customers.ToDictionary(c => c.CustomerId);
                    foreach (var t in list)
                    {
                        if (t.CustomerId.HasValue && cDict.TryGetValue(t.CustomerId.Value, out var c))
                            t.Customer = c;
                    }
                }

                if (userIds.Any())
                {
                    var users = await _databaseHelper.QueryAsync<User>(
                        "SELECT * FROM Users WHERE user_id IN @Ids",
                        new { Ids = userIds }
                    );
                    var uDict = users.ToDictionary(u => u.UserId);
                    foreach (var t in list)
                    {
                        if (t.UserId.HasValue && uDict.TryGetValue(t.UserId.Value, out var u))
                            t.User = u;
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByCustomerAsync error - CustomerId: {customerId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PackagingTransaction>> GetByTypeAsync(PackagingTransactionType type)
        {
            try
            {
                // Với TINYINT, cần pass số nguyên thay vì string
                return await _databaseHelper.QueryAsync<PackagingTransaction>(
                    PackagingTransactionQueries.GetByType,
                    new { Type = (int)type }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByTypeAsync error - Type: {type} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PackagingTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<PackagingTransaction>(
                    PackagingTransactionQueries.GetByDateRange,
                    new { StartDate = startDate, EndDate = endDate }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByDateRangeAsync error - Start: {startDate}, End: {endDate} - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalDepositAmountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = PackagingTransactionQueries.GetTotalDepositAmount;
                var parameters = new DynamicParameters();

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND created_date BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value);
                    parameters.Add("EndDate", endDate.Value);
                }

                return await _databaseHelper.ExecuteScalarAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalDepositAmountAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalRefundAmountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = PackagingTransactionQueries.GetTotalRefundAmount;
                var parameters = new DynamicParameters();

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND created_date BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value);
                    parameters.Add("EndDate", endDate.Value);
                }

                return await _databaseHelper.ExecuteScalarAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalRefundAmountAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetCustomerHoldingQuantityAsync(int customerId, int packagingId)
        {
            try
            {
                return await _databaseHelper.ExecuteScalarAsync<int>(
                    PackagingTransactionQueries.GetCustomerHoldingQuantity,
                    new { CustomerId = customerId, PackagingId = packagingId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetCustomerHoldingQuantityAsync error - CustomerId: {customerId}, PackagingId: {packagingId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CustomerHoldingPackaging>> GetCustomerHoldingsAsync(int customerId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<CustomerHoldingPackaging>(
                    PackagingTransactionQueries.GetCustomerHoldings,
                    new { CustomerId = customerId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetCustomerHoldingsAsync error - CustomerId: {customerId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsCustomerHoldingPackagingAsync(int customerId, int packagingId)
        {
            try
            {
                var result = await _databaseHelper.ExecuteScalarAsync<int?>(
                    PackagingTransactionQueries.IsCustomerHoldingPackaging,
                    new { CustomerId = customerId, PackagingId = packagingId }
                );

                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsCustomerHoldingPackagingAsync error - CustomerId: {customerId}, PackagingId: {packagingId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IssuePackagingAsync(int packagingId, int? customerId, int quantity,
                                                   decimal depositPrice, int? userId, string notes = null)
        {
            using (var connection = await _databaseHelper.CreateConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Kiểm tra đủ bao bì mới không
                        var canIssue = await _packagingInventoryRepository.IsNewPackagingSufficientAsync(packagingId, quantity);
                        if (!canIssue)
                        {
                            _logger.Warning($"Không đủ bao bì để phát hành - PackagingId: {packagingId}, Quantity: {quantity}");
                            return false;
                        }

                        // 2. Chuyển bao bì sang trạng thái đang sử dụng
                        var transferSuccess = await _packagingInventoryRepository.TransferToInUseAsync(packagingId, quantity);
                        if (!transferSuccess)
                        {
                            _logger.Error($"Không thể chuyển trạng thái bao bì - PackagingId: {packagingId}, Quantity: {quantity}");
                            transaction.Rollback();
                            return false;
                        }

                        // 3. Ghi nhận giao dịch phát hành
                        var affectedRows = await connection.ExecuteAsync(
                            PackagingTransactionQueries.InsertTransaction,
                            new
                            {
                                PackagingId = packagingId,
                                RefProductId = (int?)null,
                                CustomerId = customerId,
                                UserId = userId,
                                Type = (int)PackagingTransactionType.ISSUE, // Với TINYINT, pass số nguyên
                                OwnershipType = (int)PackagingOwnershipType.DEPOSIT, // Với TINYINT, pass số nguyên
                                Quantity = quantity,
                                DepositPrice = depositPrice,
                                RefundAmount = 0m,
                                Notes = notes
                            },
                            transaction
                        );

                        if (affectedRows > 0)
                        {
                            transaction.Commit();
                            _logger.Info($"Đã phát hành bao bì - PackagingId: {packagingId}, Quantity: {quantity}, CustomerId: {customerId}");
                            return true;
                        }

                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Error($"IssuePackagingAsync error - PackagingId: {packagingId}, CustomerId: {customerId} - {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<bool> ReturnPackagingAsync(int packagingId, int customerId, int quantity,
                                                    decimal refundAmount, int? userId, string notes = null)
        {
            using (var connection = await _databaseHelper.CreateConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Kiểm tra khách hàng có đang giữ đủ số lượng không
                        var holdingQuantity = await GetCustomerHoldingQuantityAsync(customerId, packagingId);
                        if (holdingQuantity < quantity)
                        {
                            _logger.Warning($"Khách hàng không giữ đủ bao bì - CustomerId: {customerId}, PackagingId: {packagingId}, Holding: {holdingQuantity}, Required: {quantity}");
                            return false;
                        }

                        // 2. Chuyển bao bì về trạng thái cần vệ sinh
                        var returnSuccess = await _packagingInventoryRepository.ReturnForCleaningAsync(packagingId, quantity);
                        if (!returnSuccess)
                        {
                            _logger.Error($"Không thể nhận bao bì trả về - PackagingId: {packagingId}, Quantity: {quantity}");
                            transaction.Rollback();
                            return false;
                        }

                        // 3. Ghi nhận giao dịch thu hồi
                        var affectedRows = await connection.ExecuteAsync(
                            PackagingTransactionQueries.InsertTransaction,
                            new
                            {
                                PackagingId = packagingId,
                                RefProductId = (int?)null,
                                CustomerId = customerId,
                                UserId = userId,
                                Type = (int)PackagingTransactionType.RETURN, // Với TINYINT, pass số nguyên
                                OwnershipType = (int)PackagingOwnershipType.DEPOSIT, // Với TINYINT, pass số nguyên
                                Quantity = quantity,
                                DepositPrice = 0m,
                                RefundAmount = refundAmount,
                                Notes = notes
                            },
                            transaction
                        );

                        if (affectedRows > 0)
                        {
                            transaction.Commit();
                            _logger.Info($"Đã thu hồi bao bì - PackagingId: {packagingId}, Quantity: {quantity}, CustomerId: {customerId}");
                            return true;
                        }

                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Error($"ReturnPackagingAsync error - PackagingId: {packagingId}, CustomerId: {customerId} - {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<PackagingTransactionDetail>> GetTransactionDetailsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = PackagingTransactionQueries.GetTransactionDetails;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql += " AND pt.created_date BETWEEN @FromDate AND @ToDate";
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                sql += " ORDER BY pt.created_date DESC";

                return await _databaseHelper.QueryAsync<PackagingTransactionDetail>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTransactionDetailsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PackagingTransactionSummary>> GetTransactionSummaryByPackagingAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = PackagingTransactionQueries.GetTransactionSummaryByPackaging;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql = sql.Replace("WHERE 1=1", "WHERE pt.created_date BETWEEN @FromDate AND @ToDate");
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                return await _databaseHelper.QueryAsync<PackagingTransactionSummary>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTransactionSummaryByPackagingAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CustomerPackagingSummary>> GetTransactionSummaryByCustomerAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var sql = PackagingTransactionQueries.GetTransactionSummaryByCustomer;
                var parameters = new DynamicParameters();

                if (fromDate.HasValue && toDate.HasValue)
                {
                    sql = sql.Replace("WHERE c.is_active = TRUE", "WHERE c.is_active = TRUE AND pt.created_date BETWEEN @FromDate AND @ToDate");
                    parameters.Add("FromDate", fromDate.Value);
                    parameters.Add("ToDate", toDate.Value);
                }

                return await _databaseHelper.QueryAsync<CustomerPackagingSummary>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTransactionSummaryByCustomerAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalCustomerHoldingsAsync()
        {
            try
            {
                return await _databaseHelper.ExecuteScalarAsync<int>(
                    PackagingTransactionQueries.GetTotalCustomerHoldings
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalCustomerHoldingsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<PackagingTransaction> Transactions, int TotalCount)> GetPagedTransactionsAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            PackagingTransactionType? transactionType = null,
            int? customerId = null,
            int? packagingId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            try
            {
                var whereClause = "WHERE 1=1";
                var parameters = new DynamicParameters();

                // Search condition
                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClause += @" AND (p.name LIKE @Search OR c.name LIKE @Search OR p.barcode LIKE @Search)";
                    parameters.Add("Search", $"%{search}%");
                }

                // Transaction type filter
                if (transactionType.HasValue)
                {
                    whereClause += " AND pt.type = @TransactionType";
                    parameters.Add("TransactionType", (int)transactionType.Value); // Với TINYINT, pass số nguyên
                }

                // Customer filter
                if (customerId.HasValue)
                {
                    whereClause += " AND pt.customer_id = @CustomerId";
                    parameters.Add("CustomerId", customerId.Value);
                }

                // Packaging filter
                if (packagingId.HasValue)
                {
                    whereClause += " AND pt.packaging_id = @PackagingId";
                    parameters.Add("PackagingId", packagingId.Value);
                }

                // Date range filter
                if (fromDate.HasValue)
                {
                    whereClause += " AND pt.created_date >= @FromDate";
                    parameters.Add("FromDate", fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    whereClause += " AND pt.created_date <= @ToDate";
                    parameters.Add("ToDate", toDate.Value);
                }

                // Get total count
                var countSql = PackagingTransactionQueries.PagedTransactionsCount + " " + whereClause;
                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = PackagingTransactionQueries.PagedTransactionsBase + " " + whereClause +
                         " ORDER BY pt.created_date DESC LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var transactions = await _databaseHelper.QueryAsync<PackagingTransaction>(sql, parameters);
                return (transactions, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedTransactionsAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }
    }
}
