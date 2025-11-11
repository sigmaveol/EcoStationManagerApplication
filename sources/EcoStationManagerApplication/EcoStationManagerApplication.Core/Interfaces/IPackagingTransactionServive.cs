using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IPackagingTransactionService
    {
        Task<Result<PackagingTransaction>> GetTransactionByIdAsync(int transactionId);
        Task<Result<List<PackagingTransaction>>> GetTransactionsByCustomerAsync(int customerId);
        Task<Result<List<PackagingTransaction>>> GetTransactionsByPackagingAsync(int packagingId);
        Task<Result<PackagingTransaction>> IssuePackagingAsync(int packagingId, int? customerId, int quantity, decimal depositPrice, int? userId, string notes = null);
        Task<Result<PackagingTransaction>> ReturnPackagingAsync(int packagingId, int customerId, int quantity, decimal refundAmount, int? userId, string notes = null);
        Task<Result<int>> GetCustomerHoldingQuantityAsync(int customerId, int packagingId);
        Task<Result<List<CustomerHoldingPackaging>>> GetCustomerHoldingsAsync(int customerId);
        Task<Result<decimal>> GetTotalDepositAmountAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<decimal>> GetTotalRefundAmountAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
