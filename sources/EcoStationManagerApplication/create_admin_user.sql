-- ============================================
-- Script tạo tài khoản Admin mặc định
-- Username: admin
-- Password: 123
-- ============================================

USE EcoStationManager;

-- Xóa user admin cũ nếu tồn tại (để tránh lỗi duplicate)
DELETE FROM Users WHERE username = 'admin';

-- Tạo tài khoản admin mới
-- Password hash của "123" được tính bằng SHA256 + Base64
-- Hash: jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=
INSERT INTO Users (username, password_hash, fullname, role, is_active, created_date) 
VALUES (
    'admin', 
    'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=',  -- SHA256 hash của "123"
    'Quản trị viên hệ thống', 
    'ADMIN', 
    TRUE, 
    NOW()
);
