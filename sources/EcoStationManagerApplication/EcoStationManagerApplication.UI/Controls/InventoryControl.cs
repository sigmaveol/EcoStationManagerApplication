using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EcoStationManagerApplication.UI.Common.AppColors;
using MainForm = EcoStationManagerApplication.UI.Forms.MainForm;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class InventoryControl : UserControl, IRefreshableControl
    {
        private enum ViewMode
        {
            Products,
            Packaging
        }

        private ViewMode currentMode = ViewMode.Products;
        private List<ProductInventoryDTO> productInventories;
        private List<PackagingInventoryDTO> packagingInventories;
        private List<CategoryDTO> categories;
        private bool isLoading = false;
        
        // Filter variables
        private string searchTerm = "";
        private int? selectedCategoryId = null;
        private string selectedStatus = "Tất cả";

        public InventoryControl()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            _ = LoadDataAsync();
        }

        private void InventoryControl_Load(object sender, EventArgs e)
        {
            InitializeControls();
            _ = LoadDataAsync();
        }

        private void InitializeControls()
        {
            InitializeProductDataGridView();
            InitializePackagingDataGridView();
            InitializeStatsCards();
            InitializeFilters();
            SwitchToMode(ViewMode.Products);
        }

        private async void InitializeFilters()
        {
            try
            {
                if (cmbCategoryFilter == null || cmbStatusFilter == null)
                    return;

                // Load categories
                var categoriesResult = await AppServices.CategoryService.GetActiveCategoriesAsync();
                if (categoriesResult.Success && categoriesResult.Data != null)
                {
                    categories = categoriesResult.Data.Select(c => new CategoryDTO
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name
                    }).ToList();
                }
                else
                {
                    categories = new List<CategoryDTO>();
                }

                // Populate category filter
                cmbCategoryFilter.Items.Clear();
                cmbCategoryFilter.Items.Add(new ComboItem<int?> { Text = "Tất cả", Value = null });
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        cmbCategoryFilter.Items.Add(new ComboItem<int?> { Text = category.Name, Value = category.CategoryId });
                    }
                }
                cmbCategoryFilter.DisplayMember = "Text";
                cmbCategoryFilter.ValueMember = "Value";
                if (cmbCategoryFilter.Items.Count > 0)
                    cmbCategoryFilter.SelectedIndex = 0;

                // Populate status filter
                cmbStatusFilter.Items.Clear();
                cmbStatusFilter.Items.Add("Tất cả");
                cmbStatusFilter.Items.Add("Bình thường");
                cmbStatusFilter.Items.Add("Sắp hết");
                cmbStatusFilter.Items.Add("Hết hàng");
                if (cmbStatusFilter.Items.Count > 0)
                    cmbStatusFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "khởi tạo bộ lọc");
            }
        }

        private void InitializeStatsCards()
        {
            // Cards đã được tạo trong Designer, không cần tạo động nữa
            // Chỉ cần đảm bảo các card được hiển thị và cập nhật giá trị ban đầu
            if (panelStatsCards != null && cardBatches != null && cardTotalQty != null && 
                cardLowStock != null && cardExpired != null)
            {
                // Cards đã được thêm vào panelStatsCards trong Designer
                // Chỉ cần cập nhật giá trị ban đầu
                UpdateStatCard("ProductBatches", "0", "Đang tải...");
                UpdateStatCard("ProductTotalQty", "0", "Đang tải...");
                UpdateStatCard("ProductLowStock", "0", "Đang tải...");
                UpdateStatCard("ProductExpired", "0", "Đang tải...");
            }
        }

        private void UpdateStatCard(string tag, string value, string description)
        {
            CardControl statCard = null;

            // Tìm card theo tag
            switch (tag)
            {
                case "ProductBatches":
                    statCard = cardBatches;
                    break;
                case "ProductTotalQty":
                    statCard = cardTotalQty;
                    break;
                case "ProductLowStock":
                    statCard = cardLowStock;
                    break;
                case "ProductExpired":
                    statCard = cardExpired;
                    break;
            }

            if (statCard != null)
            {
                if (statCard.InvokeRequired)
                {
                    statCard.Invoke(new Action(() =>
                    {
                        statCard.Value = value;
                        statCard.SubInfo = description;
                    }));
                }
                else
                {
                    statCard.Value = value;
                    statCard.SubInfo = description;
                }
            }
        }

        private void InitializeProductDataGridView()
        {
            dataGridViewProducts.Columns.Clear();

            var colInventoryId = new DataGridViewTextBoxColumn { Name = "InventoryId", HeaderText = "ID", Visible = false };
            dataGridViewProducts.Columns.Add(colInventoryId);
            var colProductId = new DataGridViewTextBoxColumn { Name = "ProductId", HeaderText = "ProductID", Visible = false };
            dataGridViewProducts.Columns.Add(colProductId);
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.CellSelect;

            dataGridViewProducts.Columns.Add("ProductCode", "Mã SP");
            dataGridViewProducts.Columns.Add("ProductName", "Tên sản phẩm");
            dataGridViewProducts.Columns.Add("BatchNo", "Mã lô");
            dataGridViewProducts.Columns.Add("Quantity", "Số lượng");
            dataGridViewProducts.Columns.Add("Unit", "Đơn vị");
            dataGridViewProducts.Columns.Add("ExpiryDate", "Hạn sử dụng");
            dataGridViewProducts.Columns.Add("LastUpdated", "Ngày cập nhật");
            dataGridViewProducts.Columns.Add("Status", "Trạng thái");

            // Thiết lập độ rộng cột
            if (dataGridViewProducts.Columns["ProductCode"] != null)
                dataGridViewProducts.Columns["ProductCode"].Width = 100;
            if (dataGridViewProducts.Columns["ProductName"] != null)
                dataGridViewProducts.Columns["ProductName"].Width = 200;
            if (dataGridViewProducts.Columns["BatchNo"] != null)
                dataGridViewProducts.Columns["BatchNo"].Width = 120;
            if (dataGridViewProducts.Columns["Quantity"] != null)
                dataGridViewProducts.Columns["Quantity"].Width = 100;
            if (dataGridViewProducts.Columns["Unit"] != null)
                dataGridViewProducts.Columns["Unit"].Width = 80;
            if (dataGridViewProducts.Columns["ExpiryDate"] != null)
                dataGridViewProducts.Columns["ExpiryDate"].Width = 120;
            if (dataGridViewProducts.Columns["LastUpdated"] != null)
                dataGridViewProducts.Columns["LastUpdated"].Width = 130;
            if (dataGridViewProducts.Columns["Status"] != null)
                dataGridViewProducts.Columns["Status"].Width = 120;

            // Căn giữa cho cột số lượng
            if (dataGridViewProducts.Columns["Quantity"] != null)
                dataGridViewProducts.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void InitializePackagingDataGridView()
        {
            dataGridViewPackaging.Columns.Clear();

            var colPackagingId = new DataGridViewTextBoxColumn { Name = "PackagingId", HeaderText = "ID", Visible = false };
            dataGridViewPackaging.Columns.Add(colPackagingId);
            dataGridViewPackaging.SelectionMode = DataGridViewSelectionMode.CellSelect;

            dataGridViewPackaging.Columns.Add("PackagingCode", "Mã bao bì");
            dataGridViewPackaging.Columns.Add("PackagingName", "Loại bao bì");
            dataGridViewPackaging.Columns.Add("Capacity", "Dung tích");
            dataGridViewPackaging.Columns.Add("QtyNew", "Mới");
            dataGridViewPackaging.Columns.Add("QtyInUse", "Đang dùng");
            dataGridViewPackaging.Columns.Add("QtyNeedCleaning", "Cần vệ sinh");
            dataGridViewPackaging.Columns.Add("QtyCleaned", "Đã vệ sinh");
            dataGridViewPackaging.Columns.Add("QtyDamaged", "Hỏng");
            dataGridViewPackaging.Columns.Add("Status", "Trạng thái");
            var colActions = new DataGridViewButtonColumn
            {
                Name = "colActions",
                HeaderText = "Thao tác",
                Text = "⋮",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Width = 80
            };
            dataGridViewPackaging.Columns.Add(colActions);

            // Thiết lập độ rộng cột
            if (dataGridViewPackaging.Columns["PackagingCode"] != null)
                dataGridViewPackaging.Columns["PackagingCode"].Width = 120;
            if (dataGridViewPackaging.Columns["PackagingName"] != null)
                dataGridViewPackaging.Columns["PackagingName"].Width = 200;
            if (dataGridViewPackaging.Columns["Capacity"] != null)
                dataGridViewPackaging.Columns["Capacity"].Width = 100;
            if (dataGridViewPackaging.Columns["QtyNew"] != null)
                dataGridViewPackaging.Columns["QtyNew"].Width = 100;
            if (dataGridViewPackaging.Columns["QtyInUse"] != null)
                dataGridViewPackaging.Columns["QtyInUse"].Width = 100;
            if (dataGridViewPackaging.Columns["QtyNeedCleaning"] != null)
                dataGridViewPackaging.Columns["QtyNeedCleaning"].Width = 110;
            if (dataGridViewPackaging.Columns["QtyCleaned"] != null)
                dataGridViewPackaging.Columns["QtyCleaned"].Width = 110;
            if (dataGridViewPackaging.Columns["QtyDamaged"] != null)
                dataGridViewPackaging.Columns["QtyDamaged"].Width = 100;
            if (dataGridViewPackaging.Columns["Status"] != null)
                dataGridViewPackaging.Columns["Status"].Width = 150;

            // Căn giữa cho các cột số lượng
            foreach (string colName in new[] { "QtyNew", "QtyInUse", "QtyNeedCleaning", "QtyCleaned", "QtyDamaged" })
            {
                if (dataGridViewPackaging.Columns[colName] != null)
                    dataGridViewPackaging.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dataGridViewPackaging.CellContentClick += DataGridViewPackaging_CellContentClick;
        }

        private async Task LoadDataAsync()
        {
            if (isLoading) return;

            try
            {
                isLoading = true;
                SetControlsEnabled(false);

                await LoadProductInventoryAsync();
                await LoadPackagingInventoryAsync();

                UIHelper.SafeInvoke(this, () =>
                {
                    RefreshCurrentView();
                    UpdateStatsCards();
                });
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu tồn kho");
            }
            finally
            {
                isLoading = false;
                SetControlsEnabled(true);
            }
        }

        private async Task LoadProductInventoryAsync()
        {
            try
            {
                // Lấy tất cả tồn kho theo lô
                var inventoriesResult = await AppServices.InventoryService.GetAllAsync();
                if (!inventoriesResult.Success || inventoriesResult.Data == null)
                {
                    productInventories = new List<ProductInventoryDTO>();
                    return;
                }

                var inventories = inventoriesResult.Data.ToList();
                productInventories = new List<ProductInventoryDTO>();

                // Lấy danh sách sản phẩm để map thông tin
                var productsResult = await AppServices.ProductService.GetAllProductsAsync();
                var products = productsResult.Success && productsResult.Data != null 
                    ? productsResult.Data.ToDictionary(p => p.ProductId, p => p)
                    : new Dictionary<int, Product>();

                foreach (var inv in inventories)
                {
                    if (!products.ContainsKey(inv.ProductId))
                        continue;

                    var product = products[inv.ProductId];

                    var inventoryDTO = new ProductInventoryDTO
                    {
                        InventoryId = inv.InventoryId,
                        ProductId = inv.ProductId,
                        ProductCode = product.Sku ?? "",
                        ProductName = product.Name,
                        BatchNo = inv.BatchNo ?? "",
                        Quantity = inv.Quantity,
                        Unit = product.Unit,
                        ExpiryDate = inv.ExpiryDate,
                        LastUpdated = inv.LastUpdated,
                        AlertLevel = product.MinStockLevel,
                        ProductType = product.ProductType,
                        CategoryId = product.CategoryId
                    };

                    productInventories.Add(inventoryDTO);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải tồn kho sản phẩm");
                productInventories = new List<ProductInventoryDTO>();
            }
        }

        private async Task LoadPackagingInventoryAsync()
        {
            try
            {
                var result = await AppServices.PackagingInventoryService.GetAllAsync();
                if (!result.Success || result.Data == null)
                {
                    packagingInventories = new List<PackagingInventoryDTO>();
                    return;
                }

                var packagingInventoriesList = result.Data.ToList();
                packagingInventories = new List<PackagingInventoryDTO>();

                foreach (var inv in packagingInventoriesList)
                {
                    // Lấy thông tin bao bì
                    var packagingResult = await AppServices.PackagingService.GetPackagingByIdAsync(inv.PackagingId);
                    if (!packagingResult.Success || packagingResult.Data == null)
                        continue;

                var packaging = packagingResult.Data;

                var dto = new PackagingInventoryDTO
                {
                    PackagingId = inv.PackagingId,
                    PackagingCode = packaging.Barcode ?? "",
                    PackagingName = packaging.Name,
                    Capacity = packaging.Type ?? "",
                    QtyNew = inv.QtyNew,
                    QtyInUse = inv.QtyInUse,
                    QtyReturned = inv.QtyReturned,
                    QtyNeedCleaning = inv.QtyNeedCleaning,
                    QtyCleaned = inv.QtyCleaned,
                    QtyDamaged = inv.QtyDamaged,
                };

                    packagingInventories.Add(dto);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải tồn kho bao bì");
                packagingInventories = new List<PackagingInventoryDTO>();
            }
        }

        private void RefreshCurrentView()
        {
            if (currentMode == ViewMode.Products)
            {
                BindProductData();
                lblStatusFilter.Visible = true;
                cmbStatusFilter.Visible = true;
                lblCategoryFilter.Visible = true;
                cmbCategoryFilter.Visible = true;
            }
            else
            {
                lblStatusFilter.Visible = false;
                cmbStatusFilter.Visible = false;
                lblCategoryFilter.Visible = false;
                cmbCategoryFilter.Visible = false;
                BindPackagingData();
            }
        }

        private void BindProductData()
        {
            try
            {
                if (productInventories == null)
                {
                    dataGridViewProducts.Rows.Clear();
                    return;
                }

                dataGridViewProducts.Rows.Clear();

                // Áp dụng filter
                var filteredInventories = productInventories.Where(inv =>
                {
                    if (inv == null) return false;

                    // Filter by search term
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        var searchLower = searchTerm.ToLower();
                        if ((inv.ProductName?.ToLower().Contains(searchLower) != true) &&
                            (inv.ProductCode?.ToLower().Contains(searchLower) != true) &&
                            (inv.BatchNo?.ToLower().Contains(searchLower) != true))
                            return false;
                    }

                    // Filter by category
                    if (selectedCategoryId.HasValue)
                    {
                        if (inv.CategoryId != selectedCategoryId.Value)
                            return false;
                    }

                    // Filter by status
                    if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Tất cả")
                    {
                        string status = GetProductBatchStatus(inv.Quantity, inv.ExpiryDate, inv.AlertLevel);
                        if (selectedStatus == "Bình thường" && status != "Bình thường")
                            return false;
                        if (selectedStatus == "Sắp hết" && !status.Contains("Sắp hết") && !status.Contains("Sắp hết hạn"))
                            return false;
                        if (selectedStatus == "Hết hàng" && !status.Contains("Hết hàng") && !status.Contains("Hết hạn"))
                            return false;
                    }

                    return true;
                }).OrderBy(p => p.ProductName).ThenBy(p => p.BatchNo);

                foreach (var inv in filteredInventories)
                {
                    string status = GetProductBatchStatus(inv.Quantity, inv.ExpiryDate, inv.AlertLevel);
                    string expiryDateStr = inv.ExpiryDate.HasValue 
                        ? inv.ExpiryDate.Value.ToString("dd/MM/yyyy") 
                        : "Không có";
                    string lastUpdatedStr = inv.LastUpdated.ToString("dd/MM/yyyy HH:mm");

                    var rowIndex = dataGridViewProducts.Rows.Add(
                        inv.InventoryId,
                        inv.ProductId,
                        inv.ProductCode,
                        inv.ProductName,
                        inv.BatchNo,
                        inv.Quantity.ToString("N2"),
                        inv.Unit,
                        expiryDateStr,
                        lastUpdatedStr,
                        status
                    );

                    // Đổi màu dòng theo trạng thái
                    var row = dataGridViewProducts.Rows[rowIndex];
                    if (status.Contains("Hết hạn") || status.Contains("Hết hàng"))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                        row.DefaultCellStyle.ForeColor = Error;
                    }
                    else if (status.Contains("Sắp hết hạn") || status.Contains("Sắp hết"))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 225);
                        row.DefaultCellStyle.ForeColor = Warning;
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "hiển thị dữ liệu sản phẩm");
            }
        }

        private void BindPackagingData()
        {
            try
            {
                if (packagingInventories == null)
                {
                    dataGridViewPackaging.Rows.Clear();
                    return;
                }

                dataGridViewPackaging.Rows.Clear();
                var filteredInventories = packagingInventories.Where(inv =>
                {
                    if (inv == null) return false;

                    // Filter by search term
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        var s = searchTerm.ToLower();
                        if ((inv.PackagingName?.ToLower().Contains(s) != true) &&
                            (inv.PackagingCode?.ToLower().Contains(s) != true))
                            return false;
                    }

                    // Filter by status (uses current selectedStatus if any)
                    if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Tất cả")
                    {
                        string statusCur = GetPackagingStatus(inv.QtyNew, inv.QtyDamaged);
                        if (selectedStatus == "An toàn" && statusCur != "An toàn") return false;
                        if (selectedStatus == "Sắp thiếu" && statusCur != "Sắp thiếu") return false;
                        if (selectedStatus == "Thiếu" && statusCur != "Thiếu") return false;
                        if (selectedStatus == "Hỏng nhiều" && statusCur != "Hỏng nhiều") return false;
                    }

                    return true;
                }).OrderBy(p => p.PackagingName);

                foreach (var inv in filteredInventories)
                {
                    string status = GetPackagingStatus(inv.QtyNew, inv.QtyDamaged);

                    var rowIndex = dataGridViewPackaging.Rows.Add(
                        inv.PackagingId,
                        inv.PackagingCode,
                        inv.PackagingName,
                        inv.Capacity,
                        inv.QtyNew.ToString("N0"),
                        inv.QtyInUse.ToString("N0"),
                        inv.QtyNeedCleaning.ToString("N0"),
                        inv.QtyCleaned.ToString("N0"),
                        inv.QtyDamaged.ToString("N0"),
                        status
                    );

                    // Đổi màu dòng theo trạng thái
                    var row = dataGridViewPackaging.Rows[rowIndex];
                    if (status.Contains("Sắp thiếu"))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 225);
                        row.DefaultCellStyle.ForeColor = Warning;
                    }
                    else if (status.Contains("Thiếu") || status.Contains("Hỏng nhiều"))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                        row.DefaultCellStyle.ForeColor = Error;
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "hiển thị dữ liệu bao bì");
            }
        }

        private string GetProductBatchStatus(decimal quantity, DateTime? expiryDate, decimal alertLevel)
        {
            if (quantity <= 0)
                return "Hết hàng";

            // Kiểm tra hạn sử dụng
            if (expiryDate.HasValue)
            {
                var daysUntilExpiry = (expiryDate.Value - DateTime.Today).Days;
                if (daysUntilExpiry < 0)
                    return "Hết hạn";
                if (daysUntilExpiry <= 7)
                    return "Sắp hết hạn";
            }

            // Kiểm tra số lượng
            if (quantity <= alertLevel)
                return "Sắp hết";

            return "Bình thường";
        }

        private string GetPackagingStatus(int totalStock, int qtyDamaged)
        {
            if (qtyDamaged > totalStock * 0.2)
                return "Hỏng nhiều";
            if (totalStock < 10)
                return "Thiếu";
            if (totalStock < 20)
                return "Sắp thiếu";
            return "An toàn";
        }

        private void SwitchToMode(ViewMode mode)
        {
            currentMode = mode;

            if (mode == ViewMode.Products)
            {
                btnProducts.FillColor = Primary;
                btnProducts.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                btnProducts.ForeColor = WhiteText;

                btnPackaging.FillColor = CardBackground;
                btnPackaging.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                btnPackaging.ForeColor = PrimaryText;

                dataGridViewProducts.Visible = true;
                dataGridViewPackaging.Visible = false;
            }
            else
            {
                btnPackaging.FillColor = Primary;
                btnPackaging.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                btnPackaging.ForeColor = WhiteText;

                btnProducts.FillColor = CardBackground;
                btnProducts.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                btnProducts.ForeColor = PrimaryText;

                dataGridViewPackaging.Visible = true;
                dataGridViewProducts.Visible = false;
            }

            RefreshCurrentView();
            UpdateStatsCards();
        }

        private void UpdateStatsCards()
        {
            if (panelStatsCards == null) return;

            if (currentMode == ViewMode.Products)
            {
                UpdateProductStatsCards();
            }
            else
            {
                UpdatePackagingStatsCards();
            }
        }

        private void UpdatePackagingStatsCards()
        {
            // Cập nhật label của các card
            UpdateCardLabel("ProductBatches", "Tổng số loại");
            UpdateCardLabel("ProductTotalQty", "Tổng số lượng");
            UpdateCardLabel("ProductLowStock", "Thiếu");
            UpdateCardLabel("ProductExpired", "Hỏng nhiều");

            if (packagingInventories == null || !packagingInventories.Any())
            {
                UpdateStatCard("ProductBatches", "0", "Loại bao bì");
                UpdateStatCard("ProductTotalQty", "0", "Tổng số lượng");
                UpdateStatCard("ProductLowStock", "0", "Loại thiếu");
                UpdateStatCard("ProductExpired", "0", "Loại hỏng nhiều");
                return;
            }

            // Tổng số loại bao bì
            int totalTypes = packagingInventories.Count;
            UpdateStatCard("ProductBatches", totalTypes.ToString("N0"), "Loại bao bì");

            // Tổng số lượng bao bì
            int totalStock = packagingInventories.Sum(p => p.QtyNew);
            UpdateStatCard("ProductTotalQty", totalStock.ToString("N0"), "Tổng số lượng");

            // Số loại thiếu (tổng tồn < 20)
            int lowStockTypes = packagingInventories.Count(p => p.QtyNew < 20);
            UpdateStatCard("ProductLowStock", lowStockTypes.ToString("N0"), "Loại cần bổ sung");

            // Số loại hỏng nhiều (hỏng > 20% tổng tồn)
            int damagedTypes = packagingInventories.Count(p => 
                p.QtyNew > 0 && p.QtyDamaged > p.QtyNew * 0.2m);
            UpdateStatCard("ProductExpired", damagedTypes.ToString("N0"), "Loại cần kiểm tra");
        }

        private void UpdateProductStatsCards()
        {
            // Cập nhật label của các card
            UpdateCardLabel("ProductBatches", "Tổng số lô");
            UpdateCardLabel("ProductTotalQty", "Tổng số lượng");
            UpdateCardLabel("ProductLowStock", "Sắp hết");
            UpdateCardLabel("ProductExpired", "Hết hạn");

            if (productInventories == null || !productInventories.Any())
            {
                UpdateStatCard("ProductBatches", "0", "Lô hàng");
                UpdateStatCard("ProductTotalQty", "0", "Đơn vị");
                UpdateStatCard("ProductLowStock", "0", "Lô sắp hết");
                UpdateStatCard("ProductExpired", "0", "Lô hết hạn");
                return;
            }

            // Tổng số lô
            int totalBatches = productInventories.Count;
            UpdateStatCard("ProductBatches", totalBatches.ToString("N0"), "Lô hàng");

            // Tổng số lượng
            decimal totalQuantity = productInventories.Sum(p => p.Quantity);
            string unit = productInventories.FirstOrDefault()?.Unit ?? "";
            UpdateStatCard("ProductTotalQty", totalQuantity.ToString("N2"), unit);

            // Số lô sắp hết (số lượng <= alert level hoặc sắp hết hạn trong 7 ngày)
            int lowStockBatches = productInventories.Count(p =>
            {
                bool lowQuantity = p.Quantity > 0 && p.Quantity <= p.AlertLevel;
                bool nearExpiry = p.ExpiryDate.HasValue && 
                    (p.ExpiryDate.Value - DateTime.Today).Days <= 7 && 
                    (p.ExpiryDate.Value - DateTime.Today).Days >= 0;
                return lowQuantity || nearExpiry;
            });
            UpdateStatCard("ProductLowStock", lowStockBatches.ToString("N0"), "Lô cần chú ý");

            // Số lô hết hạn hoặc hết hàng
            int expiredBatches = productInventories.Count(p =>
            {
                bool expired = p.ExpiryDate.HasValue && (p.ExpiryDate.Value - DateTime.Today).Days < 0;
                bool outOfStock = p.Quantity <= 0;
                return expired || outOfStock;
            });
            UpdateStatCard("ProductExpired", expiredBatches.ToString("N0"), "Lô cần xử lý");
        }

        private void UpdateCardLabel(string tag, string newLabel)
        {
            CardControl statCard = null;

            // Tìm card theo tag
            switch (tag)
            {
                case "ProductBatches":
                    statCard = cardBatches;
                    break;
                case "ProductTotalQty":
                    statCard = cardTotalQty;
                    break;
                case "ProductLowStock":
                    statCard = cardLowStock;
                    break;
                case "ProductExpired":
                    statCard = cardExpired;
                    break;
            }

            if (statCard != null)
            {
                if (statCard.InvokeRequired)
                {
                    statCard.Invoke(new Action(() => statCard.Title = newLabel));
                }
                else
                {
                    statCard.Title = newLabel;
                }
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            try
            {
                UIHelper.SafeInvoke(this, () =>
                {
                    btnProducts.Enabled = enabled;
                    btnPackaging.Enabled = enabled;
                    btnExportExcel.Enabled = enabled;
                    btnExportPDF.Enabled = enabled;
                    btnRefresh.Enabled = enabled;
                });
            }
            catch { }
        }

        #region Event Handlers

        private void btnProducts_Click(object sender, EventArgs e)
        {
            SwitchToMode(ViewMode.Products);
        }

        private void btnPackaging_Click(object sender, EventArgs e)
        {
            SwitchToMode(ViewMode.Packaging);
        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            try
            {
                // Tìm MainForm parent và chuyển tới tab Nhập kho, sau đó mở form tạo phiếu
                var mainForm = FindMainForm();
                if (mainForm != null)
                {
                    mainForm.NavigateToStockInAndOpenCreateForm();
                }
                else
                {
                    if (currentMode == ViewMode.Products)
                    {
                        using (var form = new MultiProductStockInForm())
                        {
                            var owner = this.FindForm() ?? this.TopLevelControl as Form;
                            DialogResult result = owner != null
                                ? FormHelper.ShowModalWithDim(owner, form)
                                : form.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                _ = LoadDataAsync();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Chức năng nhập kho bao bì sẽ được triển khai sau.", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form nhập kho");
            }
        }

        private void btnStockOut_Click(object sender, EventArgs e)
        {
            try
            {
                // Tìm MainForm parent và chuyển tới tab Xuất kho, sau đó mở form tạo phiếu
                var mainForm = FindMainForm();
                if (mainForm != null)
                {
                    mainForm.NavigateToStockOutAndOpenCreateForm();
                }
                else
                {
                    if (currentMode == ViewMode.Products)
                    {
                        using (var form = new MultiProductStockOutForm())
                        {
                            var owner = this.FindForm() ?? this.TopLevelControl as Form;
                            DialogResult result = owner != null
                                ? FormHelper.ShowModalWithDim(owner, form)
                                : form.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                _ = LoadDataAsync();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Chức năng xuất kho bao bì sẽ được triển khai sau.", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form xuất kho");
            }
        }

        /// <summary>
        /// Tìm MainForm từ control hierarchy
        /// </summary>
        private MainForm FindMainForm()
        {
            // Sử dụng FindForm() để tìm Form chứa control này
            var form = this.FindForm();
            if (form is MainForm mainForm)
            {
                return mainForm;
            }
            
            // Fallback: tìm qua TopLevelControl
            if (this.TopLevelControl is MainForm topLevelMainForm)
            {
                return topLevelMainForm;
            }
            
            return null;
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.Title = "Chọn nơi lưu PDF";
                saveDialog.FileName = currentMode == ViewMode.Products ? "TonKho_SanPham.pdf" : "TonKho_BaoBi.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var pdfExporter = new PdfExporter();
                    var title = currentMode == ViewMode.Products ? "Danh sách tồn kho sản phẩm" : "Danh sách tồn kho bao bì";

                    var dataTable = currentMode == ViewMode.Products ? BuildProductsDataTable() : BuildPackagingDataTable();
                    byte[] chartBytes = GenerateOverviewChartImage();

                    pdfExporter.ExportToPdf(dataTable, saveDialog.FileName, title, null, chartBytes, true, true);

                    MessageBox.Show($"Đã xuất PDF thành công\nFile: {saveDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất PDF");
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Chọn nơi lưu Excel";
                saveDialog.FileName = currentMode == ViewMode.Products ? "TonKho_SanPham.xlsx" : "TonKho_BaoBi.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelExporter = new ExcelExporter();
                    var worksheetName = currentMode == ViewMode.Products ? "Tồn kho sản phẩm" : "Tồn kho bao bì";
                    var title = currentMode == ViewMode.Products ? "Danh sách tồn kho sản phẩm" : "Danh sách tồn kho bao bì";

                    var dataTable = currentMode == ViewMode.Products ? BuildProductsDataTable() : BuildPackagingDataTable();
                    byte[] chartBytes = GenerateOverviewChartImage();

                    excelExporter.ExportToExcel(dataTable, saveDialog.FileName, worksheetName, null, title, chartBytes, true);

                    MessageBox.Show($"Đã xuất Excel thành công\nFile: {saveDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất Excel");
            }
        }

        private DataTable BuildProductsDataTable()
        {
            var table = new DataTable();
            table.Columns.Add("Mã SP");
            table.Columns.Add("Tên sản phẩm");
            table.Columns.Add("Mã lô");
            table.Columns.Add("Số lượng", typeof(decimal));
            table.Columns.Add("Đơn vị");
            table.Columns.Add("Hạn sử dụng");
            table.Columns.Add("Ngày cập nhật");
            table.Columns.Add("Trạng thái");

            var list = productInventories ?? new List<ProductInventoryDTO>();
            var filtered = list.Where(inv =>
            {
                if (inv == null) return false;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var s = searchTerm.ToLower();
                    if ((inv.ProductName?.ToLower().Contains(s) != true) &&
                        (inv.ProductCode?.ToLower().Contains(s) != true) &&
                        (inv.BatchNo?.ToLower().Contains(s) != true))
                        return false;
                }

                if (selectedCategoryId.HasValue && inv.CategoryId != selectedCategoryId.Value)
                    return false;

                if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Tất cả")
                {
                    string statusCur = GetProductBatchStatus(inv.Quantity, inv.ExpiryDate, inv.AlertLevel);
                    if (selectedStatus == "Bình thường" && statusCur != "Bình thường") return false;
                    if (selectedStatus == "Sắp hết" && !statusCur.Contains("Sắp hết") && !statusCur.Contains("Sắp hết hạn")) return false;
                    if (selectedStatus == "Hết hàng" && !statusCur.Contains("Hết hàng") && !statusCur.Contains("Hết hạn")) return false;
                }

                return true;
            }).OrderBy(p => p.ProductName).ThenBy(p => p.BatchNo);

            foreach (var inv in filtered)
            {
                string status = GetProductBatchStatus(inv.Quantity, inv.ExpiryDate, inv.AlertLevel);
                string expiryDateStr = inv.ExpiryDate.HasValue ? inv.ExpiryDate.Value.ToString("dd/MM/yyyy") : "Không có";
                string lastUpdatedStr = inv.LastUpdated.ToString("dd/MM/yyyy HH:mm");

                var row = table.NewRow();
                row[0] = inv.ProductCode;
                row[1] = inv.ProductName;
                row[2] = inv.BatchNo;
                row[3] = inv.Quantity;
                row[4] = inv.Unit;
                row[5] = expiryDateStr;
                row[6] = lastUpdatedStr;
                row[7] = status;
                table.Rows.Add(row);
            }
            return table;
        }

        private DataTable BuildPackagingDataTable()
        {
            var table = new DataTable();
            table.Columns.Add("Mã bao bì");
            table.Columns.Add("Loại bao bì");
            table.Columns.Add("Dung tích");
            table.Columns.Add("Mới", typeof(int));
            table.Columns.Add("Đang dùng", typeof(int));
            table.Columns.Add("Cần vệ sinh", typeof(int));
            table.Columns.Add("Đã vệ sinh", typeof(int));
            table.Columns.Add("Hỏng", typeof(int));
            table.Columns.Add("Trạng thái");

            var list = packagingInventories ?? new List<PackagingInventoryDTO>();
            var filtered = list.Where(inv =>
            {
                if (inv == null) return false;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var s = searchTerm.ToLower();
                    if ((inv.PackagingName?.ToLower().Contains(s) != true) &&
                        (inv.PackagingCode?.ToLower().Contains(s) != true))
                        return false;
                }

                if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Tất cả")
                {
                    string statusCur = GetPackagingStatus(inv.QtyNew, inv.QtyDamaged);
                    if (selectedStatus == "An toàn" && statusCur != "An toàn") return false;
                    if (selectedStatus == "Sắp thiếu" && statusCur != "Sắp thiếu") return false;
                    if (selectedStatus == "Thiếu" && statusCur != "Thiếu") return false;
                    if (selectedStatus == "Hỏng nhiều" && statusCur != "Hỏng nhiều") return false;
                }

                return true;
            }).OrderBy(p => p.PackagingName);

            foreach (var inv in filtered)
            {
                string status = GetPackagingStatus(inv.QtyNew, inv.QtyDamaged);

                var row = table.NewRow();
                row[0] = inv.PackagingCode;
                row[1] = inv.PackagingName;
                row[2] = inv.Capacity;
                row[3] = inv.QtyNew;
                row[4] = inv.QtyInUse;
                row[5] = inv.QtyNeedCleaning;
                row[6] = inv.QtyCleaned;
                row[7] = inv.QtyDamaged;
                row[8] = status;
                table.Rows.Add(row);
            }
            return table;
        }

        private byte[] GenerateOverviewChartImage()
        {
            var chart = new Chart();
            chart.Width = 800;
            chart.Height = 400;
            chart.BackColor = System.Drawing.Color.White;
            var chartArea = new ChartArea("area");
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.LabelStyle.Format = "N0";
            chart.ChartAreas.Add(chartArea);

            var series = new Series("data");
            series.ChartType = SeriesChartType.Column;
            series.Color = System.Drawing.Color.FromArgb(31, 107, 59);
            series.IsValueShownAsLabel = true;
            chart.Series.Add(series);

            if (currentMode == ViewMode.Products)
            {
                var list = productInventories ?? new List<ProductInventoryDTO>();
                int countNormal = list.Count(p => GetProductBatchStatus(p.Quantity, p.ExpiryDate, p.AlertLevel) == "Bình thường");
                int countNear = list.Count(p => GetProductBatchStatus(p.Quantity, p.ExpiryDate, p.AlertLevel).Contains("Sắp"));
                int countOut = list.Count(p => GetProductBatchStatus(p.Quantity, p.ExpiryDate, p.AlertLevel).Contains("Hết"));
                series.Points.AddXY("Bình thường", countNormal);
                series.Points.AddXY("Sắp hết/hạn", countNear);
                series.Points.AddXY("Hết hàng/hạn", countOut);
                chart.Titles.Add("Tổng quan lô sản phẩm");
            }
            else
            {
                var list = packagingInventories ?? new List<PackagingInventoryDTO>();
                int totalNew = list.Sum(p => p.QtyNew);
                int totalInUse = list.Sum(p => p.QtyInUse);
                int totalNeedClean = list.Sum(p => p.QtyNeedCleaning);
                int totalCleaned = list.Sum(p => p.QtyCleaned);
                int totalDamaged = list.Sum(p => p.QtyDamaged);
                series.Points.AddXY("Mới", totalNew);
                series.Points.AddXY("Đang dùng", totalInUse);
                series.Points.AddXY("Cần vệ sinh", totalNeedClean);
                series.Points.AddXY("Đã vệ sinh", totalCleaned);
                series.Points.AddXY("Hỏng", totalDamaged);
                chart.Titles.Add("Tổng quan bao bì");
            }

            using (var bmp = new Bitmap(chart.Width, chart.Height))
            {
                chart.DrawToBitmap(bmp, new Rectangle(0, 0, chart.Width, chart.Height));
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void dataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý click vào cell của bảng sản phẩm nếu cần
        }

        private void dataGridViewPackaging_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý click vào cell của bảng bao bì nếu cần
        }

        private async void DataGridViewPackaging_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var grid = dataGridViewPackaging;
            var colName = grid.Columns[e.ColumnIndex].Name;
            if (colName != "colActions") return;

            var packagingIdObj = grid.Rows[e.RowIndex].Cells["PackagingId"].Value;
            if (packagingIdObj == null || !int.TryParse(packagingIdObj.ToString(), out var packagingId)) return;

            var menu = new ContextMenuStrip();
            var menuMoveReturnedToClean = new ToolStripMenuItem("Chuyển trả về → cần vệ sinh");
            var menuCompleteCleaning = new ToolStripMenuItem("Chuyển sang đã vệ sinh");
            var menuMarkDamaged = new ToolStripMenuItem("Chuyển sang trạng thái hỏng");

            menuMoveReturnedToClean.Click += async (s, ev) =>
            {
                var qty = PromptQuantity("Nhập số lượng chuyển từ trả về sang cần vệ sinh");
                if (qty <= 0) return;
                var res = await AppServices.PackagingInventoryService.MoveReturnedToNeedCleaningAsync(packagingId, qty);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadPackagingInventoryAsync();
                RefreshCurrentView();
            };

            menuCompleteCleaning.Click += async (s, ev) =>
            {
                var qty = PromptQuantity("Nhập số lượng chuyển sang đã vệ sinh");
                if (qty <= 0) return;
                var res = await AppServices.PackagingInventoryService.CompleteCleaningAsync(packagingId, qty);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadPackagingInventoryAsync();
                RefreshCurrentView();
            };

            var subFromReturned = new ToolStripMenuItem("Từ trả về");
            var subFromNew = new ToolStripMenuItem("Từ mới");
            var subFromNeedClean = new ToolStripMenuItem("Từ cần vệ sinh");

            subFromReturned.Click += async (s, ev) =>
            {
                var qty = PromptQuantity("Nhập số lượng hỏng từ trả về");
                if (qty <= 0) return;
                var res = await AppServices.PackagingInventoryService.MarkReturnedAsDamagedAsync(packagingId, qty);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadPackagingInventoryAsync();
                RefreshCurrentView();
            };

            subFromNew.Click += async (s, ev) =>
            {
                var qty = PromptQuantity("Nhập số lượng hỏng từ mới");
                if (qty <= 0) return;
                var res = await AppServices.PackagingInventoryService.MarkNewAsDamagedAsync(packagingId, qty);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadPackagingInventoryAsync();
                RefreshCurrentView();
            };

            subFromNeedClean.Click += async (s, ev) =>
            {
                var qty = PromptQuantity("Nhập số lượng hỏng từ cần vệ sinh");
                if (qty <= 0) return;
                var res = await AppServices.PackagingInventoryService.MarkNeedCleaningAsDamagedAsync(packagingId, qty);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await LoadPackagingInventoryAsync();
                RefreshCurrentView();
            };

            menuMarkDamaged.DropDownItems.Add(subFromReturned);
            menuMarkDamaged.DropDownItems.Add(subFromNew);
            menuMarkDamaged.DropDownItems.Add(subFromNeedClean);

            menu.Items.Add(menuMoveReturnedToClean);
            menu.Items.Add(menuCompleteCleaning);
            menu.Items.Add(menuMarkDamaged);
            var cellRect = grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            menu.Show(grid, new Point(cellRect.Left + cellRect.Width / 2, cellRect.Bottom));
        }

        private int PromptQuantity(string title)
        {
            using (var form = new Form())
            {
                form.Text = title;
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.Width = 350;
                form.Height = 160;

                var label = new Label { Text = "Số lượng:", Left = 20, Top = 20, Width = 80 };
                var numeric = new NumericUpDown { Left = 110, Top = 16, Width = 200, Minimum = 0, Maximum = 100000, Value = 1 };
                var btnOk = new Button { Text = "OK", Left = 110, Width = 90, Top = 60, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Hủy", Left = 220, Width = 90, Top = 60, DialogResult = DialogResult.Cancel };

                form.Controls.Add(label);
                form.Controls.Add(numeric);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                var owner = this.FindForm() ?? this.TopLevelControl as Form;
                var result = owner != null ? FormHelper.ShowModalWithDim(owner, form) : form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return (int)numeric.Value;
                }
                return 0;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch == null) return;
            searchTerm = txtSearch.Text ?? "";
            RefreshCurrentView();
        }

        private void cmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategoryFilter == null || cmbCategoryFilter.SelectedItem == null)
            {
                selectedCategoryId = null;
                return;
            }

            if (cmbCategoryFilter.SelectedItem is ComboItem<int?> selectedItem)
            {
                selectedCategoryId = selectedItem.Value;
            }
            else
            {
                selectedCategoryId = null;
            }
            RefreshCurrentView();
        }

        private void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStatusFilter == null || cmbStatusFilter.SelectedItem == null)
            {
                selectedStatus = "Tất cả";
                return;
            }
            selectedStatus = cmbStatusFilter.SelectedItem.ToString() ?? "Tất cả";
            RefreshCurrentView();
        }

        #endregion

    }

    // Helper class for ComboBox
    public class ComboItem<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
        public override string ToString() => Text;
    }

    // DTO classes
    public class ProductInventoryDTO
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal AlertLevel { get; set; }
        public ProductType ProductType { get; set; }
        public int? CategoryId { get; set; }
    }

    public class PackagingInventoryDTO
    {
        public int PackagingId { get; set; }
        public string PackagingCode { get; set; }
        public string PackagingName { get; set; }
        public string Capacity { get; set; }
        public int QtyNew { get; set; }
        public int QtyInUse { get; set; }
        public int QtyReturned { get; set; }
        public int QtyNeedCleaning { get; set; }
        public int QtyCleaned { get; set; }
        public int QtyDamaged { get; set; }
    }
}
