using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.UnitOfWork
{
    public static class UnitOfWorkFactory
    {
        /// <summary>
        /// Tạo UnitOfWork mới
        /// </summary>
        public static IUnitOfWork Create()
        {
            var databaseHelper = new DatabaseHelper();
            return new UnitOfWork(databaseHelper);
        }

        /// <summary>
        /// Tạo UnitOfWork với connection string tùy chỉnh
        /// </summary>
        public static IUnitOfWork Create(string connectionString)
        {
            var databaseHelper = new DatabaseHelper(connectionString);
            return new UnitOfWork(databaseHelper);
        }
    }
}
