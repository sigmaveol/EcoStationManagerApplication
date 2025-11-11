using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core
{
    public static class ServiceFactory
    {
        //public static ICustomerService CreateCustomerService()
        //{
        //    var unitOfWork = UnitOfWorkFactory.Create();
        //    return new CustomerService(unitOfWork);
        //}

        public static IInventoryService CreateInventoryService()
        {
            var unitOfWork = UnitOfWorkFactory.Create();
            return new InventoryService(unitOfWork);
        }

        //public static IProductService CreateProductService()
        //{
        //    var unitOfWork = UnitOfWorkFactory.Create();
        //    return new ProductService(unitOfWork);
        //}

        public static IOrderService CreateOrderService()
        {
            var unitOfWork = UnitOfWorkFactory.Create();
            return new OrderService(unitOfWork);
        }

        // Thêm các service khác khi cần
    }
}
