using EcoStationManagerApplication.Common.Logging;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public abstract class BaseService
    {
        protected readonly ILogHelper _logger;

        protected BaseService(string serviceName)
        {
            _logger = LogHelperFactory.CreateLogger($"Service:{serviceName}");
        }

        protected Result<T> HandleException<T>(Exception ex, string operationName)
        {
            _logger.Error($"{operationName} error: {ex.Message}");

            if (ex.InnerException != null)
            {
                _logger.Error($"Inner exception: {ex.InnerException.Message}");
            }

            return Result<T>.Fail($"Đã xảy ra lỗi khi {operationName}. Vui lòng thử lại sau.");
        }

        protected Result HandleException(Exception ex, string operationName)
        {
            _logger.Error($"{operationName} error: {ex.Message}");

            if (ex.InnerException != null)
            {
                _logger.Error($"Inner exception: {ex.InnerException.Message}");
            }

            return Result.Fail($"Đã xảy ra lỗi khi {operationName}. Vui lòng thử lại sau.");
        }

        protected Result<T> ValidationError<T>(List<string> errors)
        {
            return Result<T>.Fail(errors, "Dữ liệu không hợp lệ");
        }

        protected Result<T> NotFoundError<T>(string entityName, int id)
        {
            return Result<T>.Fail($"{entityName} với ID {id} không tồn tại");
        }

        protected Result<T> NotFoundError<T>(string message)
        {
            return Result<T>.Fail(message);
        }
    }
}
