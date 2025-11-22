-- ================================
-- Chuyển tất cả cột ENUM sang TINYINT
-- Mapping Enum C# tương ứng được ghi chú bên dưới
-- ================================

-- Users.role: ADMIN=0, STAFF=1, MANAGER=2, DRIVER=3
ALTER TABLE Users MODIFY COLUMN role TINYINT NOT NULL DEFAULT 1; -- default STAFF

-- Customers.rank: MEMBER=0, SILVER=1, GOLD=2, DIAMONDS=3
ALTER TABLE Customers MODIFY COLUMN rank TINYINT NOT NULL DEFAULT 0; -- default MEMBER

-- Categories.category_type: PRODUCT=0, SERVICE=1, OTHER=2
ALTER TABLE Categories MODIFY COLUMN category_type TINYINT NOT NULL DEFAULT 0;

-- Products.product_type: PACKED=0, REFILLED=1, OTHER=2
ALTER TABLE Products MODIFY COLUMN product_type TINYINT NOT NULL DEFAULT 0;

-- Orders.source: GOOGLEFORM=0, EXCEL=1, EMAIL=2, MANUAL=3
ALTER TABLE Orders MODIFY COLUMN source TINYINT NOT NULL DEFAULT 3; -- default MANUAL

-- Orders.status: DRAFT=0, CONFIRMED=1, PROCESSING=2, READY=3, SHIPPED=4, COMPLETED=5, CANCELLED=6
ALTER TABLE Orders MODIFY COLUMN status TINYINT NOT NULL DEFAULT 0; -- default DRAFT

-- Orders.payment_status: UNPAID=0, PAID=1
ALTER TABLE Orders MODIFY COLUMN payment_status TINYINT NOT NULL DEFAULT 0; -- default UNPAID

-- Orders.payment_method: CASH=0, TRANSFER=1
ALTER TABLE Orders MODIFY COLUMN payment_method TINYINT NOT NULL DEFAULT 0; -- default CASH

-- CleaningSchedules.cleaning_type: TANK=0, PACKAGING=1
ALTER TABLE CleaningSchedules MODIFY COLUMN cleaning_type TINYINT NOT NULL DEFAULT 0; -- default TANK

-- CleaningSchedules.status: SCHEDULED=0, COMPLETED=1, OVERDUE=2, CANCELLED=3
ALTER TABLE CleaningSchedules MODIFY COLUMN status TINYINT NOT NULL DEFAULT 0; -- default SCHEDULED

-- Stations.station_type: WAREHOUSE=0, REFILL=1, HYBRID=2, OTHER=3
ALTER TABLE Stations MODIFY COLUMN station_type TINYINT NOT NULL DEFAULT 1; -- default REFILL

-- Notifications.type: INFO=0, WARNING=1, ERROR=2, SUCCESS=3, LOWSTOCK=4, ORDER=5, REFILL=6, SYSTEM=7
ALTER TABLE Notifications MODIFY COLUMN type TINYINT NOT NULL DEFAULT 0; -- default INFO

-- StockIn.ref_type: PRODUCT=0, PACKAGING=1
ALTER TABLE StockIn MODIFY COLUMN ref_type TINYINT NOT NULL DEFAULT 0;

-- StockOut.ref_type: PRODUCT=0, PACKAGING=1
ALTER TABLE StockOut MODIFY COLUMN ref_type TINYINT NOT NULL DEFAULT 0;

-- StockOut.purpose: SALE=0, DAMAGE=1, TRANSFER=2
ALTER TABLE StockOut MODIFY COLUMN purpose TINYINT NOT NULL DEFAULT 0; -- default SALE

-- PackagingTransactions.type: ISSUE=0, RETURN=1
ALTER TABLE PackagingTransactions MODIFY COLUMN type TINYINT NOT NULL DEFAULT 0; -- default ISSUE

-- PackagingTransactions.ownership_type: DEPOSIT=0, SOLD=1
ALTER TABLE PackagingTransactions MODIFY COLUMN ownership_type TINYINT NOT NULL DEFAULT 0; -- default DEPOSIT

-- DeliveryAssignments.status : pending=0, intransit=1, delivered=2, failed=3
-- DeliveryAssignments.payment_status : unpaid=0, paid=1
AlTER TABLE DeliveryAssignments MODIFY COLUMN status TINYINT NOT NULL DEFAULT 0;
AlTER TABLE DeliveryAssignments MODIFY COLUMN payment_status TINYINT NOT NULL DEFAULT 0;


-- ================================
-- Lưu ý chung:
-- 1. TINYINT lưu Enum C# dưới dạng số nguyên, EF Core tự map.
-- 2. DEFAULT value giữ logic tương tự ENUM cũ.
-- 3. Nếu muốn đảm bảo giá trị hợp lệ trong DB, thêm CHECK constraint.
-- ================================
