# Phân tích BaseRepository - Các vấn đề và giải pháp

## Các vấn đề phát hiện:

### 1. **Lỗi hàm ToSnakeCase() - Nghiêm trọng**
Hàm `ToSnakeCase()` hiện tại không chuyển đổi đúng:
- `OrderId` → `orderid` ❌ (phải là `order_id` ✅)
- `LastUpdated` → `lastupdated` ❌ (phải là `last_updated` ✅)
- `TotalAmount` → `totalamount` ❌ (phải là `total_amount` ✅)
- `CustomerId` → `customerid` ❌ (phải là `customer_id` ✅)

**Nguyên nhân**: Hàm chỉ thêm dấu gạch dưới trước chữ cái viết hoa, nhưng không xử lý đúng các từ viết tắt hoặc các từ có nhiều chữ cái viết hoa liên tiếp.

### 2. **Lỗi loại bỏ ID column trong AddAsync/UpdateAsync**
```csharp
.Where(p => p.Name != _idColumn) // ❌ SAI
```
- `_idColumn` = `"order_id"` (snake_case từ database)
- `p.Name` = `"OrderId"` (PascalCase từ C# property)
- So sánh này sẽ luôn trả về `true`, không loại bỏ được ID column!

**Hậu quả**: Khi INSERT/UPDATE sẽ cố gắng insert/update cả ID column, gây lỗi SQL.

### 3. **Không loại bỏ Navigation Properties**
BaseRepository sẽ cố gắng INSERT/UPDATE cả các navigation properties như:
- `Customer` (kiểu `Customer`)
- `User` (kiểu `User`)
- `OrderDetails` (kiểu `List<OrderDetail>`)

**Hậu quả**: Gây lỗi SQL vì các property này không phải là cột trong database.

### 4. **Không xử lý các property phức tạp**
- Enum types
- Nullable types
- Collections
- Complex objects

## Giải pháp đề xuất:

1. Sửa hàm `ToSnakeCase()` để chuyển đổi đúng
2. Sửa logic loại bỏ ID column bằng cách so sánh với property name (PascalCase)
3. Loại bỏ navigation properties và các property không phải là cột database
4. Xử lý đúng các kiểu dữ liệu phức tạp

