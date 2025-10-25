using System;

namespace EcoStationManagerApplication.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }

        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ValidationException : BusinessException
    {
        public ValidationException(string message) : base(message, "VALIDATION_ERROR") { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(message, "NOT_FOUND") { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string message) : base(message, "UNAUTHORIZED") { }
        public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }
    }
}