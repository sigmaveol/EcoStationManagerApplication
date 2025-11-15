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

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StockInManagementControl : UserControl
    {
        private List<StockInDetail> _stockInList = new List<StockInDetail>();
        private bool _isLoading = false;

        // Filter variables
        private string _searchTerm = "";
        private string _selectedSource = "Tất cả";
        private DateTime? _fromDate = null;
        private DateTime? _toDate = null;

        public StockInManagementControl()
        {
            InitializeComponent();
        }

        private void StockInManagementControl_Load(object sender, EventArgs e)
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
            dgvStockIn.Columns.Clear();
            dgvStockIn.AutoGenerateColumns = false;
            dgvStockIn.AllowUserToAddRows = false;
            dgvStockIn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStockIn.MultiSelect = false;

            // Thêm các cột
            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StockInId",
                HeaderText = "ID",
                DataPropertyName = "StockInId",
                Visible = false
            });

            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ReferenceNumber",
                HeaderText = "Mã phiếu",
                Width = 120,
                ReadOnly = true
            });

            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Sản phẩm",
                Width = 200,
                ReadOnly = true,
                DataPropertyName = "ProductName"
            });

            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BatchNo",
                HeaderText = "Lô hàng",
                Width = 120,
                ReadOnly = true,
                DataPropertyName = "BatchNo"
            });

            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SupplierName",
                HeaderText = "Nhà cung cấp",
                Width = 150,
                ReadOnly = true,
                DataPropertyName = "SupplierName"
            });

            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Số lượng",
                Width = 100,
                ReadOnly = true,
                DataPropertyName = "Quantity",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "Đơn giá",
                Width = 120,
                ReadOnly = true,
                DataPropertyName = "UnitPrice",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalValue",
                HeaderText = "Tổng tiền",
                Width = 130,
                ReadOnly = true,
                DataPropertyName = "TotalValue",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvStockIn.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedDate",
                HeaderText = "Ngày nhập",
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
            dgvStockIn.Columns.Add(actionColumn);
        }

        private void InitializeStatsCards()
        {
            // Cards đã được tạo trong Designer
            UpdateStatCard("TotalStockIns", "0", "Tổng phiếu nhập");
            UpdateStatCard("TotalQuantity", "0", "Tổng số lượng");
            UpdateStatCard("TotalValue", "0", "Tổng giá trị");
            UpdateStatCard("PendingCheck", "0", "Chờ kiểm tra");
        }

        private void UpdateStatCard(string tag, string value, string description)
        {
            CardControl statCard = null;

            switch (tag)
            {
                case "TotalStockIns":
                    statCard = cardTotalStockIns;
                    break;
                case "TotalQuantity":
                    statCard = cardTotalQuantity;
                    break;
                case "TotalValue":
                    statCard = cardTotalValue;
                    break;
                case "PendingCheck":
                    statCard = cardQualityPass; // Reuse cardQualityPass for pending check
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
            // Nguồn nhập
            cmbSourceFilter.Items.Clear();
            cmbSourceFilter.Items.Add("Tất cả");
            cmbSourceFilter.Items.Add("Nhà cung cấp");
            cmbSourceFilter.Items.Add("Chuyển kho");
            cmbSourceFilter.Items.Add("Trả hàng");
            cmbSourceFilter.SelectedIndex = 0;

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

                // Lấy phiếu nhập kho theo khoảng thời gian đã chọn (sử dụng query có JOIN)
                DateTime startDate = _fromDate ?? DateTime.Now.AddMonths(-1);
                DateTime endDate = _toDate ?? DateTime.Now;
                
                var result = await AppServices.StockInService.GetStockInDetailsByDateRangeAsync(
                    startDate, endDate);

                if (result.Success && result.Data != null)
                {
                    // Dữ liệu đã được JOIN từ database, không cần map thủ công
                    _stockInList = result.Data.ToList();

                    UIHelper.SafeInvoke(this, () =>
                    {
                        RefreshDataGridView();
                        UpdateStatsCards();
                    });
                }
                else
                {
                    _stockInList = new List<StockInDetail>();
                    UIHelper.SafeInvoke(this, () =>
                    {
                        RefreshDataGridView();
                        UpdateStatsCards();
                    });
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu nhập kho");
                _stockInList = new List<StockInDetail>();
            }
            finally
            {
                _isLoading = false;
                SetControlsEnabled(true);
            }
        }

        private void RefreshDataGridView()
        {
            dgvStockIn.Rows.Clear();

            if (_stockInList == null || !_stockInList.Any())
            {
                return;
            }

            // Áp dụng filter
            var filteredList = _stockInList.Where(item =>
            {
                // Filter by search term - tìm theo batch_no, product name, supplier
                if (!string.IsNullOrWhiteSpace(_searchTerm))
                {
                    var searchLower = _searchTerm.ToLower();
                    if (!item.BatchNo?.ToLower().Contains(searchLower) == true &&
                        !item.ProductName?.ToLower().Contains(searchLower) == true &&
                        !item.PackagingName?.ToLower().Contains(searchLower) == true &&
                        !item.SupplierName?.ToLower().Contains(searchLower) == true)
                        return false;
                }

                // Filter by source (based on supplier)
                if (_selectedSource != "Tất cả")
                {
                    if (_selectedSource == "Nhà cung cấp" && string.IsNullOrWhiteSpace(item.SupplierName))
                        return false;
                    if (_selectedSource != "Nhà cung cấp" && !string.IsNullOrWhiteSpace(item.SupplierName))
                        return false;
                }

                // Filter by date range
                if (_fromDate.HasValue && item.CreatedDate < _fromDate.Value)
                    return false;
                if (_toDate.HasValue && item.CreatedDate > _toDate.Value)
                    return false;

                return true;
            }).ToList();

            // Group theo BatchNo - mỗi hàng là một lô hàng (bao gồm cả lô null/trống)
            var batchGroups = filteredList
                .GroupBy(item => item.BatchNo ?? "") // Group cả null/trống
                .Select(group => new
                {
                    BatchNo = group.Key, // Có thể là "" nếu null
                    Items = group.ToList(),
                    FirstItem = group.First(),
                    TotalQuantity = group.Sum(x => x.Quantity),
                    TotalValue = group.Sum(x => x.TotalValue),
                    ProductCount = group.Count(),
                    CreatedDate = group.Max(x => x.CreatedDate),
                    SupplierName = group.First().SupplierName
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            foreach (var batchGroup in batchGroups)
            {
                var batchNo = string.IsNullOrWhiteSpace(batchGroup.BatchNo) ? "(Không có lô)" : batchGroup.BatchNo;
                var supplierName = batchGroup.SupplierName ?? "-";
                var totalQuantity = batchGroup.TotalQuantity.ToString("N2");
                var totalValue = batchGroup.TotalValue.ToString("N0");
                var createdDate = batchGroup.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                var productCount = batchGroup.ProductCount;
                
                // Đếm số sản phẩm và bao bì
                var productCountInBatch = batchGroup.Items.Count(x => !string.IsNullOrWhiteSpace(x.ProductName));
                var packagingCountInBatch = batchGroup.Items.Count(x => !string.IsNullOrWhiteSpace(x.PackagingName));
                var itemTypeText = "";
                if (productCountInBatch > 0 && packagingCountInBatch > 0)
                    itemTypeText = $"{productCountInBatch} SP, {packagingCountInBatch} BB";
                else if (productCountInBatch > 0)
                    itemTypeText = $"{productCountInBatch} sản phẩm";
                else if (packagingCountInBatch > 0)
                    itemTypeText = $"{packagingCountInBatch} bao bì";
                else
                    itemTypeText = $"{productCount} mục";
                
                var referenceNumber = string.IsNullOrWhiteSpace(batchGroup.BatchNo) 
                    ? $"Đơn nhập ({productCount} mục)" 
                    : $"Lô: {batchGroup.BatchNo} ({productCount} mục)";

                var rowIndex = dgvStockIn.Rows.Add(
                    batchGroup.BatchNo ?? "", // Store batchNo (có thể là "") trong StockInId column
                    referenceNumber,
                    itemTypeText, // Hiển thị số sản phẩm và bao bì
                    batchNo,
                    supplierName,
                    totalQuantity,
                    "-", // UnitPrice - không hiển thị cho lô
                    totalValue,
                    createdDate
                );

                // Store batchNo (có thể là "" cho null) trong row tag
                dgvStockIn.Rows[rowIndex].Tag = batchGroup.BatchNo ?? "";
            }
        }

        private void UpdateStatsCards()
        {
            if (_stockInList == null || !_stockInList.Any())
            {
                UpdateStatCard("TotalStockIns", "0", "Tổng phiếu nhập");
                UpdateStatCard("TotalQuantity", "0", "Tổng số lượng");
                UpdateStatCard("TotalValue", "0", "Tổng giá trị");
                UpdateStatCard("PendingCheck", "0", "Phiếu mới (7 ngày)");
                return;
            }

            // Tổng phiếu nhập
            int totalStockIns = _stockInList.Count;
            UpdateStatCard("TotalStockIns", totalStockIns.ToString("N0"), "Tổng phiếu nhập");

            // Tổng số lượng
            decimal totalQuantity = _stockInList.Sum(x => x.Quantity);
            UpdateStatCard("TotalQuantity", totalQuantity.ToString("N2"), "Tổng số lượng");

            // Tổng giá trị
            decimal totalValue = _stockInList.Sum(x => x.TotalValue);
            UpdateStatCard("TotalValue", totalValue.ToString("N0"), "Tổng giá trị (VNĐ)");

            // Chờ kiểm tra - đếm số phiếu có supplier (có thể cần kiểm tra thêm)
            // Hoặc có thể đếm số phiếu mới trong ngày/tuần
            int pendingCheck = _stockInList.Count(x => 
                x.CreatedDate.Date >= DateTime.Now.AddDays(-7).Date && 
                x.CreatedDate.Date <= DateTime.Now.Date);
            UpdateStatCard("PendingCheck", pendingCheck.ToString("N0"), "Phiếu mới (7 ngày)");
        }

        private void SetControlsEnabled(bool enabled)
        {
            try
            {
                UIHelper.SafeInvoke(this, () =>
                {
                    btnExportExcel.Enabled = enabled;
                    btnCreateStockIn.Enabled = enabled;
                    btnRefresh.Enabled = enabled;
                    txtSearch.Enabled = enabled;
                    cmbSourceFilter.Enabled = enabled;
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

        private void btnCreateStockIn_Click(object sender, EventArgs e)
        {
            try
            {
                // Sử dụng form nhập kho nhiều sản phẩm
                using (var form = new MultiProductStockInForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        _ = LoadDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form nhập kho");
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_stockInList == null || !_stockInList.Any())
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // TODO: Implement Excel export
                MessageBox.Show("Chức năng xuất Excel sẽ được triển khai sau.", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất Excel");
            }
        }

        private void dgvStockIn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvStockIn.Columns[e.ColumnIndex].Name == "Action")
            {
                var row = dgvStockIn.Rows[e.RowIndex];
                ShowStockInDetail(row);
            }
        }

        private async void ShowStockInDetail(DataGridViewRow row)
        {
            try
            {
                if (row == null)
                {
                    MessageBox.Show("Vui lòng chọn một dòng để xem chi tiết!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy batchNo từ row tag (có thể là "" cho null)
                var batchNo = row.Tag?.ToString() ?? "";
                
                // Nếu batchNo rỗng, lấy tất cả items không có lô từ cùng ngày
                if (string.IsNullOrWhiteSpace(batchNo))
                {
                    // Lấy từ _stockInList các items không có batchNo
                    var noBatchItems = _stockInList
                        .Where(x => string.IsNullOrWhiteSpace(x.BatchNo))
                        .OrderByDescending(x => x.CreatedDate)
                        .ToList();
                    
                    if (noBatchItems.Any())
                    {
                        using (var form = new StockInDetailForm("", noBatchItems))
                        {
                            form.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm trong đơn nhập này!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Load tất cả sản phẩm và bao bì trong lô
                    var result = await AppServices.StockInService.GetStockInDetailsByBatchAsync(batchNo);
                    if (result.Success && result.Data != null && result.Data.Any())
                    {
                        using (var form = new StockInDetailForm(batchNo, result.Data))
                        {
                            form.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm trong lô hàng này!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "hiển thị chi tiết phiếu nhập kho");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch == null) return;
            _searchTerm = txtSearch.Text ?? "";
            RefreshDataGridView();
        }

        private void cmbSourceFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSourceFilter == null || cmbSourceFilter.SelectedItem == null)
            {
                _selectedSource = "Tất cả";
                return;
            }
            _selectedSource = cmbSourceFilter.SelectedItem.ToString() ?? "Tất cả";
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

