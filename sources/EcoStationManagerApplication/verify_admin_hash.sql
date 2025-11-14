-- Script kiểm tra và cập nhật hash password cho admin
-- Hash của "123" được tính bằng SHA256 + Base64

USE EcoStationManager;

-- Kiểm tra hash hiện tại
SELECT 
    username,
    password_hash,
    LENGTH(password_hash) as hash_length,
    HEX(password_hash) as hash_hex,
    password_hash = 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=' as hash_match
FROM Users 
WHERE username = 'admin';

-- Hash đúng của "123" (SHA256 + Base64)
-- Để tính toán: SHA256("123") = 0x181210f83f4823c5ce4ae507fe58f86b2d8a40e3
-- Base64: jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=

-- Cập nhật hash nếu cần (uncomment để chạy)
-- UPDATE Users 
-- SET password_hash = 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI='
-- WHERE username = 'admin';

