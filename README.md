# EcoStationManagerApplication

## Giới thiệu
- Ứng dụng quản lý trạm nhiên liệu (Eco Station) với kiến trúc phân lớp: `UI`, `Core`, `DAL`, `Common`, `Models`, `Tests`.
- Sử dụng MySQL và Dapper, tương thích `.NET Framework 4.7.2`.

## Yêu cầu
- Windows, `.NET Framework 4.7.2`.
- Visual Studio 2019 trở lên (khuyến nghị 2022).
- MySQL Server 8.x.

## Cài đặt
- Mở solution: `sources/EcoStationManagerApplication/EcoStationManagerApplication.sln` (cũng có bản sao tại root: `EcoStationManagerApplication.sln`).
- Khôi phục gói: Visual Studio → Tools → NuGet Package Manager → Restore.
- Cấu hình build: `AnyCPU`, tắt `Prefer 32-bit` (đã cấu hình sẵn).
- Build toàn bộ solution ở cấu hình `Debug` để kiểm tra.

## Cấu hình
- File cấu hình `appsettings.json` sẽ được tự tạo ở thư mục chạy nếu chưa tồn tại.
- Vị trí điển hình: `sources/EcoStationManagerApplication/appsettings.json`.
- Các khóa quan trọng trong `Database` cần chỉnh cho phù hợp môi trường:

```
{
  "Database": {
    "Server": "localhost",
    "Database": "EcoStationManager",
    "UserId": "root",
    "Password": "<mật_khẩu>",
    "Port": 3306,
    "ConnectionTimeout": 30,
    "CommandTimeout": 120,
    "AllowZeroDateTime": false,
    "ConvertZeroDateTime": true,
    "MaxPoolSize": 100,
    "MinPoolSize": 1
  }
}
```

- Chuỗi kết nối được tạo từ cấu hình bởi `Common\Config\ConfigManager.cs` → `GetConnectionString()`.
- Có thể kiểm tra kết nối bằng `Common\Config\ConnectionHelper.cs` → `TestConnection()` trong quá trình phát triển.

## Cơ sở dữ liệu
- Khởi tạo schema/tables:
  - `sources/EcoStationManagerApplication/EcoStationManager_V0.sql`
  - Cập nhật tiếp: `sources/EcoStationManagerApplication/EcoStationManager_V1.sql`
- Tạo tài khoản quản trị: xem `sources/EcoStationManagerApplication/HUONG_DAN_TAO_ADMIN.md` hoặc chạy `sources/EcoStationManagerApplication/create_admin_user.sql`.
- Nếu cần thêm dữ liệu mẫu, tạo script seed tùy nhu cầu.

## Chạy ứng dụng
- Project khởi động: `sources/EcoStationManagerApplication/EcoStationManagerApplication.UI`.
- Lần đầu chạy sẽ sinh `appsettings.json`; cập nhật thông số DB rồi chạy lại.
- Logging được lưu tại `sources/EcoStationManagerApplication/logs/` khi ở môi trường Development.

## Kiểm thử
- Dự án kiểm thử: `sources/EcoStationManagerApplication/EcoStationManagerApplication.Tests` (MSTest).
- Chạy nhanh bằng CLI: `dotnet test` (hoặc chạy Test Explorer trong VS).
- Kết quả từng test case được tổng hợp tại `sources/EcoStationManagerApplication/EcoStationManagerApplication.Tests/TestResults.md`.

## Đóng gói
- Sử dụng project `sources/EcoStationManagerApplication/Setup` để tạo installer.
- Đảm bảo cài đặt máy đích có `.NET Framework 4.7.2` và MySQL client phù hợp.

## Khắc phục sự cố
- Lỗi kết nối DB: xác nhận thông số `Server`, `Database`, `UserId`, `Password`, `Port` trong `appsettings.json` và thử `TestConnection()`.
- Lỗi parse enum khi truy vấn: kiểm tra dữ liệu trong DB khớp với enum được định nghĩa trong `Models\Enums\Enums.cs`.
