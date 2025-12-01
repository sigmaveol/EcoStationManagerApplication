using EcoStationManagerApplication.Common.Exporters;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using static EcoStationManagerApplication.UI.Common.AppColors;
using static EcoStationManagerApplication.UI.Common.AppFonts;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StockOutManagementControl : UserControl, IRefreshableControl
    {
        private List<StockOutDetail> _stockOutList = new List<StockOutDetail>();
        private bool _isLoading = false;

        // Filter variables
        private string _searchTerm = "";
        private string _selectedPurpose = "Tất cả";
        private DateTime? _fromDate = null;
        private DateTime? _toDate = null;

        public StockOutManagementControl()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            _ = LoadDataAsync();
        }

        private void StockOutManagementControl_Load(object sender, EventArgs e)
        {
            InitializeControls();
            _ = LoadDataAsync();
        }

        private void InitializeControls()
        {
            InitializeDataGridView();
            InitializeStatsCards();
            InitializeFilters();
        }

        private void InitializeDataGridView()
        {
            dgvStockOut.Columns.Clear();
            dgvStockOut.AutoGenerateColumns = false;
            dgvStockOut.AllowUserToAddRows = false;
            dgvStockOut.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStockOut.MultiSelect = false;

            // Thêm các cột
            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StockOutId",
                HeaderText = "ID",
                DataPropertyName = "StockOutId",
                Visible = false
            });

            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ReferenceNumber",
                HeaderText = "Mã phiếu",
                Width = 120,
                ReadOnly = true
            });

            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Sản phẩm",
                Width = 200,
                ReadOnly = true,
                DataPropertyName = "ProductName"
            });

            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BatchNo",
                HeaderText = "Lô hàng",
                Width = 120,
                ReadOnly = true,
                DataPropertyName = "BatchNo"
            });

            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Số lượng",
                Width = 100,
                ReadOnly = true,
                DataPropertyName = "Quantity",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Purpose",
                HeaderText = "Mục đích",
                Width = 120,
                ReadOnly = true,
                DataPropertyName = "Purpose"
            });

            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderCode",
                HeaderText = "Đơn hàng",
                Width = 120,
                ReadOnly = true
            });

            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Notes",
                HeaderText = "Ghi chú",
                Width = 150,
                ReadOnly = true,
                DataPropertyName = "Notes"
            });

            dgvStockOut.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedDate",
                HeaderText = "Ngày xuất",
                Width = 130,
                ReadOnly = true,
                DataPropertyName = "CreatedDate"
            });

            // Cột thao tác
            var actionColumn = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Thao tác",
                Text = "Xem chi tiết",
                UseColumnTextForButtonValue = true,
                Width = 120
            };
            dgvStockOut.Columns.Add(actionColumn);
        }

        private void InitializeStatsCards()
        {
            // Cards đã được tạo trong Designer
            UpdateStatCard("TotalStockOuts", "0", "Tổng phiếu xuất");
            UpdateStatCard("Sale", "0", "Xuất bán hàng");
            UpdateStatCard("Transfer", "0", "Chuyển kho");
            UpdateStatCard("Waste", "0", "Hao hụt");
            UpdateStatCard("TotalQuantity", "0", "Tổng số lượng");
        }

        private void UpdateStatCard(string tag, string value, string description)
        {
            CardControl statCard = null;

            switch (tag)
            {
                case "TotalStockOuts":
                    statCard = cardTotalStockOuts;
                    break;
                case "Sale":
                    statCard = cardSale;
                    break;
                case "Transfer":
                    statCard = cardTransfer;
                    break;
                case "Waste":
                    statCard = cardWaste;
                    break;
                case "TotalQuantity":
                    statCard = cardTotalQuantity;
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

        private void InitializeFilters()
        {
            // Mục đích
            cmbPurposeFilter.Items.Clear();
            cmbPurposeFilter.Items.Add("Tất cả");
            cmbPurposeFilter.Items.Add("Bán hàng");
            cmbPurposeFilter.Items.Add("Chuyển kho");
            cmbPurposeFilter.Items.Add("Hao hụt");
            cmbPurposeFilter.Items.Add("Mẫu thử nghiệm");
            cmbPurposeFilter.SelectedIndex = 0;

            // Thời gian - mặc định là tháng hiện tại
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpToDate.Value = DateTime.Now;
            _fromDate = dtpFromDate.Value.Date;
            _toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1); // End of day
        }

        private async Task LoadDataAsync()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                SetControlsEnabled(false);

                // Lấy phiếu xuất kho theo khoảng thời gian đã chọn (sử dụng query có JOIN)
                DateTime startDate = _fromDate ?? DateTime.Now.AddMonths(-1);
                DateTime endDate = _toDate ?? DateTime.Now;
                
                var result = await AppServices.StockOutService.GetStockOutDetailsByDateRangeAsync(
                    startDate, endDate);

                if (result.Success && result.Data != null)
                {
                    // Dữ liệu đã được JOIN từ database, không cần map thủ công
                    _stockOutList = result.Data.ToList();

                    UIHelper.SafeInvoke(this, () =>
                    {
                        RefreshDataGridView();
                        UpdateStatsCards();
                    });
                }
                else
                {
                    _stockOutList = new List<StockOutDetail>();
                    UIHelper.SafeInvoke(this, () =>
                    {
                        RefreshDataGridView();
                        UpdateStatsCards();
                    });
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu xuất kho");
                _stockOutList = new List<StockOutDetail>();
            }
            finally
            {
                _isLoading = false;
                SetControlsEnabled(true);
            }
        }

        private void RefreshDataGridView()
        {
            dgvStockOut.Rows.Clear();

            if (_stockOutList == null || !_stockOutList.Any())
            {
                return;
            }

            // Áp dụng filter
            var filteredList = _stockOutList.Where(item =>
            {
                // Filter by search term - tìm theo batch_no, product name
                if (!string.IsNullOrWhiteSpace(_searchTerm))
                {
                    var searchLower = _searchTerm.ToLower();
                    if (!item.BatchNo?.ToLower().Contains(searchLower) == true &&
                        !item.ProductName?.ToLower().Contains(searchLower) == true &&
                        !item.PackagingName?.ToLower().Contains(searchLower) == true)
                        return false;
                }

                // Filter by purpose
                if (_selectedPurpose != "Tất cả")
                {
                    string purposeText = GetPurposeText(item.Purpose);
                    if (purposeText != _selectedPurpose)
                        return false;
                }

                // Filter by date range
                if (_fromDate.HasValue && item.CreatedDate < _fromDate.Value)
                    return false;
                if (_toDate.HasValue && item.CreatedDate > _toDate.Value)
                    return false;

                return true;
            }).OrderByDescending(x => x.CreatedDate);

            foreach (var item in filteredList)
            {
                var referenceNumber = $"XK{item.StockOutId:D6}";
                var productName = item.ProductName ?? item.PackagingName ?? "-";
                var batchNo = item.BatchNo ?? "-";
                var quantity = item.Quantity.ToString("N2");
                var purpose = GetPurposeText(item.Purpose);
                var orderCode = item.OrderId.HasValue ? $"DH{item.OrderId.Value:D6}" : "-";
                var notes = item.Notes ?? "-";
                var createdDate = item.CreatedDate.ToString("dd/MM/yyyy HH:mm");

                var rowIndex = dgvStockOut.Rows.Add(
                    item.StockOutId,
                    referenceNumber,
                    productName,
                    batchNo,
                    quantity,
                    purpose,
                    orderCode,
                    notes,
                    createdDate
                );

                // Store item in row tag for detail view
                dgvStockOut.Rows[rowIndex].Tag = item;
            }
        }

        private string GetPurposeText(StockOutPurpose purpose)
        {
            switch (purpose)
            {
                case StockOutPurpose.SALE:
                    return "Bán hàng";
                case StockOutPurpose.TRANSFER:
                    return "Chuyển kho";
                case StockOutPurpose.DAMAGE:
                    return "Hao hụt";
                default:
                    return purpose.ToString();
            }
        }

        private void UpdateStatsCards()
        {
            if (_stockOutList == null || !_stockOutList.Any())
            {
                UpdateStatCard("TotalStockOuts", "0", "Tổng phiếu xuất");
                UpdateStatCard("Sale", "0", "Xuất bán hàng");
                UpdateStatCard("Transfer", "0", "Chuyển kho");
                UpdateStatCard("Waste", "0", "Hao hụt");
                UpdateStatCard("TotalQuantity", "0", "Tổng số lượng");
                return;
            }

            // Tổng phiếu xuất
            int totalStockOuts = _stockOutList.Count;
            UpdateStatCard("TotalStockOuts", totalStockOuts.ToString("N0"), "Tổng phiếu xuất");

            // Xuất bán hàng
            int saleCount = _stockOutList.Count(x => x.Purpose == StockOutPurpose.SALE);
            UpdateStatCard("Sale", saleCount.ToString("N0"), "Phiếu bán hàng");

            // Chuyển kho
            int transferCount = _stockOutList.Count(x => x.Purpose == StockOutPurpose.TRANSFER);
            UpdateStatCard("Transfer", transferCount.ToString("N0"), "Phiếu chuyển kho");

            // Hao hụt
            int wasteCount = _stockOutList.Count(x => x.Purpose == StockOutPurpose.DAMAGE );
            UpdateStatCard("Waste", wasteCount.ToString("N0"), "Phiếu hao hụt");

            // Tổng số lượng xuất
            decimal totalQuantity = _stockOutList.Sum(x => x.Quantity);
            UpdateStatCard("TotalQuantity", totalQuantity.ToString("N2"), "Tổng số lượng");
        }

        private void SetControlsEnabled(bool enabled)
        {
            try
            {
                UIHelper.SafeInvoke(this, () =>
                {
                    btnExportExcel.Enabled = enabled;
                    btnExportPDF.Enabled = enabled;
                    btnCreateStockOut.Enabled = enabled;
                    btnRefresh.Enabled = enabled;
                    txtSearch.Enabled = enabled;
                    cmbPurposeFilter.Enabled = enabled;
                    dtpFromDate.Enabled = enabled;
                    dtpToDate.Enabled = enabled;
                });
            }
            catch { }
        }

        #region Event Handlers

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void btnCreateStockOut_Click(object sender, EventArgs e)
        {
            OpenCreateStockOutForm();
        }

        /// <summary>
        /// Public method để mở form tạo phiếu xuất kho từ bên ngoài
        /// </summary>
        public void OpenCreateStockOutForm()
        {
            try
            {
                using (var form = new MultiProductStockOutForm())
                {
                    var owner = this.FindForm();
                    DialogResult result = owner != null
                        ? FormHelper.ShowModalWithDim(owner, form)
                        : form.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        _ = LoadDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form xuất kho");
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_stockOutList == null || !_stockOutList.Any())
                {
                    UIHelper.ShowWarningMessage("Không có dữ liệu để xuất!");
                    return;
                }

                // Áp dụng filter giống như trong RefreshDataGridView
                var filteredList = _stockOutList.Where(item =>
                {
                    // Filter by search term
                    if (!string.IsNullOrWhiteSpace(_searchTerm))
                    {
                        var searchLower = _searchTerm.ToLower();
                        if (!item.BatchNo?.ToLower().Contains(searchLower) == true &&
                            !item.ProductName?.ToLower().Contains(searchLower) == true &&
                            !item.PackagingName?.ToLower().Contains(searchLower) == true)
                            return false;
                    }

                    // Filter by purpose
                    if (_selectedPurpose != "Tất cả")
                    {
                        string purposeText = GetPurposeText(item.Purpose);
                        if (purposeText != _selectedPurpose)
                            return false;
                    }

                    // Filter by date range
                    if (_fromDate.HasValue && item.CreatedDate < _fromDate.Value)
                        return false;
                    if (_toDate.HasValue && item.CreatedDate > _toDate.Value)
                        return false;

                    return true;
                }).ToList();

                if (!filteredList.Any())
                {
                    UIHelper.ShowWarningMessage("Không có dữ liệu phù hợp với bộ lọc để xuất!");
                    return;
                }

                // Hiển thị SaveFileDialog
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                    saveDialog.FileName = $"XuatKho_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    saveDialog.Title = "Xuất danh sách xuất kho ra Excel";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Tạo DataTable từ dữ liệu đã filter
                        var dataTable = CreateDataTableForExport(filteredList);

                        // Tạo headers cho Excel
                        var headers = new Dictionary<string, string>
                        {
                            { "STT", "STT" },
                            { "ReferenceNumber", "Mã phiếu" },
                            { "ProductName", "Sản phẩm/Bao bì" },
                            { "BatchNo", "Mã lô" },
                            { "Quantity", "Số lượng" },
                            { "Purpose", "Mục đích" },
                            { "OrderCode", "Đơn hàng" },
                            { "CreatedDate", "Ngày xuất" },
                            { "CreatedBy", "Người xuất" },
                            { "Notes", "Ghi chú" }
                        };

                        // Tạo title với thông tin filter
                        var fromDateStr = _fromDate?.ToString("dd/MM/yyyy") ?? "Tất cả";
                        var toDateStr = _toDate?.ToString("dd/MM/yyyy") ?? "Tất cả";
                        var purposeStr = _selectedPurpose ?? "Tất cả";
                        var title = $"DANH SÁCH XUẤT KHO\n" +
                                   $"Từ ngày: {fromDateStr} - Đến ngày: {toDateStr}\n" +
                                   $"Mục đích: {purposeStr}\n" +
                                   $"Tổng số: {filteredList.Count} phiếu xuất";

                        // Xuất Excel
                        var excelExporter = new ExcelExporter();
                        var charts = GenerateChartsForExport(filteredList);
                        excelExporter.ExportToExcel(dataTable, saveDialog.FileName, "Xuất kho", headers, title, charts);

                        UIHelper.ShowSuccessMessage($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất Excel");
            }
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (_stockOutList == null || !_stockOutList.Any())
                {
                    UIHelper.ShowWarningMessage("Không có dữ liệu để xuất!");
                    return;
                }

                var filteredList = _stockOutList.Where(item =>
                {
                    if (!string.IsNullOrWhiteSpace(_searchTerm))
                    {
                        var s = _searchTerm.ToLower();
                        if (!item.BatchNo?.ToLower().Contains(s) == true &&
                            !item.ProductName?.ToLower().Contains(s) == true &&
                            !item.PackagingName?.ToLower().Contains(s) == true)
                            return false;
                    }

                    if (_selectedPurpose != "Tất cả")
                    {
                        string purposeText = GetPurposeText(item.Purpose);
                        if (purposeText != _selectedPurpose)
                            return false;
                    }

                    if (_fromDate.HasValue && item.CreatedDate < _fromDate.Value)
                        return false;
                    if (_toDate.HasValue && item.CreatedDate > _toDate.Value)
                        return false;

                    return true;
                }).ToList();

                if (!filteredList.Any())
                {
                    UIHelper.ShowWarningMessage("Không có dữ liệu phù hợp với bộ lọc để xuất!");
                    return;
                }

                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveDialog.FileName = $"XuatKho_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    saveDialog.Title = "Xuất danh sách xuất kho ra PDF";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var dataTable = CreateDataTableForExport(filteredList);
                        var headers = new Dictionary<string, string>
                        {
                            { "STT", "STT" },
                            { "ReferenceNumber", "Mã phiếu" },
                            { "ProductName", "Sản phẩm/Bao bì" },
                            { "BatchNo", "Mã lô" },
                            { "Quantity", "Số lượng" },
                            { "Purpose", "Mục đích" },
                            { "OrderCode", "Đơn hàng" },
                            { "CreatedDate", "Ngày xuất" },
                            { "CreatedBy", "Người xuất" },
                            { "Notes", "Ghi chú" }
                        };

                        var fromDateStr = _fromDate?.ToString("dd/MM/yyyy") ?? "Tất cả";
                        var toDateStr = _toDate?.ToString("dd/MM/yyyy") ?? "Tất cả";
                        var purposeStr = _selectedPurpose ?? "Tất cả";
                        var title = $"DANH SÁCH XUẤT KHO\n" +
                                   $"Từ ngày: {fromDateStr} - Đến ngày: {toDateStr}\n" +
                                   $"Mục đích: {purposeStr}\n" +
                                   $"Tổng số: {filteredList.Count} phiếu xuất";

                        var charts = GenerateChartsForExport(filteredList);
                        var pdfExporter = new PdfExporter();
                        pdfExporter.ExportToPdf(dataTable, saveDialog.FileName, title, headers, charts);

                        UIHelper.ShowSuccessMessage($"Đã xuất PDF thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất PDF");
            }
        }

        private IList<byte[]> GenerateChartsForExport(List<StockOutDetail> list)
        {
            var charts = new List<byte[]>();

            var byPurpose = list
                .GroupBy(x => GetPurposeText(x.Purpose))
                .Select(g => new { Purpose = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var chart1 = new Chart();
            chart1.Width = 800;
            chart1.Height = 400;
            var area1 = new ChartArea("a1");
            area1.AxisX.Interval = 1;
            chart1.ChartAreas.Add(area1);
            var s1 = new Series("purpose");
            s1.ChartType = SeriesChartType.Column;
            s1.IsValueShownAsLabel = true;
            chart1.Series.Add(s1);
            foreach (var item in byPurpose) s1.Points.AddXY(item.Purpose, item.Count);
            chart1.Titles.Add("Số phiếu theo mục đích");
            using (var bmp = new Bitmap(chart1.Width, chart1.Height))
            {
                chart1.DrawToBitmap(bmp, new Rectangle(0, 0, chart1.Width, chart1.Height));
                using (var ms = new MemoryStream()) { bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png); charts.Add(ms.ToArray()); }
            }

            var byDay = list
                .GroupBy(x => x.CreatedDate.Date)
                .Select(g => new { Day = g.Key, Qty = g.Sum(i => i.Quantity) })
                .OrderBy(x => x.Day)
                .ToList();

            var chart2 = new Chart();
            chart2.Width = 800;
            chart2.Height = 400;
            var area2 = new ChartArea("a2");
            area2.AxisX.Interval = 1;
            chart2.ChartAreas.Add(area2);
            var s2 = new Series("qty");
            s2.ChartType = SeriesChartType.Line;
            s2.BorderWidth = 2;
            chart2.Series.Add(s2);
            foreach (var item in byDay) s2.Points.AddXY(item.Day.ToString("dd/MM"), (double)item.Qty);
            chart2.Titles.Add("Số lượng xuất theo ngày");
            using (var bmp = new Bitmap(chart2.Width, chart2.Height))
            {
                chart2.DrawToBitmap(bmp, new Rectangle(0, 0, chart2.Width, chart2.Height));
                using (var ms = new MemoryStream()) { bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png); charts.Add(ms.ToArray()); }
            }

            return charts;
        }

        private DataTable CreateDataTableForExport(List<StockOutDetail> stockOutList)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("STT", typeof(int));
            dataTable.Columns.Add("ReferenceNumber", typeof(string));
            dataTable.Columns.Add("ProductName", typeof(string));
            dataTable.Columns.Add("BatchNo", typeof(string));
            dataTable.Columns.Add("Quantity", typeof(decimal));
            dataTable.Columns.Add("Purpose", typeof(string));
            dataTable.Columns.Add("OrderCode", typeof(string));
            dataTable.Columns.Add("CreatedDate", typeof(string));
            dataTable.Columns.Add("CreatedBy", typeof(string));
            dataTable.Columns.Add("Notes", typeof(string));

            int stt = 1;
            foreach (var item in stockOutList.OrderByDescending(x => x.CreatedDate))
            {
                var referenceNumber = $"XK{item.StockOutId:D6}";
                var itemName = !string.IsNullOrWhiteSpace(item.ProductName) 
                    ? $"[SP] {item.ProductName}" 
                    : !string.IsNullOrWhiteSpace(item.PackagingName) 
                        ? $"[BB] {item.PackagingName}" 
                        : "-";
                var batchNo = item.BatchNo ?? "-";
                var purpose = GetPurposeText(item.Purpose);
                var orderCode = item.OrderId.HasValue ? $"DH{item.OrderId.Value:D6}" : "-";
                var createdDate = item.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                var createdBy = item.CreatedBy ?? "-";
                var notes = item.Notes ?? "-";

                dataTable.Rows.Add(
                    stt++,
                    referenceNumber,
                    itemName,
                    batchNo,
                    item.Quantity,
                    purpose,
                    orderCode,
                    createdDate,
                    createdBy,
                    notes
                );
            }

            return dataTable;
        }

        private void dgvStockOut_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvStockOut.Columns[e.ColumnIndex].Name == "Action")
            {
                var stockOutId = (int)dgvStockOut.Rows[e.RowIndex].Cells["StockOutId"].Value;
                ShowStockOutDetail(stockOutId);
            }
        }

        private async void ShowStockOutDetail(int stockOutId)
        {
            try
            {
                var detail = _stockOutList.FirstOrDefault(x => x.StockOutId == stockOutId);
                if (detail != null)
                {
                    using (var form = new StockOutDetailForm(detail))
                    {
                        var owner = this.FindForm();
                        if (owner != null)
                        {
                            FormHelper.ShowModalWithDim(owner, form);
                        }
                        else
                        {
                            form.ShowDialog();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phiếu xuất kho!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "hiển thị chi tiết phiếu xuất kho");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch == null) return;
            _searchTerm = txtSearch.Text ?? "";
            RefreshDataGridView();
        }

        private void cmbPurposeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPurposeFilter == null || cmbPurposeFilter.SelectedItem == null)
            {
                _selectedPurpose = "Tất cả";
                return;
            }
            _selectedPurpose = cmbPurposeFilter.SelectedItem.ToString() ?? "Tất cả";
            RefreshDataGridView();
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFromDate == null) return;
            _fromDate = dtpFromDate.Value.Date;
            _ = LoadDataAsync(); // Reload data when date changes
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpToDate == null) return;
            _toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1); // End of day
            _ = LoadDataAsync(); // Reload data when date changes
        }

        #endregion
    }
}

