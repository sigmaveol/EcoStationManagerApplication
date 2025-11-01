-- EcoStation Manager Database V0 (Offline)
-- Minimal Schema - MySQL Version
-- Engine: InnoDB, Charset: utf8mb4

CREATE DATABASE IF NOT EXISTS EcoStationManagerV0
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE EcoStationManagerV0;

-- 1. Roles
CREATE TABLE Roles (
    role_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) UNIQUE NOT NULL,
    permissions JSON,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- 2. Users
CREATE TABLE Users (
    user_id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(100),
    fullname VARCHAR(255),
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- 3. Stations
CREATE TABLE Stations (
    station_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    address TEXT,
    phone VARCHAR(50),
    station_type ENUM('warehouse','refill', 'hybrid', 'other') DEFAULT 'refill',
    manager INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (manager) REFERENCES Users(user_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- 4. UserRole
CREATE TABLE UserRole (
    user_id INT NOT NULL,
    role_id INT NOT NULL,
    assigned_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY(user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
) ENGINE=InnoDB;

-- 5. UserStation
CREATE TABLE UserStation (
    user_id INT NOT NULL,
    station_id INT NOT NULL,
    PRIMARY KEY(user_id, station_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (station_id) REFERENCES Stations(station_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 6. Suppliers
CREATE TABLE Suppliers (
    supplier_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    phone VARCHAR(50),
    email VARCHAR(100),
    address TEXT,
    contact_person VARCHAR(255),
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- 7. Customers
CREATE TABLE Customers (
    customer_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    phone VARCHAR(50),
    email VARCHAR(100),
    address TEXT,
    total_point INT DEFAULT 0,
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- 8. Categories
CREATE TABLE Categories (
    category_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    category_type ENUM('product', 'service', 'other') DEFAULT 'product',
    group_name VARCHAR(255),
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
) ENGINE=InnoDB;

-- 9. Products
CREATE TABLE Products (
    product_id INT PRIMARY KEY AUTO_INCREMENT,
    code VARCHAR(30) UNIQUE,
    name VARCHAR(255) NOT NULL,
    base_price DECIMAL(10,2),
    unit_measure VARCHAR(50),
    category_id INT,
    FOREIGN KEY (category_id) REFERENCES Categories(category_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- 10. VariantTypes
CREATE TABLE VariantTypes (
    type_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

-- 11. Variants
CREATE TABLE Variants (
    variant_id INT PRIMARY KEY AUTO_INCREMENT,
    SKU VARCHAR(50) UNIQUE NOT NULL,
    name VARCHAR(255),
    unit VARCHAR(50),
    price DECIMAL(10,2),
    product_id INT,
    type_id INT,
    FOREIGN KEY (product_id) REFERENCES Products(product_id) ON DELETE CASCADE,
    FOREIGN KEY (type_id) REFERENCES VariantTypes(type_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Bảng combo/set sản phẩm
CREATE TABLE IF NOT EXISTS Combos (
    combo_id INT PRIMARY KEY AUTO_INCREMENT,
    code VARCHAR(30) UNIQUE,
    name VARCHAR(255) NOT NULL,
    image VARCHAR(255),
    description TEXT,
    total_price DECIMAL(10,2) NOT NULL,
    discount_percent DECIMAL(5,2) DEFAULT 0,
    combo_type ENUM('giftset', 'promotion', 'mixmatch', 'other') DEFAULT 'giftset',
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Bảng thành phần combo
CREATE TABLE IF NOT EXISTS ComboItems (
    combo_id INT NOT NULL,
    variant_id INT NOT NULL,
    quantity DECIMAL(10,2) DEFAULT 1,  -- Hỗ trợ nửa đơn vị nếu có (vd: 0.5kg)
    unit_price DECIMAL(10,2) DEFAULT NULL,   -- Giá từng SP trong combo (optional)
    PRIMARY KEY (combo_id, variant_id),
    FOREIGN KEY (combo_id) REFERENCES Combos(combo_id) ON DELETE CASCADE, 
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 12. Orders
CREATE TABLE Orders (
    order_id INT PRIMARY KEY AUTO_INCREMENT,
    source ENUM('googleform', 'excel', 'email', 'manual' ,'other') DEFAULT 'manual',
    total_amount DECIMAL(10,2) DEFAULT 0,
    discounted_amount DECIMAL(10,2) DEFAULT 0,
    status ENUM('draft','confirmed', 'processing','completed','cancelled') DEFAULT 'draft',
    user_id INT,
    customer_id INT,
    station_id INT,
    note TEXT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- 13. OrderDetails
CREATE TABLE OrderDetails (
    order_detail_id INT PRIMARY KEY AUTO_INCREMENT, -- cho phép tách dòng để giảm giá thành sản phẩm
    order_id INT NOT NULL,
    combo_id INT, -- Nếu là combo
    variant_id INT, -- Nếu là variant đơn lẻ
    quantity DECIMAL(10,2),
    unit_price DECIMAL(10,2),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE CASCADE,
    FOREIGN KEY (combo_id) REFERENCES Combos(combo_id),
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id)
) ENGINE=InnoDB;

-- 14. Payments
CREATE TABLE Payments (
    payment_id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT,
    amount DECIMAL(10,2) NOT NULL,
    method ENUM('cash','transfer') DEFAULT 'cash',
    status ENUM('pending','completed') DEFAULT 'pending',
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id) ON DELETE SET NULL
) ENGINE=InnoDB;

-- 15. Batches
CREATE TABLE Batches (
    batch_id INT PRIMARY KEY AUTO_INCREMENT,
    variant_id INT,
    station_id INT,
    batch_no VARCHAR(100),
    purchase_price DECIMAL(12,2) NOT NULL,
    expiry_date DATE,
    quantity DECIMAL(10,2) DEFAULT 0,
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- 16. Inventories
CREATE TABLE Inventories (
    inventory_id INT PRIMARY KEY AUTO_INCREMENT,
    variant_id INT,
    station_id INT,
    current_stock DECIMAL(10,2) DEFAULT 0,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id)
) ENGINE=InnoDB;

-- 17. StockIn
CREATE TABLE StockIn (
    stockin_id INT PRIMARY KEY AUTO_INCREMENT,
    variant_id INT,
    quantity DECIMAL(10,2),
    supplier_id INT,
    batch_no VARCHAR(100),
    expiry_date DATE,
    station_id INT,
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(supplier_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- 18. StockOut
CREATE TABLE StockOut (
    stockout_id INT PRIMARY KEY AUTO_INCREMENT,
    variant_id INT,
    quantity DECIMAL(10,2),
    station_id INT,
    purpose ENUM('sale','waste','transfer') DEFAULT 'sale',
    order_id INT NULL,
    created_by INT,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (variant_id) REFERENCES Variants(variant_id),
    FOREIGN KEY (station_id) REFERENCES Stations(station_id),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (created_by) REFERENCES Users(user_id)
) ENGINE=InnoDB;

-- 19. SystemSettings
CREATE TABLE SystemSettings (
    setting_id INT PRIMARY KEY AUTO_INCREMENT,
    setting_key VARCHAR(100) UNIQUE,
    setting_value TEXT,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

