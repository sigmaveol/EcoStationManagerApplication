# KẾ HOẠCH CẬP NHẬT PROCEDURES VÀ TRIGGERS

## 📊 PHÂN TÍCH HIỆN TRẠNG

### 1. **Code C# hiện tại**
- ✅ **KHÔNG sử dụng stored procedures** - Code đang dùng Repository pattern với direct SQL INSERT/UPDATE
- ✅ **Triggers tự động chạy** - Không cần thay đổi code C#
- ✅ **Không có breaking changes** - Code C# không bị ảnh hưởng

### 2. **Database Schema**
- Schema sử dụng **PascalCase**: `Orders`, `OrderDetails`, `StockIn`, `StockOut`, etc.
- File gốc (`procedures.txt`, `triggers.txt`) dùng **lowercase**: `orders`, `orderdetails`
- File FIXED (`procedures_FIXED.txt`, `triggers_FIXED.txt`) dùng **PascalCase**: `Orders`, `OrderDetails`

### 3. **Vấn đề Case Sensitivity**
- **Windows MySQL/MariaDB**: Không phân biệt chữ hoa/thường → Triggers gốc vẫn chạy được
- **Linux MySQL/MariaDB**: Phân biệt chữ hoa/thường → Triggers gốc sẽ **LỖI**

---

## ✅ KẾT LUẬN: **CẦN CẬP NHẬT**

### Lý do:
1. **Tương thích đa nền tảng**: Đảm bảo chạy được trên cả Windows và Linux
2. **Phù hợp với schema**: Tên bảng trong triggers/procedures khớp với schema
3. **Sửa lỗi nghiêm trọng**: 
   - Thiếu kiểm tra tồn kho trong `sp_UpdateOrderStatus`
   - Race condition trong `trg_Orders_BeforeInsert`
   - Thiếu validation đầu vào
4. **Tính năng mới**: Tích điểm khi trả bao bì

---

## 🎯 KẾ HOẠCH THỰC HIỆN

### BƯỚC 1: Backup Database (BẮT BUỘC)
```sql
-- Backup toàn bộ database
mysqldump -u username -p database_name > backup_before_migration_$(date +%Y%m%d_%H%M%S).sql

-- Hoặc backup chỉ procedures và triggers
mysqldump -u username -p --routines --triggers database_name > backup_procedures_triggers.sql
```

### BƯỚC 2: Kiểm tra Schema hiện tại
```sql
-- Kiểm tra tên bảng thực tế
SHOW TABLES;

-- Kiểm tra triggers hiện có
SHOW TRIGGERS;

-- Kiểm tra procedures hiện có
SHOW PROCEDURE STATUS WHERE Db = 'database_name';
```

### BƯỚC 3: Xóa Triggers và Procedures cũ
```sql
-- Xóa tất cả triggers cũ
DROP TRIGGER IF EXISTS trg_OrderDetails_AfterInsert;
DROP TRIGGER IF EXISTS trg_OrderDetails_AfterUpdate;
DROP TRIGGER IF EXISTS trg_OrderDetails_AfterDelete;
DROP TRIGGER IF EXISTS trg_Orders_BeforeInsert;
DROP TRIGGER IF EXISTS trg_StockIn_AfterInsert;
DROP TRIGGER IF EXISTS trg_StockOut_AfterInsert;
DROP TRIGGER IF EXISTS trg_PackagingTransactions_AfterInsert;
DROP TRIGGER IF EXISTS trg_CleaningSchedules_BeforeUpdate;
DROP TRIGGER IF EXISTS trg_Orders_AfterUpdate;
DROP TRIGGER IF EXISTS trg_Orders_BeforeUpdate;
DROP TRIGGER IF EXISTS trg_Customers_BeforeInsert;
DROP TRIGGER IF EXISTS trg_Customers_AfterUpdate;

-- Xóa tất cả procedures cũ (nếu có)
DROP PROCEDURE IF EXISTS sp_CreateOrder;
DROP PROCEDURE IF EXISTS sp_UpdateOrderStatus;
DROP PROCEDURE IF EXISTS sp_StockIn_Product;
DROP PROCEDURE IF EXISTS sp_StockOut_Product;
DROP PROCEDURE IF EXISTS sp_GetLowStockAlert;
DROP PROCEDURE IF EXISTS sp_GetExpiryAlert;
DROP PROCEDURE IF EXISTS sp_IssuePackaging;
DROP PROCEDURE IF EXISTS sp_ReturnPackaging;
DROP PROCEDURE IF EXISTS sp_ProcessCleanedPackaging;
DROP PROCEDURE IF EXISTS sp_CalculateCustomerPoints;
DROP PROCEDURE IF EXISTS sp_UpdateCustomerRank;
```

### BƯỚC 4: Deploy Triggers và Procedures mới
```bash
# Chạy file triggers_FIXED.txt
mysql -u username -p database_name < triggers_FIXED.txt

# Chạy file procedures_FIXED.txt
mysql -u username -p database_name < procedures_FIXED.txt
```

Hoặc trong MySQL client:
```sql
-- Copy nội dung từ triggers_FIXED.txt và chạy
-- Copy nội dung từ procedures_FIXED.txt và chạy
```

### BƯỚC 5: Kiểm tra sau khi deploy
```sql
-- Kiểm tra triggers đã được tạo
SHOW TRIGGERS;

-- Kiểm tra procedures đã được tạo
SHOW PROCEDURE STATUS WHERE Db = 'database_name';

-- Test trigger: Tạo order detail và kiểm tra total_amount
INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price)
VALUES (1, 1, 10, 50000);
SELECT * FROM Orders WHERE order_id = 1; -- Kiểm tra total_amount đã được cập nhật

-- Test procedure: Tạo đơn hàng
CALL sp_CreateOrder(1, 3, 'Test Address', 'Test Note', 1, 
    '[{"product_id":1,"quantity":10,"unit_price":50000}]', 
    @order_id, @message);
SELECT @order_id, @message;
```

---

## ⚠️ RỦI RO VÀ CÁCH XỬ LÝ

### Rủi ro THẤP:
1. **Code C# không bị ảnh hưởng** - Không sử dụng procedures
2. **Triggers chỉ tự động chạy** - Không cần thay đổi code

### Rủi ro TRUNG BÌNH:
1. **Thay đổi behavior của triggers**:
   - Trigger `trg_Orders_BeforeInsert` có logic mới để tránh race condition
   - Trigger `trg_StockOut_AfterInsert` có error handling tốt hơn
   - **Giải pháp**: Test kỹ trên môi trường dev trước

2. **Validation mới trong procedures**:
   - `sp_CreateOrder` có validation đầu vào nghiêm ngặt hơn
   - `sp_UpdateOrderStatus` có kiểm tra tồn kho
   - **Giải pháp**: Đảm bảo dữ liệu hợp lệ trước khi deploy

### Rủi ro CAO (nếu không cẩn thận):
1. **Mất dữ liệu nếu không backup**
   - **Giải pháp**: BẮT BUỘC backup trước khi deploy

2. **Lỗi case sensitivity trên Linux**
   - **Giải pháp**: Deploy file FIXED để tương thích

---

## 📋 CHECKLIST TRƯỚC KHI DEPLOY

- [ ] Đã backup database
- [ ] Đã test trên môi trường dev/staging
- [ ] Đã kiểm tra schema (PascalCase hay lowercase?)
- [ ] Đã chuẩn bị rollback plan
- [ ] Đã thông báo team về maintenance window (nếu cần)
- [ ] Đã test các chức năng chính sau khi deploy:
  - [ ] Tạo đơn hàng
  - [ ] Cập nhật trạng thái đơn hàng
  - [ ] Nhập/xuất kho
  - [ ] Phát/thu hồi bao bì
  - [ ] Tính điểm khách hàng

---

## 🔄 ROLLBACK PLAN

Nếu có vấn đề sau khi deploy:

```sql
-- 1. Xóa triggers và procedures mới
-- (Dùng script ở BƯỚC 3)

-- 2. Restore từ backup
mysql -u username -p database_name < backup_before_migration.sql

-- Hoặc chỉ restore triggers/procedures
mysql -u username -p database_name < backup_procedures_triggers.sql
```

---

## 💡 KHUYẾN NGHỊ

### Nên làm NGAY:
1. ✅ **Deploy triggers_FIXED.txt** - Sửa case sensitivity và race condition
2. ✅ **Deploy procedures_FIXED.txt** - Sửa lỗi nghiêm trọng và thêm tính năng mới

### Có thể làm SAU:
1. ⏳ **Refactor code C# để sử dụng procedures** - Nếu muốn tận dụng stored procedures
2. ⏳ **Thêm unit tests cho procedures** - Để đảm bảo chất lượng
3. ⏳ **Tạo migration script tự động** - Để dễ deploy

---

## 📝 GHI CHÚ QUAN TRỌNG

1. **File gốc (`procedures.txt`, `triggers.txt`)**: Giữ lại để tham khảo, KHÔNG xóa
2. **File FIXED (`procedures_FIXED.txt`, `triggers_FIXED.txt`)**: Dùng để deploy vào database
3. **Code C#**: KHÔNG CẦN thay đổi vì không sử dụng procedures
4. **Tên file**: Có thể đổi tên sau khi deploy thành công:
   - `procedures_FIXED.txt` → `procedures.txt` (backup file cũ)
   - `triggers_FIXED.txt` → `triggers.txt` (backup file cũ)

---

*Tài liệu được tạo: 2025-01-20*

