-- EcoStation Manager Database V1 (full-online/cloud) - MySQL Version
-- Engine: InnoDB, Charset: utf8mb4

CREATE DATABASE IF NOT EXISTS EcoStationManager
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE EcoStationManager;

-- Bảng vai trò
CREATE TABLE IF NOT EXISTS Roles (
    role_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) UNIQUE NOT NULL,
    permissions JSON NOT NULL,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng người dùng
CREATE TABLE IF NOT EXISTS Users (
    user_id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(100) NOT NULL,
    fullname VARCHAR(255) NOT NULL,
    is_active BOOLEAN DEFAULT FALSE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng trạm
CREATE TABLE IF NOT EXISTS Stations (
    station_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    address TEXT NOT NULL,
    phone VARCHAR(50),
    station_type ENUM('warehouse', 'refill', 'hybrid', 'other') DEFAULT 'refill',
    manager INT, -- User_id của quản lý trạm
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (manager) REFERENCES Users(user_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Bảng gán Stations cho Users
CREATE TABLE IF NOT EXISTS UserStation (
    user_id INT NOT NULL,
    station_id INT NOT NULL,
    PRIMARY KEY(user_id, station_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id) ON DELETE RESTRICT
) ENGINE=InnoDB;

-- Bảng gán Roles cho Users
CREATE TABLE IF NOT EXISTS UserRole (
    user_id INT NOT NULL,
    role_id INT NOT NULL,
    assigned_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    assigned_by INT,
    PRIMARY KEY(user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (role_id) REFERENCES Roles(role_id),
    FOREIGN KEY (assigned_by) REFERENCES Users(user_id) ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS WorkShifts (
    shift_id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    shift_date DATE NOT NULL,
    start_time TIME,
    end_time TIME,
    kpi_score DECIMAL(5,2) DEFAULT NULL,
    notes TEXT,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng nhà cung cấp
CREATE TABLE IF NOT EXISTS Suppliers (
    supplier_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    address TEXT,
    phone VARCHAR(50),
    email VARCHAR(100),
    contact_person VARCHAR(255),
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng khách hàng
CREATE TABLE IF NOT EXISTS Customers (
    customer_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    phone VARCHAR(50),
    email VARCHAR(100),
    address TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    total_point INT DEFAULT 0,
    status ENUM('active', 'inactive', 'suspended') DEFAULT 'active'
) ENGINE=InnoDB;

-- Bảng danh mục sản phẩm
CREATE TABLE IF NOT EXISTS Categories (
    category_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    image VARCHAR(255),
    description TEXT,
    category_type ENUM('product', 'service', 'other') DEFAULT 'product',
    parent_id INT,
    sort_order INT DEFAULT 0,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (parent_id) REFERENCES Categories(category_id)
) ENGINE=InnoDB;

-- Bảng sản phẩm
CREATE TABLE IF NOT EXISTS Products (
    product_id INT PRIMARY KEY AUTO_INCREMENT,
    sku VARCHAR(30) UNIQUE,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    base_price DECIMAL(10,2) DEFAULT NULL, -- null nếu chỉ lưu sp
    unit_measure VARCHAR(50) NOT NULL, -- vd: g, ml, cái, hộp
    product_type ENUM('single', 'service', 'other') DEFAULT 'single',
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    category_id INT, -- trong TH chưa thêm vào danh mục
    FOREIGN KEY (category_id) REFERENCES Categories(category_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Bảng loại biến thể sản phẩm
CREATE TABLE IF NOT EXISTS VariantTypes (
    type_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng biến thể sản phẩm
CREATE TABLE IF NOT EXISTS Variants (
    variant_id INT AUTO_INCREMENT PRIMARY KEY,
    SKU VARCHAR(50) UNIQUE NOT NULL,
    barcode VARCHAR(20) UNIQUE, -- Barcode 13 chữ số chuẩn EAN13
    name VARCHAR(255) NOT NULL,
    unit VARCHAR(50) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    qr_code VARCHAR(255) UNIQUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    product_id INT NOT NULL,
    type_id INT NOT NULL,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (type_id) REFERENCES VariantTypes(type_id)
) ENGINE=InnoDB;

-- Bảng combo/set sản phẩm
CREATE TABLE IF NOT EXISTS Combos (
    combo_id INT PRIMARY KEY AUTO_INCREMENT,
    code VARCHAR(30) UNIQUE,
    name VARCHAR(255) NOT NULL,
    image VARCHAR(255),
    description TEXT,
    total_price DECIMAL(10,2) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng thành phần combo
CREATE TABLE IF NOT EXISTS ComboItems (
    combo_id INT NOT NULL,
    variant_id INT NOT NULL,
    quantity DECIMAL(10,2) DEFAULT 1,
    PRIMARY KEY (combo_id, variant_id),
    FOREIGN KEY (combo_id) REFERENCES Combos(combo_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Transfers (
    transfer_id INT PRIMARY KEY AUTO_INCREMENT,
    code VARCHAR(50) UNIQUE,
    from_station_id INT NOT NULL,
    to_station_id INT NOT NULL,
    status ENUM('pending', 'in_transit', 'completed', 'cancelled', 'other') DEFAULT 'pending',
    notes TEXT,
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (from_station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (to_station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS TransferDetails (
    transfer_id INT NOT NULL,
    variant_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (transfer_id, variant_id),
    FOREIGN KEY (transfer_id) REFERENCES Transfers(transfer_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id)
) ENGINE=InnoDB;

-- Bảng đơn hàng
CREATE TABLE IF NOT EXISTS Orders (
    order_id INT PRIMARY KEY AUTO_INCREMENT,
    source ENUM('googleform', 'excel', 'email', 'manual' ,'other') NOT NULL,
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0,
    discounted_amount DECIMAL(10,2) DEFAULT 0,
    tax_amount DECIMAL(10,2) DEFAULT 0,
    status ENUM(
        'draft', 'confirmed', 'processing', 'ready', 'shipped', 'completed', 'cancelled', 'returned'
    ) DEFAULT 'draft',
    note TEXT, 
    user_id INT, -- Người tạo đơn
    customer_id INT,
    station_id INT, -- Trạm xử lý đơn/ nếu mua tại trạm
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- Bảng chi tiết đơn hàng
CREATE TABLE IF NOT EXISTS OrderDetails (
    order_detail_id INT PRIMARY KEY AUTO_INCREMENT, -- cho phép tách dòng để giảm giá thành sản phẩm
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL,
    order_id INT NOT NULL,
    combo_id INT, -- Nếu là combo
    variant_id INT, -- Nếu là variant đơn lẻ
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE CASCADE,
    FOREIGN KEY (combo_id) REFERENCES Combos(combo_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id)
) ENGINE=InnoDB;

-- Bảng thanh toán
CREATE TABLE IF NOT EXISTS Payments (
    payment_id INT PRIMARY KEY AUTO_INCREMENT,
    payment_number VARCHAR(100) UNIQUE NOT NULL,
    order_id INT,
    amount DECIMAL(10,2) NOT NULL,
    method ENUM('cash', 'transfer', 'ewallet') NOT NULL,
    status ENUM('pending', 'completed', 'failed', 'refunded', 'other') DEFAULT 'pending',
    reference VARCHAR(100),
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Bảng hoàn tiền
CREATE TABLE IF NOT EXISTS Refunds (
    refund_id INT PRIMARY KEY AUTO_INCREMENT,
    refund_number VARCHAR(100) UNIQUE NOT NULL,
    order_id INT,
    payment_id INT,
    amount DECIMAL(10,2) NOT NULL,
    reason TEXT,
    status ENUM('pending', 'completed', 'failed', 'other') DEFAULT 'pending',
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE SET NULL,
    FOREIGN KEY (payment_id) REFERENCES Payments(payment_id) ON DELETE SET NULL,
    FOREIGN KEY (created_by) REFERENCES Users(user_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Bảng lô hàng
CREATE TABLE IF NOT EXISTS Batches (
    batch_id INT PRIMARY KEY AUTO_INCREMENT,
    batch_no VARCHAR(100) NOT NULL,
    variant_id INT NOT NULL,
    station_id INT NOT NULL,
    manufacture_date DATE,
    expiry_date DATE,
    initial_quantity DECIMAL(10,2) DEFAULT 0,
    current_quantity DECIMAL(10,2) DEFAULT 0,
    quality_status ENUM('good', 'pending', 'expired', 'rejected', 'other') DEFAULT 'good',
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS VariantStockRules (
    rule_id INT PRIMARY KEY AUTO_INCREMENT,
    variant_id INT NOT NULL,
    station_id INT NOT NULL,
    min_stock DECIMAL(10,2) DEFAULT 15,
    max_stock DECIMAL(10,2) DEFAULT 1000,
    expiry_days INT DEFAULT 15 COMMENT 'Số ngày còn lại trước khi hết hạn để cảnh báo',
    UNIQUE (variant_id, station_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id) ON DELETE CASCADE,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- Bảng cấu hình cảnh báo tồn kho
CREATE TABLE IF NOT EXISTS StockAlerts (
    alert_id INT PRIMARY KEY AUTO_INCREMENT,
    variant_id INT NOT NULL,
    station_id INT NOT NULL,
    alert_type ENUM('low_stock', 'expiry_warning', 'overstock', 'ohter') NOT NULL,
    threshold_value DECIMAL(10,2) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id) ON DELETE CASCADE,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- Bảng lịch sử kiểm kê
CREATE TABLE IF NOT EXISTS StockAudits (
    audit_id INT PRIMARY KEY AUTO_INCREMENT,
    station_id INT NOT NULL,
    variant_id INT NOT NULL,
    batch_id INT NULL,
    system_quantity DECIMAL(10,2) NOT NULL,
    actual_quantity DECIMAL(10,2) NOT NULL,
    variance DECIMAL(10,2) NOT NULL,
    reason TEXT,
    audit_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    audited_by INT NOT NULL,
    status ENUM('pending', 'approved', 'rejected', 'other') DEFAULT 'pending',
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (batch_id) REFERENCES Batches(batch_id),
    FOREIGN KEY (audited_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;


-- Bảng tồn kho tổng
CREATE TABLE IF NOT EXISTS Inventories (
    inventory_id INT PRIMARY KEY AUTO_INCREMENT,
    batch_id INT,
    variant_id INT NOT NULL,
    station_id INT NOT NULL,
    current_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    reserved_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (batch_id) REFERENCES Batches(batch_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- Bảng nhập kho
CREATE TABLE IF NOT EXISTS StockIn (
    stockin_id INT PRIMARY KEY AUTO_INCREMENT,
    reference_number VARCHAR(100) UNIQUE,
    batch_no VARCHAR(100),
    station_id INT NULL, -- nhập vào kho hay trạm
    variant_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2),
    supplier_id INT NULL, -- có thể null nếu nhập nội bộ
    batch_no VARCHAR(100),
    expiry_date DATE,
    source_type ENUM('supplier', 'transfer', 'return') DEFAULT 'supplier', -- return: khách refunds
    quality_check ENUM('pass', 'fail', 'pending') DEFAULT 'pass',
    notes TEXT,
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng xuất kho
CREATE TABLE IF NOT EXISTS StockOut (
    stockout_id INT PRIMARY KEY AUTO_INCREMENT,
    reference_number VARCHAR(100) UNIQUE,
    quantity DECIMAL(10,2) NOT NULL,
    purpose ENUM('sale', 'transfer', 'waste', 'adjustment') NOT NULL,
    reason TEXT,
    notes TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    station_id INT NOT NULL,
    dest_station_id INT NULL,  -- nếu chuyển hàng nội bộ
    variant_id INT NOT NULL,
    order_id INT NULL,
    created_by INT NOT NULL,
    batch_id INT NOT NULL,
    FOREIGN KEY (batch_id) REFERENCES Batches(batch_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (dest_station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS DeliveryAssignments (
    assignment_id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT NOT NULL,
    driver_id INT NOT NULL, -- user_id có role 'Driver'
    assigned_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    status ENUM('pending','intransit','delivered','failed') DEFAULT 'pending',
    notes TEXT,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE CASCADE,
    FOREIGN KEY (driver_id) REFERENCES Users(user_id) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS DeliveryRoutes (
    route_id INT PRIMARY KEY AUTO_INCREMENT,
    assignment_id INT NOT NULL,
    latitude DECIMAL(10,7),
    longitude DECIMAL(10,7),
    recorded_time DATETIME,
    source ENUM('manualexcel','app') DEFAULT 'manualexcel',
    FOREIGN KEY (assignment_id) REFERENCES DeliveryAssignments(assignment_id)
) ENGINE=InnoDB;

-- Bảng bồn chứa
CREATE TABLE IF NOT EXISTS Tanks (
    tank_id INT PRIMARY KEY AUTO_INCREMENT,
    station_id INT NOT NULL,
    name VARCHAR(100) NOT NULL,
    material ENUM('glass', 'plastic', 'metal') DEFAULT 'plastic',
    capacity DECIMAL(10,2) NOT NULL,
    unit ENUM('ml', 'liter', 'kg') DEFAULT 'liter',
    current_level DECIMAL(10,2) DEFAULT 0,
    status ENUM('active', 'maintenance', 'outoforder') DEFAULT 'active',
    last_clean_date DATETIME,
    next_clean_date DATETIME,
    note TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- Bảng lịch vệ sinh
CREATE TABLE IF NOT EXISTS CleaningSchedules (
    cs_id INT PRIMARY KEY AUTO_INCREMENT,
    station_id INT NOT NULL,
    cleaning_type ENUM('tank', 'package') NOT NULL,
    cleaning_date DATETIME NOT NULL,
    cleaning_by INT,
    status ENUM('scheduled', 'completed', 'overdue', 'cancelled') DEFAULT 'scheduled',
    notes TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (cleaning_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng bao bì
CREATE TABLE IF NOT EXISTS Packaging (
    packaging_id INT PRIMARY KEY AUTO_INCREMENT,
    type ENUM('bottle', 'box', 'container') NOT NULL,
    qr_code VARCHAR(255) UNIQUE,
    status ENUM('new', 'inuse', 'needcleaning', 'damaged', 'disposed') DEFAULT 'new',
    has_qr_code BOOLEAN DEFAULT FALSE,
    time_reused INT DEFAULT 0,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng giao dịch bao bì
CREATE TABLE IF NOT EXISTS PackagingTransactions (
    transaction_id INT PRIMARY KEY AUTO_INCREMENT,
    station_id INT NOT NULL,
    type ENUM('issue', 'return', 'clean', 'dispose') NOT NULL,
    points_earned INT DEFAULT 0,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    customer_id INT,
    packaging_id INT,
    quantity INT NOT NULL DEFAULT 1,
    notes TEXT,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (packaging_id) REFERENCES Packaging(packaging_id)
) ENGINE=InnoDB;

-- Bảng cấu hình modules: PM open
CREATE TABLE IF NOT EXISTS Modules (
    module_id INT PRIMARY KEY AUTO_INCREMENT,
    module_key VARCHAR(100) UNIQUE NOT NULL,
    name VARCHAR(255) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    config JSON,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;


-- Bảng cài đặt hệ thống
CREATE TABLE IF NOT EXISTS SystemSettings (
    ss_id INT PRIMARY KEY AUTO_INCREMENT,
    setting_key VARCHAR(100) UNIQUE NOT NULL,
    setting_value TEXT NOT NULL,
    data_type ENUM('string', 'integer', 'decimal', 'boolean', 'date') DEFAULT 'string',
    description TEXT,
    updated_by INT,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (updated_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Tạo indexes để tối ưu hiệu suất

-- Users
CREATE INDEX idx_users_username ON Users(username);
CREATE INDEX idx_users_email ON Users(email);

-- Stations
CREATE INDEX idx_stations_name ON Stations(name);
CREATE INDEX idx_stations_manager ON Stations(manager);
CREATE INDEX idx_stations_parent ON Stations(parent_station_id);

-- UserStation
CREATE INDEX idx_userstation_user ON UserStation(user_id);
CREATE INDEX idx_userstation_station ON UserStation(station_id);

-- UserRoles
CREATE INDEX idx_userroles_user ON UserRoles(user_id);
CREATE INDEX idx_userroles_role ON UserRoles(role_id);
CREATE INDEX idx_userroles_assigned_by ON UserRoles(assigned_by);

-- WorkShifts
CREATE INDEX idx_workshifts_user ON WorkShifts(user_id);
CREATE INDEX idx_workshifts_shift_date ON WorkShifts(shift_date);

-- Suppliers
CREATE INDEX idx_suppliers_name ON Suppliers(name);
CREATE INDEX idx_suppliers_email ON Suppliers(email);

-- Customers
CREATE INDEX idx_customers_name ON Customers(name);
CREATE INDEX idx_customers_phone ON Customers(phone);
CREATE INDEX idx_customers_status ON Customers(status);

-- Categories
CREATE INDEX idx_categories_name ON Categories(name);
CREATE INDEX idx_categories_parent ON Categories(parent_id);

-- Products
CREATE INDEX idx_products_name ON Products(name);
CREATE INDEX idx_products_code ON Products(code);
CREATE INDEX idx_products_category ON Products(category_id);

-- Variants
CREATE INDEX idx_variants_sku ON Variants(SKU);
CREATE INDEX idx_variants_barcode ON Variants(barcode);
CREATE INDEX idx_variants_product ON Variants(product_id);
CREATE INDEX idx_variants_type ON Variants(type_id);

-- Combos
CREATE INDEX idx_combos_name ON Combos(name);
CREATE INDEX idx_combos_code ON Combos(code);

-- Transfers
CREATE INDEX idx_transfers_from_station ON Transfers(from_station_id);
CREATE INDEX idx_transfers_to_station ON Transfers(to_station_id);
CREATE INDEX idx_transfers_status ON Transfers(status);
CREATE INDEX idx_transfers_created_by ON Transfers(created_by);

-- Orders
CREATE INDEX idx_orders_user ON Orders(user_id);
CREATE INDEX idx_orders_customer ON Orders(customer_id);
CREATE INDEX idx_orders_station ON Orders(station_id);
CREATE INDEX idx_orders_status ON Orders(status);

-- Payments
CREATE INDEX idx_payments_order ON Payments(order_id);
CREATE INDEX idx_payments_status ON Payments(status);

-- Refunds
CREATE INDEX idx_refunds_order ON Refunds(order_id);
CREATE INDEX idx_refunds_payment ON Refunds(payment_id);
CREATE INDEX idx_refunds_created_by ON Refunds(created_by);

-- Batches
CREATE INDEX idx_batches_variant ON Batches(variant_id);
CREATE INDEX idx_batches_station ON Batches(station_id);
CREATE INDEX idx_batches_expiry ON Batches(expiry_date);

-- Inventories
CREATE INDEX idx_inventories_variant ON Inventories(variant_id);
CREATE INDEX idx_inventories_station ON Inventories(station_id);

-- StockIn
CREATE INDEX idx_stockin_variant ON StockIn(variant_id);
CREATE INDEX idx_stockin_station ON StockIn(station_id);
CREATE INDEX idx_stockin_batch ON StockIn(batch_id);
CREATE INDEX idx_stockin_supplier ON StockIn(supplier_id);

-- StockOut
CREATE INDEX idx_stockout_variant ON StockOut(variant_id);
CREATE INDEX idx_stockout_station ON StockOut(station_id);
CREATE INDEX idx_stockout_order ON StockOut(order_id);

-- DeliveryAssignments
CREATE INDEX idx_delivery_assignments_order ON DeliveryAssignments(order_id);
CREATE INDEX idx_delivery_assignments_driver ON DeliveryAssignments(driver_id);

-- DeliveryRoutes
CREATE INDEX idx_delivery_routes_assignment ON DeliveryRoutes(assignment_id);

-- Tanks
CREATE INDEX idx_tanks_station ON Tanks(station_id);

-- CleaningSchedules
CREATE INDEX idx_cleaningschedules_station ON CleaningSchedules(station_id);
CREATE INDEX idx_cleaningschedules_by ON CleaningSchedules(cleaning_by);

-- PackagingTransactions
CREATE INDEX idx_packaging_transactions_station ON PackagingTransactions(station_id);
CREATE INDEX idx_packaging_transactions_customer ON PackagingTransactions(customer_id);
CREATE INDEX idx_packaging_transactions_packaging ON PackagingTransactions(packaging_id);

-- SystemSettings
CREATE INDEX idx_systemsettings_key ON SystemSettings(setting_key);

-- VariantTypes
CREATE INDEX idx_varianttypes_name ON VariantTypes(name);

-- StockAlerts
CREATE INDEX idx_stockalerts_variant ON StockAlerts(variant_id);
CREATE INDEX idx_stockalerts_station ON StockAlerts(station_id);
CREATE INDEX idx_stockaudits_station ON StockAudits(station_id);
CREATE INDEX idx_stockaudits_variant ON StockAudits(variant_id);

-- StockAudits
CREATE INDEX idx_stockaudits_date ON StockAudits(audit_date);
