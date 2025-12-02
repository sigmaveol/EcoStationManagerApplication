using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class PackagingInventoryQueries
    {
        // Lấy tồn kho bao bì theo packagingId
        public const string GetByPackaging = @"
            SELECT * FROM PackagingInventories 
            WHERE packaging_id = @PackagingId";

        // Lấy bao bì sắp hết hàng (qty_new < 10)
        public const string GetLowStockPackaging = @"
            SELECT pi.*, p.name as packaging_name, p.type as packaging_type
            FROM PackagingInventories pi
            JOIN Packaging p ON pi.packaging_id = p.packaging_id
            WHERE pi.qty_new < 10
            ORDER BY pi.qty_new ASC";

        // Cập nhật số lượng các loại bao bì
        public const string UpdateQuantities = @"
            UPDATE PackagingInventories 
            SET qty_new = @QtyNew,
                qty_in_use = @QtyInUse,
                qty_returned = @QtyReturned,
                qty_need_cleaning = @QtyNeedCleaning,
                qty_cleaned = @QtyCleaned,
                qty_damaged = @QtyDamaged,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId";

        // Chuyển bao bì mới sang đang sử dụng
        public const string TransferToInUse = @"
            UPDATE PackagingInventories 
            SET qty_new = qty_new - @Quantity,
                qty_in_use = qty_in_use + @Quantity,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId
            AND qty_new >= @Quantity";

        // Nhận bao bì trả về cần vệ sinh
        public const string ReturnForCleaning = @"
            UPDATE PackagingInventories 
            SET qty_in_use = qty_in_use - @Quantity,
                qty_need_cleaning = qty_need_cleaning + @Quantity,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId
            AND qty_in_use >= @Quantity";

        // Chuyển từ trạng thái trả về sang cần vệ sinh
        public const string MoveReturnedToNeedCleaning = @"
            UPDATE PackagingInventories 
            SET qty_returned = qty_returned - @Quantity,
                qty_need_cleaning = qty_need_cleaning + @Quantity,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId
            AND qty_returned >= @Quantity";

        // Hoàn thành vệ sinh bao bì
        public const string CompleteCleaning = @"
            UPDATE PackagingInventories 
            SET qty_need_cleaning = qty_need_cleaning - @Quantity,
                qty_cleaned = qty_cleaned + @Quantity,
                qty_new = qty_new + @Quantity,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId
            AND qty_need_cleaning >= @Quantity";

        // Đánh dấu bao bì hỏng
        public const string MarkAsDamaged = @"
            UPDATE PackagingInventories 
            SET qty_cleaned = qty_cleaned - @Quantity,
                qty_damaged = qty_damaged + @Quantity,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId
            AND qty_cleaned >= @Quantity";

        // Đánh dấu hỏng từ trạng thái trả về
        public const string MarkReturnedAsDamaged = @"
            UPDATE PackagingInventories 
            SET qty_returned = qty_returned - @Quantity,
                qty_damaged = qty_damaged + @Quantity,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId
            AND qty_returned >= @Quantity";

        // Đánh dấu hỏng từ trạng thái mới
        public const string MarkNewAsDamaged = @"
            UPDATE PackagingInventories 
            SET qty_new = qty_new - @Quantity,
                qty_damaged = qty_damaged + @Quantity,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId
            AND qty_new >= @Quantity";

        // Đánh dấu hỏng từ trạng thái cần vệ sinh
        public const string MarkNeedCleaningAsDamaged = @"
            UPDATE PackagingInventories 
            SET qty_need_cleaning = qty_need_cleaning - @Quantity,
                qty_damaged = qty_damaged + @Quantity,
                last_updated = NOW()
            WHERE packaging_id = @PackagingId
            AND qty_need_cleaning >= @Quantity";

        // Lấy tổng số lượng bao bì theo trạng thái
        public const string GetPackagingQuantities = @"
            SELECT qty_new, qty_in_use, qty_returned, 
                   qty_need_cleaning, qty_cleaned, qty_damaged
            FROM PackagingInventories 
            WHERE packaging_id = @PackagingId";

        // Kiểm tra số lượng bao bì mới có đủ không
        public const string IsNewPackagingSufficient = @"
            SELECT 1 FROM PackagingInventories 
            WHERE packaging_id = @PackagingId 
            AND qty_new >= @RequiredQuantity";

        // Tạo mới tồn kho bao bì nếu chưa tồn tại
        public const string CreatePackagingInventory = @"
            INSERT INTO PackagingInventories 
            (packaging_id, qty_new, qty_in_use, qty_returned, qty_need_cleaning, qty_cleaned, qty_damaged)
            VALUES (@PackagingId, 0, 0, 0, 0, 0, 0)";

        // Thống kê tổng quan bao bì
        public const string GetPackagingSummary = @"
            SELECT p.packaging_id, p.name, p.type,
                   COALESCE(pi.qty_new, 0) as qty_new,
                   COALESCE(pi.qty_in_use, 0) as qty_in_use,
                   COALESCE(pi.qty_cleaned, 0) as qty_cleaned,
                   COALESCE(pi.qty_need_cleaning, 0) as qty_need_cleaning,
                   COALESCE(pi.qty_damaged, 0) as qty_damaged
            FROM Packaging p
            LEFT JOIN PackagingInventories pi ON p.packaging_id = pi.packaging_id
            ORDER BY p.name";
    }
}
