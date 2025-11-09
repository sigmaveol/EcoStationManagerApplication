using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IPackagingTransactionRepository : IRepository<PackagingTransaction>
    {
        /// <summary>
        /// Lấy lịch sử giao dịch theo bao bì
        /// </summary>
        Task<IEnumerable<PackagingTransaction>> GetByPackagingAsync(int packagingId);

        /// <summary>
        /// Lấy lịch sử giao dịch theo khách hàng
        /// </summary>
        Task<IEnumerable<PackagingTransaction>> GetByCustomerAsync(int customerId);

        /// <summary>
        /// Lấy lịch sử giao dịch theo loại (ISSUE/RETURN)
        /// </summary>
        Task<IEnumerable<PackagingTransaction>> GetByTypeAsync(PackagingTransactionType type);

        /// <summary>
        /// Lấy lịch sử giao dịch theo khoảng thời gian
        /// </summary>
        Task<IEnumerable<PackagingTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy tổng tiền ký quỹ
        /// </summary>
        Task<decimal> GetTotalDepositAmountAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy tổng tiền hoàn trả
        /// </summary>
        Task<decimal> GetTotalRefundAmountAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy số lượng bao bì đang được khách hàng giữ
        /// </summary>
        Task<int> GetCustomerHoldingQuantityAsync(int customerId, int packagingId);

        /// <summary>
        /// Lấy tất cả bao bì khách hàng đang giữ
        /// </summary>
        Task<IEnumerable<CustomerHoldingPackaging>> GetCustomerHoldingsAsync(int customerId);

        /// <summary>
        /// Kiểm tra khách hàng có đang giữ bao bì không
        /// </summary>
        Task<bool> IsCustomerHoldingPackagingAsync(int customerId, int packagingId);

        /// <summary>
        /// Phát hành bao bì cho khách hàng
        /// </summary>
        Task<bool> IssuePackagingAsync(int packagingId, int? customerId, int quantity,
                                      decimal depositPrice, int? userId, string notes = null);

        /// <summary>
        /// Thu hồi bao bì từ khách hàng
        /// </summary>
        Task<bool> ReturnPackagingAsync(int packagingId, int customerId, int quantity,
                                       decimal refundAmount, int? userId, string notes = null);

        /// <summary>
        /// Lấy lịch sử giao dịch chi tiết với thông tin đầy đủ
        /// </summary>
        Task<IEnumerable<PackagingTransactionDetail>> GetTransactionDetailsAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Thống kê giao dịch theo bao bì
        /// </summary>
        Task<IEnumerable<PackagingTransactionSummary>> GetTransactionSummaryByPackagingAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Thống kê giao dịch theo khách hàng
        /// </summary>
        Task<IEnumerable<CustomerPackagingSummary>> GetTransactionSummaryByCustomerAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy tổng số bao bì đang được khách hàng giữ
        /// </summary>
        Task<int> GetTotalCustomerHoldingsAsync();

        /// <summary>
        /// Phân trang giao dịch
        /// </summary>
        Task<(IEnumerable<PackagingTransaction> Transactions, int TotalCount)> GetPagedTransactionsAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            PackagingTransactionType? transactionType = null,
            int? customerId = null,
            int? packagingId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null
        );
    }
}
