using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Composition
{
    public static class ServiceRegistry
    {
        private static IUnitOfWork _unitOfWork;

        private static IOrderService _orderService;
        private static IInventoryService _inventoryService;
        private static ICategoryService _categoryService;
        private static IProductService _productService;
        private static IPackagingService _packagingService;
        private static IUserService _userService;
        private static ICustomerService _customerService;
        private static ISupplierService _supplierService;
        private static IPackagingInventoryService _packagingInventoryService;
        private static IPackagingTransactionService _packagingTransactionService;
        private static IStockInService _stockInService;
        private static IStockOutService _stockOutService;
        private static IOrderDetailService _orderDetailService;
        private static IExportService _exportService;
        private static IReportService _reportService;
        private static IStationService _stationService;
        private static IDeliveryService _deliveryService;
        private static IWorkShiftService _workShiftService;
        private static IImportService _importService;
        private static ICleaningScheduleService _cleaningScheduleService;

        private static IUnitOfWork GetUnitOfWork()
        {
            if (_unitOfWork == null)
            {
                _unitOfWork = UnitOfWorkFactory.Create();
            }
            return _unitOfWork;
        }

        public static IOrderService OrderService => _orderService ?? (_orderService = new OrderService(GetUnitOfWork()));

        public static ICategoryService CategoryService => _categoryService ?? (_categoryService = new CategoryService(GetUnitOfWork()));

        public static IProductService ProductService => _productService ?? (_productService = new ProductService(
            GetUnitOfWork(), CategoryService));

        public static IInventoryService InventoryService => _inventoryService ?? (_inventoryService = new InventoryService(
            GetUnitOfWork(),
            ProductService));

        public static IPackagingService PackagingService => _packagingService ?? (_packagingService = new PackagingService(GetUnitOfWork()));

        public static IUserService UserService => _userService ?? (_userService = new UserService(GetUnitOfWork()));

        public static ISupplierService SupplierService => _supplierService ?? (_supplierService = new SupplierService(GetUnitOfWork()));

        public static ICustomerService CustomerService => _customerService ?? (_customerService = new CustomerService(GetUnitOfWork()));

        public static IPackagingInventoryService PackagingInventoryService => _packagingInventoryService ?? (_packagingInventoryService = new PackagingInventoryService(
            GetUnitOfWork(),
            PackagingService));

        public static IPackagingTransactionService PackagingTransactionService => _packagingTransactionService ?? (_packagingTransactionService = new PackagingTransactionService(
            GetUnitOfWork(),
            PackagingService,
            CustomerService,
            PackagingInventoryService));

        public static IStockInService StockInService => _stockInService ?? (_stockInService = new StockInService(
            GetUnitOfWork(),
            InventoryService,
            PackagingInventoryService));

        public static IStockOutService StockOutService => _stockOutService ?? (_stockOutService = new StockOutService(
            GetUnitOfWork(),
            InventoryService,
            PackagingInventoryService));

        public static IExportService ExportService => _exportService ?? (_exportService = new ExportService(
            OrderService,
            CustomerService));

        public static IReportService ReportService => _reportService ?? (_reportService = new ReportService(GetUnitOfWork()));

        public static IOrderDetailService OrderDetailService => _orderDetailService ?? (_orderDetailService = new OrderDetailService(GetUnitOfWork()));
        public static IStationService StationService => _stationService ?? (_stationService = new StationService(GetUnitOfWork()));

        public static IDeliveryService DeliveryService => _deliveryService ?? (_deliveryService = new DeliveryService(GetUnitOfWork()));

        public static IWorkShiftService WorkShiftService => _workShiftService ?? (_workShiftService = new WorkShiftService(GetUnitOfWork()));

        public static IImportService ImportService => _importService ?? (_importService = new ImportService(
            GetUnitOfWork(),
            OrderService,
            CustomerService,
            ProductService));

        public static ICleaningScheduleService CleaningScheduleService => _cleaningScheduleService ?? (_cleaningScheduleService = new CleaningScheduleService(GetUnitOfWork()));
    }
}
