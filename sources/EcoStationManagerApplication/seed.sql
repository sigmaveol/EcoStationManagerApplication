USE EcoStationManager;

-- 1. Users
INSERT INTO Users (username, password_hash, fullname, role, is_active) VALUES
('admin', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Nguyễn Văn Admin', 'ADMIN', TRUE),
('manager1', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Trần Thị Quản Lý', 'MANAGER', TRUE),
('staff1', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Lê Văn Nhân Viên', 'STAFF', TRUE),
('staff2', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Phạm Thị Bán Hàng', 'STAFF', TRUE),
('driver1', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Hoàng Văn Giao Hàng', 'DRIVER', TRUE),
('driver2', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'Vũ Thị Vận Chuyển', 'DRIVER', TRUE);

-- 2. Suppliers
INSERT INTO Suppliers (name, contact_person, phone, email, address) VALUES
('Công ty Nước Giải Khát ABC', 'Ông Nguyễn Văn A', '0987654321', 'contact@abcbeverage.com', '123 Đường ABC, Quận 1, TP.HCM'),
('Nhà Máy Nước Tinh Khiết XYZ', 'Bà Trần Thị B', '0912345678', 'info@xyzwater.com', '456 Đường XYZ, Quận 2, TP.HCM'),
('Công ty Bao Bì DEF', 'Anh Lê Văn C', '0978123456', 'sales@defpackaging.com', '789 Đường DEF, Quận 3, TP.HCM');

-- 3. Categories
INSERT INTO Categories (name, category_type, is_active) VALUES
('Nước tinh khiết', 'PRODUCT', TRUE),
('Nước khoáng', 'PRODUCT', TRUE),
('Nước có ga', 'PRODUCT', TRUE),
('Nước trái cây', 'PRODUCT', TRUE),
('Trà', 'PRODUCT', TRUE),
('Dịch vụ giao hàng', 'SERVICE', TRUE),
('Dịch vụ vệ sinh', 'SERVICE', TRUE),
('Bao bì', 'OTHER', TRUE);

-- 4. Packaging
INSERT INTO Packaging (barcode, name, type, deposit_price) VALUES
('BB001', 'Bình 20L', 'bottle', 100000.00),
('BB002', 'Bình 5L', 'bottle', 30000.00),
('BB003', 'Chai 1L', 'bottle', 5000.00),
('BB004', 'Chai 500ml', 'bottle', 3000.00),
('BB005', 'Thùng giấy 24 chai', 'box', 15000.00);

-- 5. Products
INSERT INTO Products (sku, name, image, product_type, unit, price, min_stock_level, category_id, is_active) VALUES
-- Nước tinh khiết
('NWTK20L', 'Nước tinh khiết 20L', 'nutinhkhiet20l.jpg', 'REFILLED', 'bình', 50000.00, 10, 1, TRUE),
('NWTK5L', 'Nước tinh khiết 5L', 'nutinhkhiet5l.jpg', 'REFILLED', 'bình', 20000.00, 20, 1, TRUE),
('NWTK1L', 'Nước tinh khiết 1L', 'nutinhkhiet1l.jpg', 'PACKED', 'chai', 10000.00, 50, 1, TRUE),
('NWTK500', 'Nước tinh khiết 500ml', 'nutinhkhiet500ml.jpg', 'PACKED', 'chai', 6000.00, 100, 1, TRUE),

-- Nước khoáng
('NWK20L', 'Nước khoáng 20L', 'nuockhoang20l.jpg', 'REFILLED', 'bình', 60000.00, 8, 2, TRUE),
('NWK1L', 'Nước khoáng 1L', 'nuockhoang1l.jpg', 'PACKED', 'chai', 12000.00, 40, 2, TRUE),

-- Nước có ga
('COCA330', 'Coca Cola 330ml', 'cocacola330ml.jpg', 'PACKED', 'lon', 12000.00, 80, 3, TRUE),
('PEPSI330', 'Pepsi 330ml', 'pepsi330ml.jpg', 'PACKED', 'lon', 11000.00, 80, 3, TRUE),
('7UP330', '7Up 330ml', '7up330ml.jpg', 'PACKED', 'lon', 11000.00, 60, 3, TRUE),

-- Nước trái cây
('CAM200', 'Nước cam 200ml', 'nuoccam200ml.jpg', 'PACKED', 'hộp', 15000.00, 50, 4, TRUE),
('TAO200', 'Nước táo 200ml', 'nuoctao200ml.jpg', 'PACKED', 'hộp', 14000.00, 50, 4, TRUE),

-- Trà
('TRAXANH500', 'Trà xanh 500ml', 'traxanh500ml.jpg', 'PACKED', 'chai', 13000.00, 60, 5, TRUE),
('TRAOLONG500', 'Trà ô long 500ml', 'traolong500ml.jpg', 'PACKED', 'chai', 14000.00, 60, 5, TRUE);

-- 6. Customers
INSERT INTO Customers (customer_code, name, phone, total_point, rank, is_active) VALUES
('KH001', 'Công ty TNHH ABC', '02838223344', 1500, 'DIAMONDS', TRUE),
('KH002', 'Trường Tiểu học XYZ', '02839998877', 800, 'GOLD', TRUE),
('KH003', 'Nhà hàng Hải Sản Biển', '02837776655', 300, 'SILVER', TRUE),
('KH004', 'Quán Cafe Sài Gòn', '0909123456', 150, 'MEMBER', TRUE),
('KH005', 'Gia đình Anh Minh', '0918765432', 50, 'MEMBER', TRUE),
('KH006', 'Văn phòng Luật DEF', '02836665544', 600, 'SILVER', TRUE);

-- 7. Inventories
INSERT INTO Inventories (batch_no, product_id, quantity, expiry_date) VALUES
-- Nước tinh khiết
('BATCH_NWTK20L_001', 1, 25, '2024-12-31'),
('BATCH_NWTK5L_001', 2, 45, '2024-12-31'),
('BATCH_NWTK1L_001', 3, 120, '2024-10-31'),
('BATCH_NWTK500_001', 4, 200, '2024-10-31'),

-- Nước khoáng
('BATCH_NWK20L_001', 5, 15, '2024-12-31'),
('BATCH_NWK1L_001', 6, 80, '2024-11-30'),

-- Nước có ga
('BATCH_COCA330_001', 7, 150, '2024-09-30'),
('BATCH_PEPSI330_001', 8, 120, '2024-09-30'),
('BATCH_7UP330_001', 9, 90, '2024-09-30'),

-- Nước trái cây
('BATCH_CAM200_001', 10, 100, '2024-08-31'),
('BATCH_TAO200_001', 11, 80, '2024-08-31'),

-- Trà
('BATCH_TRAXANH500_001', 12, 110, '2024-10-31'),
('BATCH_TRAOLONG500_001', 13, 95, '2024-10-31');

-- 8. PackagingInventories
INSERT INTO PackagingInventories (packaging_id, qty_new, qty_in_use, qty_returned, qty_need_cleaning, qty_cleaned, qty_damaged) VALUES
(1, 50, 25, 10, 8, 32, 2),
(2, 100, 45, 20, 12, 68, 5),
(3, 200, 80, 50, 25, 125, 10),
(4, 300, 120, 80, 40, 180, 15),
(5, 80, 30, 15, 8, 42, 3);

-- 9. Orders
INSERT INTO Orders (customer_id, order_code, source, total_amount, discounted_amount, status, payment_status, payment_method, address, note, user_id) VALUES
(1, 'ORD001', 'MANUAL', 350000.00, 0, 'COMPLETED', 'PAID', 'TRANSFER', '123 Đường ABC, Quận 1, TP.HCM', 'Giao giờ hành chính', 3),
(2, 'ORD002', 'EMAIL', 280000.00, 15000.00, 'COMPLETED', 'PAID', 'CASH', '456 Đường XYZ, Quận 2, TP.HCM', 'Giao cổng sau', 3),
(3, 'ORD003', 'GOOGLEFORM', 420000.00, 20000.00, 'SHIPPED', 'UNPAID', 'CASH', '789 Đường DEF, Quận 3, TP.HCM', 'Khách yêu cầu gọi trước', 4),
(4, 'ORD004', 'MANUAL', 150000.00, 0, 'PROCESSING', 'UNPAID', 'CASH', '321 Đường GHI, Quận 4, TP.HCM', 'Giao buổi sáng', 4),
(5, 'ORD005', 'MANUAL', 80000.00, 5000.00, 'CONFIRMED', 'PAID', 'CASH', '654 Đường JKL, Quận 5, TP.HCM', NULL, 3),
(6, 'ORD006', 'EXCEL', 220000.00, 10000.00, 'DRAFT', 'UNPAID', 'TRANSFER', '987 Đường MNO, Quận 6, TP.HCM', 'Hóa đơn VAT', 3);

-- 10. OrderDetails
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) VALUES
-- Order 1
(1, 1, 5, 50000.00),
(1, 3, 10, 10000.00),

-- Order 2
(2, 5, 3, 60000.00),
(2, 6, 10, 12000.00),

-- Order 3
(3, 1, 6, 50000.00),
(3, 7, 12, 12000.00),
(3, 12, 6, 13000.00),

-- Order 4
(4, 2, 5, 20000.00),
(4, 4, 10, 6000.00),

-- Order 5
(5, 3, 8, 10000.00),

-- Order 6
(6, 5, 2, 60000.00),
(6, 8, 10, 11000.00);

-- 11. StockIn
INSERT INTO StockIn (batch_no, ref_type, ref_id, quantity, unit_price, notes, supplier_id, expiry_date, created_by) VALUES
-- Nhập sản phẩm
('SI_NWTK20L_001', 'PRODUCT', 1, 30, 35000.00, 'Nhập hàng đầu kỳ', 1, '2024-12-31', 1),
('SI_NWTK5L_001', 'PRODUCT', 2, 50, 15000.00, 'Nhập hàng đầu kỳ', 1, '2024-12-31', 1),
('SI_COCA330_001', 'PRODUCT', 7, 200, 9000.00, 'Nhập khuyến mãi', 1, '2024-09-30', 1),

-- Nhập bao bì
('SI_BB20L_001', 'PACKAGING', 1, 20, 80000.00, 'Bổ sung bình 20L', 3, NULL, 1),
('SI_BB5L_001', 'PACKAGING', 2, 40, 25000.00, 'Bổ sung bình 5L', 3, NULL, 1);

-- 12. StockOut
INSERT INTO StockOut (batch_no, ref_type, ref_id, quantity, purpose, notes, created_by) VALUES
-- Xuất bán hàng
('SO_ORD001', 'PRODUCT', 1, 5, 'SALE', 'Xuất cho đơn ORD001', 3),
('SO_ORD001', 'PRODUCT', 3, 10, 'SALE', 'Xuất cho đơn ORD001', 3),

-- Xuất hỏng
('SO_DAMAGE_001', 'PRODUCT', 4, 5, 'DAMAGE', 'Hỏng do vận chuyển', 2),
('SO_DAMAGE_002', 'PACKAGING', 3, 10, 'DAMAGE', 'Bể vỡ không sử dụng được', 2);

-- 13. CleaningSchedules
INSERT INTO CleaningSchedules (cleaning_type, cleaning_date, status, notes, cleaning_by) VALUES
('TANK', '2024-01-15 08:00:00', 'COMPLETED', 'Vệ sinh bồn chứa định kỳ', 3),
('PACKAGING', '2024-01-16 09:00:00', 'COMPLETED', 'Vệ sinh bình 20L', 4),
('TANK', '2024-01-20 08:00:00', 'SCHEDULED', 'Vệ sinh bồn chứa theo kế hoạch', NULL),
('PACKAGING', '2024-01-18 10:00:00', 'SCHEDULED', 'Vệ sinh bình 5L', NULL);

-- 14. PackagingTransactions
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, refund_amount, notes) VALUES
-- Phát bao bì
(1, 1, 1, 3, 'ISSUE', 'DEPOSIT', 5, 100000.00, 0, 'Phát bình cho đơn ORD001'),
(2, 2, 2, 3, 'ISSUE', 'DEPOSIT', 3, 30000.00, 0, 'Phát bình cho đơn ORD002'),
(1, 1, 3, 4, 'ISSUE', 'DEPOSIT', 6, 100000.00, 0, 'Phát bình cho đơn ORD003'),

-- Thu hồi bao bì
(1, NULL, 1, 3, 'RETURN', 'DEPOSIT', 3, 0, 100000.00, 'Khách trả bình cũ'),
(2, NULL, 2, 4, 'RETURN', 'DEPOSIT', 2, 0, 30000.00, 'Thu hồi bình sau sử dụng');

-- 15. DeliveryAssignments
INSERT INTO DeliveryAssignments (order_id, driver_id, assigned_date, status, cod_amount, payment_status, notes) VALUES
(1, 5, '2024-01-10 08:00:00', 'delivered', 0, 'paid', 'Giao hàng thành công'),
(2, 6, '2024-01-10 09:00:00', 'delivered', 265000.00, 'paid', 'Đã thu tiền mặt'),
(3, 5, '2024-01-11 08:30:00', 'intransit', 400000.00, 'unpaid', 'Đang trên đường giao hàng'),
(4, 6, '2024-01-11 10:00:00', 'pending', 150000.00, 'unpaid', 'Chờ xác nhận');

-- 16. WorkShifts
INSERT INTO WorkShifts (user_id, shift_date, start_time, end_time, kpi_score, notes) VALUES
(3, '2024-01-10', '08:00:00', '17:00:00', 85.5, 'Hoàn thành tốt công việc'),
(4, '2024-01-10', '08:00:00', '17:00:00', 92.0, 'Bán hàng tốt'),
(5, '2024-01-10', '07:30:00', '16:30:00', 88.0, 'Giao hàng đúng giờ'),
(6, '2024-01-10', '07:30:00', '16:30:00', 95.5, 'Khách hàng hài lòng'),
(3, '2024-01-11', '08:00:00', '17:00:00', NULL, 'Ca làm việc'),
(4, '2024-01-11', '08:00:00', '17:00:00', NULL, 'Ca làm việc');