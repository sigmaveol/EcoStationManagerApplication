using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<Result<Customer>> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<Customer>("Khách hàng", customerId);

                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<Customer>("Khách hàng", customerId);

                return Result<Customer>.Ok(customer);
            }
            catch (Exception ex)
            {
                return HandleException<Customer>(ex, "lấy thông tin khách hàng");
            }
        }

        public async Task<Result<List<Customer>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetAllAsync();
                return Result<List<Customer>>.Ok(customers.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Customer>>(ex, "lấy danh sách khách hàng");
            }
        }

        public async Task<Result<List<Customer>>> GetActiveCustomersAsync()
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetAllAsync();
                var activeCustomers = customers.Where(c => c.IsActive).ToList();
                return Result<List<Customer>>.Ok(activeCustomers);
            }
            catch (Exception ex)
            {
                return HandleException<List<Customer>>(ex, "lấy danh sách khách hàng đang hoạt động");
            }
        }

        public async Task<Result<Customer>> CreateCustomerAsync(Customer customer)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateCustomer(customer);
                if (validationErrors.Any())
                    return ValidationError<Customer>(validationErrors);

                // Kiểm tra trùng số điện thoại
                if (!string.IsNullOrWhiteSpace(customer.Phone))
                {
                    var existingCustomer = await _unitOfWork.Customers.GetByPhoneAsync(customer.Phone);
                    if (existingCustomer != null)
                        return BusinessError<Customer>("Số điện thoại đã tồn tại trong hệ thống");
                }

                // Kiểm tra trùng email
                if (!string.IsNullOrWhiteSpace(customer.Email))
                {
                    var customers = await _unitOfWork.Customers.SearchAsync(customer.Email);
                    if (customers.Any(c => c.Email?.ToLower() == customer.Email.ToLower()))
                        return BusinessError<Customer>("Email đã tồn tại trong hệ thống");
                }

                // Set default values
                customer.TotalPoint = 0;
                customer.Rank = CustomerRank.MEMBER;
                customer.IsActive = true;
                customer.CreatedDate = DateTime.Now;

                // Tạo khách hàng mới
                var customerId = await _unitOfWork.Customers.AddAsync(customer);
                if (customerId <= 0)
                    return BusinessError<Customer>("Không thể tạo khách hàng mới");

                // Lấy thông tin khách hàng vừa tạo
                var createdCustomer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                return Result<Customer>.Ok(createdCustomer, "Đã tạo khách hàng mới thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Customer>(ex, "tạo khách hàng mới");
            }
        }

        public async Task<Result<Customer>> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                if (customer == null || customer.CustomerId <= 0)
                    return NotFoundError<Customer>("Khách hàng", customer?.CustomerId ?? 0);

                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateCustomer(customer);
                if (validationErrors.Any())
                    return ValidationError<Customer>(validationErrors);

                // Kiểm tra khách hàng tồn tại
                var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(customer.CustomerId);
                if (existingCustomer == null)
                    return NotFoundError<Customer>("Khách hàng", customer.CustomerId);

                // Kiểm tra trùng số điện thoại (trừ chính nó)
                if (!string.IsNullOrWhiteSpace(customer.Phone))
                {
                    var customerWithSamePhone = await _unitOfWork.Customers.GetByPhoneAsync(customer.Phone);
                    if (customerWithSamePhone != null && customerWithSamePhone.CustomerId != customer.CustomerId)
                        return BusinessError<Customer>("Số điện thoại đã được sử dụng bởi khách hàng khác");
                }

                // Kiểm tra trùng email (trừ chính nó)
                if (!string.IsNullOrWhiteSpace(customer.Email))
                {
                    var customers = await _unitOfWork.Customers.SearchAsync(customer.Email);
                    if (customers.Any(c => c.Email?.ToLower() == customer.Email.ToLower() && c.CustomerId != customer.CustomerId))
                        return BusinessError<Customer>("Email đã được sử dụng bởi khách hàng khác");
                }

                // Cập nhật thông tin
                var success = await _unitOfWork.Customers.UpdateAsync(customer);
                if (!success)
                    return BusinessError<Customer>("Không thể cập nhật thông tin khách hàng");

                // Lấy thông tin khách hàng đã cập nhật
                var updatedCustomer = await _unitOfWork.Customers.GetByIdAsync(customer.CustomerId);
                return Result<Customer>.Ok(updatedCustomer, "Đã cập nhật thông tin khách hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Customer>(ex, "cập nhật thông tin khách hàng");
            }
        }

        public async Task<Result<bool>> DeleteCustomerAsync(int customerId)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<bool>("Khách hàng", customerId);

                // Kiểm tra khách hàng tồn tại
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<bool>("Khách hàng", customerId);

                // Xóa khách hàng
                var success = await _unitOfWork.Customers.DeleteAsync(customerId);
                if (!success)
                    return BusinessError<bool>("Không thể xóa khách hàng");

                return Result<bool>.Ok(true, "Đã xóa khách hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa khách hàng");
            }
        }

        public async Task<Result<bool>> ToggleCustomerStatusAsync(int customerId, bool isActive)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<bool>("Khách hàng", customerId);

                // Kiểm tra khách hàng tồn tại
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<bool>("Khách hàng", customerId);

                var success = await _unitOfWork.Customers.ToggleActiveAsync(customerId, isActive);
                if (!success)
                    return BusinessError<bool>("Không thể thay đổi trạng thái khách hàng");

                var statusText = isActive ? "kích hoạt" : "vô hiệu hóa";
                return Result<bool>.Ok(true, $"Đã {statusText} khách hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "thay đổi trạng thái khách hàng");
            }
        }

        public async Task<Result<Customer>> GetCustomerByPhoneAsync(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                    return BusinessError<Customer>("Số điện thoại không được để trống");

                var customer = await _unitOfWork.Customers.GetByPhoneAsync(phone);
                if (customer == null)
                    return NotFoundError<Customer>($"Không tìm thấy khách hàng với số điện thoại {phone}");

                return Result<Customer>.Ok(customer);
            }
            catch (Exception ex)
            {
                return HandleException<Customer>(ex, "tìm khách hàng theo số điện thoại");
            }
        }

        public async Task<Result<List<Customer>>> SearchCustomersAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllCustomersAsync();

                var customers = await _unitOfWork.Customers.SearchAsync(keyword);
                return Result<List<Customer>>.Ok(customers.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Customer>>(ex, "tìm kiếm khách hàng");
            }
        }

        public async Task<Result<List<Customer>>> GetCustomersByRankAsync(CustomerRank rank)
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetByRankAsync(rank);
                return Result<List<Customer>>.Ok(customers.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Customer>>(ex, "lấy khách hàng theo hạng");
            }
        }

        public async Task<Result<bool>> UpdateCustomerPointsAsync(int customerId, int pointsToAdd)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<bool>("Khách hàng", customerId);

                // Kiểm tra khách hàng tồn tại
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<bool>("Khách hàng", customerId);

                var success = await _unitOfWork.Customers.UpdatePointsAsync(customerId, pointsToAdd);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật điểm tích lũy");

                var action = pointsToAdd >= 0 ? "cộng" : "trừ";
                var points = Math.Abs(pointsToAdd);
                return Result<bool>.Ok(true, $"Đã {action} {points} điểm tích lũy cho khách hàng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật điểm tích lũy");
            }
        }

        public async Task<Result<bool>> UpdateCustomerRankAsync(int customerId, CustomerRank newRank)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<bool>("Khách hàng", customerId);

                // Kiểm tra khách hàng tồn tại
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null)
                    return NotFoundError<bool>("Khách hàng", customerId);

                var success = await _unitOfWork.Customers.UpdateRankAsync(customerId, newRank);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật hạng khách hàng");

                return Result<bool>.Ok(true, $"Đã cập nhật hạng khách hàng thành {newRank}");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật hạng khách hàng");
            }
        }

        public async Task<Result<List<Customer>>> GetTopCustomersAsync(int limit = 10)
        {
            try
            {
                if (limit <= 0 || limit > 50)
                    return BusinessError<List<Customer>>("Số lượng khách hàng phải từ 1 đến 50");

                var topCustomers = await _unitOfWork.Customers.GetTopCustomersAsync(limit);
                return Result<List<Customer>>.Ok(topCustomers.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Customer>>(ex, "lấy top khách hàng");
            }
        }
    }
}
