using EcoStationManagerApplication.Core.Composition;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.UI.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.UI.Common
{
    public static class AppServices
    {
        // Business Services
        public static IOrderService OrderService => ServiceRegistry.OrderService;
        public static IInventoryService InventoryService => ServiceRegistry.InventoryService;
        public static ICategoryService CategoryService => ServiceRegistry.CategoryService;
        public static IProductService ProductService => ServiceRegistry.ProductService;
        public static IPackagingService PackagingService => ServiceRegistry.PackagingService;
        public static IUserService UserService => ServiceRegistry.UserService;
        public static ICustomerService CustomerService => ServiceRegistry.CustomerService;
        public static IPackagingInventoryService PackagingInventoryService => ServiceRegistry.PackagingInventoryService;
        public static IPackagingTransactionService PackagingTransactionService => ServiceRegistry.PackagingTransactionService;
        public static IStockInService StockInService => ServiceRegistry.StockInService;
        public static IStockOutService StockOutService => ServiceRegistry.StockOutService;
        public static ISupplierService SupplierService => ServiceRegistry.SupplierService;
        public static IExportService ExportService => ServiceRegistry.ExportService;
        public static IReportService ReportService => ServiceRegistry.ReportService;
        public static IOrderDetailService OrderDetailService => ServiceRegistry.OrderDetailService;
        public static IStationService StationService => ServiceRegistry.StationService;
        public static IDeliveryService DeliveryService => ServiceRegistry.DeliveryService;
        public static IWorkShiftService WorkShiftService => ServiceRegistry.WorkShiftService;
        public static IImportService ImportService => ServiceRegistry.ImportService;
        public static ICleaningScheduleService CleaningScheduleService => ServiceRegistry.CleaningScheduleService;
        public static INotificationService NotificationService => ServiceRegistry.NotificationService;
        public static IDatabaseBackupService DatabaseBackupService => ServiceRegistry.DatabaseBackupService;

        // UI Services
        public static NavigationService Navigation => NavigationService.Instance;
        public static DialogService Dialog => DialogService.Instance;
        public static AppStateManager State => AppStateManager.Instance;
    }
}
