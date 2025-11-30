# Sơ đồ Sequence (Mermaid)

Tài liệu này tổng hợp các sơ đồ sequence chính của hệ thống. Dán các khối mã Mermaid vào trình xem Markdown hỗ trợ Mermaid để hiển thị sơ đồ.

## Đăng nhập người dùng
```mermaid
sequenceDiagram
    actor User as Người dùng
    participant UI as Giao diện (LoginForm)
    participant US as UserService
    participant DB as Database

    User ->> UI: Nhập username/password
    UI ->> US: AuthenticateAsync(username, password)
    US ->> DB: GetByUsername(username)
    DB -->> US: User entity
    US ->> US: Hash & so sánh mật khẩu
    alt Xác thực thành công
        US ->> US: EnsureWorkShiftForToday(userId)
        US -->> UI: Result<UserDTO>
        UI ->> UI: AppStateManager.CurrentUser = UserDTO
        UI -->> User: Hiển thị thành công, chuyển dashboard
    else Xác thực thất bại
        US -->> UI: Lỗi xác thực
        UI -->> User: Hiển thị lỗi đăng nhập
    end
```

Tham chiếu mã: `EcoStationManagerApplication.Core/Services/UserService.cs:27`, `EcoStationManagerApplication.UI/Forms/LoginForm.cs:213`, `EcoStationManagerApplication.DAL/Repositories/UserRepository.cs:22`.

## Thêm đơn hàng
```mermaid
sequenceDiagram
    actor Staff as Nhân viên
    participant UI as Giao diện đơn hàng
    participant OC as OrderControl
    participant OS as OrderService
    participant DB as Database

    Staff ->> UI: Mở form/thêm đơn hàng, nhập thông tin
    UI ->> OC: Submit đơn hàng (data)
    OC ->> OS: CreateOrderAsync(order, orderDetails)

    OS ->> DB: BEGIN TRANSACTION
    OS ->> DB: Insert Orders (trả về order_id)
    DB -->> OS: order_id
    OS ->> DB: Insert OrderDetails (mỗi sản phẩm)
    OS ->> OS: Tính tổng tiền, trạng thái
    OS ->> DB: Update Orders (total_amount, status)
    OS ->> DB: COMMIT

    alt Tạo đơn thành công
        OS -->> OC: Result<order_id>
        OC -->> UI: Cập nhật danh sách, hiển thị success
        UI -->> Staff: Thêm đơn hàng thành công
    else Lỗi tạo đơn
        OS -->> OC: Result.Fail(message)
        OC -->> UI: Hiển thị lỗi
        UI -->> Staff: Thất bại (thông báo lỗi)
    end
```

Tham chiếu mã: `EcoStationManagerApplication.Core/Services/OrderService.cs:274`, `EcoStationManagerApplication.DAL/UnitOfWork/UnitOfWork .cs:48`, `EcoStationManagerApplication.UI/Controls/OrdersControl.cs:603`.

## Import đơn hàng từ Excel
```mermaid
sequenceDiagram
    actor Staff as Nhân viên
    participant UI as Giao diện đơn hàng
    participant OC as OrdersControl
    participant IS as ImportService
    participant OS as OrderService
    participant DB as Database

    Staff ->> UI: Nhấn "Import Excel"
    UI ->> OC: btnImportExcel_Click(filePath)
    OC ->> IS: ImportOrdersFromExcelTemplateAsync(filePath)
    IS ->> IS: Đọc Excel, map cột, nhóm theo OrderCode
    loop Cho mỗi nhóm đơn hàng
        IS ->> OS: CreateOrderAsync(order, orderDetails)
        OS ->> DB: BEGIN TRANSACTION
        OS ->> DB: Insert Orders
        DB -->> OS: order_id
        OS ->> DB: Insert OrderDetails
        OS ->> OS: Tính tổng tiền
        OS ->> DB: Update Orders (total_amount)
        OS ->> DB: COMMIT
        OS -->> IS: Result(order_id)
    end
    IS -->> OC: ImportResult(successCount, errorCount, errors[])
    OC -->> UI: Hiển thị kết quả và refresh danh sách
    UI -->> Staff: Báo cáo import (thành công/lỗi)
```

Tham chiếu mã: `EcoStationManagerApplication.UI/Controls/OrdersControl.cs:524`, `EcoStationManagerApplication.Core/Services/ImportService.cs:70`, `EcoStationManagerApplication.Core/Services/OrderService.cs:209`.

## Cập nhật trạng thái/thanh toán đơn hàng
```mermaid
sequenceDiagram
    actor Staff as Nhân viên
    participant UI as Giao diện đơn hàng
    participant UF as UpdateOrderForm
    participant OS as OrderService
    participant DB as Database

    Staff ->> UI: Mở form cập nhật đơn
    UI ->> UF: LoadOrderAsync(orderId)
    UF ->> UF: Chọn trạng thái và thanh toán mới
    alt Có thay đổi trạng thái
        UF ->> OS: UpdateOrderStatusAsync(orderId, newStatus)
        OS ->> DB: Update Orders.status
        OS -->> UF: Result
    end
    alt Có thay đổi thanh toán
        UF ->> OS: UpdatePaymentStatusAsync(orderId, newPaymentStatus)
        OS ->> DB: Update Orders.payment_status
        OS -->> UF: Result
    end
    UF -->> UI: Thông báo thành công, đóng form
    UI ->> OC: Refresh danh sách đơn hàng
```

Tham chiếu mã: `EcoStationManagerApplication.UI/Forms/UpdateOrderForm.cs:105`, `EcoStationManagerApplication.Core/Services/OrderService.cs:314`, `EcoStationManagerApplication.Core/Services/OrderService.cs:366`, `EcoStationManagerApplication.UI/Controls/OrdersControl.cs:729`.

## Xóa đơn hàng
```mermaid
sequenceDiagram
    actor Staff as Nhân viên
    participant UI as Giao diện đơn hàng
    participant OC as OrdersControl
    participant OS as OrderService
    participant DB as Database

    Staff ->> UI: Nhấn "Xóa" tại dòng đơn
    UI ->> OC: DeleteOrder(orderId) + xác nhận
    OC ->> OS: DeleteOrderAsync(orderId)
    OS ->> DB: BEGIN TRANSACTION (nếu khả dụng)
    OS ->> DB: Delete OrderDetails by orderId
    OS ->> DB: Delete Orders
    OS ->> DB: COMMIT
    OS -->> OC: Result(success/fail)
    OC -->> UI: Hiển thị thông báo, refresh danh sách
    UI -->> Staff: Đã xóa đơn hàng
```

Tham chiếu mã: `EcoStationManagerApplication.UI/Controls/OrdersControl.cs:743`, `EcoStationManagerApplication.Core/Services/OrderService.cs:609`.

## Import đơn hàng từ Excel
```mermaid
sequenceDiagram
    actor Staff as Nhân viên
    participant UI as Giao diện đơn hàng
    participant IS as ImportService
    participant OS as OrderService
    participant DB as Database

    Staff ->> UI: Chọn file Excel và nhấn Import
    UI ->> IS: ImportOrdersFromExcelTemplateAsync(filePath)
    IS ->> IS: Đọc file, map cột, gom nhóm theo OrderCode
    loop Mỗi đơn hàng trong file
        IS ->> OS: CreateOrderAsync(order, orderDetails)
        OS ->> DB: BEGIN TRANSACTION
        OS ->> DB: Insert Orders
        OS ->> DB: Insert OrderDetails
        OS ->> OS: Tính tổng tiền
        OS ->> DB: Update Orders (total_amount)
        OS ->> DB: COMMIT
        OS -->> IS: Result
    end
    IS -->> UI: ImportResult (SuccessCount, ErrorCount, CreatedOrderIds)
    UI -->> Staff: Hiển thị kết quả import
```

Tham chiếu mã: `EcoStationManagerApplication.UI/Controls/OrdersControl.cs:524`, `EcoStationManagerApplication.Core/Services/ImportService.cs:216`, `EcoStationManagerApplication.Core/Services/ImportService.cs:70`, `EcoStationManagerApplication.Core/Services/OrderService.cs:209`.

## Cập nhật đơn hàng (trạng thái & thanh toán)
```mermaid
sequenceDiagram
    actor Staff as Nhân viên
    participant UI as UpdateOrderForm
    participant OS as OrderService
    participant DB as Database

    Staff ->> UI: Mở form cập nhật đơn hàng
    UI ->> OS: UpdateOrderStatusAsync(orderId, newStatus)
    OS ->> DB: GetById(orderId)
    alt Hủy đơn hàng
        OS ->> DB: RollbackStockForOrderAsync(orderId)
        OS -->> UI: Kết quả hoàn trả tồn kho
    else Thay đổi trạng thái khác
        OS ->> DB: UpdateOrderStatus(orderId, newStatus)
    end
    UI ->> OS: UpdatePaymentStatusAsync(orderId, newPaymentStatus)
    OS ->> DB: UpdatePaymentStatus(orderId, newPaymentStatus)
    OS -->> UI: Kết quả cập nhật
    UI -->> Staff: Thông báo thành công / lỗi
```

Tham chiếu mã: `EcoStationManagerApplication.UI/Forms/UpdateOrderForm.cs:105`, `EcoStationManagerApplication.Core/Services/OrderService.cs:314`, `EcoStationManagerApplication.Core/Services/OrderService.cs:366`, `EcoStationManagerApplication.Core/Services/OrderService.cs:593`.

## Xóa đơn hàng
```mermaid
sequenceDiagram
    actor Staff as Nhân viên
    participant UI as Giao diện đơn hàng
    participant OS as OrderService
    participant DB as Database

    Staff ->> UI: Nhấn "Xóa" trên danh sách đơn hàng
    UI ->> OS: DeleteOrderAsync(orderId)
    OS ->> DB: GetById(orderId)
    alt Hợp lệ để xóa
        OS ->> DB: BEGIN TRANSACTION
        OS ->> DB: Delete OrderDetails (by orderId)
        OS ->> DB: Delete Orders (by orderId)
        OS ->> DB: COMMIT
        OS -->> UI: Result.Ok
        UI -->> Staff: Thông báo xóa thành công
    else Không hợp lệ / không tìm thấy
        OS -->> UI: Result.Fail(message)
        UI -->> Staff: Hiển thị lỗi
    end
```

Tham chiếu mã: `EcoStationManagerApplication.UI/Controls/OrdersControl.cs:743`, `EcoStationManagerApplication.Core/Services/OrderService.cs:609`, `EcoStationManagerApplication.Core/Services/OrderService.cs:642`.
