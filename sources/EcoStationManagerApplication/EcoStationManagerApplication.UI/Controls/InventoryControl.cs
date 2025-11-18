using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
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
using static EcoStationManagerApplication.UI.Common.AppFonts;
using MainForm = EcoStationManagerApplication.UI.Forms.MainForm;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class InventoryControl : UserControl
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

            dataGridViewPackaging.Columns.Add("PackagingCode", "Mã bao bì");
            dataGridViewPackaging.Columns.Add("PackagingName", "Loại bao bì");
            dataGridViewPackaging.Columns.Add("Capacity", "Dung tích");
            dataGridViewPackaging.Columns.Add("QtyInUse", "Đang dùng");
            dataGridViewPackaging.Columns.Add("QtyReturned", "Thu hồi");
            dataGridViewPackaging.Columns.Add("QtyDamaged", "Hỏng");
            dataGridViewPackaging.Columns.Add("TotalStock", "Tổng tồn");
            dataGridViewPackaging.Columns.Add("Status", "Trạng thái");

            // Thiết lập độ rộng cột
            if (dataGridViewPackaging.Columns["PackagingCode"] != null)
                dataGridViewPackaging.Columns["PackagingCode"].Width = 120;
            if (dataGridViewPackaging.Columns["PackagingName"] != null)
                dataGridViewPackaging.Columns["PackagingName"].Width = 200;
            if (dataGridViewPackaging.Columns["Capacity"] != null)
                dataGridViewPackaging.Columns["Capacity"].Width = 100;
            if (dataGridViewPackaging.Columns["QtyInUse"] != null)
                dataGridViewPackaging.Columns["QtyInUse"].Width = 100;
            if (dataGridViewPackaging.Columns["QtyReturned"] != null)
                dataGridViewPackaging.Columns["QtyReturned"].Width = 100;
            if (dataGridViewPackaging.Columns["QtyDamaged"] != null)
                dataGridViewPackaging.Columns["QtyDamaged"].Width = 100;
            if (dataGridViewPackaging.Columns["TotalStock"] != null)
                dataGridViewPackaging.Columns["TotalStock"].Width = 120;
            if (dataGridViewPackaging.Columns["Status"] != null)
                dataGridViewPackaging.Columns["Status"].Width = 150;

            // Căn giữa cho các cột số lượng
            foreach (string colName in new[] { "QtyInUse", "QtyReturned", "QtyDamaged", "TotalStock" })
            {
                if (dataGridViewPackaging.Columns[colName] != null)
                    dataGridViewPackaging.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
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
                    int totalStock = inv.QtyNew + inv.QtyInUse + inv.QtyReturned + inv.QtyCleaned;

                    var dto = new PackagingInventoryDTO
                    {
                        PackagingId = inv.PackagingId,
                        PackagingCode = packaging.Barcode ?? "",
                        PackagingName = packaging.Name,
                        Capacity = packaging.Type ?? "",
                        QtyInUse = inv.QtyInUse,
                        QtyReturned = inv.QtyReturned,
                        QtyDamaged = inv.QtyDamaged,
                        TotalStock = totalStock
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
            }
            else
            {
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

                foreach (var inv in packagingInventories.OrderBy(p => p.PackagingName))
                {
                    string status = GetPackagingStatus(inv.TotalStock, inv.QtyDamaged);

                    var rowIndex = dataGridViewPackaging.Rows.Add(
                        inv.PackagingId,
                        inv.PackagingCode,
                        inv.PackagingName,
                        inv.Capacity,
                        inv.QtyInUse.ToString("N0"),
                        inv.QtyReturned.ToString("N0"),
                        inv.QtyDamaged.ToString("N0"),
                        inv.TotalStock.ToString("N0"),
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
            int totalStock = packagingInventories.Sum(p => p.TotalStock);
            UpdateStatCard("ProductTotalQty", totalStock.ToString("N0"), "Tổng số lượng");

            // Số loại thiếu (tổng tồn < 20)
            int lowStockTypes = packagingInventories.Count(p => p.TotalStock < 20);
            UpdateStatCard("ProductLowStock", lowStockTypes.ToString("N0"), "Loại cần bổ sung");

            // Số loại hỏng nhiều (hỏng > 20% tổng tồn)
            int damagedTypes = packagingInventories.Count(p => 
                p.TotalStock > 0 && p.QtyDamaged > p.TotalStock * 0.2m);
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
                    btnStockIn.Enabled = enabled;
                    btnStockOut.Enabled = enabled;
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
                    // Fallback: mở form trực tiếp nếu không tìm thấy MainForm
                    if (currentMode == ViewMode.Products)
                    {
                        using (var form = new StockInForm())
                        {
                            if (form.ShowDialog() == DialogResult.OK)
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
                    // Fallback: mở form trực tiếp nếu không tìm thấy MainForm
                    if (currentMode == ViewMode.Products)
                    {
                        using (var form = new StockOutForm())
                        {
                            if (form.ShowDialog() == DialogResult.OK)
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
                if (currentMode == ViewMode.Products)
                {
                    MessageBox.Show("Chức năng xuất PDF báo cáo sản phẩm sẽ được triển khai sau.", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Chức năng xuất PDF báo cáo bao bì sẽ được triển khai sau.", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất PDF");
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
        public int QtyInUse { get; set; }
        public int QtyReturned { get; set; }
        public int QtyDamaged { get; set; }
        public int TotalStock { get; set; }
    }
}
