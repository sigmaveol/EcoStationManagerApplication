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
        private string searchTerm = "";
        private bool isLoading = false;

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

        /// <summary>
        /// Tải dữ liệu khách hàng từ database
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

                UIHelper.SafeInvoke(this, () => BindCustomersData());
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
                    dataGridViewCustomers.Columns["CustomerAction"].Width = 100;
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

                // Lọc dữ liệu theo searchTerm
                var filteredCustomers = customers.Where(customer =>
                {
                    if (string.IsNullOrWhiteSpace(searchTerm))
                        return true;

                    var searchLower = searchTerm.ToLower();
                    return (customer.Name != null && customer.Name.ToLower().Contains(searchLower)) ||
                           (customer.Phone != null && customer.Phone.ToLower().Contains(searchLower)) ||
                           (customer.CustomerCode != null && customer.CustomerCode.ToLower().Contains(searchLower));
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
                        GetRankDisplayName(customer.Rank),
                        customer.IsActive == ActiveStatus.ACTIVE ? "Hoạt động" : "Ngưng",
                        "👁️ Xem"
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
        /// Lấy tên hiển thị của hạng khách hàng
        /// </summary>
        private string GetRankDisplayName(CustomerRank rank)
        {
            switch (rank)
            {
                case CustomerRank.MEMBER:
                    return "Thành viên";
                case CustomerRank.SILVER:
                    return "Bạc";
                case CustomerRank.GOLD:
                    return "Vàng";
                case CustomerRank.DIAMONDS:
                    return "Kim cương";
                default:
                    return rank.ToString();
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
                    dataGridViewCustomers.Enabled = enabled;
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
        /// Xử lý sự kiện click vào cell trong DataGridView
        /// </summary>
        private async void dataGridViewCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                // Chỉ xử lý khi click vào cột Action
                if (e.ColumnIndex == dataGridViewCustomers.Columns["CustomerAction"]?.Index)
                {
                    var customerIdCell = dataGridViewCustomers.Rows[e.RowIndex].Cells["CustomerId"];
                    if (customerIdCell?.Value == null || !int.TryParse(customerIdCell.Value.ToString(), out int customerId))
                    {
                        UIHelper.ShowWarningMessage("Không thể xác định khách hàng được chọn.");
                        return;
                    }

                    Form mainForm = this.FindForm();
                    while (mainForm != null && !(mainForm is MainForm))
                    {
                        mainForm = mainForm.ParentForm ?? mainForm.Owner;
                    }

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
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xem thông tin khách hàng");
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
