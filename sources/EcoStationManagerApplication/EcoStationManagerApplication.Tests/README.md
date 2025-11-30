# Hướng dẫn Unit Test chi tiết cho EcoStationManagerApplication

Tài liệu chi tiết cách viết, tổ chức và chạy Unit Test cho toàn bộ phần mềm, sử dụng MSTest V2 trên .NET Framework 4.7.2.

## Kiến trúc và phạm vi kiểm thử
- `Models`: DTO, Entities, Enums, `Results` (`Result`, `OperationStatus`, `PagedResult`).
- `Common`: `Exporters` (Excel/PDF), `Helpers` (JSON, Security), `Utilities` (Format), `Logging`.
- `DAL`: `Repositories`, `SqlQueries`, `DatabaseHelper`, `UnitOfWork`.
- `Core`: `Services` nghiệp vụ, `Helpers` (Validation, Mapping), `Composition` (ServiceRegistry).
- `UI`: Controls/Forms WinForms; chỉ kiểm thử phần logic thuần (không UI), đặc biệt `BackupControl` và `ReportControlHelpers`.

## Framework và cấu trúc dự án test
- Framework kiểm thử: MSTest V2 (`Microsoft.VisualStudio.TestPlatform.TestFramework`, `MSTest.TestAdapter`).
- Project test: `EcoStationManagerApplication.Tests` (TargetFramework: `v4.7.2`).
- Vị trí: `sources/EcoStationManagerApplication/EcoStationManagerApplication.Tests/`.
- Quy ước tên: `ClassNameTests.cs`, method: `Method_Should_Behavior_When_Condition`.
- Vòng đời test: `[ClassInitialize]`, `[TestInitialize]`, `[TestCleanup]`, `TestContext` để log.

## Hướng dẫn theo từng tầng

### Models
- Kiểm thử DTO mapping cơ bản và bất biến dữ liệu.
- Kiểm thử `Results`:
  - `Result<T>` trả `Success/Message/Data` đúng khi `Success=true/false`.
  - `PagedResult<T>` trả `Items/TotalCount/PageSize/PageIndex` nhất quán.
- Tham chiếu:
  - `EcoStationManagerApplication.Models/Results/Result.cs`
  - `EcoStationManagerApplication.Models/Results/PagedResult.cs`

Ví dụ test `Result`:
```csharp
[TestMethod]
public void Result_Should_Carry_Data_When_Success()
{
    var r = new EcoStationManagerApplication.Models.Results.Result<int>(true, 42, null);
    Assert.IsTrue(r.Success);
    Assert.AreEqual(42, r.Data);
}
```

### Common
- Excel: kiểm `ExportMultipleSheets` tạo đủ sheet, header và tiêu đề.
  - `EcoStationManagerApplication.Common/Exporters/ExcelExporter.cs:499`.
- PDF: kiểm `ExportMultipleSections` tạo đủ section với header.
  - `EcoStationManagerApplication.Common/Exporters/PdfExporter.cs:338`.
- JSON: kiểm `Serialize/Deserialize` round-trip.
  - `EcoStationManagerApplication.Common/Helpers/JsonHelper.cs:15`, `:27`.
- Format: kiểm `FormatCurrency`, `FormatQuantity`, `FormatDate` cho input biên.
  - `EcoStationManagerApplication.Common/Utilities/FormatHelper.cs:14`, `:30`, `:46`.

Ví dụ JSON:
```csharp
[TestMethod]
public void JsonHelper_RoundTrip_Should_Preserve_Data()
{
    var obj = new { A = 1, B = "x" };
    var json = EcoStationManagerApplication.Common.Helpers.JsonHelper.Serialize(obj);
    var back = EcoStationManagerApplication.Common.Helpers.JsonHelper.Deserialize<dynamic>(json);
    Assert.AreEqual(1, (int)back["A"]);
}
```

### DAL
- `DatabaseHelper`: không kết nối DB thật trong unit test. Dùng fake `IDbConnection` hoặc kiểm thử logic xử lý ngoại lệ (khai thác try/catch).
  - Phương thức: `QueryAsync`, `ExecuteAsync`, `ExecuteScalarAsync` như tham chiếu `EcoStationManagerApplication.DAL/Database/DatabaseHelper.cs:270`, `:343`, `:429`.
- `UnitOfWork`: kiểm `Begin/Commit/Rollback` gọi đúng chuỗi khi dùng transaction fake.
  - `EcoStationManagerApplication.DAL/UnitOfWork/UnitOfWork .cs:107`, `:127`, `:149`.
- `Repositories`: kiểm `OrderRepository/ProductRepository/...` bằng cách giả lập `IDatabaseHelper` trả dữ liệu tĩnh, xác nhận mapping và tham số.

Ví dụ fake đơn giản cho `IDatabaseHelper`:
```csharp
class FakeDb : EcoStationManagerApplication.DAL.Interfaces.IDatabaseHelper
{
    public Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, IDbTransaction tx = null)
        => Task.FromResult((IEnumerable<T>)new[] { default(T) });
    public Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction tx = null)
        => Task.FromResult(1);
    // triển khai các method khác nếu cần
}
```

### Core
- Validation: kiểm tất cả rule chính cho `Order`, `OrderDetail`, `StockIn/Out`, `User`.
  - `EcoStationManagerApplication.Core/Helpers/ValidationHelper.cs:131` (Order), `:94` (OrderDetail), `:276` (StockIn), `:304` (StockOut), `:320` (User).
- Mapping: kiểm `MapToOrderDTO`, `MapToOrderDetailDTO`, `MapToUserDTO`.
  - `EcoStationManagerApplication.Core/Helpers/MappingHelper.cs:14`, `:34`, `:218`.
- Services: kiểm `OrderDetailService.GetOrderDetailsByOrderAsync` trả `Result` đúng khi repo trả dữ liệu/exception.
  - `EcoStationManagerApplication.Core/Services/OrderDetailService.cs:44`.
- `PackagingInventoryService.GetAllAsync/GetPackagingInventoryAsync` cho happy path và lỗi.
  - `EcoStationManagerApplication.Core/Services/PackagingInventoryService.cs:27`, `:50`.
- `ExportService.GetOrdersForExportAsync` với filter và date-range.
  - `EcoStationManagerApplication.Core/Services/ExportService.cs:27`, `:107`.

Ví dụ Validation:
```csharp
[TestMethod]
public void ValidateOrder_Should_Fail_When_Note_TooLong()
{
    var order = new EcoStationManagerApplication.Models.Entities.Order { Note = new string('x', 2000) };
    var errors = EcoStationManagerApplication.Core.Helpers.ValidationHelper.ValidateOrder(order);
    Assert.IsTrue(errors.Any());
}
```

### UI (logic thuần)
- `BackupControl`: kiểm `BackupExcelDataAsync` tạo đủ sheet/headers và `BackupPdfDataAsync` tạo đủ sections.
  - `EcoStationManagerApplication.UI/Controls/BackupControl.cs:204`, `:286`.
  - Builders: `BuildOrderDetailsDataTableAsync` (`:783`), `BuildPackagingInventoryDataTable` (`:815`).
  - Headers: `GetOrderDetailColumnHeaders` (`:840`), `GetPackagingInventoryColumnHeaders` (`:853`).
- `ReportControlHelpers`: kiểm các helper, ví dụ `GetOrderSourceName`.
  - `EcoStationManagerApplication.UI/Controls/ReportControlHelpers.cs:68`.
- Tránh kiểm thử WinForms rendering; tập trung dữ liệu và định dạng.

## Mẫu kiểm thử Excel/PDF (gián tiếp)
- Dùng file tạm (`Path.GetTempFileName()`), gọi hàm, xác nhận nội dung và xóa file.
- Excel đọc bằng `DocumentFormat.OpenXml.Packaging.SpreadsheetDocument`.
- PDF nên xác nhận dữ liệu đầu vào tới exporter (ở unit), không parse PDF.

## Test bất đồng bộ
- Viết test `async Task`, dùng `await` để tránh deadlock.
- Có thể dùng `[Timeout]` để tránh treo nếu cần.

## Dựng dữ liệu giả (không dùng thư viện mocking)
- Tạo fake cho `IUnitOfWork`, `IDatabaseHelper`, các repository/service.
- Tiêm fake vào dịch vụ cần test qua constructor.

## Chạy test
- Visual Studio: `Test > Test Explorer > Run All`.
- Dòng lệnh Windows (điều chỉnh đường dẫn VS):
```
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" ^
  sources\EcoStationManagerApplication\EcoStationManagerApplication.Tests\bin\Debug\EcoStationManagerApplication.Tests.dll
```

## Coverage (khuyến nghị)
- Cân nhắc chuyển các project sang SDK-style để dùng `coverlet.collector`.
- Trước mắt, đảm bảo phạm vi test bao phủ các rule nghiệp vụ trọng yếu.

## Nguyên tắc chất lượng
- Mỗi sửa lỗi đi kèm một test tái hiện.
- Test độc lập, dọn dẹp tài nguyên tạm.
- Tránh phụ thuộc thời gian/chung trạng thái.

## Lộ trình mở rộng
- Viết test cho builder dữ liệu và headers của backup.
- Viết test cho `ValidationHelper` trên tập case biên.
- Viết test cho `MappingHelper` với dữ liệu đầy đủ `Order` và `OrderDetail`.
- Viết test cho `ExportService` với nhiều filter.

---
Có thể bổ sung thêm lớp test mẫu cho từng tầng nếu bạn muốn chạy thử ngay trong `EcoStationManagerApplication.Tests`.
