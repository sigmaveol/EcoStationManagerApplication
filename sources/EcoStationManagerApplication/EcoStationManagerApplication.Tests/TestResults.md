# Kết quả chạy Unit Test

- Thời gian: ${DATE_TIME}
- Môi trường: `dotnet 9.0.304` trên Windows
- Dự án test: `sources/EcoStationManagerApplication/EcoStationManagerApplication.Tests/EcoStationManagerApplication.Tests.csproj`
- Lệnh chạy: `dotnet test` (build thành công, mã thoát `0`)

## Tổng quan
-
- Build thành công và chạy test không lỗi (exit code `0`).
- Số lượng test: 78 test methods trên 27 file.

## Bao phủ theo nhóm dịch vụ
-
- Kho sản phẩm: InventoryService (thêm tồn, giảm tồn, kiểm tra đủ hàng)
- Danh mục & sản phẩm: CategoryService, ProductService, liên kết Category→Product
- Bao bì: PackagingService, PackagingInventoryService, PackagingTransactionService
- Đơn hàng & chi tiết: OrderService, OrderDetailService
- Xuất/nhập kho: StockInService, StockOutService
- Báo cáo & export: ReportService, ExportService
- Người dùng/ca làm: UserService, WorkShiftService
- Trạm/giao hàng: StationService, DeliveryService
- Lịch vệ sinh: CleaningScheduleService
- Khách hàng/nhà cung cấp: CustomerService, SupplierService
- Import: ImportService
- Thông báo: NotificationService
- Sao lưu CSDL: DatabaseBackupService (nhánh lỗi khôi phục)

## Danh sách file test và số lượng test methods
-
```
CoreDatabaseBackupServiceTests.cs: 1
ImportServiceMoqTests.cs: 2
SupplierServiceMoqTests.cs: 2
PackagingTransactionServiceMoqTests.cs: 1
CategoryProductServiceMoqTests.cs: 3
CustomerServiceMoqTests.cs: 2
OrderDetailServiceMoqTests.cs: 2
ExportServiceMoqTests.cs: 1
PackagingServiceMoqTests.cs: 2
InventoryServiceMoqTests.cs: 4
ReportServiceMoqTests.cs: 6
OrderServiceMoqTests.cs: 3
NotificationServiceMoqTests.cs: 1
DeliveryServiceMoqTests.cs: 3
StationServiceMoqTests.cs: 3
PackagingInventoryServiceMoqTests.cs: 4
UserServiceMoqTests.cs: 4
WorkShiftServiceMoqTests.cs: 4
StockOutServiceMoqTests.cs: 4
StockInServiceMoqTests.cs: 4
CleaningScheduleServiceMoqTests.cs: 5
ProductServiceMoqTests.cs: 4
CategoryServiceMoqTests.cs: 4
CommonPdfExporterTests.cs: 1
CommonExcelExporterTests.cs: 1
CommonJsonHelperTests.cs: 1
CommonFormatHelperTests.cs: 6
```

## Ghi chú thực thi
-
- `dotnet test` trả về mã thoát `0` → tất cả test đã chạy thành công trong môi trường hiện tại.
- Logger `.trx` và file chẩn đoán chưa được ghi ra do giới hạn môi trường; có thể thiết lập đường dẫn đầu ra tùy ý để xuất báo cáo chi tiết nếu cần.

## Đề xuất tiếp theo
-
- Bổ sung test biên cho Report (khoảng thời gian rỗng, dữ liệu null) và Order (xóa, đổi trạng thái thanh toán, phân trang) nếu muốn tăng độ chắc chắn.

## Kết quả từng test case
-
- CoreDatabaseBackupServiceTests.RestoreFromSqlFileAsync_FileNotFound_Throws — Passed
- ImportServiceMoqTests.ImportOrders_NonExistingFile_Fail — Passed
- ImportServiceMoqTests.ImportOrders_WrongExtension_Fail — Passed
- SupplierServiceMoqTests.GetByContact_Empty_Fail — Passed
- SupplierServiceMoqTests.GetByContact_Success — Passed
- PackagingTransactionServiceMoqTests.IssuePackaging_NotEnough_Fail — Passed
- CategoryProductServiceMoqTests.Category_Create_DuplicateName_Fail — Passed
- CategoryProductServiceMoqTests.Product_Create_InvalidSku_Duplicate_Fail — Passed
- CategoryProductServiceMoqTests.Product_GetByCategory_ChecksCategoryExists — Passed
- CustomerServiceMoqTests.Create_DuplicatePhone_Fail — Passed
- CustomerServiceMoqTests.UpdatePoints_Invalid_Fail — Passed
- OrderDetailServiceMoqTests.AddRange_InvalidOrderStatus_Fail — Passed
- OrderDetailServiceMoqTests.AddRange_InsufficientStock_Fail — Passed
- ExportServiceMoqTests.GetOrdersForExport_FilterCompleted_MapsCustomerName — Passed
- PackagingServiceMoqTests.GetByBarcode_Empty_Fail — Passed
- PackagingServiceMoqTests.DeletePackaging_InUseOrHasStock_Fail — Passed
- InventoryServiceMoqTests.AddStock_InvalidInputs_Fail — Passed
- InventoryServiceMoqTests.AddStock_Valid_Success — Passed
- InventoryServiceMoqTests.ReduceStock_Insufficient_Fail — Passed
- InventoryServiceMoqTests.IsStockSufficient_Success — Passed
- ReportServiceMoqTests.GetRevenueReport_Day_Aggregates — Passed
- ReportServiceMoqTests.GetPackagingRecoveryReport_Aggregates — Passed
- ReportServiceMoqTests.GetRevenueReport_Week_Aggregates — Passed
- ReportServiceMoqTests.GetRevenueReport_Month_Aggregates — Passed
- ReportServiceMoqTests.GetEnvironmentalImpactReport_Aggregates — Passed
- ReportServiceMoqTests.GetCustomerReturnReport_Aggregates — Passed
- OrderServiceMoqTests.CreateOrder_Computes_Total_Success — Passed
- OrderServiceMoqTests.UpdateOrderStatus_NotFound_Fails — Passed
- OrderServiceMoqTests.AddOrderDetails_InvalidStatus_Fails — Passed
- NotificationServiceMoqTests.GenerateAutoNotifications_LowStock_AddsNotifications — Passed
- DeliveryServiceMoqTests.Create_Invalid_Fails — Passed
- DeliveryServiceMoqTests.UpdateStatus_Ok — Passed
- DeliveryServiceMoqTests.GetTotalCOD_Ok — Passed
- StationServiceMoqTests.CreateStation_NameExists_Fails — Passed
- StationServiceMoqTests.UpdateStation_NotFound_Fails — Passed
- StationServiceMoqTests.ToggleStatus_Ok — Passed
- PackagingInventoryServiceMoqTests.UpdateQuantities_Invalid_Fails — Passed
- PackagingInventoryServiceMoqTests.UpdateQuantities_Valid_Ok — Passed
- PackagingInventoryServiceMoqTests.TransferToInUse_Sufficient_Ok — Passed
- PackagingInventoryServiceMoqTests.MarkAsDamaged_NotEnough_Fails — Passed
- UserServiceMoqTests.Authenticate_Success — Passed
- UserServiceMoqTests.Authenticate_Inactive_Fails — Passed
- UserServiceMoqTests.ChangePassword_WrongCurrent_Fails — Passed
- UserServiceMoqTests.ToggleUserStatus_NotFound_Fails — Passed
- WorkShiftServiceMoqTests.Create_Duplicate_Fails — Passed
- WorkShiftServiceMoqTests.Update_NotFound_Fails — Passed
- WorkShiftServiceMoqTests.UpdateStartTime_Ok — Passed
- WorkShiftServiceMoqTests.CalculateKPI_Updates_Shift — Passed
- StockOutServiceMoqTests.Create_Product_Insufficient_Fails — Passed
- StockOutServiceMoqTests.Create_Product_Success — Passed
- StockOutServiceMoqTests.Create_Packaging_Success — Passed
- StockOutServiceMoqTests.StockOutForOrder_Valid_Ok — Passed
- StockInServiceMoqTests.Create_Product_Path_Success — Passed
- StockInServiceMoqTests.Create_Packaging_Path_Success — Passed
- StockInServiceMoqTests.Create_BatchExists_Fails — Passed
- StockInServiceMoqTests.CreateMultiple_Valid_All_Success — Passed
- CleaningScheduleServiceMoqTests.GetAll_Ok — Passed
- CleaningScheduleServiceMoqTests.GetById_Invalid_Fail — Passed
- CleaningScheduleServiceMoqTests.GetUpcoming_InvalidRange_Fail — Passed
- CleaningScheduleServiceMoqTests.Create_InvalidDate_Fail — Passed
- CleaningScheduleServiceMoqTests.MarkAsCompleted_Ok — Passed

