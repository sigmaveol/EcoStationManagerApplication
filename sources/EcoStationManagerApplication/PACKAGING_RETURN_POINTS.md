# QUY TẮC TÍCH ĐIỂM KHI TRẢ BAO BÌ

## 📋 TỔNG QUAN

Hệ thống đã được cập nhật để **tích điểm thưởng** cho khách hàng khi họ trả bao bì về. Điều này khuyến khích khách hàng:
- ✅ Trả bao bì về để tái sử dụng (bảo vệ môi trường)
- ✅ Giữ bao bì sạch sẽ (để được nhiều điểm hơn)
- ✅ Tăng tần suất quay lại của khách hàng

---

## 🎯 QUY TẮC TÍCH ĐIỂM

### Điểm cơ bản
- **Mỗi bao bì trả về = 10 điểm** (có thể điều chỉnh trong procedure)

### Hệ số theo điều kiện bao bì

| Điều kiện | Mô tả | Hệ số | Điểm/bao bì |
|-----------|-------|-------|-------------|
| **0 - Sạch** | Bao bì sạch, có thể tái sử dụng ngay | **1.0** (100%) | **10 điểm** |
| **1 - Cần vệ sinh** | Bao bì bẩn, cần làm sạch | **0.5** (50%) | **5 điểm** |
| **2 - Hỏng** | Bao bì bị hỏng, không thể tái sử dụng | **0.0** (0%) | **0 điểm** |

### Ví dụ tính điểm

**Ví dụ 1:** Khách hàng trả 5 bao bì sạch
- Điểm = 5 × 10 × 1.0 = **50 điểm**

**Ví dụ 2:** Khách hàng trả 3 bao bì sạch + 2 bao bì cần vệ sinh
- Điểm = (3 × 10 × 1.0) + (2 × 10 × 0.5) = 30 + 10 = **40 điểm**

**Ví dụ 3:** Khách hàng trả 4 bao bì hỏng
- Điểm = 4 × 10 × 0.0 = **0 điểm**

---

## 🔧 CẬP NHẬT TRONG CODE

### Procedure: `sp_ReturnPackaging`

Đã được cập nhật với logic tích điểm:

```sql
-- Tính điểm dựa trên số lượng và điều kiện
IF v_condition_multiplier > 0 THEN
    SET v_points_per_unit = 10; -- 10 điểm mỗi bao bì
    SET v_points_earned = FLOOR(p_quantity * v_points_per_unit * v_condition_multiplier);
    
    -- Cập nhật điểm khách hàng
    UPDATE Customers
    SET total_point = total_point + v_points_earned
    WHERE customer_id = p_customer_id;
    
    -- Kiểm tra nâng hạng
    CALL sp_UpdateCustomerRank(p_customer_id, v_rank_msg);
END IF;
```

---

## 📊 SO SÁNH VỚI TÍCH ĐIỂM ĐƠN HÀNG

| Hoạt động | Cách tích điểm | Ví dụ |
|-----------|----------------|-------|
| **Đơn hàng hoàn thành** | 1 điểm / 10,000 VNĐ giá trị đơn | Đơn 500,000 VNĐ = 50 điểm |
| **Trả bao bì sạch** | 10 điểm / bao bì | 5 bao bì = 50 điểm |
| **Trả bao bì cần vệ sinh** | 5 điểm / bao bì | 5 bao bì = 25 điểm |

---

## ⚙️ ĐIỀU CHỈNH QUY TẮC

Nếu muốn thay đổi số điểm, chỉnh sửa trong procedure `sp_ReturnPackaging`:

```sql
-- Thay đổi điểm cơ bản (hiện tại: 10 điểm)
SET v_points_per_unit = 20; -- Đổi thành 20 điểm mỗi bao bì

-- Thay đổi hệ số điều kiện
-- Bao bì cần vệ sinh: từ 0.5 thành 0.7 (70% điểm)
SET v_condition_multiplier = 0.7;
```

---

## ✅ LỢI ÍCH

1. **Khuyến khích bảo vệ môi trường**: Khách hàng có động lực trả bao bì về
2. **Giảm chi phí**: Bao bì được tái sử dụng thay vì mua mới
3. **Tăng lòng trung thành**: Khách hàng có thêm cách tích điểm
4. **Khuyến khích giữ sạch**: Bao bì sạch được nhiều điểm hơn

---

## 🔍 KIỂM TRA

Để test tính năng này:

```sql
-- Test trả bao bì sạch
CALL sp_ReturnPackaging(1, 1, 5, 0, 50000.00, 1, @message);
SELECT @message;
-- Kết quả: "Packaging returned successfully. Earned 50 points"

-- Kiểm tra điểm khách hàng
SELECT customer_id, name, total_point, `rank` 
FROM Customers 
WHERE customer_id = 1;
```

---

## 📝 LƯU Ý

1. **Điểm chỉ được tích khi bao bì không hỏng** (condition != 2)
2. **Điểm được cộng vào tổng điểm** và có thể làm khách hàng nâng hạng
3. **Quy tắc tích điểm có thể điều chỉnh** theo chính sách của công ty
4. **Nên thông báo cho khách hàng** về số điểm đã tích được khi trả bao bì

---

*Tài liệu được cập nhật: 2025-01-20*

