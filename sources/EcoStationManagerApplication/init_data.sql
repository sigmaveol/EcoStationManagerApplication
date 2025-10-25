-- EcoStation Manager Database Schema - MySQL Version
-- Engine: InnoDB, Charset: utf8mb4

CREATE DATABASE IF NOT EXISTS EcoStationManager
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE EcoStationManager;

-- Bảng vai trò
CREATE TABLE IF NOT EXISTS Roles (
    roleID INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) UNIQUE NOT NULL,
    permissions JSON NOT NULL,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng người dùng
CREATE TABLE IF NOT EXISTS Users (
    userID INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(100) NOT NULL,
    fullname VARCHAR(255) NOT NULL,
    is_active BOOLEAN DEFAULT FALSE,
    roleID INT NOT NULL,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (roleID) REFERENCES Roles(roleID)
) ENGINE=InnoDB;

-- Bảng nhà cung cấp
CREATE TABLE IF NOT EXISTS Suppliers (
    supplierID INT PRIMARY KEY AUTO_INCREMENT,
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
    customerID INT PRIMARY KEY AUTO_INCREMENT,
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
    categoryID INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    image VARCHAR(255),
    description TEXT,
    parent_id INT,
    sort_order INT DEFAULT 0,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (parent_id) REFERENCES Categories(categoryID)
) ENGINE=InnoDB;

-- Bảng sản phẩm
CREATE TABLE IF NOT EXISTS Products (
    productID INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    base_price DECIMAL(10,2) DEFAULT NULL,
    unit_measure VARCHAR(50) NOT NULL,
    description TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    categoryID INT,
    FOREIGN KEY (categoryID) REFERENCES Categories(categoryID)
) ENGINE=InnoDB;

-- Bảng biến thể sản phẩm
CREATE TABLE IF NOT EXISTS Variants (
    variantID INT AUTO_INCREMENT PRIMARY KEY,
    barcode VARCHAR(20) UNIQUE NOT NULL, -- Barcode 13 chữ số chuẩn EAN13
    SKU VARCHAR(50) UNIQUE NOT NULL,
    name VARCHAR(255) NOT NULL,
    base_price DECIMAL(10,2) NOT NULL,
    unit VARCHAR(50) NOT NULL,
    min_stock DECIMAL(10,2) DEFAULT 10,
    max_stock DECIMAL(10,2) DEFAULT 1000,
    qr_code VARCHAR(255) UNIQUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    productID INT NOT NULL,
    FOREIGN KEY (productID) REFERENCES Products(productID)
) ENGINE=InnoDB;

-- Bảng combo/set sản phẩm
CREATE TABLE IF NOT EXISTS Combos (
    comboID INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng thành phần combo
CREATE TABLE IF NOT EXISTS ComboItems (
    combo_itemID INT PRIMARY KEY AUTO_INCREMENT,
    quantity DECIMAL(10,2) NOT NULL,
    comboID INT NOT NULL,
    variantID INT NOT NULL,
    FOREIGN KEY (comboID) REFERENCES Combos(comboID),
    FOREIGN KEY (variantID) REFERENCES Variants(variantID)
) ENGINE=InnoDB;

-- Bảng trạm
CREATE TABLE IF NOT EXISTS Stations (
    stationID INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    address TEXT NOT NULL,
    manager INT, -- UserID của quản lý trạm
    phone VARCHAR(50),
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (manager) REFERENCES Users(userID)
) ENGINE=InnoDB;

-- Bảng đơn hàng
CREATE TABLE IF NOT EXISTS Orders (
    orderID INT PRIMARY KEY AUTO_INCREMENT,
    source ENUM('GoogleForm', 'Excel', 'Email', 'Manual') NOT NULL,
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0,
    discounted_amount DECIMAL(10,2) DEFAULT 0,
    tax_amount DECIMAL(10,2) DEFAULT 0,
    status ENUM('Draft', 'Confirmed', 'Processing', 'Ready', 'Shipped', 'Completed', 'Cancelled', 'Returned') DEFAULT 'Draft',
    note TEXT,  
    payment_method ENUM('Cash', 'Transfer', 'EWallet') DEFAULT 'Cash',
    payment_status ENUM('Unpaid', 'Paid', 'Partial', 'Refunded') DEFAULT 'Unpaid',
    userID INT, -- Người tạo đơn
    customerID INT,
    stationID INT, -- Trạm xử lý đơn/ nếu mua tại trạm
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (userID) REFERENCES Users(userID),
    FOREIGN KEY (customerID) REFERENCES Customers(customerID),
    FOREIGN KEY (stationID) REFERENCES Stations(stationID)
) ENGINE=InnoDB;

-- Bảng chi tiết đơn hàng
CREATE TABLE IF NOT EXISTS OrderDetails (
    order_detailID INT PRIMARY KEY AUTO_INCREMENT,
    orderID INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL,
    comboID INT, -- Nếu là combo
    variantID INT, -- Nếu là variant đơn lẻ
    line_total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (orderID) REFERENCES Orders(orderID),
    FOREIGN KEY (comboID) REFERENCES Combos(comboID),
    FOREIGN KEY (variantID) REFERENCES Variants(variantID)
) ENGINE=InnoDB;

-- Bảng tồn kho tổng
CREATE TABLE IF NOT EXISTS Inventories (
    inventoryID INT PRIMARY KEY AUTO_INCREMENT,
    productID INT NULL,
    variantID INT NOT NULL,
    current_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    reserved_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    last_update DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (productID) REFERENCES Products(productID),
    FOREIGN KEY (variantID) REFERENCES Variants(variantID)
) ENGINE=InnoDB;

-- Bảng nhập kho
CREATE TABLE IF NOT EXISTS StockIn (
    stockinID INT PRIMARY KEY AUTO_INCREMENT,
    reference_number VARCHAR(100) UNIQUE,
    productID INT NULL,
    variantID INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2),
    supplierID INT,
    batch_no VARCHAR(100),
    expiry_date DATE,
    quality_check ENUM('Pass', 'Fail', 'Pending') DEFAULT 'Pass',
    notes TEXT,
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (productID) REFERENCES Products(productID),
    FOREIGN KEY (variantID) REFERENCES Variants(variantID),
    FOREIGN KEY (supplierID) REFERENCES Suppliers(supplierID),
    FOREIGN KEY (created_by) REFERENCES Users(userID)
) ENGINE=InnoDB;

-- Bảng xuất kho
CREATE TABLE IF NOT EXISTS StockOut (
    stockoutID INT PRIMARY KEY AUTO_INCREMENT,
    productID INT NULL,
    variantID INT NOT NULL,
    orderID INT,
    stationID INT,
    quantity DECIMAL(10,2) NOT NULL,
    purpose ENUM('Sale', 'Transfer', 'Waste', 'Adjustment') NOT NULL,
    reason TEXT,
    notes TEXT,
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (productID) REFERENCES Products(productID),
    FOREIGN KEY (variantID) REFERENCES Variants(variantID),
    FOREIGN KEY (orderID) REFERENCES Orders(orderID),
    FOREIGN KEY (stationID) REFERENCES Stations(stationID),
    FOREIGN KEY (created_by) REFERENCES Users(userID)
) ENGINE=InnoDB;

-- Bảng tồn kho theo trạm
CREATE TABLE IF NOT EXISTS StationInventories (
    station_inventoryID INT PRIMARY KEY AUTO_INCREMENT,
    stationID INT NOT NULL,
    productID INT NULL,
    variantID INT NOT NULL,
    current_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    reserved_stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    last_update DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (stationID) REFERENCES Stations(stationID),
    FOREIGN KEY (productID) REFERENCES Products(productID),
    FOREIGN KEY (variantID) REFERENCES Variants(variantID)
) ENGINE=InnoDB;

-- Bảng bồn chứa
CREATE TABLE IF NOT EXISTS Tanks (
    tankID INT PRIMARY KEY AUTO_INCREMENT,
    stationID INT NOT NULL,
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
    FOREIGN KEY (stationID) REFERENCES Stations(stationID)
) ENGINE=InnoDB;

-- Bảng lịch vệ sinh
CREATE TABLE IF NOT EXISTS CleaningSchedules (
    csID INT PRIMARY KEY AUTO_INCREMENT,
    stationID INT NOT NULL,
    cleaning_type ENUM('Tank', 'Package') NOT NULL,
    cleaning_date DATETIME NOT NULL,
    cleaning_by INT,
    status ENUM('Scheduled', 'Completed', 'Overdue', 'Cancelled') DEFAULT 'Scheduled',
    notes TEXT,
    FOREIGN KEY (stationID) REFERENCES Stations(stationID),
    FOREIGN KEY (cleaning_by) REFERENCES Users(userID)
) ENGINE=InnoDB;

-- Bảng bao bì
CREATE TABLE IF NOT EXISTS Packaging (
    packagingID INT PRIMARY KEY AUTO_INCREMENT,
    type ENUM('Bottle', 'Box', 'Container') NOT NULL,
    qr_code VARCHAR(255) UNIQUE,
    status ENUM('New', 'InUse', 'NeedCleaning', 'Damaged', 'Disposed') DEFAULT 'New',
    has_qr_code BOOLEAN DEFAULT FALSE,
    time_reused INT DEFAULT 0,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng giao dịch bao bì
CREATE TABLE IF NOT EXISTS PackagingTransactions (
    transactionID INT PRIMARY KEY AUTO_INCREMENT,
    stationID INT NOT NULL,
    type ENUM('Issue', 'Return', 'Clean', 'Dispose') NOT NULL,
    points_earned INT DEFAULT 0,
    date DATETIME DEFAULT CURRENT_TIMESTAMP,
    customerID INT,
    packagingID INT,
    quantity INT NOT NULL DEFAULT 1,
    notes TEXT,
    FOREIGN KEY (stationID) REFERENCES Stations(stationID),
    FOREIGN KEY (customerID) REFERENCES Customers(customerID),
    FOREIGN KEY (packagingID) REFERENCES Packaging(packagingID)
) ENGINE=InnoDB;

-- Bảng thanh toán
CREATE TABLE IF NOT EXISTS Payments (
    paymentID INT PRIMARY KEY AUTO_INCREMENT,
    payment_number VARCHAR(100) UNIQUE NOT NULL,
    orderID INT NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    method ENUM('Cash', 'Transfer', 'EWallet') NOT NULL,
    status ENUM('Pending', 'Completed', 'Failed', 'Refunded') DEFAULT 'Pending',
    reference VARCHAR(100),
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (orderID) REFERENCES Orders(orderID)
) ENGINE=InnoDB;

-- Bảng hoàn tiền
CREATE TABLE IF NOT EXISTS Refunds (
    refundID INT PRIMARY KEY AUTO_INCREMENT,
    orderID INT NOT NULL,
    paymentID INT,
    refund_number VARCHAR(100) UNIQUE NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    reason TEXT,
    status ENUM('Pending', 'Completed', 'Failed') DEFAULT 'Pending',
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (orderID) REFERENCES Orders(orderID),
    FOREIGN KEY (paymentID) REFERENCES Payments(paymentID),
    FOREIGN KEY (created_by) REFERENCES Users(userID)
) ENGINE=InnoDB;

-- Bảng cài đặt hệ thống
CREATE TABLE IF NOT EXISTS SystemSettings (
    ssID INT PRIMARY KEY AUTO_INCREMENT,
    setting_key VARCHAR(100) UNIQUE NOT NULL,
    setting_value TEXT NOT NULL,
    data_type ENUM('String', 'Integer', 'Decimal', 'Boolean', 'Date') DEFAULT 'String',
    description TEXT,
    updated_by INT,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (updated_by) REFERENCES Users(userID)
) ENGINE=InnoDB;

-- Tạo indexes để tối ưu hiệu suất
CREATE INDEX idx_suppliers_name ON Suppliers(name);
CREATE INDEX idx_customers_phone ON Customers(phone);
CREATE INDEX idx_customers_email ON Customers(email);
CREATE INDEX idx_users_username ON Users(username);
CREATE INDEX idx_users_role ON Users(roleID);
CREATE INDEX idx_categories_parent ON Categories(parent_id);
CREATE INDEX idx_products_category ON Products(categoryID);
CREATE INDEX idx_variants_sku ON Variants(SKU);
CREATE INDEX idx_variants_product ON Variants(productID);
CREATE INDEX idx_orders_date ON Orders(created_date);
CREATE INDEX idx_orders_status ON Orders(status);
CREATE INDEX idx_orders_customer ON Orders(customerID);
CREATE INDEX idx_orders_station ON Orders(stationID);
CREATE INDEX idx_orderdetails_order ON OrderDetails(orderID);
CREATE INDEX idx_inventories_product ON Inventories(productID);
CREATE INDEX idx_inventories_variant ON Inventories(variantID);
CREATE INDEX idx_stockin_date ON StockIn(created_date);
CREATE INDEX idx_stockin_supplier ON StockIn(supplierID);
CREATE INDEX idx_stockout_date ON StockOut(created_date);
CREATE INDEX idx_stockout_order ON StockOut(orderID);
CREATE INDEX idx_stations_manager ON Stations(manager);
CREATE INDEX idx_stationinventories_station ON StationInventories(stationID);
CREATE INDEX idx_stationinventories_product ON StationInventories(productID);
CREATE INDEX idx_tanks_station ON Tanks(stationID);
CREATE INDEX idx_tanks_status ON Tanks(status);
CREATE INDEX idx_cleaning_station ON CleaningSchedules(stationID);
CREATE INDEX idx_cleaning_date ON CleaningSchedules(cleaning_date);
CREATE INDEX idx_packaging_type ON Packaging(type);
CREATE INDEX idx_packaging_status ON Packaging(status);
CREATE INDEX idx_packagingtransactions_station ON PackagingTransactions(stationID);
CREATE INDEX idx_packagingtransactions_date ON PackagingTransactions(date);
CREATE INDEX idx_payments_order ON Payments(orderID);
CREATE INDEX idx_refunds_order ON Refunds(orderID);
CREATE INDEX idx_systemsettings_key ON SystemSettings(setting_key);


-- Dữ liệu mẫu cho vai trò
INSERT IGNORE INTO Roles (name, permissions) VALUES
('Admin', '{"Dashboard":{"View":true},"Suppliers":{"View":true,"Create":true,"Edit":true,"Delete":true},"Customers":{"View":true,"Create":true,"Edit":true,"Delete":true},"Products":{"View":true,"Create":true,"Edit":true,"Delete":true},"Combos":{"View":true,"Create":true,"Edit":true,"Delete":true},"Orders":{"View":true,"Create":true,"Edit":true,"Delete":true,"Approve":true},"OrderDetails":{"View":true,"Create":true,"Edit":true,"Delete":true},"Payments":{"View":true,"Create":true,"Edit":true,"Delete":true},"Inventories":{"View":true,"Create":true,"Edit":true,"Delete":true,"Adjust":true},"StockIn":{"View":true,"Create":true,"Edit":true,"Delete":true},"StockOut":{"View":true,"Create":true,"Edit":true,"Delete":true},"Stations":{"View":true,"Create":true,"Edit":true,"Delete":true},"StationInventories":{"View":true,"Create":true,"Edit":true,"Delete":true},"Tanks":{"View":true,"Create":true,"Edit":true,"Delete":true},"CleaningSchedules":{"View":true,"Create":true,"Edit":true,"Delete":true},"Packaging":{"View":true,"Create":true,"Edit":true,"Delete":true},"PackagingTransactions":{"View":true,"Create":true,"Edit":true,"Delete":true},"Reports":{"View":true,"Export":true},"Users":{"View":true,"Create":true,"Edit":true,"Delete":true},"SystemSettings":{"View":true,"Edit":true}}'),
('Manager', '{"Dashboard":{"View":true},"Suppliers":{"View":true,"Create":true,"Edit":true,"Delete":true},"Customers":{"View":true,"Create":true,"Edit":true,"Delete":true},"Products":{"View":true,"Create":true,"Edit":true,"Delete":true},"Combos":{"View":true,"Create":true,"Edit":true,"Delete":true},"Orders":{"View":true,"Create":true,"Edit":true,"Delete":false,"Approve":true},"OrderDetails":{"View":true,"Create":true,"Edit":true,"Delete":true},"Payments":{"View":true,"Create":true,"Edit":true,"Delete":true},"Inventories":{"View":true,"Create":true,"Edit":true,"Delete":true,"Adjust":true},"StockIn":{"View":true,"Create":true,"Edit":true,"Delete":true},"StockOut":{"View":true,"Create":true,"Edit":true,"Delete":true},"Stations":{"View":true,"Create":true,"Edit":true,"Delete":true},"StationInventories":{"View":true,"Create":true,"Edit":true,"Delete":true},"Tanks":{"View":true,"Create":true,"Edit":true,"Delete":true},"CleaningSchedules":{"View":true,"Create":true,"Edit":true,"Delete":true},"Packaging":{"View":true,"Create":true,"Edit":true,"Delete":true},"PackagingTransactions":{"View":true,"Create":true,"Edit":true,"Delete":true},"Reports":{"View":true,"Export":true},"Users":{"View":false,"Create":false,"Edit":false,"Delete":false},"SystemSettings":{"View":false,"Edit":false}}'),
('WarehouseStaff', '{"Dashboard":{"View":true,"Limited":true},"Suppliers":{"View":false,"Create":false,"Edit":false,"Delete":false},"Customers":{"View":false,"Create":false,"Edit":false,"Delete":false},"Products":{"View":true,"Create":false,"Edit":false,"Delete":false},"Combos":{"View":false,"Create":false,"Edit":false,"Delete":false},"Orders":{"View":false,"Create":false,"Edit":false,"Delete":false,"Approve":false},"OrderDetails":{"View":false,"Create":false,"Edit":false,"Delete":false},"Payments":{"View":false,"Create":false,"Edit":false,"Delete":false},"Inventories":{"View":true,"Create":true,"Edit":true,"Delete":false,"Adjust":true},"StockIn":{"View":true,"Create":true,"Edit":true,"Delete":true},"StockOut":{"View":true,"Create":true,"Edit":true,"Delete":true},"Stations":{"View":false,"Create":false,"Edit":false,"Delete":false},"StationInventories":{"View":true,"Create":true,"Edit":true,"Delete":true},"Tanks":{"View":false,"Create":false,"Edit":false,"Delete":false},"CleaningSchedules":{"View":false,"Create":false,"Edit":false,"Delete":false},"Packaging":{"View":true,"Create":true,"Edit":true,"Delete":true},"PackagingTransactions":{"View":true,"Create":true,"Edit":true,"Delete":true},"Reports":{"View":true,"Export":true,"Limited":true},"Users":{"View":false,"Create":false,"Edit":false,"Delete":false},"SystemSettings":{"View":false,"Edit":false}}'),
('Cashier', '{"Dashboard":{"View":true,"Limited":true},"Suppliers":{"View":false,"Create":false,"Edit":false,"Delete":false},"Customers":{"View":true,"Create":true,"Edit":true,"Delete":false},"Products":{"View":true,"Create":false,"Edit":false,"Delete":false},"Combos":{"View":true,"Create":false,"Edit":false,"Delete":false},"Orders":{"View":true,"Create":true,"Edit":true,"Delete":false,"Approve":false},"OrderDetails":{"View":true,"Create":true,"Edit":true,"Delete":false},"Payments":{"View":true,"Create":true,"Edit":true,"Delete":false},"Inventories":{"View":false,"Create":false,"Edit":false,"Delete":false,"Adjust":false},"StockIn":{"View":false,"Create":false,"Edit":false,"Delete":false},"StockOut":{"View":false,"Create":false,"Edit":false,"Delete":false},"Stations":{"View":false,"Create":false,"Edit":false,"Delete":false},"StationInventories":{"View":false,"Create":false,"Edit":false,"Delete":false},"Tanks":{"View":false,"Create":false,"Edit":false,"Delete":false},"CleaningSchedules":{"View":false,"Create":false,"Edit":false,"Delete":false},"Packaging":{"View":true,"Create":true,"Edit":true,"Delete":true},"PackagingTransactions":{"View":true,"Create":true,"Edit":true,"Delete":true},"Reports":{"View":true,"Export":true,"Limited":true},"Users":{"View":false,"Create":false,"Edit":false,"Delete":false},"SystemSettings":{"View":false,"Edit":false}}'),
('Cleaner', '{"Dashboard":{"View":false,"Create":false,"Edit":false,"Delete":false},"Suppliers":{"View":false,"Create":false,"Edit":false,"Delete":false},"Customers":{"View":false,"Create":false,"Edit":false,"Delete":false},"Products":{"View":false,"Create":false,"Edit":false,"Delete":false},"Combos":{"View":false,"Create":false,"Edit":false,"Delete":false},"Orders":{"View":false,"Create":false,"Edit":false,"Delete":false,"Approve":false},"OrderDetails":{"View":false,"Create":false,"Edit":false,"Delete":false},"Payments":{"View":false,"Create":false,"Edit":false,"Delete":false},"Inventories":{"View":false,"Create":false,"Edit":false,"Delete":false,"Adjust":false},"StockIn":{"View":false,"Create":false,"Edit":false,"Delete":false},"StockOut":{"View":false,"Create":false,"Edit":false,"Delete":false},"Stations":{"View":false,"Create":false,"Edit":false,"Delete":false},"StationInventories":{"View":false,"Create":false,"Edit":false,"Delete":false},"Tanks":{"View":true,"Create":false,"Edit":true,"Delete":false},"CleaningSchedules":{"View":true,"Create":true,"Edit":true,"Delete":false},"Packaging":{"View":true,"Create":true,"Edit":true,"Delete":true},"PackagingTransactions":{"View":true,"Create":true,"Edit":true,"Delete":true},"Reports":{"View":false,"Export":false},"Users":{"View":false,"Create":false,"Edit":false,"Delete":false},"SystemSettings":{"View":false,"Edit":false}}'),
('Customer', '{"Dashboard":{"View":false,"Create":false,"Edit":false,"Delete":false},"Suppliers":{"View":false,"Create":false,"Edit":false,"Delete":false},"Customers":{"View":true,"Create":false,"Edit":true,"Delete":false,"OwnOnly":true},"Products":{"View":false,"Create":false,"Edit":false,"Delete":false},"Combos":{"View":false,"Create":false,"Edit":false,"Delete":false},"Orders":{"View":true,"Create":false,"Edit":false,"Delete":false,"Approve":false,"OwnOnly":true},"OrderDetails":{"View":false,"Create":false,"Edit":false,"Delete":false},"Payments":{"View":true,"Create":false,"Edit":false,"Delete":false,"OwnOnly":true},"Inventories":{"View":false,"Create":false,"Edit":false,"Delete":false,"Adjust":false},"StockIn":{"View":false,"Create":false,"Edit":false,"Delete":false},"StockOut":{"View":false,"Create":false,"Edit":false,"Delete":false},"Stations":{"View":false,"Create":false,"Edit":false,"Delete":false},"StationInventories":{"View":false,"Create":false,"Edit":false,"Delete":false},"Tanks":{"View":false,"Create":false,"Edit":false,"Delete":false},"CleaningSchedules":{"View":false,"Create":false,"Edit":false,"Delete":false},"Packaging":{"View":true,"Create":false,"Edit":false,"Delete":false,"PointsOnly":true},"PackagingTransactions":{"View":true,"Create":false,"Edit":false,"Delete":false,"PointsOnly":true},"Reports":{"View":false,"Export":false},"Users":{"View":false,"Create":false,"Edit":false,"Delete":false},"SystemSettings":{"View":false,"Edit":false}}');

-- Dữ liệu mẫu cho người dùng (tạm thời không có stationID)
INSERT IGNORE INTO Users (username, password_hash, fullname, roleID, is_active) VALUES
('admin', '$2b$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Admin User', 1, TRUE),
('manager', '$2b$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Manager User', 2, TRUE),
('warehouse', '$2b$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Warehouse Staff', 3, TRUE),
('cashier', '$2b$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Cashier Staff', 4, TRUE),
('cleaner', '$2b$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Cleaner Staff', 5, TRUE);

-- Dữ liệu mẫu cho trạm
INSERT IGNORE INTO Stations (name, address, manager, phone, is_active) VALUES
('Trạm EcoStation 1', '123 Green Street, District 1', 1, '0123456789', TRUE),
('Trạm EcoStation 2', '456 Eco Avenue, District 2', 2, '0987654321', TRUE),
('Trạm EcoStation 3', '789 Nature Road, District 3', 2, '0555666777', TRUE);

-- Dữ liệu mẫu cho danh mục
INSERT IGNORE INTO Categories (name, description, sort_order) VALUES
('Hạt/Ngũ cốc', 'Các loại hạt và ngũ cốc', 1),
('Làm đẹp', 'Sản phẩm chăm sóc sắc đẹp', 2),
('Tẩy rửa', 'Sản phẩm tẩy rửa gia dụng', 3),
('Gia vị', 'Các loại gia vị nấu ăn', 4),
('Thú cưng', 'Sản phẩm cho thú cưng', 5),
('Dịch vụ', 'Các dịch vụ bổ sung', 6);

-- Dữ liệu mẫu cho nhà cung cấp
INSERT IGNORE INTO Suppliers (name, address, phone, email) VALUES
('Nhà cung cấp hạt ngũ cốc ABC', '123 Đường Hạt, Quận 1', '0123456789', 'hat@abc.com'),
('Công ty mỹ phẩm XYZ', '456 Đường Đẹp, Quận 2', '0987654321', 'mypham@xyz.com'),
('Nhà cung cấp gia vị DEF', '789 Đường Vị, Quận 3', '0555666777', 'giavi@def.com');

-- Dữ liệu mẫu cho sản phẩm
INSERT IGNORE INTO Products (name, base_price, unit_measure, description, categoryID, is_active) VALUES
-- Hạt/Ngũ cốc
('Cà phê', 180000, 'kg', 'Hạt cà phê rang/xanh', 1, TRUE),
('Hạt Mắc ca', 450000, 'kg', 'Hạt mắc ca', 1, TRUE),
('Hạt dẻ cười', 520000, 'kg', 'Pistachio', 1, TRUE),
('Hạt óc chó', 380000, 'kg', 'Walnut', 1, TRUE),
('Hạt hạnh nhân', 360000, 'kg', 'Almond', 1, TRUE),
-- Làm đẹp
('Dầu gội đầu', 45000, 'chai 400ml', 'Dầu gội tự nhiên', 2, TRUE),
('Dầu xả', 45000, 'chai 400ml', 'Dầu xả tự nhiên', 2, TRUE),
('Nước hoa hồng', 90000, 'chai 200ml', 'Toner thiên nhiên', 2, TRUE),
('Sữa rửa mặt', 85000, 'chai 200ml', 'Sữa rửa mặt dịu nhẹ', 2, TRUE),
-- Tẩy rửa
('Nước rửa chén', 35000, 'chai 500ml', 'Nước rửa chén sinh học', 3, TRUE),
('Nước giặt', 55000, 'chai 1L', 'Nước giặt sinh học', 3, TRUE),
('Nước xả', 50000, 'chai 1L', 'Nước xả vải', 3, TRUE),
-- Gia vị
('Đường', 25000, 'kg', 'Đường', 4, TRUE),
('Muối', 10000, 'kg', 'Muối', 4, TRUE),
('Dầu ăn', 68000, 'L', 'Dầu thực vật', 4, TRUE);

-- Dữ liệu mẫu cho biến thể (với SKU EAN13)
INSERT IGNORE INTO Variants (SKU, name, base_price, unit, min_stock, max_stock, productID, is_active) VALUES
-- Cà phê variants
('1234567890123', 'Cà phê Arabica', 180000, 'kg', 10, 100, 1, TRUE),
('1234567890124', 'Cà phê Robusta', 160000, 'kg', 10, 100, 1, TRUE),
-- Hạt Mắc ca variants
('1234567890125', 'Hạt Mắc ca Úc', 450000, 'kg', 5, 50, 2, TRUE),
('1234567890126', 'Hạt Mắc ca Nam Phi', 420000, 'kg', 5, 50, 2, TRUE),
-- Dầu gội variants
('1234567890127', 'Dầu gội Lavender', 45000, 'chai 400ml', 20, 200, 6, TRUE),
('1234567890128', 'Dầu gội Tea Tree', 48000, 'chai 400ml', 20, 200, 6, TRUE),
-- Nước rửa chén variants
('1234567890129', 'Nước rửa chén Chanh', 35000, 'chai 500ml', 30, 300, 10, TRUE),
('1234567890130', 'Nước rửa chén Cam', 36000, 'chai 500ml', 30, 300, 10, TRUE);

-- Dữ liệu mẫu cho combo
INSERT IGNORE INTO Combos (name, price, description, is_active) VALUES
('Set quà tặng (Hạt/Ngũ cốc)', 350000, 'Hộp quà hạt ngũ cốc', TRUE),
('Set quà tặng (Làm đẹp)', 250000, 'Hộp quà làm đẹp', TRUE),
('Set quà tặng (Gia dụng)', 220000, 'Bộ quà tặng gia dụng', TRUE),
('Travel Kit (Kit du lịch)', 150000, 'Bộ chiết du lịch', TRUE);

-- Dữ liệu mẫu cho thành phần combo
INSERT IGNORE INTO ComboItems (quantity, comboID, variantID) VALUES
-- Set quà hạt ngũ cốc
(0.5, 1, 1); -- Cà phê Arabica 0.5kg