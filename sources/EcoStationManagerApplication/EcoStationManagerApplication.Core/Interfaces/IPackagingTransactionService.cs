using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
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
        Task<Result<List<PackagingTransaction>>> GetTransactionsByCustomerAsync(int customerId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<Result<List<PackagingTransaction>>> GetTransactionsByPackagingAsync(int packagingId);
        Task<Result<PackagingTransaction>> IssuePackagingAsync(int packagingId, int? customerId, int quantity, decimal depositPrice, int? userId, PackagingOwnershipType ownershipType, int? refProductId, string notes = null);
        Task<Result<PackagingTransaction>> ReturnPackagingAsync(int packagingId, int customerId, int quantity, decimal refundAmount, int? userId, PackagingOwnershipType ownershipType, int? refProductId, string notes = null);
        Task<Result<int>> GetCustomerHoldingQuantityAsync(int customerId, int packagingId);
        Task<Result<List<CustomerHoldingPackaging>>> GetCustomerHoldingsAsync(int customerId);
        Task<Result<decimal>> GetTotalDepositAmountAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<decimal>> GetTotalRefundAmountAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
