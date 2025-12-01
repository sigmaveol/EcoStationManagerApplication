USE EcoStationManager;

-- Migration: Thêm cột cleaned_datetime vào bảng CleaningSchedules
-- Chỉ khi status là COMPLETED thì cleaned_datetime mới có giá trị

ALTER TABLE CleaningSchedules 
ADD COLUMN cleaned_datetime DATETIME NULL AFTER status;

-- Cập nhật dữ liệu hiện có: Nếu status là COMPLETED và có thông tin trong notes, 
-- cố gắng extract cleaned_datetime từ notes (tùy chọn, có thể bỏ qua nếu không cần)
-- UPDATE CleaningSchedules 
-- SET cleaned_datetime = cleaning_date 
-- WHERE status = 'COMPLETED' AND cleaned_datetime IS NULL;



CREATE TABLE Notifications (
    notification_id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NULL,
    title VARCHAR(255),
    message TEXT,
    type ENUM('info','warning','error','success','lowstock','order','refill','system'),
    is_read BOOLEAN DEFAULT FALSE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);