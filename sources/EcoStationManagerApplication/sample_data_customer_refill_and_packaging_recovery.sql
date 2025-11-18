-- Dữ liệu mẫu: Khách hàng quay lại và Tỷ lệ thu hồi bao bì
-- EcoStation Manager Database - Sample Data
-- Lưu ý: Categories, Products, Packaging đã có sẵn trong seed.sql, không cần insert thêm

USE EcoStationManager;

-- ============================================
-- 1. KHÁCH HÀNG (Một số khách hàng sẽ quay lại nhiều lần)
-- Lưu ý: seed.sql đã có KH001-KH006, nên bắt đầu từ KH007
-- Sử dụng INSERT IGNORE để bỏ qua các bản ghi đã tồn tại
-- ============================================

INSERT IGNORE INTO Customers (customer_code, name, phone, total_point, rank, is_active) VALUES
('KH007', 'Nguyễn Văn An', '0901234567', 150, 'GOLD', TRUE),
('KH008', 'Trần Thị Bình', '0902345678', 80, 'SILVER', TRUE),
('KH009', 'Lê Văn Cường', '0903456789', 250, 'DIAMONDS', TRUE),
('KH010', 'Phạm Thị Dung', '0904567890', 45, 'MEMBER', TRUE),
('KH011', 'Hoàng Văn Em', '0905678901', 120, 'GOLD', TRUE),
('KH012', 'Vũ Thị Phương', '0906789012', 30, 'MEMBER', TRUE),
('KH013', 'Đỗ Văn Giang', '0907890123', 180, 'GOLD', TRUE),
('KH014', 'Bùi Thị Hoa', '0908901234', 60, 'SILVER', TRUE),
('KH015', 'Ngô Văn Ích', '0909012345', 200, 'DIAMONDS', TRUE),
('KH016', 'Đinh Thị Khoa', '0910123456', 25, 'MEMBER', TRUE);

-- ============================================
-- 2. ĐƠN HÀNG - Khách hàng quay lại nhiều lần
-- Sử dụng Products từ seed.sql:
-- 1=NWTK20L (Nước tinh khiết 20L), 2=NWTK5L, 3=NWTK1L, 4=NWTK500
-- 5=NWK20L (Nước khoáng 20L), 6=NWK1L
-- ============================================

-- Khách hàng KH007 (Nguyễn Văn An) - Quay lại 5 lần
INSERT IGNORE INTO Orders (customer_id, order_code, source, total_amount, discounted_amount, status, payment_status, payment_method, user_id, last_updated) VALUES
(7, 'ORD007', 'MANUAL', 250000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-09-15 10:30:00'),
(7, 'ORD008', 'MANUAL', 100000.00, 10000.00, 'COMPLETED', 'PAID', 'TRANSFER', 3, '2025-09-25 14:20:00'),
(7, 'ORD009', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-05 09:15:00'),
(7, 'ORD010', 'MANUAL', 250000.00, 25000.00, 'COMPLETED', 'PAID', 'TRANSFER', 3, '2025-10-18 16:45:00'),
(7, 'ORD011', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-05 11:30:00');

-- Khách hàng KH009 (Lê Văn Cường) - Quay lại 6 lần
INSERT IGNORE INTO Orders (customer_id, order_code, source, total_amount, discounted_amount, status, payment_status, payment_method, user_id, last_updated) VALUES
(9, 'ORD012', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-09-08 08:00:00'),
(9, 'ORD013', 'MANUAL', 300000.00, 30000.00, 'COMPLETED', 'PAID', 'TRANSFER', 3, '2025-09-20 13:30:00'),
(9, 'ORD014', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-05 10:20:00'),
(9, 'ORD015', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-18 15:10:00'),
(9, 'ORD016', 'MANUAL', 100000.00, 10000.00, 'COMPLETED', 'PAID', 'TRANSFER', 3, '2025-11-02 09:45:00'),
(9, 'ORD017', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-15 14:00:00');

-- Khách hàng KH011 (Hoàng Văn Em) - Quay lại 4 lần
INSERT IGNORE INTO Orders (customer_id, order_code, source, total_amount, discounted_amount, status, payment_status, payment_method, user_id, last_updated) VALUES
(11, 'ORD018', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-09-12 11:00:00'),
(11, 'ORD019', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-03 16:30:00'),
(11, 'ORD020', 'MANUAL', 300000.00, 30000.00, 'COMPLETED', 'PAID', 'TRANSFER', 3, '2025-10-22 10:15:00'),
(11, 'ORD021', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-08 13:45:00');

-- Khách hàng KH013 (Đỗ Văn Giang) - Quay lại 5 lần
INSERT IGNORE INTO Orders (customer_id, order_code, source, total_amount, discounted_amount, status, payment_status, payment_method, user_id, last_updated) VALUES
(13, 'ORD022', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-09-18 09:30:00'),
(13, 'ORD023', 'MANUAL', 300000.00, 30000.00, 'COMPLETED', 'PAID', 'TRANSFER', 3, '2025-09-28 14:20:00'),
(13, 'ORD024', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-10 11:10:00'),
(13, 'ORD025', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-25 15:30:00'),
(13, 'ORD026', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-12 10:00:00');

-- Khách hàng KH015 (Ngô Văn Ích) - Quay lại 7 lần (khách hàng trung thành nhất)
INSERT IGNORE INTO Orders (customer_id, order_code, source, total_amount, discounted_amount, status, payment_status, payment_method, user_id, last_updated) VALUES
(15, 'ORD027', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-09-05 08:15:00'),
(15, 'ORD028', 'MANUAL', 300000.00, 30000.00, 'COMPLETED', 'PAID', 'TRANSFER', 3, '2025-09-22 13:30:00'),
(15, 'ORD029', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-05 10:45:00'),
(15, 'ORD030', 'MANUAL', 360000.00, 50000.00, 'COMPLETED', 'PAID', 'TRANSFER', 3, '2025-10-18 16:20:00'),
(15, 'ORD031', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-01 09:30:00'),
(15, 'ORD032', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-10 14:15:00'),
(15, 'ORD033', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-18 11:00:00');

-- Khách hàng khác - quay lại ít hơn (2-3 lần)
INSERT IGNORE INTO Orders (customer_id, order_code, source, total_amount, discounted_amount, status, payment_status, payment_method, user_id, last_updated) VALUES
(8, 'ORD034', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-09-15 10:00:00'),
(8, 'ORD035', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-20 15:30:00'),
(10, 'ORD036', 'MANUAL', 60000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-09-25 09:00:00'),
(10, 'ORD037', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-05 14:00:00'),
(12, 'ORD038', 'MANUAL', 60000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-10-12 11:00:00'),
(14, 'ORD039', 'MANUAL', 100000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-09-28 10:30:00'),
(14, 'ORD040', 'MANUAL', 300000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-12 16:00:00'),
(16, 'ORD041', 'MANUAL', 60000.00, 0, 'COMPLETED', 'PAID', 'CASH', 3, '2025-11-15 09:00:00');

-- ============================================
-- 3. CHI TIẾT ĐƠN HÀNG
-- Sử dụng Products từ seed.sql:
-- 1=NWTK20L (50000), 2=NWTK5L (20000), 3=NWTK1L (10000), 4=NWTK500 (6000)
-- 5=NWK20L (60000), 6=NWK1L (12000)
-- Sử dụng subquery để lấy order_id từ order_code
-- ============================================

-- Đơn hàng của KH007
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 5, 50000.00 FROM Orders WHERE order_code = 'ORD007';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD008';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD009';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 5, 50000.00 FROM Orders WHERE order_code = 'ORD010';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD011';

-- Đơn hàng của KH009
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD012';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 5, 50000.00 FROM Orders WHERE order_code = 'ORD013';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD014';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD015';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD016';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD017';

-- Đơn hàng của KH011
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD018';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD019';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD020';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD021';

-- Đơn hàng của KH013
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD022';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD023';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD024';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD025';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD026';

-- Đơn hàng của KH015
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD027';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD028';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD029';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD030';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 5, 1, 60000.00 FROM Orders WHERE order_code = 'ORD030';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD031';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD032';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 6, 50000.00 FROM Orders WHERE order_code = 'ORD033';

-- Đơn hàng khác
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD034';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD035';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 3, 6, 10000.00 FROM Orders WHERE order_code = 'ORD036';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD037';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 3, 6, 10000.00 FROM Orders WHERE order_code = 'ORD038';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 2, 5, 20000.00 FROM Orders WHERE order_code = 'ORD039';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 1, 5, 50000.00 FROM Orders WHERE order_code = 'ORD040';
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) 
SELECT order_id, 3, 6, 10000.00 FROM Orders WHERE order_code = 'ORD041';

-- ============================================
-- 4. GIAO DỊCH BAO BÌ - PHÁT VÀ THU HỒI
-- Sử dụng Packaging từ seed.sql:
-- 1=Bình 20L (deposit 100000), 2=Bình 5L (deposit 30000), 3=Chai 1L (deposit 5000), 4=Chai 500ml (deposit 3000)
-- ============================================

-- PHÁT BAO BÌ (ISSUE) - Khi khách hàng mua sản phẩm refill
-- KH007 - Phát 5 bình (từ 5 đơn hàng)
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, created_date) VALUES
(1, 1, 7, 3, 'ISSUE', 'DEPOSIT', 5, 100000.00, '2025-09-15 10:30:00'), -- Bình 20L x5
(2, 2, 7, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-09-25 14:20:00'), -- Bình 5L x5
(1, 1, 7, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-10-05 09:15:00'), -- Bình 20L x6
(1, 1, 7, 3, 'ISSUE', 'DEPOSIT', 5, 100000.00, '2025-10-18 16:45:00'), -- Bình 20L x5
(2, 2, 7, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-11-05 11:30:00'); -- Bình 5L x5

-- KH007 - Thu hồi 4 lần (tỷ lệ 80%)
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, refund_amount, created_date) VALUES
(1, NULL, 7, 3, 'RETURN', 'DEPOSIT', 4, 100000.00, '2025-09-28 10:00:00'), -- Trả 4 bình 20L
(2, NULL, 7, 3, 'RETURN', 'DEPOSIT', 5, 30000.00, '2025-10-08 14:00:00'), -- Trả 5 bình 5L
(1, NULL, 7, 3, 'RETURN', 'DEPOSIT', 5, 100000.00, '2025-10-22 09:00:00'), -- Trả 5 bình 20L
(1, NULL, 7, 3, 'RETURN', 'DEPOSIT', 4, 100000.00, '2025-11-10 16:00:00'); -- Trả 4 bình 20L

-- KH009 - Phát 6 lần
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, created_date) VALUES
(2, 2, 9, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-09-08 08:00:00'), -- Bình 5L x5
(1, 1, 9, 3, 'ISSUE', 'DEPOSIT', 5, 100000.00, '2025-09-20 13:30:00'), -- Bình 20L x5
(1, 1, 9, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-10-05 10:20:00'), -- Bình 20L x6
(1, 1, 9, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-10-18 15:10:00'), -- Bình 20L x6
(2, 2, 9, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-11-02 09:45:00'), -- Bình 5L x5
(1, 1, 9, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-11-15 14:00:00'); -- Bình 20L x6

-- KH009 - Thu hồi 5 lần (tỷ lệ 83.3%)
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, refund_amount, created_date) VALUES
(2, NULL, 9, 3, 'RETURN', 'DEPOSIT', 5, 30000.00, '2025-09-25 08:00:00'), -- Trả 5 bình 5L
(1, NULL, 9, 3, 'RETURN', 'DEPOSIT', 4, 100000.00, '2025-10-10 13:00:00'), -- Trả 4 bình 20L
(1, NULL, 9, 3, 'RETURN', 'DEPOSIT', 5, 100000.00, '2025-10-22 10:00:00'), -- Trả 5 bình 20L
(1, NULL, 9, 3, 'RETURN', 'DEPOSIT', 5, 100000.00, '2025-11-05 15:00:00'), -- Trả 5 bình 20L
(2, NULL, 9, 3, 'RETURN', 'DEPOSIT', 4, 30000.00, '2025-11-18 09:00:00'); -- Trả 4 bình 5L

-- KH011 - Phát 4 lần
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, created_date) VALUES
(2, 2, 11, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-09-12 11:00:00'), -- Bình 5L x5
(1, 1, 11, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-10-03 16:30:00'), -- Bình 20L x6
(1, 1, 11, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-10-22 10:15:00'), -- Bình 20L x6
(2, 2, 11, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-11-08 13:45:00'); -- Bình 5L x5

-- KH011 - Thu hồi 3 lần (tỷ lệ 75%)
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, refund_amount, created_date) VALUES
(2, NULL, 11, 3, 'RETURN', 'DEPOSIT', 4, 30000.00, '2025-09-28 11:00:00'), -- Trả 4 bình 5L
(1, NULL, 11, 3, 'RETURN', 'DEPOSIT', 5, 100000.00, '2025-10-15 16:00:00'), -- Trả 5 bình 20L
(1, NULL, 11, 3, 'RETURN', 'DEPOSIT', 5, 100000.00, '2025-11-12 10:00:00'); -- Trả 5 bình 20L

-- KH013 - Phát 5 lần
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, created_date) VALUES
(2, 2, 13, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-09-18 09:30:00'), -- Bình 5L x5
(1, 1, 13, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-09-28 14:20:00'), -- Bình 20L x6
(1, 1, 13, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-10-10 11:10:00'), -- Bình 20L x6
(1, 1, 13, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-10-25 15:30:00'), -- Bình 20L x6
(2, 2, 13, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-11-12 10:00:00'); -- Bình 5L x5

-- KH013 - Thu hồi 4 lần (tỷ lệ 80%)
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, refund_amount, created_date) VALUES
(2, NULL, 13, 3, 'RETURN', 'DEPOSIT', 4, 30000.00, '2025-10-03 09:00:00'), -- Trả 4 bình 5L
(1, NULL, 13, 3, 'RETURN', 'DEPOSIT', 5, 100000.00, '2025-10-15 14:00:00'), -- Trả 5 bình 20L
(1, NULL, 13, 3, 'RETURN', 'DEPOSIT', 5, 100000.00, '2025-10-30 11:00:00'), -- Trả 5 bình 20L
(1, NULL, 13, 3, 'RETURN', 'DEPOSIT', 5, 100000.00, '2025-11-15 15:00:00'); -- Trả 5 bình 20L

-- KH015 - Phát 7 lần (khách hàng trung thành nhất)
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, created_date) VALUES
(1, 1, 15, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-09-05 08:15:00'), -- Bình 20L x6
(1, 1, 15, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-09-22 13:30:00'), -- Bình 20L x6
(2, 2, 15, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-10-05 10:45:00'), -- Bình 5L x5
(1, 1, 15, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-10-18 16:20:00'), -- Bình 20L x6
(1, 5, 15, 3, 'ISSUE', 'DEPOSIT', 1, 100000.00, '2025-10-18 16:20:00'), -- Bình 20L (khoáng) x1
(1, 1, 15, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-11-01 09:30:00'), -- Bình 20L x6
(1, 1, 15, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-11-10 14:15:00'), -- Bình 20L x6
(1, 1, 15, 3, 'ISSUE', 'DEPOSIT', 6, 100000.00, '2025-11-18 11:00:00'); -- Bình 20L x6

-- KH015 - Thu hồi 7 lần (tỷ lệ 100% - khách hàng tốt nhất)
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, refund_amount, created_date) VALUES
(1, NULL, 15, 3, 'RETURN', 'DEPOSIT', 6, 100000.00, '2025-09-18 08:00:00'), -- Trả 6 bình 20L
(1, NULL, 15, 3, 'RETURN', 'DEPOSIT', 6, 100000.00, '2025-09-30 13:00:00'), -- Trả 6 bình 20L
(2, NULL, 15, 3, 'RETURN', 'DEPOSIT', 5, 30000.00, '2025-10-10 10:00:00'), -- Trả 5 bình 5L
(1, NULL, 15, 3, 'RETURN', 'DEPOSIT', 7, 100000.00, '2025-10-25 16:00:00'), -- Trả 7 bình 20L
(1, NULL, 15, 3, 'RETURN', 'DEPOSIT', 6, 100000.00, '2025-11-05 09:00:00'), -- Trả 6 bình 20L
(1, NULL, 15, 3, 'RETURN', 'DEPOSIT', 6, 100000.00, '2025-11-12 14:00:00'), -- Trả 6 bình 20L
(1, NULL, 15, 3, 'RETURN', 'DEPOSIT', 6, 100000.00, '2025-11-20 11:00:00'); -- Trả 6 bình 20L

-- Khách hàng khác - Phát và thu hồi
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, created_date) VALUES
(2, 2, 8, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-09-15 10:00:00'), -- Bình 5L x5
(2, 2, 8, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-10-20 15:30:00'), -- Bình 5L x5
(3, 3, 10, 3, 'ISSUE', 'DEPOSIT', 6, 5000.00, '2025-09-25 09:00:00'), -- Chai 1L x6
(3, 3, 10, 3, 'ISSUE', 'DEPOSIT', 6, 5000.00, '2025-11-05 14:00:00'), -- Chai 1L x6
(3, 3, 12, 3, 'ISSUE', 'DEPOSIT', 6, 5000.00, '2025-10-12 11:00:00'), -- Chai 1L x6
(2, 2, 14, 3, 'ISSUE', 'DEPOSIT', 5, 30000.00, '2025-09-28 10:30:00'), -- Bình 5L x5
(1, 1, 14, 3, 'ISSUE', 'DEPOSIT', 5, 100000.00, '2025-11-12 16:00:00'), -- Bình 20L x5
(3, 3, 16, 3, 'ISSUE', 'DEPOSIT', 6, 5000.00, '2025-11-15 09:00:00'); -- Chai 1L x6

-- Khách hàng khác - Thu hồi (một số không thu hồi để có tỷ lệ thấp hơn)
INSERT INTO PackagingTransactions (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, refund_amount, created_date) VALUES
(2, NULL, 8, 3, 'RETURN', 'DEPOSIT', 4, 30000.00, '2025-10-05 10:00:00'), -- KH008 thu hồi 1/2
(3, NULL, 10, 3, 'RETURN', 'DEPOSIT', 5, 5000.00, '2025-10-08 09:00:00'), -- KH010 thu hồi 1/2
(2, NULL, 14, 3, 'RETURN', 'DEPOSIT', 4, 30000.00, '2025-10-15 10:00:00'); -- KH014 thu hồi 1/2
-- KH012 và KH016 không thu hồi (tỷ lệ 0%)

-- ============================================
-- TÓM TẮT DỮ LIỆU MẪU
-- ============================================
-- 
-- KHÁCH HÀNG QUAY LẠI:
-- - KH007 (Nguyễn Văn An): 5 đơn hàng
-- - KH009 (Lê Văn Cường): 6 đơn hàng  
-- - KH011 (Hoàng Văn Em): 4 đơn hàng
-- - KH013 (Đỗ Văn Giang): 5 đơn hàng
-- - KH015 (Ngô Văn Ích): 7 đơn hàng (trung thành nhất)
-- - Các khách hàng khác: 1-2 đơn hàng
--
-- TỶ LỆ THU HỒI BAO BÌ:
-- - KH007: 4/5 = 80%
-- - KH009: 5/6 = 83.3%
-- - KH011: 3/4 = 75%
-- - KH013: 4/5 = 80%
-- - KH015: 7/7 = 100% (tốt nhất)
-- - KH008: 1/2 = 50%
-- - KH010: 1/2 = 50%
-- - KH012: 0/1 = 0%
-- - KH014: 1/2 = 50%
-- - KH016: 0/1 = 0%
--
-- TỔNG: Phát nhiều bao bì, Thu hồi với tỷ lệ trung bình ~74%
-- Sử dụng Products và Packaging từ seed.sql
