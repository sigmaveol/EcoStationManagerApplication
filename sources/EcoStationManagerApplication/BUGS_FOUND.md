# Danh sách các lỗi không mong muốn đã phát hiện

## 🔴 Lỗi nghiêm trọng - Có thể gây crash:

### 1. **DashboardControl.cs - NullReferenceException khi truy cập Data**
**Vị trí**: Nhiều dòng
- **Dòng 94**: `todayOrdersResult.Data.Count()` - Không kiểm tra `Data != null`
- **Dòng 162**: `recentOrdersResult.Data.OrderByDescending(...)` - Không kiểm tra `Data != null`
- **Dòng 246**: `lowStockResult.Data.Count()` - Không kiểm tra `Data != null`
- **Dòng 262**: `packagingResult.Data.Sum(...)` - Không kiểm tra `Data != null`
- **Dòng 277**: `pendingOrdersResult.Data.Count()` - Không kiểm tra `Data != null`
- **Dòng 194**: `productResult.Data` - Không kiểm tra `Data != null` trước khi dùng

**Hậu quả**: Nếu service trả về `Success = true` nhưng `Data = null`, sẽ throw `NullReferenceException`

**Giải pháp**: Thêm kiểm tra `&& result.Data != null` trước khi truy cập Data

---

### 2. **DatabaseHelper.cs - RegisterCustomTypeMappings() không hoạt động**
**Vị trí**: Dòng 69
```csharp
var entityTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.Namespace == "EcoStationManagerApplication.Models.Entities" ...
```

**Vấn đề**: 
- `Assembly.GetExecutingAssembly()` trả về assembly của DAL
- Entities nằm trong assembly `Models`, không phải `DAL`
- Sẽ không tìm thấy bất kỳ entity nào → mapping không được đăng ký

**Hậu quả**: Dapper không thể map snake_case sang PascalCase → các properties sẽ là null/default

**Giải pháp**: 
```csharp
// Tìm assembly chứa entities
var entityAssembly = typeof(Order).Assembly; // Hoặc bất kỳ entity nào
var entityTypes = entityAssembly.GetTypes()...
```

---

### 3. **DashboardControl.cs - Logic duplicate và không nhất quán**
**Vị trí**: Dòng 167-171 và 212-215

**Vấn đề**:
- Dòng 167-171: Kiểm tra `!recentOrders.Any()` → MessageBox.Show và return
- Dòng 212-215: Lại kiểm tra `!recentOrders.Any()` → Thêm row vào grid

**Hậu quả**: 
- Nếu không có orders, code sẽ return ở dòng 170, không bao giờ đến dòng 212
- Logic không nhất quán

**Giải pháp**: Xóa một trong hai check, chỉ giữ một

---

### 4. **DashboardControl.cs - Label không khớp**
**Vị trí**: Dòng 112 và 122

**Vấn đề**:
- Dòng 112: `UpdateStatCard("Bao bì đang được sử dụng", ...)`
- Dòng 122: `UpdateStatCard("Bao bì đang sử dụng", ...)` (thiếu "được")

**Hậu quả**: UpdateStatCard không tìm thấy card → không cập nhật được giá trị

**Giải pháp**: Sửa label cho nhất quán

---

## ⚠️ Lỗi tiềm ẩn - Có thể gây kết quả sai:

### 5. **DashboardControl.cs - CalculateMonthlyRevenue() logic sai**
**Vị trí**: Dòng 230
```csharp
var endDate = DateTime.Now; // ❌ SAI - phải là cuối tháng
```

**Vấn đề**: 
- `endDate` nên là ngày cuối tháng, không phải `DateTime.Now`
- Nếu chạy vào ngày 15, sẽ chỉ tính từ ngày 1 đến ngày 15, không phải cả tháng

**Giải pháp**: 
```csharp
var endDate = startDate.AddMonths(1).AddDays(-1);
```

---

### 6. **DashboardControl.cs - LoadRecentOrders() không kiểm tra Data null**
**Vị trí**: Dòng 157-165

**Vấn đề**:
```csharp
if (recentOrdersResult.Success && dgvRecentOrders != null)
{
    var recentOrders = recentOrdersResult.Data.OrderByDescending(...)
```

**Hậu quả**: Nếu `Data` là null, sẽ throw NullReferenceException

**Giải pháp**: Thêm `&& recentOrdersResult.Data != null`

---

### 7. **DashboardControl.cs - Product null checking không đầy đủ**
**Vị trí**: Dòng 194-198

**Vấn đề**:
```csharp
if (productResult.Success)
{
    var product = productResult.Data; // ❌ Không kiểm tra null
    productInfo = $"{product.Name} ({firstProduct.Quantity} {product.Unit})";
}
```

**Hậu quả**: Nếu `Data` là null, sẽ throw NullReferenceException khi truy cập `product.Name`

**Giải pháp**: Thêm `&& productResult.Data != null`

---

## 📋 Tóm tắt các lỗi cần sửa:

1. ✅ Thêm null checking cho tất cả `.Data` properties
2. ✅ Sửa `Assembly.GetExecutingAssembly()` thành `typeof(Order).Assembly`
3. ✅ Xóa duplicate check trong LoadRecentOrders()
4. ✅ Sửa label "Bao bì đang được sử dụng" cho nhất quán
5. ✅ Sửa `endDate` trong CalculateMonthlyRevenue()
6. ✅ Thêm null checking cho productResult.Data

