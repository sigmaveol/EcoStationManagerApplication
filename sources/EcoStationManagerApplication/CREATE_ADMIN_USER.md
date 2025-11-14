# Hướng dẫn tạo tài khoản Admin mặc định

## Thông tin tài khoản
- **Username:** `admin`
- **Password:** `123`
- **Role:** `ADMIN`

## Các bước thực hiện

### Cách 1: Sử dụng Script SQL (Khuyến nghị)

1. **Mở MySQL Workbench hoặc MySQL Command Line**

2. **Kết nối đến database `EcoStationManager`**

3. **Chạy script SQL:**
   ```sql
   USE EcoStationManager;

   -- Xóa user admin cũ nếu tồn tại
   DELETE FROM Users WHERE username = 'admin';

   -- Tạo tài khoản admin mới
   INSERT INTO Users (username, password_hash, fullname, role, is_active, created_date) 
   VALUES (
       'admin', 
       'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=',  -- SHA256 hash của "123"
       'Quản trị viên hệ thống', 
       'ADMIN', 
       TRUE, 
       NOW()
   );

   -- Kiểm tra kết quả
   SELECT user_id, username, fullname, role, is_active, created_date 
   FROM Users 
   WHERE username = 'admin';
   ```

4. **Hoặc chạy file script:**
   ```bash
   mysql -u root -p EcoStationManager < create_admin_user.sql
   ```

### Cách 2: Sử dụng Tool PasswordHasher (Để tính hash password mới)

1. **Biên dịch và chạy file `PasswordHasher.cs`:**
   ```bash
   csc PasswordHasher.cs
   PasswordHasher.exe
   ```

2. **Nhập password cần hash (hoặc Enter để dùng '123')**

3. **Copy hash được tạo và sử dụng trong SQL INSERT**

### Cách 3: Sử dụng ứng dụng (Nếu đã có form quản lý user)

1. Mở ứng dụng với quyền admin hiện có
2. Vào phần quản lý người dùng
3. Tạo user mới với thông tin:
   - Username: `admin`
   - Password: `123`
   - Role: `ADMIN`

## Xác minh đăng nhập

Sau khi tạo tài khoản, thử đăng nhập với:
- **Username:** `admin`
- **Password:** `123`

## Lưu ý

- Hash password được tính bằng SHA256 + Base64 encoding (giống `SecurityHelper.HashPassword`)
- Hash của "123" là: `jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=`
- Nếu muốn đổi password, sử dụng tool `PasswordHasher.cs` để tính hash mới

## Troubleshooting

### Lỗi: Duplicate entry 'admin'
- User admin đã tồn tại. Chạy `DELETE FROM Users WHERE username = 'admin';` trước khi INSERT

### Lỗi: Table 'Users' doesn't exist
- Chưa tạo database. Chạy script `EcoStationManager_V0.sql` hoặc `EcoStationManager_V1.sql` trước

### Không đăng nhập được
- Kiểm tra password hash có đúng không
- Kiểm tra `is_active = TRUE`
- Kiểm tra role có đúng 'ADMIN' không

