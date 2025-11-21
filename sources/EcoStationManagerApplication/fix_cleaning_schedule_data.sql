-- Script để kiểm tra và sửa dữ liệu không hợp lệ trong bảng CleaningSchedules
-- Chạy script này để tìm và sửa các bản ghi có cleaning_type hoặc status không hợp lệ

-- 1. Kiểm tra các bản ghi có cleaning_type NULL hoặc rỗng
SELECT cs_id, cleaning_type, status, cleaning_date, notes
FROM CleaningSchedules
WHERE cleaning_type IS NULL 
   OR cleaning_type = ''
   OR cleaning_type NOT IN ('TANK', 'PACKAGING');

-- 2. Kiểm tra các bản ghi có status NULL hoặc rỗng hoặc không hợp lệ
SELECT cs_id, cleaning_type, status, cleaning_date, notes
FROM CleaningSchedules
WHERE status IS NULL 
   OR status = ''
   OR status NOT IN ('SCHEDULED', 'COMPLETED', 'OVERDUE', 'CANCELLED');

-- 3. Sửa các bản ghi có cleaning_type NULL hoặc rỗng (mặc định là TANK)
UPDATE CleaningSchedules
SET cleaning_type = 'TANK'
WHERE cleaning_type IS NULL 
   OR cleaning_type = '';

-- 4. Sửa các bản ghi có status NULL hoặc rỗng (mặc định là SCHEDULED)
UPDATE CleaningSchedules
SET status = 'SCHEDULED'
WHERE status IS NULL 
   OR status = '';

-- 5. Kiểm tra lại sau khi sửa
SELECT cs_id, cleaning_type, status, cleaning_date, notes
FROM CleaningSchedules
WHERE cleaning_type IS NULL 
   OR cleaning_type = ''
   OR status IS NULL 
   OR status = '';


