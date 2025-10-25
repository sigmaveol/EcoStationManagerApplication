using EcoStationManagerApplication.Core.Exceptions;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class PackagingService : BaseService<Packaging>, IPackagingService
    {
        private readonly IPackagingRepository _packagingRepository;
        private readonly IPackagingTransactionRepository _packagingTransactionRepository;

        public PackagingService(
            IPackagingRepository packagingRepository,
            IPackagingTransactionRepository packagingTransactionRepository)
            : base(packagingRepository)
        {
            _packagingRepository = packagingRepository;
            _packagingTransactionRepository = packagingTransactionRepository;
        }

        public async Task<Packaging> GetPackagingByQrCodeAsync(string qrCode)
        {
            if (string.IsNullOrWhiteSpace(qrCode))
                throw new ArgumentException("QR Code không được để trống", nameof(qrCode));

            return await _packagingRepository.GetPackagingByQrCodeAsync(qrCode);
        }

        public async Task<IEnumerable<Packaging>> GetPackagingsByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status không được để trống", nameof(status));

            return await _packagingRepository.GetPackagingsByStatusAsync(status);
        }

        public async Task<bool> UpdatePackagingStatusAsync(int packagingId, string status)
        {
            if (packagingId <= 0)
                throw new ArgumentException("Packaging ID phải lớn hơn 0", nameof(packagingId));

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status không được để trống", nameof(status));

            var packaging = await _packagingRepository.GetByIdAsync(packagingId);
            if (packaging == null)
                throw new NotFoundException($"Bao bì với ID {packagingId} không tồn tại");

            packaging.Status = status;
            await _packagingRepository.UpdateAsync(packaging);

            return true;
        }

        public async Task<int> GetReuseCountAsync(int packagingId)
        {
            if (packagingId <= 0)
                throw new ArgumentException("Packaging ID phải lớn hơn 0", nameof(packagingId));

            return await _packagingRepository.GetReuseCountAsync(packagingId);
        }

        public async Task<PackagingTransaction> IssuePackagingAsync(int customerId, int stationId, int packagingId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Customer ID phải lớn hơn 0", nameof(customerId));

            if (stationId <= 0)
                throw new ArgumentException("Station ID phải lớn hơn 0", nameof(stationId));

            if (packagingId <= 0)
                throw new ArgumentException("Packaging ID phải lớn hơn 0", nameof(packagingId));

            // Kiểm tra packaging tồn tại
            var packaging = await _packagingRepository.GetByIdAsync(packagingId);
            if (packaging == null)
                throw new NotFoundException($"Bao bì với ID {packagingId} không tồn tại");

            // Tạo transaction
            var transaction = new PackagingTransaction
            {
                StationId = stationId,
                CustomerId = customerId,
                PackagingId = packagingId,
                Type = "issue",
                PointsEarned = 0, // Có thể tính điểm dựa trên chính sách
                Date = DateTime.Now,
                Quantity = 1
            };

            // Cập nhật trạng thái packaging
            packaging.Status = "inuse";
            packaging.TimeReused += 1;
            await _packagingRepository.UpdateAsync(packaging);

            return await _packagingTransactionRepository.AddAsync(transaction);
        }

        public async Task<PackagingTransaction> ReturnPackagingAsync(int customerId, int stationId, int packagingId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Customer ID phải lớn hơn 0", nameof(customerId));

            if (stationId <= 0)
                throw new ArgumentException("Station ID phải lớn hơn 0", nameof(stationId));

            if (packagingId <= 0)
                throw new ArgumentException("Packaging ID phải lớn hơn 0", nameof(packagingId));

            // Kiểm tra packaging tồn tại
            var packaging = await _packagingRepository.GetByIdAsync(packagingId);
            if (packaging == null)
                throw new NotFoundException($"Bao bì với ID {packagingId} không tồn tại");

            // Tạo transaction
            var transaction = new PackagingTransaction
            {
                StationId = stationId,
                CustomerId = customerId,
                PackagingId = packagingId,
                Type = "return",
                PointsEarned = 10, // Ví dụ: trả lại được 10 điểm
                Date = DateTime.Now,
                Quantity = 1
            };

            // Cập nhật trạng thái packaging
            packaging.Status = "needcleaning"; // Sau khi trả lại cần vệ sinh
            await _packagingRepository.UpdateAsync(packaging);

            return await _packagingTransactionRepository.AddAsync(transaction);
        }

        public async Task<int> CalculatePackagingPointsAsync(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Customer ID phải lớn hơn 0", nameof(customerId));

            var transactions = await _packagingTransactionRepository.GetTransactionsByCustomerAsync(customerId);
            int totalPoints = 0;

            foreach (var transaction in transactions)
            {
                totalPoints += transaction.PointsEarned;
            }

            return totalPoints;
        }

        protected override async Task ValidateEntityAsync(Packaging packaging)
        {
            if (packaging == null)
                throw new ArgumentNullException(nameof(packaging));

            if (string.IsNullOrWhiteSpace(packaging.Type))
                throw new ValidationException("Loại bao bì là bắt buộc");

            if (packaging.TimeReused < 0)
                throw new ValidationException("Số lần tái sử dụng không được âm");
        }
    }

    // Các Service khác như TankService, CleaningService có thể triển khai tương tự
}