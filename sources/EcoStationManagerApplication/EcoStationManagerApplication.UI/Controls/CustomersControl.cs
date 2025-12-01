using EcoStationManagerApplication.Common.Exporters;
using EcoStationManagerApplication.Common.Utilities;
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

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class CustomersControl : UserControl, IRefreshableControl
    {
        private List<Customer> customers;
        private List<PackagingTransaction> packagingTransactions;
        private string searchTerm = "";
        private bool isLoading = false;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private int? selectedCustomerId = null;
        private CustomerRank? selectedRank = null;

        public CustomersControl()
        {
            InitializeComponent();
            
        }

        public void RefreshData()
        {
            _ = LoadDataAsync();
        }

        private void CustomersControl_Load(object sender, EventArgs e)
        {
            InitializeControls();
            _ = LoadDataAsync();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// Tải dữ liệu từ database
        /// </summary>
        private async Task LoadDataAsync()
        {
            if (isLoading) return;

            try
            {
                isLoading = true;
                SetControlsEnabled(false);

                // Load Customers từ database - dùng SearchCustomersAsync với empty string để lấy tất cả
                var customersResult = await AppServices.CustomerService.SearchCustomersAsync("");
                
                if (customersResult.Success && customersResult.Data != null)
                {
                    customers = customersResult.Data.ToList();
                }
                else
                {
                    customers = new List<Customer>();
                    if (!string.IsNullOrEmpty(customersResult.Message))
                    {
                        UIHelper.ShowWarningMessage(customersResult.Message);
                    }
                }

                UIHelper.SafeInvoke(this, () =>
                {
                    BindCustomersData();
                    ClearPackagingTransactions();
                });
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu khách hàng");
                customers = new List<Customer>();
                UIHelper.SafeInvoke(this, () => BindCustomersData());
            }
            finally
            {
                isLoading = false;
                SetControlsEnabled(true);
            }
        }

        /// <summary>
        /// Khởi tạo các controls
        /// </summary>
        private void InitializeControls()
        {
            InitializeDataGridView();
            InitializePackagingTransactionsGridView();
            InitializeFilters();
            if (dgvPackagingTransactions != null)
            {
                dgvPackagingTransactions.CellContentClick += dgvPackagingTransactions_CellContentClick;
                dgvPackagingTransactions.CellDoubleClick += dgvPackagingTransactions_CellContentClick;
            }
        }

        /// <summary>
        /// Khởi tạo DataGridView với các cột cần thiết
        /// </summary>
        private void InitializeDataGridView()
        {
            try
            {
                dataGridViewCustomers.Columns.Clear();

                // Cột ẩn chứa CustomerId
                var colCustomerId = new DataGridViewTextBoxColumn 
                { 
                    Name = "CustomerId", 
                    HeaderText = "ID", 
                    Visible = false 
                };
                dataGridViewCustomers.Columns.Add(colCustomerId);

                // Các cột hiển thị
                dataGridViewCustomers.Columns.Add("CustomerCode", "Mã KH");
                dataGridViewCustomers.Columns.Add("CustomerName", "Tên khách hàng");
                dataGridViewCustomers.Columns.Add("CustomerPhone", "Số điện thoại");
                dataGridViewCustomers.Columns.Add("TotalPoint", "Điểm tích lũy");
                dataGridViewCustomers.Columns.Add("CustomerRank", "Hạng");
                dataGridViewCustomers.Columns.Add("CustomerStatus", "Trạng thái");
                dataGridViewCustomers.Columns.Add("CustomerAction", "Thao tác");

                // Thiết lập độ rộng cột
                if (dataGridViewCustomers.Columns["CustomerCode"] != null)
                    dataGridViewCustomers.Columns["CustomerCode"].Width = 120;
                if (dataGridViewCustomers.Columns["CustomerName"] != null)
                    dataGridViewCustomers.Columns["CustomerName"].Width = 250;
                if (dataGridViewCustomers.Columns["CustomerPhone"] != null)
                    dataGridViewCustomers.Columns["CustomerPhone"].Width = 150;
                if (dataGridViewCustomers.Columns["TotalPoint"] != null)
                    dataGridViewCustomers.Columns["TotalPoint"].Width = 120;
                if (dataGridViewCustomers.Columns["CustomerRank"] != null)
                    dataGridViewCustomers.Columns["CustomerRank"].Width = 120;
                if (dataGridViewCustomers.Columns["CustomerStatus"] != null)
                    dataGridViewCustomers.Columns["CustomerStatus"].Width = 120;
                if (dataGridViewCustomers.Columns["CustomerAction"] != null)
                {
                    dataGridViewCustomers.Columns["CustomerAction"].Width = 280;
                    dataGridViewCustomers.Columns["CustomerAction"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Căn giữa cho cột điểm tích lũy
                if (dataGridViewCustomers.Columns["TotalPoint"] != null)
                    dataGridViewCustomers.Columns["TotalPoint"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "khởi tạo DataGridView");
            }
        }

        /// <summary>
        /// Khởi tạo DataGridView cho packaging transactions
        /// </summary>
        private void InitializePackagingTransactionsGridView()
        {
            try
            {
                dgvPackagingTransactions.Columns.Clear();
                dgvPackagingTransactions.AutoGenerateColumns = false;
                dgvPackagingTransactions.AllowUserToAddRows = false;
                dgvPackagingTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Cột ẩn 
                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TransactionId",
                    HeaderText = "ID",
                    Visible = false
                });

                // Thêm các cột
                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TransactionDate",
                    HeaderText = "Ngày giao dịch",
                    Width = 150,
                    DataPropertyName = "CreatedDate"
                });

                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TransactionType",
                    HeaderText = "Loại giao dịch",
                    Width = 120,
                    DataPropertyName = "Type"
                });

                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "PackagingName",
                    HeaderText = "Tên bao bì",
                    Width = 180,
                    DataPropertyName = "PackagingName"
                });

                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Quantity",
                    HeaderText = "Số lượng",
                    Width = 80,
                    DataPropertyName = "Quantity",
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N0",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                });

                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DepositPrice",
                    HeaderText = "Tiền cọc",
                    Width = 100,
                    DataPropertyName = "DepositPrice",
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N0",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                });

                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "RefundAmount",
                    HeaderText = "Tiền hoàn",
                    Width = 100,
                    DataPropertyName = "RefundAmount",
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N0",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                });

                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "OwnershipType",
                    HeaderText = "Hình thức",
                    Width = 100,
                    DataPropertyName = "OwnershipType"
                });

                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Notes",
                    HeaderText = "Ghi chú",
                    Width = 150,
                    DataPropertyName = "Notes"
                });

                dgvPackagingTransactions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CreatedBy",
                    HeaderText = "Người thực hiện",
                    Width = 120,
                    DataPropertyName = "CreatedBy"
                });

                // Thêm cột thao tác
                var actionColumn = new DataGridViewButtonColumn
                {
                    Name = "Action",
                    HeaderText = "Thao tác",
                    Text = "Chi tiết",
                    UseColumnTextForButtonValue = true,
                    Width = 80
                };
                dgvPackagingTransactions.Columns.Add(actionColumn);
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "khởi tạo DataGridView giao dịch bao bì");
            }
        }

        /// <summary>
        /// Khởi tạo bộ lọc
        /// </summary>
        private void InitializeFilters()
        {
            // Khởi tạo datetime pickers
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpToDate.Value = DateTime.Now;
            fromDate = dtpFromDate.Value.Date;
            toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);

            cmbCustomerRank.Items.Clear();
            cmbCustomerRank.Items.Add("Tất cả hạng");

            var rankDictionary = EnumHelper.GetEnumDictionary<CustomerRank>();

            foreach (var rank in rankDictionary)
            {
                cmbCustomerRank.Items.Add(new ComboItem<CustomerRank>
                {
                    Text = rank.Value,  // Tên hiển thị
                    Value = rank.Key    // Giá trị enum
                });
            }

            cmbCustomerRank.DisplayMember = "Text";
            cmbCustomerRank.ValueMember = "Value";
            cmbCustomerRank.SelectedIndex = 0;
        }

        /// <summary>
        /// Bind dữ liệu khách hàng vào DataGridView
        /// </summary>
        private void BindCustomersData()
        {
            try
            {
                if (customers == null)
                {
                    dataGridViewCustomers.Rows.Clear();
                    return;
                }

                var filteredCustomers = customers.Where(customer =>
                {
                    // Filter theo search term
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        var searchLower = searchTerm.ToLower();
                        if (!(customer.Name != null && customer.Name.ToLower().Contains(searchLower)) &&
                            !(customer.Phone != null && customer.Phone.ToLower().Contains(searchLower)) &&
                            !(customer.CustomerCode != null && customer.CustomerCode.ToLower().Contains(searchLower)))
                            return false;
                    }

                    // Filter theo hạng thành viên
                    if (selectedRank.HasValue && customer.Rank != selectedRank.Value)
                        return false;

                    return true;
                }).OrderBy(c => c.Name).ToList();

                dataGridViewCustomers.Rows.Clear();

                foreach (var customer in filteredCustomers)
                {
                    var rowIndex = dataGridViewCustomers.Rows.Add(
                        customer.CustomerId,
                        customer.CustomerCode ?? "",
                        customer.Name ?? "",
                        customer.Phone ?? "",
                        customer.TotalPoint.ToString("N0"),
                        customer.Rank.GetDisplayName(),
                        customer.IsActive == ActiveStatus.ACTIVE ? "Hoạt động" : "Ngưng",
                        "👁️ Xem | 📦 Phát | 🔄 Thu"
                    );

                    // Đổi màu dòng nếu khách hàng không hoạt động
                    if (customer.IsActive != ActiveStatus.ACTIVE)
                    {
                        dataGridViewCustomers.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }

                // Cập nhật label mô tả với số lượng
                UpdateDescriptionLabel(filteredCustomers.Count);
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "hiển thị dữ liệu khách hàng");
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi hạng thành viên
        /// </summary>
        private void cmbCustomerRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCustomerRank.SelectedIndex == 0)
                {
                    selectedRank = null; // "Tất cả hạng"
                }
                else if (cmbCustomerRank.SelectedItem is ComboItem<CustomerRank> rankItem)
                {
                    selectedRank = rankItem.Value;
                }
                else
                {
                    selectedRank = null;
                }

                BindCustomersData();
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "lọc theo hạng thành viên");
            }
        }


        /// <summary>
        /// Tải dữ liệu packaging transactions cho khách hàng được chọn
        /// </summary>
        private async Task LoadPackagingTransactionsAsync(int customerId)
        {
            if (isLoading) return;

            try
            {
                selectedCustomerId = customerId;

                var result = await AppServices.PackagingTransactionService.GetTransactionsByCustomerAsync(customerId, fromDate, toDate);

                if (result.Success && result.Data != null)
                {
                    packagingTransactions = result.Data.ToList();
                    BindPackagingTransactionsData();
                }
                else
                {
                    packagingTransactions = new List<PackagingTransaction>();
                    BindPackagingTransactionsData();
                }

                // Hiển thị thông tin khách hàng được chọn
                var customer = customers.FirstOrDefault(c => c.CustomerId == customerId);
                if (customer != null)
                {
                    lblSelectedCustomer.Text = $"Giao dịch bao bì: {customer.Name} ({customer.CustomerCode})";
                    lblTransactionCount.Text = $"Tổng số giao dịch: {packagingTransactions?.Count ?? 0}";
                    // Tính tổng số lượng bao bì đang phát
                    var totalIssued = packagingTransactions?
                        .Where(t => t.Type == PackagingTransactionType.ISSUE)
                        .Sum(t => t.Quantity) ?? 0;
                    var totalReturned = packagingTransactions?
                        .Where(t => t.Type == PackagingTransactionType.RETURN)
                        .Sum(t => t.Quantity) ?? 0;
                    var currentHolding = totalIssued - totalReturned;

                    lblTransactionCount.Text += $", Đang giữ: {currentHolding} bao bì";
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu giao dịch bao bì");
                packagingTransactions = new List<PackagingTransaction>();
                BindPackagingTransactionsData();
            }
        }

        /// <summary>
        /// Bind dữ liệu packaging transactions vào DataGridView
        /// </summary>
        private async Task BindPackagingTransactionsData()
        {
            try
            {
                dgvPackagingTransactions.Rows.Clear();

                if (packagingTransactions == null || !packagingTransactions.Any())
                {
                    return;
                }

                foreach (var transaction in packagingTransactions.OrderByDescending(t => t.CreatedDate))
                {

                    var rowIndex = dgvPackagingTransactions.Rows.Add(
                    transaction.TransactionId, // Ẩn, để sử dụng cho các thao tác khác
                    transaction.CreatedDate.ToString("dd/MM/yyyy HH:mm"),
                    transaction.Type.GetDisplayName(),
                    transaction.Packaging.Name,
                    transaction.Quantity,
                    FormatHelper.FormatCurrency(transaction.DepositPrice),
                    FormatHelper.FormatCurrency(transaction.RefundAmount),
                    transaction.OwnershipType.GetDisplayName(),
                    transaction.Notes ?? "-",
                    transaction.User.Fullname ?? "-"
                    );

                    // Đổi màu dòng theo loại giao dịch
                    if (transaction.Type == PackagingTransactionType.ISSUE)
                    {
                        dgvPackagingTransactions.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                        dgvPackagingTransactions.Rows[rowIndex].DefaultCellStyle.Font = new Font(dgvPackagingTransactions.Font, FontStyle.Bold);
                    }
                    else if (transaction.Type == PackagingTransactionType.RETURN)
                    {
                        dgvPackagingTransactions.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "hiển thị dữ liệu giao dịch bao bì");
            }
        }

        private void dgvPackagingTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvPackagingTransactions.Columns[e.ColumnIndex].Name != "Action") return;
            var idObj = dgvPackagingTransactions.Rows[e.RowIndex].Cells["TransactionId"].Value;
            if (idObj == null) return;
            if (!int.TryParse(idObj.ToString(), out int transactionId)) return;
            var transaction = packagingTransactions?.FirstOrDefault(t => t.TransactionId == transactionId);
            if (transaction == null) return;

            var detailForm = new PackagingTransactionDetailForm(transaction);
            Form mainForm = this.FindForm();
            while (mainForm != null && !(mainForm is MainForm))
            {
                mainForm = mainForm.ParentForm ?? mainForm.Owner;
            }
            if (mainForm != null)
                FormHelper.ShowModalWithDim(mainForm, detailForm);
            else
                detailForm.ShowDialog();
        }

        private void ClearPackagingTransactions()
        {
            packagingTransactions = new List<PackagingTransaction>();
            dgvPackagingTransactions.Rows.Clear();
            lblSelectedCustomer.Text = "Giao dịch bao bì";
            lblTransactionCount.Text = "Chọn khách hàng để xem giao dịch";
            selectedCustomerId = null;
        }

        /// <summary>
        /// Cập nhật label mô tả với số lượng khách hàng
        /// </summary>
        private void UpdateDescriptionLabel(int count)
        {
            try
            {
                if (lblDescription != null)
                {
                    if (string.IsNullOrWhiteSpace(searchTerm))
                    {
                        lblDescription.Text = $"Quản lý thông tin khách hàng, điểm tích lũy, hạng thành viên. Tổng số: {count} khách hàng";
                    }
                    else
                    {
                        lblDescription.Text = $"Tìm thấy {count} khách hàng phù hợp với từ khóa \"{searchTerm}\"";
                    }
                }
            }
            catch
            {
                // Ignore errors in UI update
            }
        }

        /// <summary>
        /// Bật/tắt các controls
        /// </summary>
        private void SetControlsEnabled(bool enabled)
        {
            try
            {
                UIHelper.SafeInvoke(this, () =>
                {
                    btnAddCustomer.Enabled = enabled;
                    txtSearch.Enabled = enabled;
                    btnRefresh.Enabled = enabled;
                    btnExportExcel.Enabled = enabled;
                    btnExportPDF.Enabled = enabled;
                    dtpFromDate.Enabled = enabled;
                    dtpToDate.Enabled = enabled;
                    cmbCustomerRank.Enabled = enabled;
                    dataGridViewCustomers.Enabled = enabled;
                    dgvPackagingTransactions.Enabled = enabled;
                });
            }
            catch
            {
                // Ignore errors
            }
        }

        #region Event Handlers

        /// <summary>
        /// Xử lý sự kiện thay đổi text trong ô tìm kiếm
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                searchTerm = txtSearch?.Text ?? "";
                BindCustomersData();
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tìm kiếm khách hàng");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút thêm khách hàng
        /// </summary>
        private async void btnAddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                Form mainForm = this.FindForm();
                while (mainForm != null && !(mainForm is MainForm))
                {
                    mainForm = mainForm.ParentForm ?? mainForm.Owner;
                }

                using (var addCustomerForm = new AddCustomerForm())
                {
                    DialogResult result = mainForm != null
                        ? FormHelper.ShowModalWithDim(mainForm, addCustomerForm)
                        : addCustomerForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        await RefreshCustomersData();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form thêm khách hàng");
            }
        }

        /// <summary>
        /// Làm mới dữ liệu khách hàng
        /// </summary>
        private async Task RefreshCustomersData()
        {
            if (isLoading) return;

            try
            {
                isLoading = true;
                SetControlsEnabled(false);

                var customersResult = await AppServices.CustomerService.SearchCustomersAsync("");
                
                if (customersResult.Success && customersResult.Data != null)
                {
                    customers = customersResult.Data.ToList();
                }
                else
                {
                    customers = new List<Customer>();
                    if (!string.IsNullOrEmpty(customersResult.Message))
                    {
                        UIHelper.ShowWarningMessage(customersResult.Message);
                    }
                }

                UIHelper.SafeInvoke(this, () => BindCustomersData());
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "làm mới dữ liệu khách hàng");
            }
            finally
            {
                isLoading = false;
                SetControlsEnabled(true);
            }
        }


        /// <summary>
        /// Xử lý sự kiện click nút xuất Excel
        /// </summary>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (packagingTransactions == null || !packagingTransactions.Any())
                {
                    UIHelper.ShowWarningMessage("Không có dữ liệu giao dịch để xuất!");
                    return;
                }

                // Hiển thị SaveFileDialog
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                    saveDialog.FileName = $"GiaoDichBaoBi_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    saveDialog.Title = "Xuất giao dịch bao bì ra Excel";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Tạo DataTable từ dữ liệu
                        var dataTable = CreateDataTableForExport();

                        // Tạo headers cho Excel
                        var headers = new Dictionary<string, string>
                        {
                            { "STT", "STT" },
                            { "TransactionDate", "Ngày giao dịch" },
                            { "TransactionType", "Loại giao dịch" },
                            { "PackagingName", "Tên bao bì" },
                            { "Quantity", "Số lượng" },
                            { "Notes", "Ghi chú" },
                            { "CreatedBy", "Người thực hiện" }
                        };

                        // Lấy thông tin khách hàng
                        var customer = selectedCustomerId.HasValue
                            ? customers.FirstOrDefault(c => c.CustomerId == selectedCustomerId.Value)
                            : null;

                        // Tạo title với thông tin filter
                        var fromDateStr = fromDate?.ToString("dd/MM/yyyy") ?? "Tất cả";
                        var toDateStr = toDate?.ToString("dd/MM/yyyy") ?? "Tất cả";
                        var customerName = lblSelectedCustomer.Text.Replace("Giao dịch bao bì: ", "");
                        var title = $"Khách hàng: {customerName}\n" +
                                    $"Từ ngày: {fromDateStr} - Đến ngày: {toDateStr}\n";

                        // Xuất Excel
                        var excelExporter = new ExcelExporter();
                        excelExporter.ExportToExcel(dataTable, saveDialog.FileName, "Giao dịch bao bì", headers, title);

                        UIHelper.ShowSuccessMessage($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất Excel");
            }
        }


        /// <summary>
        /// Xử lý sự kiện click nút xuất PDF
        /// </summary>
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (packagingTransactions == null || !packagingTransactions.Any())
                {
                    UIHelper.ShowWarningMessage("Không có dữ liệu giao dịch để xuất!");
                    return;
                }

                UIHelper.ShowWarningMessage("Tính năng xuất PDF đang được phát triển.");
                // TODO: Implement PDF export functionality
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xuất PDF");
            }
        }

        /// <summary>
        /// Tạo DataTable cho việc xuất Excel
        /// </summary>
        private DataTable CreateDataTableForExport()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("STT", typeof(int));
            dataTable.Columns.Add("TransactionDate", typeof(string));
            dataTable.Columns.Add("TransactionType", typeof(string));
            dataTable.Columns.Add("PackagingName", typeof(string));
            dataTable.Columns.Add("Quantity", typeof(int));
            dataTable.Columns.Add("Notes", typeof(string));
            dataTable.Columns.Add("CreatedBy", typeof(string));

            int stt = 1;
            foreach (var transaction in packagingTransactions.OrderByDescending(t => t.CreatedDate))
            {
                dataTable.Rows.Add(
                    stt++,
                    transaction.CreatedDate.ToString("dd/MM/yyyy HH:mm"),
                    transaction.Type.GetDisplayName(),
                    transaction.Packaging?.Name ?? "-",
                    transaction.Quantity,
                    transaction.Notes ?? "-",
                    transaction.User?.Fullname ?? "-"
                );
            }

            return dataTable;
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi ngày bắt đầu
        /// </summary>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            fromDate = dtpFromDate.Value.Date;
            if (selectedCustomerId.HasValue)
            {
                _ = LoadPackagingTransactionsAsync(selectedCustomerId.Value);
            }
        }


        /// <summary>
        /// Xử lý sự kiện thay đổi ngày kết thúc
        /// </summary>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);
            if (selectedCustomerId.HasValue)
            {
                _ = LoadPackagingTransactionsAsync(selectedCustomerId.Value);
            }
        }

        /// <summary>
        /// Xử lý sự kiện click vào cell trong DataGridView
        /// </summary>
        private async void dataGridViewCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                var customerIdCell = dataGridViewCustomers.Rows[e.RowIndex].Cells["CustomerId"];
                if (customerIdCell?.Value == null || !int.TryParse(customerIdCell.Value.ToString(), out int customerId))
                {
                    UIHelper.ShowWarningMessage("Không thể xác định khách hàng được chọn.");
                    return;
                }

                // Chỉ xử lý khi click vào cột Action
                if (e.ColumnIndex == dataGridViewCustomers.Columns["CustomerAction"]?.Index)
                {
                    var actionCell = dataGridViewCustomers.Rows[e.RowIndex].Cells["CustomerAction"];
                    var actionText = actionCell?.Value?.ToString() ?? "";

                    Form mainForm = this.FindForm();
                    while (mainForm != null && !(mainForm is MainForm))
                    {
                        mainForm = mainForm.ParentForm ?? mainForm.Owner;
                    }

                    // Xác định hành động dựa trên vị trí click
                    var cellBounds = dataGridViewCustomers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    var clickX = dataGridViewCustomers.PointToClient(Control.MousePosition).X - cellBounds.X;

                    if (clickX < cellBounds.Width / 3)
                    {
                        // Click vào "Xem"
                        using (var editCustomerForm = new AddCustomerForm(customerId))
                        {
                            DialogResult result = mainForm != null
                                ? FormHelper.ShowModalWithDim(mainForm, editCustomerForm)
                                : editCustomerForm.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                await RefreshCustomersData();
                            }
                        }
                    }
                    else if (clickX < (cellBounds.Width * 2) / 3)
                    {
                        // Click vào "Phát bao bì"
                        await OpenIssuePackagingForm(customerId, mainForm);
                    }
                    else
                    {
                        // Click vào "Thu hồi bao bì"
                        await OpenReturnPackagingForm(customerId, mainForm);
                    }
                } else
                {
                    await LoadPackagingTransactionsAsync(customerId);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xử lý thao tác khách hàng");
            }
        }

        /// <summary>
        /// Mở form phát bao bì cho khách hàng
        /// </summary>
        private async Task OpenIssuePackagingForm(int customerId, Form parentForm)
        {
            try
            {
                using (var form = new PackagingTransactionForm(customerId, PackagingTransactionType.ISSUE))
                {
                    DialogResult result = parentForm != null
                        ? FormHelper.ShowModalWithDim(parentForm, form)
                        : form.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        await RefreshCustomersData();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form phát bao bì");
            }
        }

        /// <summary>
        /// Mở form thu hồi bao bì từ khách hàng
        /// </summary>
        private async Task OpenReturnPackagingForm(int customerId, Form parentForm)
        {
            try
            {
                using (var form = new PackagingTransactionForm(customerId, PackagingTransactionType.RETURN))
                {
                    DialogResult result = parentForm != null
                        ? FormHelper.ShowModalWithDim(parentForm, form)
                        : form.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        await RefreshCustomersData();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "mở form thu hồi bao bì");
            }
        }

        /// <summary>
        /// Xử lý sự kiện hover vào nút thêm khách hàng
        /// </summary>
        private void btnAddCustomer_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (btnAddCustomer.Enabled)
                {
                    btnAddCustomer.FillColor = Color.FromArgb(33, 140, 73);
                }
            }
            catch
            {
                // Ignore errors
            }
        }

        /// <summary>
        /// Xử lý sự kiện hover ra khỏi nút thêm khách hàng
        /// </summary>
        private void btnAddCustomer_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (btnAddCustomer.Enabled)
                {
                    btnAddCustomer.FillColor = Color.FromArgb(31, 107, 59);
                }
            }
            catch
            {
                // Ignore errors
            }
        }


        #endregion
    }
}
