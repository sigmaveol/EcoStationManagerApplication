using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{

    /// <summary>
    /// Một đơn vị công việc (Unit of Work) trong hệ thống,
    /// quản lý các repository và đảm bảo tất cả thao tác dữ liệu
    /// thực hiện trong cùng một transaction.
    public interface IUnitOfWork : IDisposable
    {
        // User Management
        IUserRepository Users { get; }

        // Customer Management
        ICustomerRepository Customers { get; }
        ISupplierRepository Suppliers { get; }

        // Product & Category Management
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IPackagingRepository Packaging { get; }

        // Inventory Management
        IInventoryRepository Inventories { get; }
        IPackagingInventoryRepository PackagingInventories { get; }

        // Stock Management
        IStockInRepository StockIn { get; }
        IStockOutRepository StockOut { get; }

        // Order Management
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }

        // Packaging Transactions
        IPackagingTransactionRepository PackagingTransactions { get; }

        // TODO: Triển khai sau
        // IDeliveryAssignmentRepository DeliveryAssignments { get; }
        // IWorkShiftRepository WorkShifts { get; }
        // ICleaningScheduleRepository CleaningSchedules { get; }

        /// <summary>
        /// Lưu tất cả thay đổi
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Bắt đầu transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rollback transaction
        /// </summary>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        Task<bool> TestConnectionAsync();
    }

}
