# Hướng dẫn sửa lỗi đăng nhập Admin

## Vấn đề
Không thể đăng nhập với:
- Username: `admin`
- Password: `123`
- Hash trong DB: `jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=`

## Các bước kiểm tra và sửa

### Bước 1: Kiểm tra hash trong database

Chạy query sau trong MySQL:
```sql
USE EcoStationManager;

SELECT 
    username,
    password_hash,
    LENGTH(password_hash) as hash_length,
    HEX(password_hash) as hash_hex
FROM Users 
WHERE username = 'admin';
```

### Bước 2: Xác minh hash đúng

Hash đúng của password "123" phải là:
```
jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=
```

Độ dài: 44 ký tự

### Bước 3: Cập nhật hash nếu sai

Nếu hash trong database không đúng, chạy:
```sql
USE EcoStationManager;

UPDATE Users 
SET password_hash = 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI='
WHERE username = 'admin';

-- Kiểm tra lại
SELECT username, password_hash FROM Users WHERE username = 'admin';
```

### Bước 4: Xóa và tạo lại user admin

Nếu vẫn không được, xóa và tạo lại:
```sql
USE EcoStationManager;

-- Xóa user cũ
DELETE FROM Users WHERE username = 'admin';

-- Tạo lại với hash đúng
INSERT INTO Users (username, password_hash, fullname, role, is_active, created_date) 
VALUES (
    'admin', 
    'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=',
    'Quản trị viên hệ thống', 
    'ADMIN', 
    TRUE, 
    NOW()
);

-- Kiểm tra
SELECT * FROM Users WHERE username = 'admin';
```

### Bước 5: Kiểm tra log

Sau khi chạy ứng dụng và thử đăng nhập, kiểm tra log file trong thư mục `logs/` để xem:
- Hash được tính toán từ password input
- Hash được lấy từ database
- So sánh có match không

Log sẽ hiển thị:
```
Authentication attempt - Username: admin, StoredHash: '...', InputHash: '...', Match: true/false
```

## Nguyên nhân có thể

1. **Hash trong database sai**: Có thể hash được insert sai hoặc bị thay đổi
2. **Khoảng trắng**: Có thể có khoảng trắng thừa trong password_hash
3. **Encoding**: Vấn đề với encoding khi đọc từ database
4. **Case sensitivity**: MySQL có thể so sánh case-sensitive tùy collation

## Giải pháp đã áp dụng

Code đã được cập nhật để:
- Lấy user theo username trước
- So sánh hash trong C# code thay vì SQL
- Trim cả hai hash trước khi so sánh
- Log chi tiết để debug

## Test lại

Sau khi cập nhật hash trong database:
1. Chạy lại ứng dụng
2. Thử đăng nhập với `admin` / `123`
3. Kiểm tra log để xem hash có match không

