-- EcoStation Manager Database Schema - MySQL Version
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
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng trạm (tạo TRƯỚC Users - không có foreign key)
CREATE TABLE IF NOT EXISTS Stations (
    station_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    address TEXT NOT NULL,
    phone VARCHAR(50),
    station_type ENUM('warehouse', 'refill', 'hybrid') DEFAULT 'refill',
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    manager INT, -- User_id của quản lý trạm / -- Sẽ thêm FK sau
    parent_station_id INT NULL DEFAULT NULL
) ENGINE=InnoDB;

-- Bảng người dùng
CREATE TABLE IF NOT EXISTS Users (
    user_id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(100) NOT NULL,
    fullname VARCHAR(255) NOT NULL,
    is_active BOOLEAN DEFAULT FALSE,
    station_id INT NULL,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- Thêm khóa ngoại cho Stations SAU KHI có Users
ALTER TABLE Stations 
ADD FOREIGN KEY (manager) REFERENCES Users(user_id),
ADD FOREIGN KEY (parent_station_id) REFERENCES Stations(station_id);

-- Bảng gán Roles cho Users
CREATE TABLE IF NOT EXISTS UserRoles (
    user_id INT NOT NULL,
    role_id INT NOT NULL,
    PRIMARY KEY(user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
) ENGINE=InnoDB;

CREATE TABLE WorkShifts (
    shift_id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    shift_date DATE NOT NULL,
    start_time TIME,
    end_time TIME,
    kpi_score DECIMAL(5,2) DEFAULT NULL,
    notes TEXT,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng nhà cung cấp
CREATE TABLE IF NOT EXISTS Suppliers (
    supplier_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    address TEXT,
    phone VARCHAR(50),
    email VARCHAR(100),
    fax VARCHAR(50),
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
    status ENUM('Active', 'Inactive', 'Suspended') DEFAULT 'Active'
) ENGINE=InnoDB;

-- Bảng danh mục sản phẩm
CREATE TABLE IF NOT EXISTS Categories (
    category_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    image VARCHAR(255),
    description TEXT,
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
    code VARCHAR(30) UNIQUE,
    name VARCHAR(255) NOT NULL,
    base_price DECIMAL(10,2) DEFAULT NULL,
    unit_measure VARCHAR(50) NOT NULL,
    description TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    category_id INT,
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS VariantTypes (
    type_id INT AUTO_INCREMENT PRIMARY KEY,
    code VARCHAR(30) UNIQUE NOT NULL,
    name VARCHAR(50) NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE
) ENGINE=InnoDB;

-- Bảng biến thể sản phẩm
CREATE TABLE IF NOT EXISTS Variants (
    variant_id INT AUTO_INCREMENT PRIMARY KEY,
    barcode VARCHAR(20) UNIQUE, -- Barcode 13 chữ số chuẩn EAN13
    SKU VARCHAR(50) UNIQUE NOT NULL,
    name VARCHAR(255) NOT NULL,
    base_price DECIMAL(10,2) NOT NULL,
    unit VARCHAR(50) NOT NULL,
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
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng thành phần combo
CREATE TABLE IF NOT EXISTS ComboItems (
    combo_item_id INT PRIMARY KEY AUTO_INCREMENT,
    quantity DECIMAL(10,2) NOT NULL,
    combo_id INT NOT NULL,
    variant_id INT NOT NULL,
    FOREIGN KEY (combo_id) REFERENCES Combos(combo_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Transfers (
    transfer_id INT PRIMARY KEY AUTO_INCREMENT,
    from_station_id INT NOT NULL,
    to_station_id INT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    status ENUM('pending', 'in_transit', 'completed', 'cancelled') DEFAULT 'pending',
    FOREIGN KEY (from_station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (to_station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS TransferDetails (
    transfer_detail_id INT PRIMARY KEY AUTO_INCREMENT,
    transfer_id INT NOT NULL,
    variant_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (transfer_id) REFERENCES Transfers(transfer_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id)
) ENGINE=InnoDB;

-- Bảng đơn hàng
CREATE TABLE IF NOT EXISTS Orders (
    order_id INT PRIMARY KEY AUTO_INCREMENT,
    source ENUM('GoogleForm', 'Excel', 'Email', 'Manual') NOT NULL,
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0,
    discounted_amount DECIMAL(10,2) DEFAULT 0,
    tax_amount DECIMAL(10,2) DEFAULT 0,
    status ENUM('Draft', 'Confirmed', 'Processing', 'Ready', 'Shipped', 'Completed', 'Cancelled', 'Returned') DEFAULT 'Draft',
    note TEXT,  
    payment_method ENUM('Cash', 'Transfer', 'EWallet') DEFAULT 'Cash',
    payment_status ENUM('Unpaid', 'Paid', 'Partial', 'Refunded') DEFAULT 'Unpaid',
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
    order_detail_id INT PRIMARY KEY AUTO_INCREMENT,
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL,
    line_total DECIMAL(10,2) NOT NULL,
    order_id INT NOT NULL,
    combo_id INT, -- Nếu là combo
    variant_id INT, -- Nếu là variant đơn lẻ
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (combo_id) REFERENCES Combos(combo_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id)
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
    quality_status ENUM('Good', 'Pending', 'Expired', 'Rejected') DEFAULT 'Good',
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
    UNIQUE (variant_id, station_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- Bảng tồn kho tổng
CREATE TABLE IF NOT EXISTS Inventories (
    inventory_id INT PRIMARY KEY AUTO_INCREMENT,
    batch_id INT NOT NULL,
    variant_id INT NOT NULL,
    station_id INT NOT NULL,
    current_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    reserved_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    min_stock DECIMAL(10,2) DEFAULT 15,
    max_stock DECIMAL(10,2) DEFAULT 1000,
    last_update DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (batch_id) REFERENCES Batches(batch_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- Bảng nhập kho
CREATE TABLE IF NOT EXISTS StockIn (
    stockin_id INT PRIMARY KEY AUTO_INCREMENT,
    reference_number VARCHAR(100) UNIQUE,
    batch_id INT NOT NULL,
    station_id INT NULL, -- nhập vào kho hay trạm
    variant_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2),
    supplier_id INT NULL, -- có thể null nếu nhập nội bộ
    batch_no VARCHAR(100),
    expiry_date DATE,
    source_type ENUM('supplier', 'transfer', 'return') DEFAULT 'supplier', -- return: khách refunds
    quality_check ENUM('Pass', 'Fail', 'Pending') DEFAULT 'Pass',
    notes TEXT,
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (batch_id) REFERENCES Batches(batch_id),
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
    purpose ENUM('Sale', 'Transfer', 'Waste', 'Adjustment') NOT NULL,
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
    status ENUM('Pending','InTransit','Delivered','Failed') DEFAULT 'Pending',
    notes TEXT,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (driver_id) REFERENCES Users(user_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS DeliveryRoutes (
    route_id INT PRIMARY KEY AUTO_INCREMENT,
    assignment_id INT NOT NULL,
    latitude DECIMAL(10,7),
    longitude DECIMAL(10,7),
    recorded_time DATETIME,
    source ENUM('ManualExcel','App') DEFAULT 'ManualExcel',
    FOREIGN KEY (assignment_id) REFERENCES DeliveryAssignments(assignment_id)
) ENGINE=InnoDB;

-- Bảng bồn chứa
CREATE TABLE IF NOT EXISTS Tanks (
    tank_id INT PRIMARY KEY AUTO_INCREMENT,
    station_id INT NOT NULL,
    name VARCHAR(100) NOT NULL,
    material ENUM('Glass', 'Plastic', 'Metal') DEFAULT 'Plastic',
    capacity DECIMAL(10,2) NOT NULL,
    unit ENUM('ml', 'liter', 'kg') DEFAULT 'liter',
    current_level DECIMAL(10,2) DEFAULT 0,
    status ENUM('Active', 'Maintenance', 'OutOfOrder') DEFAULT 'Active',
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
    cleaning_type ENUM('Tank', 'Package') NOT NULL,
    cleaning_date DATETIME NOT NULL,
    cleaning_by INT,
    status ENUM('Scheduled', 'Completed', 'Overdue', 'Cancelled') DEFAULT 'Scheduled',
    notes TEXT,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (cleaning_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng bao bì
CREATE TABLE IF NOT EXISTS Packaging (
    packaging_id INT PRIMARY KEY AUTO_INCREMENT,
    type ENUM('Bottle', 'Box', 'Container') NOT NULL,
    qr_code VARCHAR(255) UNIQUE,
    status ENUM('New', 'InUse', 'NeedCleaning', 'Damaged', 'Disposed') DEFAULT 'New',
    has_qr_code BOOLEAN DEFAULT FALSE,
    time_reused INT DEFAULT 0,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng thanh toán
CREATE TABLE IF NOT EXISTS Payments (
    payment_id INT PRIMARY KEY AUTO_INCREMENT,
    payment_number VARCHAR(100) UNIQUE NOT NULL,
    order_id INT NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    method ENUM('Cash', 'Transfer', 'EWallet') NOT NULL,
    status ENUM('Pending', 'Completed', 'Failed', 'Refunded') DEFAULT 'Pending',
    reference VARCHAR(100),
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id)
) ENGINE=InnoDB;

-- Bảng hoàn tiền
CREATE TABLE IF NOT EXISTS Refunds (
    refund_id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT NOT NULL,
    payment_id INT,
    refund_number VARCHAR(100) UNIQUE NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    reason TEXT,
    status ENUM('Pending', 'Completed', 'Failed') DEFAULT 'Pending',
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (payment_id) REFERENCES Payments(payment_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng giao dịch bao bì
CREATE TABLE IF NOT EXISTS PackagingTransactions (
    transaction_id INT PRIMARY KEY AUTO_INCREMENT,
    station_id INT NOT NULL,
    type ENUM('Issue', 'Return', 'Clean', 'Dispose') NOT NULL,
    points_earned INT DEFAULT 0,
    date DATETIME DEFAULT CURRENT_TIMESTAMP,
    customer_id INT,
    packaging_id INT,
    quantity INT NOT NULL DEFAULT 1,
    notes TEXT,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (packaging_id) REFERENCES Packaging(packaging_id)
) ENGINE=InnoDB;

--- Bảng cấu hình modules -> PM open
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
    data_type ENUM('String', 'Integer', 'Decimal', 'Boolean', 'Date') DEFAULT 'String',
    description TEXT,
    updated_by INT,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (updated_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Tạo indexes để tối ưu hiệu suất
CREATE INDEX idx_userroles_user ON UserRoles(user_id);
CREATE INDEX idx_userroles_role ON UserRoles(role_id);
CREATE INDEX idx_users_station ON Users(station_id);
CREATE INDEX idx_stations_parent ON Stations(parent_station_id);
CREATE INDEX idx_suppliers_name ON Suppliers(name);
CREATE INDEX idx_customers_phone ON Customers(phone);
CREATE INDEX idx_customers_email ON Customers(email);
CREATE INDEX idx_users_username ON Users(username);
CREATE INDEX idx_categories_parent ON Categories(parent_id);
CREATE INDEX idx_products_category ON Products(category_id);
CREATE INDEX idx_variants_sku ON Variants(SKU);
CREATE INDEX idx_variants_product ON Variants(product_id);
CREATE INDEX idx_orders_date ON Orders(created_date);
CREATE INDEX idx_orders_status ON Orders(status);
CREATE INDEX idx_orders_customer ON Orders(customer_id);
CREATE INDEX idx_orders_station ON Orders(station_id);
CREATE INDEX idx_orderdetails_order ON OrderDetails(order_id);
CREATE INDEX idx_inventories_variant ON Inventories(variant_id);
CREATE INDEX idx_stockin_date ON StockIn(created_date);
CREATE INDEX idx_stockin_supplier ON StockIn(supplier_id);
CREATE INDEX idx_stockout_date ON StockOut(created_date);
CREATE INDEX idx_stockout_order ON StockOut(order_id);
CREATE INDEX idx_stations_manager ON Stations(manager);
CREATE INDEX idx_tanks_station ON Tanks(station_id);
CREATE INDEX idx_tanks_status ON Tanks(status);
CREATE INDEX idx_cleaning_station ON CleaningSchedules(station_id);
CREATE INDEX idx_cleaning_date ON CleaningSchedules(cleaning_date);
CREATE INDEX idx_packaging_type ON Packaging(type);
CREATE INDEX idx_packaging_status ON Packaging(status);
CREATE INDEX idx_packagingtransactions_station ON PackagingTransactions(station_id);
CREATE INDEX idx_packagingtransactions_date ON PackagingTransactions(date);
CREATE INDEX idx_payments_order ON Payments(order_id);
CREATE INDEX idx_refunds_order ON Refunds(order_id);
CREATE INDEX idx_systemsettings_key ON SystemSettings(setting_key);