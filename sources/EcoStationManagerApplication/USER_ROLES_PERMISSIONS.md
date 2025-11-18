# Phân Quyền Người Dùng - User Roles và Permissions

## Tổng Quan

Hệ thống có **4 vai trò người dùng** (UserRole) với các quyền truy cập khác nhau:

1. **ADMIN** - Quản trị viên
2. **STAFF** - Nhân viên  
3. **MANAGER** - Quản lý trạm
4. **DRIVER** - Tài xế

## Chi Tiết Permissions cho Từng Role

### 1. ADMIN (Quản trị viên)
**Mô tả:** Toàn quyền truy cập hệ thống, có thể quản lý tất cả tính năng và người dùng

**Permissions:**
- ✅ `order_manage` - Quản lý đơn hàng
- ✅ `inventory_view` - Xem tồn kho
- ✅ `inventory_edit` - Chỉnh sửa tồn kho
- ✅ `report_view` - Xem báo cáo
- ✅ `settings_edit` - Cài đặt hệ thống
- ✅ `user_manage` - Quản lý người dùng
- ✅ `product_manage` - Quản lý sản phẩm & Bao bì
- ✅ `customer_manage` - Quản lý khách hàng
- ✅ `stock_in` - Nhập kho
- ✅ `stock_out` - Xuất kho
- ✅ `payment_manage` - Quản lý thanh toán

**Tính năng có thể truy cập:**
- Dashboard
- Quản lý đơn hàng
- Sản phẩm & Bao bì
- Nhà cung cấp
- Khách hàng
- Tồn Kho (xem và chỉnh sửa)
- Nhập Kho
- Xuất Kho
- Thanh Toán
- Báo cáo
- Cài đặt hệ thống
- Quản lý người dùng

---

### 2. MANAGER (Quản lý trạm)
**Mô tả:** Quản lý hoạt động trạm, có thể quản lý đơn hàng, kho, sản phẩm, khách hàng và xem báo cáo

**Permissions:**
- ✅ `order_manage` - Quản lý đơn hàng
- ✅ `inventory_view` - Xem tồn kho
- ✅ `inventory_edit` - Chỉnh sửa tồn kho
- ✅ `report_view` - Xem báo cáo
- ✅ `product_manage` - Quản lý sản phẩm & Bao bì
- ✅ `customer_manage` - Quản lý khách hàng
- ✅ `stock_in` - Nhập kho
- ✅ `stock_out` - Xuất kho
- ✅ `payment_manage` - Quản lý thanh toán
- ❌ `settings_edit` - Cài đặt hệ thống
- ❌ `user_manage` - Quản lý người dùng

**Tính năng có thể truy cập:**
- Dashboard
- Quản lý đơn hàng
- Sản phẩm & Bao bì
- Nhà cung cấp
- Khách hàng
- Tồn Kho (xem và chỉnh sửa)
- Nhập Kho
- Xuất Kho
- Thanh Toán
- Báo cáo
- ❌ Cài đặt hệ thống (không có quyền)
- ❌ Quản lý người dùng (không có quyền)

---

### 3. STAFF (Nhân viên)
**Mô tả:** Thao tác cơ bản: quản lý đơn hàng, xem kho, quản lý sản phẩm, khách hàng, xuất kho và thanh toán

**Permissions:**
- ✅ `order_manage` - Quản lý đơn hàng
- ✅ `inventory_view` - Xem tồn kho
- ✅ `product_manage` - Quản lý sản phẩm & Bao bì
- ✅ `customer_manage` - Quản lý khách hàng
- ✅ `stock_out` - Xuất kho
- ✅ `payment_manage` - Quản lý thanh toán
- ❌ `inventory_edit` - Chỉnh sửa tồn kho
- ❌ `report_view` - Xem báo cáo
- ❌ `settings_edit` - Cài đặt hệ thống
- ❌ `user_manage` - Quản lý người dùng
- ❌ `stock_in` - Nhập kho

**Tính năng có thể truy cập:**
- Dashboard
- Quản lý đơn hàng
- Sản phẩm & Bao bì
- Nhà cung cấp
- Khách hàng
- Tồn Kho (chỉ xem, không chỉnh sửa)
- Xuất Kho
- Thanh Toán
- ❌ Nhập Kho (không có quyền)
- ❌ Báo cáo (không có quyền)
- ❌ Cài đặt hệ thống (không có quyền)
- ❌ Quản lý người dùng (không có quyền)

---

### 4. DRIVER (Tài xế)
**Mô tả:** Xem đơn hàng, xem kho và xem báo cáo để thực hiện giao hàng

**Permissions:**
- ✅ `order_manage` - Quản lý đơn hàng
- ✅ `inventory_view` - Xem tồn kho
- ✅ `report_view` - Xem báo cáo
- ❌ `inventory_edit` - Chỉnh sửa tồn kho
- ❌ `settings_edit` - Cài đặt hệ thống
- ❌ `user_manage` - Quản lý người dùng
- ❌ `product_manage` - Quản lý sản phẩm & Bao bì
- ❌ `customer_manage` - Quản lý khách hàng
- ❌ `stock_in` - Nhập kho
- ❌ `stock_out` - Xuất kho
- ❌ `payment_manage` - Quản lý thanh toán

**Tính năng có thể truy cập:**
- Dashboard
- Quản lý đơn hàng
- Tồn Kho (chỉ xem)
- Báo cáo
- ❌ Sản phẩm & Bao bì (không có quyền)
- ❌ Nhà cung cấp (không có quyền)
- ❌ Khách hàng (không có quyền)
- ❌ Nhập Kho (không có quyền)
- ❌ Xuất Kho (không có quyền)
- ❌ Thanh Toán (không có quyền)
- ❌ Cài đặt hệ thống (không có quyền)
- ❌ Quản lý người dùng (không có quyền)

---

## Cách Tạo Tài Khoản Nhân Viên

### Trong Ứng Dụng:
1. Đăng nhập với tài khoản **ADMIN**
2. Vào **Cài đặt hệ thống** → Tab **Quản lý người dùng**
3. Click nút **"Thêm người dùng"**
4. Điền thông tin:
   - **Tên đăng nhập** (username): Tối thiểu 3 ký tự
   - **Email**: (tùy chọn)
   - **Vai trò**: Chọn một trong 4 vai trò (ADMIN, STAFF, MANAGER, DRIVER)
5. Nhập **mật khẩu**: Tối thiểu 6 ký tự
6. Click **"Lưu"** để tạo tài khoản

### Mặc Định:
- Khi tạo tài khoản mới, vai trò mặc định là **STAFF** (Nhân viên)
- Tài khoản sẽ được kích hoạt tự động (`is_active = TRUE`)

## Lưu Ý

- Chỉ có **ADMIN** mới có quyền tạo và quản lý người dùng
- Mật khẩu phải có ít nhất 6 ký tự (trừ admin đầu tiên)
- Tên đăng nhập phải duy nhất trong hệ thống
- Permissions được tự động gán dựa trên vai trò được chọn

## Code Reference

- **RolePermissionHelper**: `EcoStationManagerApplication.Common.Helpers.RolePermissionHelper`
- **UserRole Enum**: `EcoStationManagerApplication.Models.Enums.UserRole`
- **UserService**: `EcoStationManagerApplication.Core.Services.UserService`

