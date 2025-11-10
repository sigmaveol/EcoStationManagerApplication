using EcoStationManagerApplication.Common.Logging;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseHelper _databaseHelper;
        private readonly ILogHelper _logger;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        // Repositories
        private IUserRepository _users;
        private ICustomerRepository _customers;
        private ICategoryRepository _categories;
        private IProductRepository _products;
        private IInventoryRepository _inventories;
        private IPackagingRepository _packaging;
        private IPackagingInventoryRepository _packagingInventories;
        private IPackagingTransactionRepository _packagingTransactions;
        private IOrderRepository _orders;
        private IOrderDetailRepository _orderDetails;
        private ISupplierRepository _suppliers;
        private IStockInRepository _stockIn;
        private IStockOutRepository _stockOut;

        public UnitOfWork(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _logger = LogHelperFactory.CreateLogger("UnitOfWork");
        }

        // User Management
        public IUserRepository Users => _users ?? (_users = new UserRepository(_databaseHelper));

        // Customer Management
        public ICustomerRepository Customers => _customers ?? (_customers = new CustomerRepository(_databaseHelper));

        // Product & Inventory
        public ICategoryRepository Categories => _categories ?? (_categories = new CategoryRepository(_databaseHelper));
        public IProductRepository Products => _products ?? (_products = new ProductRepository(_databaseHelper));
        public IInventoryRepository Inventories => _inventories ?? (_inventories = new InventoryRepository(_databaseHelper));

        // Packaging Management
        public IPackagingRepository Packaging => _packaging ?? (_packaging = new PackagingRepository(_databaseHelper));
        public IPackagingInventoryRepository PackagingInventories => _packagingInventories ?? (_packagingInventories = new PackagingInventoryRepository(_databaseHelper));
        public IPackagingTransactionRepository PackagingTransactions => _packagingTransactions ?? (_packagingTransactions = new PackagingTransactionRepository(_databaseHelper, PackagingInventories));

        // Order Management
        public IOrderRepository Orders => _orders ?? (_orders = new OrderRepository(_databaseHelper));
        public IOrderDetailRepository OrderDetails => _orderDetails ?? (_orderDetails = new OrderDetailRepository(_databaseHelper));

        // Supplier Management
        public ISupplierRepository Suppliers => _suppliers ?? (_suppliers = new SupplierRepository(_databaseHelper));

        // Stock Management
        public IStockInRepository StockIn => _stockIn ?? (_stockIn = new StockInRepository(_databaseHelper));
        public IStockOutRepository StockOut => _stockOut ?? (_stockOut = new StockOutRepository(_databaseHelper, Inventories, PackagingInventories));

        // TODO: Triển khai sau
        // public IDeliveryAssignmentRepository DeliveryAssignments => throw new NotImplementedException();
        // public IWorkShiftRepository WorkShifts => throw new NotImplementedException();
        // public ICleaningScheduleRepository CleaningSchedules => throw new NotImplementedException();

        public async Task<int> SaveChangesAsync()
        {
            try
            {   
                // Dapper đã tự quản lí các transaction rồi
                // Method này chủ yếu cho consistency với Entity Framework
                _logger.Info("UnitOfWork SaveChangesAsync called");
                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveChangesAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task BeginTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    throw new InvalidOperationException("Transaction already started");
                }

                _connection = await _databaseHelper.CreateConnectionAsync();
                _transaction = _connection.BeginTransaction();
                _logger.Info("Transaction started");
            }
            catch (Exception ex)
            {
                _logger.Error($"BeginTransactionAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                if (_transaction == null)
                {
                    throw new InvalidOperationException("No transaction to commit");
                }

                _transaction.Commit();
                _transaction = null;
                _connection?.Close();
                _connection = null;
                _logger.Info("Transaction committed");
            }
            catch (Exception ex)
            {
                _logger.Error($"CommitTransactionAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction == null)
                {
                    throw new InvalidOperationException("No transaction to rollback");
                }

                _transaction.Rollback();
                _transaction = null;
                _connection?.Close();
                _connection = null;
                _logger.Info("Transaction rolled back");
            }
            catch (Exception ex)
            {
                _logger.Error($"RollbackTransactionAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                return await _databaseHelper.TestConnectionAsync();
            }
            catch (Exception ex)
            {
                _logger.Error($"TestConnectionAsync error: {ex.Message}");
                return false;
            }
        }

        #region IDisposable Implementation

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose transaction và connection
                    _transaction?.Dispose();
                    _transaction = null;

                    _connection?.Dispose();
                    _connection = null;

                    _logger.Info("UnitOfWork disposed");
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
