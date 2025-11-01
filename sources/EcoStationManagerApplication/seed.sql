USE EcoStationManager;

-- =======================
-- Roles
-- =======================
INSERT IGNORE INTO Roles (name, permissions) VALUES
('Admin', '{}'),
('Manager', '{}'),
('WarehouseStaff', '{}');

-- =======================
-- Users
-- =======================
INSERT IGNORE INTO Users (username, password_hash, email, fullname, is_active) VALUES
('admin', '$2b$10$hash...', 'admin@eco.com', 'Admin User', TRUE),
('manager', '$2b$10$hash...', 'manager@eco.com', 'Manager User', TRUE),
('warehouse', '$2b$10$hash...', 'warehouse@eco.com', 'Warehouse Staff', TRUE),
('cashier', '$2b$10$hash...', 'cashier@eco.com', 'Cashier Staff', TRUE),
('cleaner', '$2b$10$hash...', 'cleaner@eco.com', 'Cleaner Staff', TRUE);

-- =======================
-- UserRoles
-- =======================
INSERT IGNORE INTO UserRoles (user_id, role_id, assigned_by) VALUES
(1, 1, 1), -- admin -> Admin
(2, 2, 1), -- manager -> Manager
(3, 3, 2), -- warehouse -> WarehouseStaff
(4, 3, 2), -- cashier -> WarehouseStaff
(5, 3, 2); -- cleaner -> WarehouseStaff

-- =======================
-- Stations
-- =======================
INSERT IGNORE INTO Stations (name, address, phone, station_type, manager, is_active) VALUES
('Trạm EcoStation 1', '123 Green Street, District 1', '0123456789', 'hybrid', 2, TRUE),
('Trạm EcoStation 2', '456 Eco Avenue, District 2', '0987654321', 'warehouse', 2, TRUE),
('Trạm EcoStation 3', '789 Nature Road, District 3', '0555666777', 'refill', 2, TRUE);

-- =======================
-- UserStation
-- =======================
INSERT IGNORE INTO UserStation (user_id, station_id) VALUES
(2, 1),
(2, 2),
(3, 1),
(4, 1),
(5, 3);

-- =======================
-- Categories
-- =======================
INSERT IGNORE INTO Categories (name, description, sort_order) VALUES
('Hạt/Ngũ cốc', 'Các loại hạt và ngũ cốc', 1),
('Làm đẹp', 'Sản phẩm chăm sóc sắc đẹp', 2),
('Tẩy rửa', 'Sản phẩm tẩy rửa gia dụng', 3),
('Gia vị', 'Các loại gia vị nấu ăn', 4);

-- =======================
-- Products
-- =======================
INSERT IGNORE INTO Products (code, name, description, base_price, unit_measure, category_id) VALUES
('CF', 'Cà phê', 'Hạt cà phê rang/xanh', 180000, 'kg', 1),
('MC', 'Hạt Mắc ca', 'Hạt mắc ca', 450000, 'kg', 1),
('SH', 'Dầu gội đầu', 'Dầu gội tự nhiên', 45000, 'chai 400ml', 2);

-- =======================
-- VariantTypes
-- =======================
INSERT IGNORE INTO VariantTypes (name, description) VALUES
('Standard', 'Biến thể tiêu chuẩn');

-- =======================
-- Variants
-- =======================
INSERT IGNORE INTO Variants (SKU, name, unit, price, product_id, type_id) VALUES
('CF-ARAB', 'Cà phê Arabica', 'kg', 180000, 1, 1),
('CF-ROB', 'Cà phê Robusta', 'kg', 160000, 1, 1),
('MC-AU', 'Hạt Mắc ca Úc', 'kg', 450000, 2, 1),
('MC-SA', 'Hạt Mắc ca Nam Phi', 'kg', 420000, 2, 1),
('SH-LAV', 'Dầu gội Lavender', 'chai 400ml', 45000, 3, 1);

-- =======================
-- Combos
-- =======================
INSERT IGNORE INTO Combos (code, name, description, total_price) VALUES
('GIFT-NUTS', 'Set quà tặng Hạt/Ngũ cốc', 'Hộp quà hạt ngũ cốc', 350000),
('GIFT-BEAUTY', 'Set quà tặng Làm đẹp', 'Hộp quà chăm sóc cá nhân', 250000);

-- =======================
-- ComboItems
-- =======================
INSERT IGNORE INTO ComboItems (combo_id, variant_id, quantity) VALUES
(1, 1, 1),
(1, 3, 1),
(2, 5, 1);

-- =======================
-- Suppliers
-- =======================
INSERT IGNORE INTO Suppliers (name, address, phone, email) VALUES
('Nhà cung cấp hạt ngũ cốc ABC', '123 Đường Hạt, Quận 1', '0123456789', 'hat@abc.com'),
('Công ty mỹ phẩm XYZ', '456 Đường Đẹp, Quận 2', '0987654321', 'mypham@xyz.com');
