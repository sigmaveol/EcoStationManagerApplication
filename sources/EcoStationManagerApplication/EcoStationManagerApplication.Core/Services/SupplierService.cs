using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
            : base("SupplierService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Supplier>> GetSupplierByIdAsync(int supplierId)
        {
            try
            {
                if (supplierId <= 0)
                    return NotFoundError<Supplier>("Nhà cung cấp", supplierId);

                var supplier = await _unitOfWork.Suppliers.GetByIdAsync(supplierId);
                if (supplier == null)
                    return NotFoundError<Supplier>("Nhà cung cấp", supplierId);

                return Result<Supplier>.Ok(supplier);
            }
            catch (Exception ex)
            {
                return HandleException<Supplier>(ex, "lấy thông tin nhà cung cấp");
            }
        }

        public async Task<Result<List<Supplier>>> GetAllSuppliersAsync()
        {
            try
            {
                var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
                return Result<List<Supplier>>.Ok(suppliers.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Supplier>>(ex, "lấy danh sách nhà cung cấp");
            }
        }

        public async Task<Result<Supplier>> CreateSupplierAsync(Supplier supplier)
        {
            try
            {
                // Validate dữ liệu
                var validationResult = await ValidateSupplierAsync(supplier);
                if (!validationResult.Success)
                    return Result<Supplier>.Fail(validationResult.Message);

                // Kiểm tra email trùng
                if (!string.IsNullOrWhiteSpace(supplier.Email))
                {
                    var emailExistsResult = await IsEmailExistsAsync(supplier.Email);
                    if (emailExistsResult.Success && emailExistsResult.Data)
                        return BusinessError<Supplier>($"Email '{supplier.Email}' đã tồn tại");
                }

                // Kiểm tra số điện thoại trùng
                if (!string.IsNullOrWhiteSpace(supplier.Phone))
                {
                    var phoneExistsResult = await IsPhoneExistsAsync(supplier.Phone);
                    if (phoneExistsResult.Success && phoneExistsResult.Data)
                        return BusinessError<Supplier>($"Số điện thoại '{supplier.Phone}' đã tồn tại");
                }

                // Set default values
                supplier.CreatedAt = DateTime.Now;

                // Tạo nhà cung cấp mới
                var supplierId = await _unitOfWork.Suppliers.AddAsync(supplier);
                if (supplierId <= 0)
                    return BusinessError<Supplier>("Không thể tạo nhà cung cấp mới");

                // Lấy thông tin nhà cung cấp vừa tạo
                var createdSupplier = await _unitOfWork.Suppliers.GetByIdAsync(supplierId);
                return Result<Supplier>.Ok(createdSupplier, "Đã tạo nhà cung cấp mới thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Supplier>(ex, "tạo nhà cung cấp mới");
            }
        }

        public async Task<Result<Supplier>> UpdateSupplierAsync(Supplier supplier)
        {
            try
            {
                if (supplier == null || supplier.SupplierId <= 0)
                    return NotFoundError<Supplier>("Nhà cung cấp", supplier?.SupplierId ?? 0);

                // Validate dữ liệu
                var validationResult = await ValidateSupplierAsync(supplier);
                if (!validationResult.Success)
                    return Result<Supplier>.Fail(validationResult.Message);

                // Kiểm tra nhà cung cấp tồn tại
                var existingSupplier = await _unitOfWork.Suppliers.GetByIdAsync(supplier.SupplierId);
                if (existingSupplier == null)
                    return NotFoundError<Supplier>("Nhà cung cấp", supplier.SupplierId);

                // Kiểm tra email trùng (trừ chính nó)
                if (!string.IsNullOrWhiteSpace(supplier.Email))
                {
                    var emailExistsResult = await IsEmailExistsAsync(supplier.Email, supplier.SupplierId);
                    if (emailExistsResult.Success && emailExistsResult.Data)
                        return BusinessError<Supplier>($"Email '{supplier.Email}' đã được sử dụng bởi nhà cung cấp khác");
                }

                // Kiểm tra số điện thoại trùng (trừ chính nó)
                if (!string.IsNullOrWhiteSpace(supplier.Phone))
                {
                    var phoneExistsResult = await IsPhoneExistsAsync(supplier.Phone, supplier.SupplierId);
                    if (phoneExistsResult.Success && phoneExistsResult.Data)
                        return BusinessError<Supplier>($"Số điện thoại '{supplier.Phone}' đã được sử dụng bởi nhà cung cấp khác");
                }

                // Cập nhật nhà cung cấp
                var success = await _unitOfWork.Suppliers.UpdateAsync(supplier);
                if (!success)
                    return BusinessError<Supplier>("Không thể cập nhật thông tin nhà cung cấp");

                // Lấy thông tin nhà cung cấp đã cập nhật
                var updatedSupplier = await _unitOfWork.Suppliers.GetByIdAsync(supplier.SupplierId);
                return Result<Supplier>.Ok(updatedSupplier, "Đã cập nhật thông tin nhà cung cấp thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Supplier>(ex, "cập nhật nhà cung cấp");
            }
        }

        public async Task<Result<bool>> DeleteSupplierAsync(int supplierId)
        {
            try
            {
                if (supplierId <= 0)
                    return NotFoundError<bool>("Nhà cung cấp", supplierId);

                // Kiểm tra nhà cung cấp tồn tại
                var supplier = await _unitOfWork.Suppliers.GetByIdAsync(supplierId);
                if (supplier == null)
                    return NotFoundError<bool>("Nhà cung cấp", supplierId);

                // TODO: Kiểm tra xem nhà cung cấp có đang được sử dụng trong StockIn không

                // Xóa nhà cung cấp
                var success = await _unitOfWork.Suppliers.DeleteAsync(supplierId);
                if (!success)
                    return BusinessError<bool>("Không thể xóa nhà cung cấp");

                return Result<bool>.Ok(true, "Đã xóa nhà cung cấp thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa nhà cung cấp");
            }
        }

        public async Task<Result<List<Supplier>>> SearchSuppliersAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllSuppliersAsync();

                var suppliers = await _unitOfWork.Suppliers.SearchAsync(keyword);
                return Result<List<Supplier>>.Ok(suppliers.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Supplier>>(ex, "tìm kiếm nhà cung cấp");
            }
        }

        public async Task<Result<List<Supplier>>> GetSuppliersByContactPersonAsync(string contactPerson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contactPerson))
                    return BusinessError<List<Supplier>>("Tên người liên hệ không được để trống");

                var suppliers = await _unitOfWork.Suppliers.GetByContactPersonAsync(contactPerson);
                return Result<List<Supplier>>.Ok(suppliers.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Supplier>>(ex, "lấy nhà cung cấp theo người liên hệ");
            }
        }

        public async Task<Result<bool>> ValidateSupplierAsync(Supplier supplier)
        {
            try
            {
                if (supplier == null)
                    return BusinessError<bool>("Thông tin nhà cung cấp không được để trống");

                if (string.IsNullOrWhiteSpace(supplier.Name))
                    return BusinessError<bool>("Tên nhà cung cấp không được để trống");

                if (supplier.Name.Length > 150)
                    return BusinessError<bool>("Tên nhà cung cấp không được vượt quá 150 ký tự");

                if (!string.IsNullOrWhiteSpace(supplier.Email) && !ValidationHelper.IsValidEmail(supplier.Email))
                    return BusinessError<bool>("Email không hợp lệ");

                if (!string.IsNullOrWhiteSpace(supplier.Phone) && !ValidationHelper.IsValidPhone(supplier.Phone))
                    return BusinessError<bool>("Số điện thoại không hợp lệ");

                return Result<bool>.Ok(true, "Nhà cung cấp hợp lệ");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "validate nhà cung cấp");
            }
        }

        public async Task<Result<bool>> IsEmailExistsAsync(string email, int? excludeSupplierId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return BusinessError<bool>("Email không được để trống");

                var exists = await _unitOfWork.Suppliers.IsEmailExistsAsync(email, excludeSupplierId);
                return Result<bool>.Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra email tồn tại");
            }
        }

        public async Task<Result<bool>> IsPhoneExistsAsync(string phone, int? excludeSupplierId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                    return BusinessError<bool>("Số điện thoại không được để trống");

                var exists = await _unitOfWork.Suppliers.IsPhoneExistsAsync(phone, excludeSupplierId);
                return Result<bool>.Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra số điện thoại tồn tại");
            }
        }
    }

}
