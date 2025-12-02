USE EcoStationManager;

CREATE TABLE GoogleIntegrationConfig (
    integration_id INT AUTO_INCREMENT PRIMARY KEY,
    sheet_url VARCHAR(500) NOT NULL,
    spreadsheet_id VARCHAR(100) NOT NULL,
    sheet_name VARCHAR(100) NULL,
    api_key VARCHAR(200) NULL,
    last_sync_time DATETIME NULL,
    is_active BIT NOT NULL DEFAULT 1,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE GoogleOrderMapping (
    id INT AUTO_INCREMENT PRIMARY KEY,
    sheet_row_index INT NOT NULL,
    order_id INT NOT NULL,
    synced_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    config_id INT NULL,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (config_id) REFERENCES GoogleIntegrationConfig(integration_id)
);



