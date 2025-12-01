// File: CustomerService.cs
using EcoStationManagerApplication.Common.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
            : base("CustomerService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<Customer>>> SearchCustomersAsync(string searchTerm)
        {
            try
            {
                // Nếu searchTerm rỗng, repository sẽ tự động trả về tất cả khách hàng
                var customers = await _unitOfWork.Customers.SearchAsync(searchTerm ?? "");
                
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    return Result<IEnumerable<Customer>>.Ok(customers, "Lấy danh sách khách hàng thành công");
                }
                else
                {
                    return Result<IEnumerable<Customer>>.Ok(customers, "Tìm kiếm khách hàng thành công");
                }
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Customer>>(ex, "tìm kiếm khách hàng");
            }
        }

        public async Task<Result<Customer>> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                if (customerId <= 0)
                    return Result<Customer>.Fail("ID khách hàng không hợp lệ");

                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<Customer>("Khách hàng", customerId);

                return Result<Customer>.Ok(customer, "Lấy thông tin khách hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Customer>(ex, "lấy thông tin khách hàng");
            }
        }

        public async Task<Result<Customer>> GetCustomerByCodeAsync(string customerCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerCode))
                    return Result<Customer>.Fail("Mã khách hàng không được để trống");

                var customer = await _unitOfWork.Customers.GetByCustomerCode(customerCode);
                if (customer == null)
                    return Result<Customer>.Fail($"Không tìm thấy khách hàng với mã: {customerCode}");

                return Result<Customer>.Ok(customer, "Lấy thông tin khách hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Customer>(ex, "lấy thông tin khách hàng theo mã");
            }
        }

        public async Task<Result<IEnumerable<Customer>>> GetCustomersByRankAsync(CustomerRank rank)
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetByRankAsync(rank);
                return Result<IEnumerable<Customer>>.Ok(customers, $"Lấy danh sách khách hàng theo rank {rank} thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Customer>>(ex, "lấy khách hàng theo rank");
            }
        }

        public async Task<Result<int>> CreateCustomerAsync(Customer customer)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidateCustomer(customer);
                if (validationErrors.Any())
                    return ValidationError<int>(validationErrors);

                // Kiểm tra mã khách hàng trùng
                var isCodeExists = await _unitOfWork.Customers.IsCodeExistsAsync(customer.CustomerCode);
                if (isCodeExists)
                    return Result<int>.Fail($"Mã khách hàng '{customer.CustomerCode}' đã tồn tại");

                // Kiểm tra số điện thoại trùng (nếu có)
                if (!string.IsNullOrWhiteSpace(customer.Phone))
                {
                    var isPhoneExists = await _unitOfWork.Customers.IsPhoneExistsAsync(customer.Phone);
                    if (isPhoneExists)
                        return Result<int>.Fail($"Số điện thoại '{customer.Phone}' đã tồn tại");
                }

                // Set giá trị mặc định
                customer.IsActive = ActiveStatus.ACTIVE;
                customer.CreatedDate = DateTime.Now;

                // Thêm khách hàng
                var customerId = await _unitOfWork.Customers.AddAsync(customer);
                _logger.Info($"Đã tạo khách hàng mới: {customer.Name} (ID: {customerId}, Code: {customer.CustomerCode})");

                return Result<int>.Ok(customerId, $"Thêm khách hàng '{customer.Name}' thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "thêm khách hàng");
            }
        }

        public async Task<Result<bool>> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidateCustomer(customer);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra tồn tại
                var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(customer.CustomerId);
                if (existingCustomer == null)
                    return NotFoundError<bool>("Khách hàng", customer.CustomerId);

                // Kiểm tra mã khách hàng trùng (trừ chính nó)
                var isCodeExists = await _unitOfWork.Customers.IsCodeExistsAsync(customer.CustomerCode, customer.CustomerId);
                if (isCodeExists)
                    return Result<bool>.Fail($"Mã khách hàng '{customer.CustomerCode}' đã tồn tại");

                // Kiểm tra số điện thoại trùng (trừ chính nó)
                if (!string.IsNullOrWhiteSpace(customer.Phone))
                {
                    var isPhoneExists = await _unitOfWork.Customers.IsPhoneExistsAsync(customer.Phone, customer.CustomerId);
                    if (isPhoneExists)
                        return Result<bool>.Fail($"Số điện thoại '{customer.Phone}' đã tồn tại");
                }

                // Cập nhật
                var success = await _unitOfWork.Customers.UpdateAsync(customer);
                if (success)
                {
                    _logger.Info($"Đã cập nhật khách hàng: {customer.Name} (ID: {customer.CustomerId})");
                    return Result<bool>.Ok(true, $"Cập nhật khách hàng '{customer.Name}' thành công");
                }

                return Result<bool>.Fail("Cập nhật khách hàng thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật khách hàng");
            }
        }

        public async Task<Result<bool>> UpdateCustomerPointsAsync(int customerId, int points)
        {
            try
            {
                if (customerId <= 0)
                    return Result<bool>.Fail("ID khách hàng không hợp lệ");

                if (points < 0)
                    return Result<bool>.Fail("Điểm tích lũy không được âm");

                // Kiểm tra tồn tại
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<bool>("Khách hàng", customerId);

                var success = await _unitOfWork.Customers.UpdatePointsAsync(customerId, points);
                if (success)
                {
                    _logger.Info($"Đã cập nhật điểm khách hàng ID {customerId}: {points} điểm");
                    return Result<bool>.Ok(true, $"Cập nhật điểm tích lũy thành công: {points} điểm");
                }

                return Result<bool>.Fail("Cập nhật điểm tích lũy thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật điểm khách hàng");
            }
        }

        public async Task<Result<bool>> AddCustomerPointsAsync(int customerId, int points)
        {
            try
            {
                if (customerId <= 0)
                    return Result<bool>.Fail("ID khách hàng không hợp lệ");

                if (points <= 0)
                    return Result<bool>.Fail("Số điểm thêm phải lớn hơn 0");

                // Kiểm tra tồn tại
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<bool>("Khách hàng", customerId);

                var newPoints = customer.TotalPoint + points;
                var success = await _unitOfWork.Customers.UpdatePointsAsync(customerId, newPoints);
                if (success)
                {
                    _logger.Info($"Đã thêm điểm khách hàng ID {customerId}: +{points} điểm (Tổng: {newPoints})");
                    return Result<bool>.Ok(true, $"Đã thêm {points} điểm tích lũy");
                }

                return Result<bool>.Fail("Thêm điểm tích lũy thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "thêm điểm khách hàng");
            }
        }

        public async Task<Result<bool>> UpdateCustomerRankAsync(int customerId, CustomerRank rank)
        {
            try
            {
                if (customerId <= 0)
                    return Result<bool>.Fail("ID khách hàng không hợp lệ");

                // Kiểm tra tồn tại
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<bool>("Khách hàng", customerId);

                var success = await _unitOfWork.Customers.UpdateRankAsync(customerId, rank);
                if (success)
                {
                    _logger.Info($"Đã cập nhật rank khách hàng ID {customerId}: {rank}");
                    return Result<bool>.Ok(true, $"Cập nhật rank thành công: {rank}");
                }

                return Result<bool>.Fail("Cập nhật rank thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật rank khách hàng");
            }
        }

        public async Task<Result<bool>> CheckPhoneNumberExistsAsync(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    return Result<bool>.Fail("Số điện thoại không được để trống");

                var exists = await _unitOfWork.Customers.IsPhoneExistsAsync(phoneNumber);
                return Result<bool>.Ok(exists, exists ? "Số điện thoại đã tồn tại" : "Số điện thoại có thể sử dụng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra số điện thoại");
            }
        }

        public async Task<Result<bool>> CheckCustomerCodeExistsAsync(string customerCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerCode))
                    return Result<bool>.Fail("Mã khách hàng không được để trống");

                var exists = await _unitOfWork.Customers.IsCodeExistsAsync(customerCode);
                return Result<bool>.Ok(exists, exists ? "Mã khách hàng đã tồn tại" : "Mã khách hàng có thể sử dụng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra mã khách hàng");
            }
        }

        private List<string> ValidateCustomer(Customer customer)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(customer.CustomerCode))
                errors.Add("Mã khách hàng không được để trống");

            if (customer.CustomerCode?.Length > 50)
                errors.Add("Mã khách hàng không được vượt quá 50 ký tự");

            if (string.IsNullOrWhiteSpace(customer.Name))
                errors.Add("Tên khách hàng không được để trống");

            if (customer.Name?.Length > 255)
                errors.Add("Tên khách hàng không được vượt quá 255 ký tự");

            if (customer.Phone?.Length > 50)
                errors.Add("Số điện thoại không được vượt quá 50 ký tự");

            if (customer.TotalPoint < 0)
                errors.Add("Điểm tích lũy không được âm");

            return errors;
        }
    }
}