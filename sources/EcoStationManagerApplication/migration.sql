-- thay đổi cấu trúc nếu có

USE EcoStationManager;
-- Bảng trạm
CREATE TABLE IF NOT EXISTS Stations (
    station_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255),
    address TEXT,
    phone VARCHAR(50),
    station_type ENUM('warehouse', 'refill', 'hybrid', 'other') DEFAULT 'refill',
    manager INT, -- User_id của quản lý trạm
    is_active BOOLEAN DEFAULT TRUE,
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (manager) REFERENCES Users(user_id) ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE Notifications (
    notification_id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NULL,
    title VARCHAR(255),
    message TEXT,
    type ENUM('info','warning','error','success','lowstock','order','refill','system'),
    is_read BOOLEAN DEFAULT FALSE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);