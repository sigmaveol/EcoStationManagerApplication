# EcoStationManagerApplication

## Yêu cầu môi trường
- Windows, .NET Framework 4.7.2
- Visual Studio 2019+ (khuyến nghị 2022)
- MySQL Server 8.x

## Khôi phục và build
1. Mở `EcoStationManagerApplication.sln` bằng Visual Studio.
2. NuGet Restore: Tools → NuGet Package Manager → Restore.
3. Cấu hình build: AnyCPU, Prefer 32-bit = false (đã cấu hình sẵn).
4. Build Solution (Debug).

## Cấu hình kết nối CSDL
- File cấu hình JSON sẽ tự tạo lần đầu: `appsettings.json` (cùng thư mục chạy).
- Nếu chưa tồn tại, chạy app 1 lần để sinh file, sau đó chỉnh:
  - Server, Database, UserId, Password, Port…
- Kết nối được dùng ở `Common\Config\ConfigManager.cs` và `ConnectionHelper.cs`.

## Khởi tạo dữ liệu
1. Tạo schema/tables:
   - `sources/EcoStationManagerApplication/EcoStationManager_V0.sql`
   - (hoặc) `migration.sql`
2. Thêm thủ tục/hàm/kích hoạt nếu có:
   - `procedures.txt`, `triggers.txt`
3. Seed dữ liệu mẫu:
   - `seed.sql`

## Chạy ứng dụng
- Project khởi động: `EcoStationManagerApplication.UI`
- Lần đầu chạy sẽ sinh `appsettings.json`. Chỉnh thông số DB rồi chạy lại.
- UI đã tách DAL: UI truy cập service qua `UI\Common\AppServices.cs` → Core `ServiceRegistry`.

## Ghi chú tương thích
- Binding redirects đã bật (tự sinh) và được bổ sung cho UI.
- Các gói đã cố định phiên bản tương thích .NET 4.7.2 (ví dụ: ConfigurationManager 6.0.0).

## Kiểm thử
- Project: `EcoStationManagerApplication.Tests` (MSTest). Nên bổ sung test cho Service quan trọng.

## Đóng gói
- Dùng project `Setup` để tạo installer; đảm bảo kèm dependencies cần thiết.