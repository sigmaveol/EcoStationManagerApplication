-- EcoStation Manager Database V0 (full-onfline-LAN) - MySQL Version
-- Engine: InnoDB, Charset: utf8mb4

CREATE DATABASE IF NOT EXISTS EcoStationManager
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE EcoStationManager;

-- Users
CREATE TABLE IF NOT EXISTS Users (
    user_id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    fullname VARCHAR(255),
    role ENUM('ADMIN','STAFF','MANAGER', 'DRIVER') DEFAULT 'STAFF',
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng khách hàng
CREATE TABLE IF NOT EXISTS Customers (
    customer_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    phone VARCHAR(50),
    email VARCHAR(100),
    total_point INT DEFAULT 0,
    rank ENUM('MEMBER', 'SILVER', 'GOLD', 'DIAMONDS') DEFAULT 'MEMBER',
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- suppliers
CREATE TABLE IF NOT EXISTS Suppliers (
  supplier_id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(150) NOT NULL,
  contact_person VARCHAR(150),
  phone VARCHAR(30),
  email VARCHAR(150),
  address TEXT,
  created_at DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Bảng danh mục sản phẩm
CREATE TABLE IF NOT EXISTS Categories (
    category_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    category_type ENUM('PRODUCT', 'SERVICE', 'OTHER') NOT NULL DEFAULT 'PRODUCT',
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng sản phẩm
CREATE TABLE IF NOT EXISTS Products (
    product_id INT PRIMARY KEY AUTO_INCREMENT,
    sku VARCHAR(20) UNIQUE,
    name VARCHAR(255) NOT NULL,
    product_type ENUM('PACKED', 'REFILLED', 'OTHER') NOT NULL DEFAULT 'PACKED',
    unit VARCHAR (50) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    min_stock_level DECIMAL (10, 2) DEFAULT 15,
    category_id INT,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES Categories(category_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Bảng bao bì
CREATE TABLE IF NOT EXISTS Packaging (
    packaging_id INT PRIMARY KEY AUTO_INCREMENT,
    barcode VARCHAR(20) UNIQUE,
    name VARCHAR(150) NOT NULL,
    type VARCHAR(50), -- bottle, box, container
    deposit_price DECIMAL(10,2) DEFAULT 0.00
) ENGINE=InnoDB;

-- Bảng tồn kho tổng cho sản phẩm
CREATE TABLE IF NOT EXISTS Inventories (
    inventory_id INT PRIMARY KEY AUTO_INCREMENT,
    batch_no VARCHAR(100),
    product_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL DEFAULT 0,
    expiry_date DATE,
    last_updated DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS PackagingInventories (
    pk_inv_id INT PRIMARY KEY AUTO_INCREMENT,
    packaging_id INT NOT NULL,
    qty_new INT DEFAULT 0,
    qty_in_use INT DEFAULT 0,
    qty_returned INT DEFAULT 0,
    qty_need_cleaning INT DEFAULT 0,
    qty_cleaned INT DEFAULT 0,
    qty_damaged INT DEFAULT 0,
    last_updated DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (packaging_id) REFERENCES packaging(packaging_id)
);

-- Bảng đơn hàng
CREATE TABLE IF NOT EXISTS Orders (
    order_id INT PRIMARY KEY AUTO_INCREMENT,
    customer_id INT,
    source ENUM('GOOGLEFORM', 'EXCEL', 'EMAIL', 'MANUAL') NOT NULL DEFAULT 'MANUAL',
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0,
    discounted_amount DECIMAL(10,2) DEFAULT 0,
    status ENUM ('DRAFT', 'CONFIRMED', 'PROCESSING', 'READY', 'SHIPPED', 'COMPLETED', 'CANCELLED') DEFAULT 'DRAFT',
    payment_status ENUM ('UNPAID', 'PAID') DEFAULT 'UNPAID',
    payment_method ENUM ('CASH', 'TRANSFER') DEFAULT 'CASH',
    address VARCHAR(255) NULL,
    note TEXT, 
    user_id INT, -- Người tạo đơn
    last_updated DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id)
) ENGINE=InnoDB;

-- Bảng chi tiết đơn hàng
CREATE TABLE IF NOT EXISTS OrderDetails (
    order_detail_id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT NOT NULL,
    product_id INT,
    quantity DECIMAL(10,2),
    unit_price DECIMAL(10,2),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
) ENGINE=InnoDB;

-- Bảng nhập kho
CREATE TABLE IF NOT EXISTS StockIn (
    stockin_id INT PRIMARY KEY AUTO_INCREMENT,
    batch_no VARCHAR(100),
    ref_type ENUM('PRODUCT', 'PACKAGING') NOT NULL,
    ref_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    unit_price DECIMAL(10,2) DEFAULT 0.00,
    notes TEXT,
    supplier_id INT, -- có thể null nếu nhập nội bộ
    expiry_date DATE,
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng xuất kho
CREATE TABLE IF NOT EXISTS StockOut (
    stockout_id INT PRIMARY KEY AUTO_INCREMENT, 
    batch_no VARCHAR(100),
    ref_type ENUM('PRODUCT', 'PACKAGING') NOT NULL,
    ref_id INT NOT NULL,
    quantity DECIMAL(10,2) NOT NULL,
    purpose ENUM ('SALE', 'DAMAGE', 'TRANSFER') DEFAULT 'SALE',
    notes TEXT,
    created_by INT NOT NULL,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- Bảng lịch vệ sinh
CREATE TABLE IF NOT EXISTS CleaningSchedules (
    cs_id INT PRIMARY KEY AUTO_INCREMENT,
    cleaning_type ENUM('TANK','PACKAGING') NOT NULL,
    cleaning_date DATETIME NOT NULL,
    status ENUM('SCHEDULED', 'COMPLETED', 'OVERDUE', 'CANCELLED') DEFAULT 'SCHEDULED',
    notes TEXT,
    cleaning_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (cleaning_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS PackagingTransactions (
    transaction_id INT AUTO_INCREMENT PRIMARY KEY,
    packaging_id INT NOT NULL,
    ref_product_id INT,
    customer_id INT,
    user_id INT NULL,
    type ENUM('ISSUE','RETURN') NOT NULL,           -- phát / thu hồi
    ownership_type ENUM('DEPOSIT','SOLD') DEFAULT 'DEPOSIT',
    quantity INT DEFAULT 1,                         -- số lượng phát/thu
    deposit_price DECIMAL(10,2) DEFAULT 0.00,       -- tiền ký quỹ (nếu có)
    refund_amount DECIMAL(10,2) DEFAULT 0.00,       -- tiền hoàn khi trả
    notes TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (packaging_id) REFERENCES Packaging(packaging_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
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

-- Tạo indexes TỐI ƯU NHẤT - chỉ những index thực sự cần thiết

-- USERS (đăng nhập, phân quyền, tìm kiếm)
CREATE UNIQUE INDEX idx_users_username ON Users(username);
CREATE INDEX idx_users_role_active ON Users(role, is_active);

-- CUSTOMERS (tìm kiếm khách hàng, lọc theo rank)
CREATE INDEX idx_customers_name_phone ON Customers(name, phone);
CREATE INDEX idx_customers_rank ON Customers(rank);

-- PRODUCTS (tìm kiếm bán hàng, lọc theo category và tồn kho)
CREATE INDEX idx_products_name_type_category ON Products(name, product_type, category_id);
CREATE INDEX idx_products_minstock ON Products(min_stock_level);

-- INVENTORIES (tồn kho, cảnh báo, kiểm tra HSD)
CREATE INDEX idx_inventories_product ON Inventories(product_id);
CREATE INDEX idx_inventories_quantity ON Inventories(quantity);
CREATE INDEX idx_inventories_expiry ON Inventories(expiry_date);

-- PACKAGING (tìm kiếm loại bao bì)
CREATE INDEX idx_packaging_type_name ON Packaging(type, name);

-- PACKAGING INVENTORIES (theo dõi số lượng, trạng thái)
CREATE INDEX idx_pkg_inventory_status ON PackagingInventories(qty_new, qty_in_use, qty_need_cleaning, qty_cleaned, qty_damaged);

-- ORDERS (quản lý vòng đời đơn hàng)
CREATE INDEX idx_orders_status_user_date ON Orders(status, user_id, last_updated);
CREATE INDEX idx_orders_customer_date ON Orders(customer_id, last_updated);

-- ORDER DETAILS (thống kê, liên kết đơn hàng – sản phẩm)
CREATE INDEX idx_orderdetails_order_product ON OrderDetails(order_id, product_id);

-- STOCKIN / STOCKOUT (nhập – xuất kho, kiểm tồn kho)
CREATE INDEX idx_stockin_ref ON StockIn(ref_type, ref_id);
CREATE INDEX idx_stockin_date_product ON StockIn(created_date, ref_id);
CREATE INDEX idx_stockout_ref ON StockOut(ref_type, ref_id);
CREATE INDEX idx_stockout_date_product ON StockOut(created_date, ref_id);

-- PACKAGING TRANSACTIONS (theo dõi phát/thu bao bì)
CREATE INDEX idx_pkg_trans_date_type ON PackagingTransactions(created_date, type);

