using System;

namespace EcoStationManagerApplication.Common
{
    public static class Constants
    {
        // Application Constants
        public const string APPLICATION_NAME = "EcoStation Manager";
        public const string VERSION = "1.0.0";
        public const string COPYRIGHT = "© 2024 EcoStation Manager. All rights reserved.";

        // Database Constants
        public const string DEFAULT_CONNECTION_NAME = "EcoStationDB";
        public const int DEFAULT_COMMAND_TIMEOUT = 30;
        public const int DEFAULT_SESSION_TIMEOUT = 30;

        // Business Constants
        public const decimal DEFAULT_TAX_RATE = 0.1m;
        public const int LOW_STOCK_THRESHOLD = 10;
        public const int EXPIRY_WARNING_DAYS = 15;
        public const int MAX_RETRY_ATTEMPTS = 3;
        public const int BATCH_SIZE = 100;

        // Order Status
        public static class OrderStatus
        {
            public const string DRAFT = "draft";
            public const string CONFIRMED = "confirmed";
            public const string PROCESSING = "processing";
            public const string READY = "ready";
            public const string SHIPPED = "shipped";
            public const string COMPLETED = "completed";
            public const string CANCELLED = "cancelled";
            public const string RETURNED = "returned";
        }

        // Payment Status
        public static class PaymentStatus
        {
            public const string PENDING = "pending";
            public const string COMPLETED = "completed";
            public const string FAILED = "failed";
            public const string REFUNDED = "refunded";
        }

        // Payment Methods
        public static class PaymentMethods
        {
            public const string CASH = "cash";
            public const string TRANSFER = "transfer";
            public const string E_WALLET = "ewallet";
        }

        // Order Sources
        public static class OrderSources
        {
            public const string GOOGLE_FORM = "googleform";
            public const string EXCEL = "excel";
            public const string EMAIL = "email";
            public const string MANUAL = "manual";
            public const string OTHER = "other";
        }

        // Station Types
        public static class StationTypes
        {
            public const string WAREHOUSE = "warehouse";
            public const string REFILL = "refill";
            public const string HYBRID = "hybrid";
            public const string OTHER = "other";
        }

        // Product Types
        public static class ProductTypes
        {
            public const string SINGLE = "single";
            public const string SERVICE = "service";
            public const string OTHER = "other";
        }

        // Variant Types
        public static class VariantTypes
        {
            public const string PACKED = "packed";
            public const string REFILLED = "refilled";
            public const string OTHER = "other";
        }

        // Quality Status
        public static class QualityStatus
        {
            public const string GOOD = "good";
            public const string PENDING = "pending";
            public const string EXPIRED = "expired";
            public const string REJECTED = "rejected";
        }

        // Stock Operations
        public static class StockOperations
        {
            public const string SALE = "sale";
            public const string TRANSFER = "transfer";
            public const string WASTE = "waste";
            public const string ADJUSTMENT = "adjustment";
        }

        // Stock Sources
        public static class StockSources
        {
            public const string SUPPLIER = "supplier";
            public const string TRANSFER = "transfer";
            public const string RETURN = "return";
        }

        // Quality Check
        public static class QualityCheck
        {
            public const string PASS = "pass";
            public const string FAIL = "fail";
            public const string PENDING = "pending";
        }

        // Customer Status
        public static class CustomerStatus
        {
            public const string ACTIVE = "active";
            public const string INACTIVE = "inactive";
            public const string SUSPENDED = "suspended";
        }

        // Packaging Status
        public static class PackagingStatus
        {
            public const string NEW = "new";
            public const string IN_USE = "inuse";
            public const string NEED_CLEANING = "needcleaning";
            public const string DAMAGED = "damaged";
            public const string DISPOSED = "disposed";
        }

        // Packaging Transaction Types
        public static class PackagingTransactionTypes
        {
            public const string ISSUE = "issue";
            public const string RETURN = "return";
            public const string CLEAN = "clean";
            public const string DISPOSE = "dispose";
        }

        // Tank Materials
        public static class TankMaterials
        {
            public const string GLASS = "glass";
            public const string PLASTIC = "plastic";
            public const string METAL = "metal";
        }

        // Tank Units
        public static class TankUnits
        {
            public const string ML = "ml";
            public const string LITER = "liter";
            public const string KG = "kg";
        }

        // Tank Status
        public static class TankStatus
        {
            public const string ACTIVE = "active";
            public const string MAINTENANCE = "maintenance";
            public const string OUT_OF_ORDER = "outoforder";
        }

        // Cleaning Types
        public static class CleaningTypes
        {
            public const string TANK = "tank";
            public const string PACKAGE = "package";
        }

        // Cleaning Status
        public static class CleaningStatus
        {
            public const string SCHEDULED = "scheduled";
            public const string COMPLETED = "completed";
            public const string OVERDUE = "overdue";
            public const string CANCELLED = "cancelled";
        }

        // Transfer Status
        public static class TransferStatus
        {
            public const string PENDING = "pending";
            public const string IN_TRANSIT = "in_transit";
            public const string COMPLETED = "completed";
            public const string CANCELLED = "cancelled";
            public const string OTHER = "other";
        }

        // Delivery Status
        public static class DeliveryStatus
        {
            public const string PENDING = "pending";
            public const string IN_TRANSIT = "intransit";
            public const string DELIVERED = "delivered";
            public const string FAILED = "failed";
        }

        // Refund Status
        public static class RefundStatus
        {
            public const string PENDING = "Pending";
            public const string COMPLETED = "Completed";
            public const string FAILED = "Failed";
        }

        // Audit Status
        public static class AuditStatus
        {
            public const string PENDING = "pending";
            public const string APPROVED = "approved";
            public const string REJECTED = "rejected";
        }

        // Point Transaction Types
        public static class PointTransactionTypes
        {
            public const string EARNED = "earned";
            public const string USED = "used";
            public const string EXPIRED = "expired";
            public const string REFUNDED = "refunded";
        }

        // Point Rule Actions
        public static class PointRuleActions
        {
            public const string PURCHASE = "purchase";
            public const string REFILL = "refill";
            public const string RETURN_PACKAGING = "return_packaging";
            public const string REFERRAL = "referral";
        }

        // Data Types
        public static class DataTypes
        {
            public const string STRING = "string";
            public const string INTEGER = "integer";
            public const string DECIMAL = "decimal";
            public const string BOOLEAN = "boolean";
            public const string DATE = "date";
            public const string JSON = "json";
        }

        // Backup Types
        public static class BackupTypes
        {
            public const string FULL = "full";
            public const string INCREMENTAL = "incremental";
            public const string MANUAL = "manual";
        }

        // Backup Status
        public static class BackupStatus
        {
            public const string IN_PROGRESS = "in_progress";
            public const string COMPLETED = "completed";
            public const string FAILED = "failed";
        }

        // Import Sources
        public static class ImportSources
        {
            public const string GOOGLE_FORM = "google_form";
            public const string EXCEL = "excel";
            public const string CSV = "csv";
            public const string EMAIL = "email";
        }

        // Import Status
        public static class ImportStatus
        {
            public const string PENDING = "pending";
            public const string PROCESSING = "processing";
            public const string COMPLETED = "completed";
            public const string FAILED = "failed";
        }

        // Import Detail Status
        public static class ImportDetailStatus
        {
            public const string SUCCESS = "success";
            public const string ERROR = "error";
            public const string WARNING = "warning";
        }

        // Route Sources
        public static class RouteSources
        {
            public const string MANUAL_EXCEL = "manualexcel";
            public const string APP = "app";
        }

        // Category Types
        public static class CategoryTypes
        {
            public const string PRODUCT = "product";
            public const string SERVICE = "service";
            public const string OTHER = "other";
        }

        // Module Keys
        public static class ModuleKeys
        {
            public const string ORDER_MANAGEMENT = "order_management";
            public const string INVENTORY_MANAGEMENT = "inventory_management";
            public const string CUSTOMER_MANAGEMENT = "customer_management";
            public const string PRODUCT_MANAGEMENT = "product_management";
            public const string STATION_MANAGEMENT = "station_management";
            public const string PACKAGING_MANAGEMENT = "packaging_management";
            public const string REPORTING = "reporting";
            public const string SYSTEM_CONFIG = "system_config";
        }

        // File Extensions
        public static class FileExtensions
        {
            public const string EXCEL = ".xlsx";
            public const string CSV = ".csv";
            public const string PDF = ".pdf";
            public const string JSON = ".json";
            public const string XML = ".xml";
        }

        // Date Formats
        public static class DateFormats
        {
            public const string DISPLAY = "dd/MM/yyyy";
            public const string DISPLAY_WITH_TIME = "dd/MM/yyyy HH:mm";
            public const string DATABASE = "yyyy-MM-dd";
            public const string DATABASE_WITH_TIME = "yyyy-MM-dd HH:mm:ss";
        }

        // Number Formats
        public static class NumberFormats
        {
            public const string CURRENCY = "N0";
            public const string DECIMAL = "N2";
            public const string PERCENTAGE = "P2";
        }
    }
}
