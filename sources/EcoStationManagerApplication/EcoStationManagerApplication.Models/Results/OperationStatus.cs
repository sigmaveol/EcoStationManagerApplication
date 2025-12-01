using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Results
{
    public static class OperationStatus
    {
        // Success codes
        public const string SUCCESS = "SUCCESS";
        public const string CREATED = "CREATED";
        public const string UPDATED = "UPDATED";
        public const string DELETED = "DELETED";

        // Error codes
        public const string NOT_FOUND = "NOT_FOUND";
        public const string VALIDATION_ERROR = "VALIDATION_ERROR";
        public const string DUPLICATE_ERROR = "DUPLICATE_ERROR";
        public const string UNAUTHORIZED = "UNAUTHORIZED";
        public const string DATABASE_ERROR = "DATABASE_ERROR";
        public const string BUSINESS_RULE_ERROR = "BUSINESS_RULE_ERROR";
    }
}
