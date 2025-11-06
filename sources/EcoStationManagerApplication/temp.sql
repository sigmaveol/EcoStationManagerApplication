-- EcoStation Manager Database V0 (full-onfline-LAN) - MySQL Version
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

-- Users
CREATE TABLE IF NOT EXISTS Users (
    user_id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(100),
    fullname VARCHAR(255),
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng trạm
CREATE TABLE IF NOT EXISTS Stations (
    station_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    address TEXT NOT NULL,
    phone VARCHAR(50),
    station_type VARCHAR(50) DEFAULT 'refill',
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
    PRIMARY KEY(user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES Roles(role_id) ON DELETE CASCADE
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

-- Bảng khách hàng
CREATE TABLE IF NOT EXISTS Customers (
    customer_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    phone VARCHAR(50),
    email VARCHAR(100),
    address TEXT,
    total_point INT DEFAULT 0,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng danh mục sản phẩm
CREATE TABLE IF NOT EXISTS Categories (
    category_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    category_type ENUM('product', 'service', 'other') DEFAULT 'product',
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng sản phẩm
CREATE TABLE IF NOT EXISTS Products (
    product_id INT PRIMARY KEY AUTO_INCREMENT,
    product_code VARCHAR(30) UNIQUE,
    name VARCHAR(255) NOT NULL,
    product_type VARCHAR(50) DEFAULT 'packed',
    price DECIMAL(10,2) NOT NULL,
    unit VARCHAR (50) NOT NULL,
    min_stock_level DECIMAL (10, 2) DEFAULT 15,
    category_id INT,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES Categories(category_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Bảng combo/set sản phẩm
CREATE TABLE IF NOT EXISTS Combos (
    combo_id INT PRIMARY KEY AUTO_INCREMENT,
    code VARCHAR(30) UNIQUE,
    name VARCHAR(255) NOT NULL,
    total_price DECIMAL(10,2) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng thành phần combo
CREATE TABLE IF NOT EXISTS ComboItems (
    combo_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity DECIMAL(10,2) DEFAULT 1,
    PRIMARY KEY (combo_id, product_id),
    FOREIGN KEY (combo_id) REFERENCES Combos(combo_id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES Products(product_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- Bảng đơn hàng
CREATE TABLE IF NOT EXISTS Orders (
    order_id INT PRIMARY KEY AUTO_INCREMENT,
    source VARCHAR(50) NOT NULL, -- googleform, excel, email, manual, other
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0,
    discounted_amount DECIMAL(10,2) DEFAULT 0,
    status VARCHAR(50) DEFAULT 'draft',
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
    order_id INT NOT NULL,
    combo_id INT, -- Nếu là combo
    product_id INT, -- Nếu là product đơn lẻ
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE CASCADE,
    FOREIGN KEY (combo_id) REFERENCES Combos(combo_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
) ENGINE=InnoDB;

-- Bảng thanh toán
CREATE TABLE IF NOT EXISTS Payments (
    payment_id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT,
    amount DECIMAL(10,2) NOT NULL,
    method VARCHAR(50) NOT NULL, -- cash, transfer, ewallet, voucher, other
    status VARCHAR(50) DEFAULT 'unpaid', -- unpaid, paid, refunded, other
    reference VARCHAR(255), -- mã giao dịch / biên nhận
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Bảng lô hàng
CREATE TABLE IF NOT EXISTS Batches (
    batch_id INT PRIMARY KEY AUTO_INCREMENT,
    batch_no VARCHAR(100) NOT NULL,
    product_id INT NOT NULL,
    station_id INT NOT NULL,
    expiry_date DATE,
    current_quantity DECIMAL(10,2) DEFAULT 0,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- Bảng lịch sử kiểm kê
CREATE TABLE IF NOT EXISTS StockAudits (
    audit_id INT PRIMARY KEY AUTO_INCREMENT,
    station_id INT NOT NULL,
    product_id INT NOT NULL,
    batch_id INT NULL,
    system_quantity DECIMAL(10,2) NOT NULL,
    actual_quantity DECIMAL(10,2) NOT NULL,
    variance DECIMAL(10,2) NOT NULL,
    reason TEXT,
    audit_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    audited_by INT NOT NULL,
    status VARCHAR(50) DEFAULT 'pending', -- pending, approved, rejected
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (batch_id) REFERENCES Batches(batch_id),
    FOREIGN KEY (audited_by) REFERENCES Users(user_id),
) ENGINE=InnoDB;

-- Bảng tồn kho tổng
CREATE TABLE IF NOT EXISTS Inventories (
    inventory_id INT PRIMARY KEY AUTO_INCREMENT,
    batch_id INT,
    product_id INT NOT NULL,
    station_id INT NOT NULL,
    current_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (batch_id) REFERENCES Batches(batch_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    UNIQUE KEY uk_station_product(station_id, product_id)
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

-- Bảng nhập kho
CREATE TABLE IF NOT EXISTS StockIn (
    stockin_id INT PRIMARY KEY AUTO_INCREMENT,
    batch_no VARCHAR(100),
    station_id INT NULL, -- nhập vào kho hay trạm
    product_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2),
    notes TEXT,
    supplier_id INT NULL, -- có thể null nếu nhập nội bộ
    expiry_date DATE,
    source_type VARCHAR(50) DEFAULT 'supplier', -- supplier, transfer, return
    quality_check VARCHAR(50) DEFAULT 'pass', -- pass, fail, pending
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng xuất kho
CREATE TABLE IF NOT EXISTS StockOut (
    stockout_id INT PRIMARY KEY AUTO_INCREMENT, 
    batch_no VARCHAR(100),
    product_id INT NOT NULL,
    station_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    purpose VARCHAR(50) NOT NULL, -- sale, transfer, waste, adjustment
    notes TEXT,
    created_by INT NOT NULL,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS DeliveryAssignments (
    assignment_id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT NOT NULL,
    driver_id INT NOT NULL, -- user_id có role 'Driver'
    assigned_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(50) DEFAULT 'pending', -- pending, intransit, delivered, failed
    cod_amount DECIMAL(10,2) DEFAULT 0,
    payment_status VARCHAR(50) DEFAULT 'unpaid', -- unpaid, paid
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
    source VARCHAR(50) DEFAULT 'manualexcel', -- manualexcel, app
    FOREIGN KEY (assignment_id) REFERENCES DeliveryAssignments(assignment_id)
) ENGINE=InnoDB;

-- Bảng lịch vệ sinh
CREATE TABLE IF NOT EXISTS CleaningSchedules (
    cs_id INT PRIMARY KEY AUTO_INCREMENT,
    station_id INT NOT NULL,
    cleaning_type VARCHAR(50) NOT NULL, -- tank, package
    cleaning_date DATETIME NOT NULL,
    cleaning_by INT,
    status VARCHAR(50) DEFAULT 'scheduled', -- scheduled, completed, overdue, cancelled
    notes TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (cleaning_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng bao bì
CREATE TABLE IF NOT EXISTS Packaging (
    packaging_id INT PRIMARY KEY AUTO_INCREMENT,
    product_id INT NOT NULL,
    station_id INT NOT NULL,
    type VARCHAR(50) NULL, -- bottle, box, container
    qr_code VARCHAR(255) UNIQUE,
    status VARCHAR(50) DEFAULT 'new', -- new, inuse, needcleaning, damaged, disposed
    has_qr_code BOOLEAN DEFAULT FALSE,
    time_reused INT DEFAULT 0,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- Bảng giao dịch bao bì
CREATE TABLE IF NOT EXISTS PackagingTransactions (
    transaction_id INT PRIMARY KEY AUTO_INCREMENT,
    packaging_id INT NOT NULL,
    station_id INT NOT NULL,
    type VARCHAR(50) NOT NULL, -- issue, return, clean, dispose
    points_earned INT DEFAULT 0,
    quantity INT NOT NULL DEFAULT 1,
    notes TEXT,
    customer_id INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (packaging_id) REFERENCES Packaging(packaging_id)
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

-- Bảng cấu hình modules: PM open
CREATE TABLE IF NOT EXISTS Modules (
    module_id INT PRIMARY KEY AUTO_INCREMENT,
    module_key VARCHAR(100) UNIQUE NOT NULL,
    name VARCHAR(255) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    config JSON,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- OrderHistory
CREATE TABLE IF NOT EXISTS OrderHistory (
    history_id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT NOT NULL,
    action VARCHAR(100) NOT NULL, -- e.g., created, status_changed, payment_recorded
    old_value TEXT,
    new_value TEXT,
    notes TEXT,
    changed_by INT,
    changed_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE CASCADE,
    FOREIGN KEY (changed_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- InventoryHistory
CREATE TABLE IF NOT EXISTS InventoryHistory (
    inv_hist_id INT PRIMARY KEY AUTO_INCREMENT,
    product_id INT NOT NULL,
    station_id INT NOT NULL,
    change_type VARCHAR(50) NOT NULL, -- stockin, stockout, adjustment
    quantity_change DECIMAL(10,2) NOT NULL,
    reason TEXT,
    changed_by INT,
    changed_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (changed_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Tạo indexes TỐI ƯU NHẤT - chỉ những index thực sự cần thiết

-- === USERS === (Quan trọng: đăng nhập, tìm kiếm)
CREATE INDEX idx_users_username_active ON Users(username, is_active);
CREATE INDEX idx_users_email_active ON Users(email, is_active);

-- === STATIONS === (Quan trọng: quản lý trạm)
CREATE INDEX idx_stations_name_active ON Stations(name, is_active);
CREATE INDEX idx_stations_type_active ON Stations(station_type, is_active);

-- === WORKSHIFTS === (Quan trọng: chấm công)
CREATE UNIQUE INDEX idx_workshifts_user_date ON WorkShifts(user_id, shift_date);

-- === CUSTOMERS === (Quan trọng: tìm kiếm KH)
CREATE INDEX idx_customers_phone_active ON Customers(phone, is_active);
CREATE INDEX idx_customers_name_active ON Customers(name, is_active);

-- === PRODUCTS === (Quan trọng: bán hàng, tìm kiếm)
CREATE INDEX idx_products_search ON Products(name, product_type, is_active);
CREATE INDEX idx_products_code_active ON Products(product_code, is_active);
CREATE INDEX idx_products_category_active ON Products(category_id, is_active);

-- === ORDERS === (QUAN TRỌNG NHẤT: quản lý đơn hàng)
CREATE INDEX idx_orders_status_date ON Orders(status, created_date);
CREATE INDEX idx_orders_customer_date ON Orders(customer_id, created_date);
CREATE INDEX idx_orders_station_date ON Orders(station_id, created_date);
CREATE INDEX idx_orders_user_date ON Orders(user_id, created_date);

-- === ORDER DETAILS === (Quan trọng: thống kê bán hàng)
CREATE INDEX idx_orderdetails_order_product ON OrderDetails(order_id, product_id);

-- === PAYMENTS === (Quan trọng: quản lý thanh toán)
CREATE INDEX idx_payments_order_status ON Payments(order_id, status);
CREATE INDEX idx_payments_date_status ON Payments(created_date, status);

-- === BATCHES === (Quan trọng: quản lý lô hàng)
CREATE UNIQUE INDEX idx_batches_unique ON Batches(batch_no, product_id, station_id);
CREATE INDEX idx_batches_expiry_quantity ON Batches(expiry_date, current_quantity);

-- === INVENTORIES === (QUAN TRỌNG: tồn kho thời gian thực)
CREATE INDEX idx_inventories_station_product ON Inventories(station_id, product_id, current_stock);
CREATE INDEX idx_inventories_low_stock ON Inventories(current_stock);

-- === STOCK IN/OUT === (Quan trọng: nhập xuất kho)
CREATE INDEX idx_stockin_date_product ON StockIn(created_date, product_id);
CREATE INDEX idx_stockout_date_product ON StockOut(created_date, product_id);

-- === DELIVERY === (Quan trọng: giao hàng)
CREATE INDEX idx_deliveryassignments_status_date ON DeliveryAssignments(status, assigned_date);
CREATE INDEX idx_deliveryassignments_order_status ON DeliveryAssignments(order_id, status);

-- === PACKAGING === (Quan trọng: quản lý bao bì)
CREATE INDEX idx_packaging_status_station ON Packaging(status, station_id);
CREATE INDEX idx_packagingtransactions_date_type ON PackagingTransactions(created_date, type);
