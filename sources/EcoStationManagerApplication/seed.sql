USE EcoStationManager;

-- Seed data cho Users
INSERT INTO Users (username, password_hash, fullname, role, is_active) VALUES
('admin', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Nguyễn Văn Admin', 'ADMIN', TRUE),
('manager1', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Trần Thị Quản Lý', 'MANAGER', TRUE),
('staff1', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Lê Văn Nhân Viên', 'STAFF', TRUE),
('driver1', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Phạm Văn Tài Xế', 'DRIVER', TRUE),
('staff2', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Hoàng Thị Nhàn', 'STAFF', TRUE);

-- Seed data cho Suppliers
INSERT INTO Suppliers (name, contact_person, phone, email, address) VALUES
('Công ty TNHH Môi trường Xanh', 'Ông Trần Văn A', '0909123456', 'contact@moitruongxanh.com', '123 Đường Lê Lợi, Quận 1, TP.HCM'),
('Nhà cung cấp Bao bì Việt', 'Bà Nguyễn Thị B', '0909123457', 'sales@baobiviet.com', '456 Đường Nguyễn Huệ, Quận 1, TP.HCM'),
('Công ty Hóa chất An Toàn', 'Ông Lê Văn C', '0909123458', 'info@hoachatantoan.com', '789 Đường Pasteur, Quận 3, TP.HCM');

-- Seed data cho Categories
INSERT INTO Categories (name, category_type, is_active) VALUES
('Nước tinh khiết', 'PRODUCT', TRUE),
('Nước khoáng', 'PRODUCT', TRUE),
('Nước có ga', 'PRODUCT', TRUE),
('Bình nước', 'PRODUCT', TRUE),
('Dịch vụ giao hàng', 'SERVICE', TRUE),
('Dịch vụ vệ sinh', 'SERVICE', TRUE),
('Vật tư khác', 'OTHER', TRUE);

-- Seed data cho Packaging
INSERT INTO Packaging (barcode, name, type, deposit_price) VALUES
('BB001', 'Bình thủy tinh 500ml', 'bottle', 5000.00),
('BB002', 'Bình nhựa PET 1L', 'bottle', 3000.00),
('BB003', 'Bình inox 2L', 'bottle', 15000.00),
('BB004', 'Thùng giấy 12 chai', 'box', 10000.00),
('BB005', 'Bình composite 20L', 'container', 50000.00);

-- Seed data cho Products
INSERT INTO Products (sku, name, product_type, unit, price, min_stock_level, category_id, is_active) VALUES
('NK001', 'Nước tinh khiết Lavie 500ml', 'PACKED', 'chai', 8000.00, 100, 1, TRUE),
('NK002', 'Nước khoáng Vital 1L', 'PACKED', 'chai', 12000.00, 80, 2, TRUE),
('NK003', 'Nước có gas Coca Cola 330ml', 'PACKED', 'lon', 10000.00, 150, 3, TRUE),
('NK004', 'Nước tinh khiết refill 500ml', 'REFILLED', 'chai', 5000.00, 50, 1, TRUE),
('NK005', 'Nước khoáng refill 1L', 'REFILLED', 'chai', 8000.00, 40, 2, TRUE),
('NK006', 'Bình nước thể thao 750ml', 'PACKED', 'bình', 25000.00, 30, 4, TRUE),
('NK007', 'Nước tinh khiết Aquafina 500ml', 'PACKED', 'chai', 7000.00, 120, 1, TRUE),
('SV001', 'Phí giao hàng', 'OTHER', 'lần', 15000.00, 0, 5, TRUE),
('SV002', 'Vệ sinh bình nước', 'OTHER', 'bình', 10000.00, 0, 6, TRUE);

-- Seed data cho Inventories
INSERT INTO Inventories (batch_no, product_id, quantity, expiry_date) VALUES
('BATCH_NK001_001', 1, 150.00, '2025-12-31'),
('BATCH_NK002_001', 2, 100.00, '2025-11-30'),
('BATCH_NK003_001', 3, 200.00, '2025-10-31'),
('BATCH_NK004_001', 4, 80.00, '2025-12-31'),
('BATCH_NK005_001', 5, 60.00, '2025-11-30'),
('BATCH_NK006_001', 6, 40.00, '2026-12-31'),
('BATCH_NK007_001', 7, 180.00, '2025-12-31');

-- Seed data cho PackagingInventories
INSERT INTO PackagingInventories (packaging_id, qty_new, qty_in_use, qty_returned, qty_need_cleaning, qty_cleaned, qty_damaged) VALUES
(1, 50, 30, 10, 5, 25, 2),
(2, 100, 80, 15, 10, 65, 5),
(3, 30, 20, 5, 3, 12, 1),
(4, 40, 25, 8, 4, 20, 2),
(5, 20, 15, 3, 2, 10, 0);

-- Seed data cho Customers
INSERT INTO Customers (customer_code, name, phone, total_point, rank, is_active) VALUES
('KH001', 'Công ty TNHH ABC', '02838234567', 1500, 'GOLD', TRUE),
('KH002', 'Trường Tiểu học XYZ', '02838234568', 800, 'SILVER', TRUE),
('KH003', 'Quán cà phê Moon', '0909123001', 300, 'MEMBER', TRUE),
('KH004', 'Hộ gia đình Anh Minh', '0909123002', 2500, 'DIAMONDS', TRUE),
('KH005', 'Văn phòng luật DEF', '02838234569', 600, 'SILVER', TRUE),
('KH006', 'Cửa hàng tiện lợi 24h', '0909123003', 400, 'MEMBER', TRUE);

-- Seed data cho Orders
INSERT INTO Orders (customer_id, order_code, source, total_amount, discounted_amount, status, payment_status, payment_method, address, note, user_id) VALUES
(1, 'ORD001', 'MANUAL', 185000.00, 5000.00, 'COMPLETED', 'PAID', 'TRANSFER', '123 Lê Lợi, Quận 1, TP.HCM', 'Giao giờ hành chính', 3),
(2, 'ORD002', 'GOOGLEFORM', 120000.00, 0.00, 'PROCESSING', 'UNPAID', 'CASH', '456 Nguyễn Huệ, Quận 1, TP.HCM', 'Giao tại phòng kế toán', 3),
(3, 'ORD003', 'MANUAL', 75000.00, 0.00, 'READY', 'PAID', 'CASH', '789 Pasteur, Quận 3, TP.HCM', 'Khách yêu cầu giao sáng', 4),
(4, 'ORD004', 'EMAIL', 210000.00, 10000.00, 'SHIPPED', 'PAID', 'TRANSFER', '321 Lý Tự Trọng, Quận 1, TP.HCM', 'Có COD 50.000', 3),
(5, 'ORD005', 'MANUAL', 95000.00, 0.00, 'CONFIRMED', 'UNPAID', 'CASH', '654 Hai Bà Trưng, Quận 1, TP.HCM', 'Giao trước 17h', 4);

-- Seed data cho OrderDetails
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) VALUES
(1, 1, 10.00, 8000.00),
(1, 2, 5.00, 12000.00),
(1, 8, 1.00, 15000.00),
(2, 3, 8.00, 10000.00),
(2, 6, 2.00, 25000.00),
(3, 4, 10.00, 5000.00),
(3, 9, 2.00, 10000.00),
(4, 1, 15.00, 8000.00),
(4, 5, 5.00, 8000.00),
(4, 8, 1.00, 15000.00),
(5, 2, 6.00, 12000.00),
(5, 7, 3.00, 7000.00);

-- Seed data cho StockIn
INSERT INTO StockIn (batch_no, ref_type, ref_id, quantity, unit_price, notes, supplier_id, expiry_date, created_by) VALUES
('IMPORT001', 'PRODUCT', 1, 200.00, 6000.00, 'Nhập hàng định kỳ', 1, '2025-12-31', 2),
('IMPORT002', 'PRODUCT', 2, 150.00, 9000.00, 'Nhập hàng theo đơn', 1, '2025-11-30', 2),
('IMPORT003', 'PACKAGING', 1, 50.00, 20000.00, 'Nhập bình thủy tinh mới', 2, NULL, 2),
('IMPORT004', 'PRODUCT', 3, 250.00, 7000.00, 'Nhập nước có gas', 1, '2025-10-31', 2);

-- Seed data cho StockOut
INSERT INTO StockOut (batch_no, ref_type, ref_id, quantity, purpose, notes, created_by) VALUES
('EXPORT001', 'PRODUCT', 1, 50.00, 'SALE', 'Xuất bán cho đơn hàng ORD001', 3),
('EXPORT002', 'PRODUCT', 2, 30.00, 'SALE', 'Xuất bán cho đơn hàng ORD002', 3),
('EXPORT003', 'PACKAGING', 1, 5.00, 'DAMAGE', 'Bình vỡ trong quá trình vận chuyển', 2),
('EXPORT004', 'PRODUCT', 3, 20.00, 'SALE', 'Xuất bán cho đơn hàng ORD004', 3);

-- Seed data cho CleaningSchedules
INSERT INTO CleaningSchedules (cleaning_type, cleaning_date, status, notes, cleaning_by) VALUES
('TANK', '2024-01-15 08:00:00', 'COMPLETED', 'Vệ sinh bồn chứa định kỳ', 3),
('PACKAGING', '2024-01-16 09:00:00', 'SCHEDULED', 'Vệ sinh bình trả về', 4),
('TANK', '2024-01-20 08:00:00', 'SCHEDULED', 'Vệ sinh bồn chứa tuần tới', 3),
('PACKAGING', '2024-01-14 14:00:00', 'COMPLETED', 'Vệ sinh bình cho khách VVIP', 4);

-- Seed data cho PackagingTransactions
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, refund_amount, notes) VALUES
(1, 1, 1, 3, 'ISSUE', 'DEPOSIT', 10, 5000.00, 0.00, 'Phát bình cho đơn ORD001'),
(2, 2, 2, 3, 'ISSUE', 'DEPOSIT', 8, 3000.00, 0.00, 'Phát bình cho đơn ORD002'),
(1, NULL, 1, 3, 'RETURN', 'DEPOSIT', 5, 0.00, 25000.00, 'Khách trả bình cũ'),
(3, 6, 4, 3, 'ISSUE', 'SOLD', 2, 15000.00, 0.00, 'Bán bình inox cho khách');

-- Seed data cho DeliveryAssignments
INSERT INTO DeliveryAssignments (order_id, driver_id, assigned_date, status, cod_amount, payment_status, notes) VALUES
(1, 4, '2024-01-10 08:30:00', 'delivered', 0.00, 'paid', 'Giao hàng thành công'),
(2, 4, '2024-01-11 09:00:00', 'pending', 120000.00, 'unpaid', 'Chờ giao hàng'),
(3, 4, '2024-01-11 10:00:00', 'intransit', 0.00, 'paid', 'Đang trên đường giao'),
(4, 4, '2024-01-11 11:00:00', 'intransit', 50000.00, 'unpaid', 'Có COD 50.000');

-- Seed data cho WorkShifts
INSERT INTO WorkShifts (user_id, shift_date, start_time, end_time, kpi_score, notes) VALUES
(3, '2024-01-10', '08:00:00', '17:00:00', 85.50, 'Hoàn thành tốt công việc'),
(4, '2024-01-10', '07:30:00', '16:30:00', 90.00, 'Giao hàng đúng hẹn'),
(3, '2024-01-11', '08:00:00', '17:00:00', NULL, 'Ca làm việc hôm nay'),
(4, '2024-01-11', '07:30:00', '16:30:00', NULL, 'Ca giao hàng');