# Hướng dẫn cài đặt và sử dụng FastReport.NET

## Cài đặt FastReport.NET

### Cách 1: Sử dụng NuGet Package Manager
1. Mở Visual Studio
2. Right-click vào project `EcoStationManagerApplication.UI`
3. Chọn "Manage NuGet Packages..."
4. Tìm kiếm: `FastReport.OpenSource` hoặc `FastReport.Net`
5. Click "Install"

### Cách 2: Sử dụng Package Manager Console
```powershell
Install-Package FastReport.OpenSource
```

### Cách 3: Sử dụng .NET CLI
```bash
dotnet add package FastReport.OpenSource
```

## Cấu trúc thư mục Templates

Sau khi cài đặt, tạo thư mục cho report templates:
```
EcoStationManagerApplication.UI/
  └── Reports/
      └── Templates/
          ├── RevenueReport.frx
          ├── OrderReport.frx
          ├── CustomerReport.frx
          ├── PackagingReport.frx
          └── EnvironmentalReport.frx
```

## Kích hoạt FastReport trong Code

Sau khi cài đặt package, cần uncomment các dòng code trong:

1. **ReportsControl.cs**:
   - Uncomment các dòng `using FastReport.*;`
   - Uncomment code trong các method `ExportToPDF` và `ExportToExcel`

2. **FastReportHelper.cs**:
   - Uncomment tất cả các dòng code đã được comment
   - Implement các method với FastReport API

## Tạo Report Templates (.frx)

### Sử dụng FastReport Designer:

1. Mở FastReport Designer (có trong package)
2. Tạo report mới
3. Thiết kế layout với:
   - Header: Tiêu đề báo cáo, ngày tạo
   - Data Band: Hiển thị dữ liệu từ DataTable
   - Footer: Tổng kết, số trang
4. Lưu file với tên tương ứng (ví dụ: `RevenueReport.frx`)
5. Copy vào thư mục `Reports/Templates/`

### Cấu trúc DataTable cho từng loại báo cáo:

#### RevenueReport
- Columns: `Period`, `Revenue`, `OrderCount`
- Parameters: `ReportTitle`, `FromDate`, `ToDate`, `GeneratedDate`

#### CustomerReport
- Columns: `CustomerName`, `Phone`, `TotalOrders`, `ReturnCount`, `ReturnFrequency`
- Parameters: `ReportTitle`, `FromDate`, `ToDate`, `GeneratedDate`

#### PackagingReport
- Columns: `PackagingName`, `Issued`, `Returned`, `RecoveryRate`
- Parameters: `ReportTitle`, `FromDate`, `ToDate`, `GeneratedDate`

#### EnvironmentalReport
- Columns: `Period`, `Refills`, `PlasticSavedKg`, `CO2SavedKg`
- Parameters: `ReportTitle`, `FromDate`, `ToDate`, `GeneratedDate`

## Các tính năng đã được implement

✅ **UI Structure**:
- Sidebar với danh sách loại báo cáo
- Filter Panel với filters động
- Report Viewer với stats cards, charts, và data grid

✅ **Dynamic Filters**:
- Filters tự động thay đổi theo loại báo cáo
- Quick filter cho date range
- Custom filters cho từng loại báo cáo

✅ **Export Functions**:
- PDF export (cần FastReport)
- Excel export (cần FastReport)

## Lưu ý

- FastReport.OpenSource là phiên bản miễn phí với một số giới hạn
- FastReport.Net là phiên bản thương mại với đầy đủ tính năng
- Nếu không muốn dùng FastReport, có thể sử dụng PDFsharp và ClosedXML đã có sẵn trong project

## Alternative: Sử dụng PDFsharp và ClosedXML

Nếu không muốn cài FastReport, có thể implement export bằng:
- **PDFsharp** (đã có trong packages.config) cho PDF
- **ClosedXML** (đã có trong packages.config) cho Excel

Cần update code trong `ExportToPDF` và `ExportToExcel` methods.

