using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SuppliersControl : UserControl, IRefreshableControl
    {
        private List<Supplier> _suppliers = new List<Supplier>();
        private string _currentSearchTerm = "";
        private bool _isLoading = false;

        public SuppliersControl()
        {
            InitializeComponent();
        }

        private void SuppliersControl_Load(object sender, EventArgs e)
        {
            SetupDataGridStyle(dgvSuppliers);
            InitializeDataGridColumns();
            InitializeEvents();
            _ = LoadSuppliersAsync();
        }

        public void RefreshData()
        {
            // Đảm bảo columns đã được khởi tạo
            if (dgvSuppliers != null && (dgvSuppliers.Columns.Count == 0 || dgvSuppliers.Columns["SupplierCode"] == null))
            {
                SetupDataGridStyle(dgvSuppliers);
                InitializeDataGridColumns();
                InitializeEvents();
            }
            _ = LoadSuppliersAsync();
        }


        private void InitializeEvents()
        {
            if (btnAddSupplier != null)
                btnAddSupplier.Click += BtnAddSupplier_Click;

            if (txtSearch != null)
                txtSearch.TextChanged += txtSearch_TextChanged;

            if (dgvSuppliers != null)
            {
                dgvSuppliers.CellContentClick += DgvSuppliers_CellContentClick;
                dgvSuppliers.CellFormatting += DgvSuppliers_CellFormatting;
            }
        }

        private void InitializeDataGridColumns()
        {
            if (dgvSuppliers == null) return;

            // Luôn clear để đảm bảo không có columns cũ
            dgvSuppliers.Columns.Clear();

            var columns = new[]
            {
                new { Name = "SupplierCode", Header = "Mã NCC", Width = 100 },
                new { Name = "Name", Header = "Tên nhà cung cấp", Width = 200 },
                new { Name = "ContactPerson", Header = "Người liên hệ", Width = 150 },
                new { Name = "Contact", Header = "Liên lạc", Width = 200 },
                new { Name = "Address", Header = "Địa chỉ", Width = 250 },
                new { Name = "Status", Header = "Trạng thái", Width = 120 }
            };

            foreach (var col in columns)
            {
                dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    ReadOnly = true,
                    Width = col.Width
                });
            }

            // Cột Thao tác với icon chỉnh sửa
            dgvSuppliers.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colEdit",
                HeaderText = "Thao\ntác",
                Text = "✏️",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Width = 100
            });

            // Tự co giãn cột Địa chỉ
            dgvSuppliers.Columns["Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSuppliers.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private async Task LoadSuppliersAsync()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                ShowLoading(true);

                if (dgvSuppliers == null || this.Parent == null)
                {
                    return;
                }

                UIHelper.SafeInvoke(this, () =>
                {
                    if (dgvSuppliers != null)
                    {
                        dgvSuppliers.Rows.Clear();
                    }
                });

                var suppliersResult = await AppServices.SupplierService.GetAllSuppliersAsync();

                if (!suppliersResult.Success || suppliersResult.Data == null || !suppliersResult.Data.Any())
                {
                    UIHelper.SafeInvoke(this, () =>
                    {
                        if (dgvSuppliers != null)
                        {
                            dgvSuppliers.Rows.Add("", "Không có nhà cung cấp nào", "", "", "", "", "");
                        }
                    });
                    return;
                }

                _suppliers = suppliersResult.Data.ToList();

                // Áp dụng tìm kiếm nếu có
                var filteredSuppliers = _suppliers;
                if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
                {
                    var searchLower = _currentSearchTerm.ToLower();
                    filteredSuppliers = _suppliers.Where(s =>
                        (s.Name?.ToLower().Contains(searchLower) ?? false) ||
                        (s.ContactPerson?.ToLower().Contains(searchLower) ?? false) ||
                        (s.Phone?.ToLower().Contains(searchLower) ?? false) ||
                        (s.Email?.ToLower().Contains(searchLower) ?? false) ||
                        (s.Address?.ToLower().Contains(searchLower) ?? false) ||
                        GetSupplierCode(s.SupplierId).ToLower().Contains(searchLower)
                    ).ToList();
                }

                // Populate DataGridView với thread-safe
                UIHelper.SafeInvoke(this, () =>
                {
                    if (dgvSuppliers == null) return;

                    foreach (var supplier in filteredSuppliers)
                    {
                        string contactInfo = FormatContactInfo(supplier.Phone, supplier.Email);
                        string status = "Hoạt động"; // Mặc định, có thể kiểm tra IsActive nếu có trong model

                        var rowIndex = dgvSuppliers.Rows.Add(
                            GetSupplierCode(supplier.SupplierId),
                            supplier.Name ?? "",
                            supplier.ContactPerson ?? "",
                            contactInfo,
                            supplier.Address ?? "",
                            status
                        );

                        // Lưu SupplierId vào Tag để dùng khi edit
                        dgvSuppliers.Rows[rowIndex].Tag = supplier;
                    }
                });
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải danh sách nhà cung cấp");
                UIHelper.SafeInvoke(this, () =>
                {
                    if (dgvSuppliers != null)
                    {
                        dgvSuppliers.Rows.Add("", "Lỗi tải dữ liệu", "", "", "", "", "");
                    }
                });
            }
            finally
            {
                _isLoading = false;
                ShowLoading(false);
            }
        }

        private string GetSupplierCode(int supplierId)
        {
            return $"SUP{supplierId:D3}";
        }

        private string FormatContactInfo(string phone, string email)
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(phone))
            {
                parts.Add($"📞 {phone}");
            }
            
            if (!string.IsNullOrWhiteSpace(email))
            {
                parts.Add($"✉ {email}");
            }

            return parts.Any() ? string.Join(" | ", parts) : "";
        }

        private void ShowLoading(bool show)
        {
            Cursor = show ? Cursors.WaitCursor : Cursors.Default;
        }

        private void SetupDataGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(240, 240, 240);
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 40;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 245, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 35;
        }

        private void DgvSuppliers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string colName = dgvSuppliers.Columns[e.ColumnIndex].Name;
            
            if (colName == "Status")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    e.CellStyle.BackColor = GetStatusBadgeColor(status);
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private Color GetStatusBadgeColor(string status)
        {
            switch (status)
            {
                case "Hoạt động":
                    return Color.FromArgb(200, 230, 201); // Xanh nhạt
                case "Ngừng hoạt động":
                    return Color.FromArgb(255, 205, 210); // Đỏ nhạt
                default:
                    return Color.LightGray;
            }
        }

        private void DgvSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvSuppliers.Columns[e.ColumnIndex].Name;

            if (colName == "colEdit")
            {
                var row = dgvSuppliers.Rows[e.RowIndex];
                var supplier = row.Tag as Supplier;

                if (supplier != null)
                {
                    OpenEditSupplierForm(supplier);
                }
            }
        }

        private async void BtnAddSupplier_Click(object sender, EventArgs e)
        {
            Form mainForm = this.FindForm();
            while (mainForm != null && !(mainForm is MainForm))
            {
                mainForm = mainForm.ParentForm ?? mainForm.Owner;
            }

            using (var addSupplierForm = new AddSupplierForm())
            {
                DialogResult result = mainForm != null
                    ? FormHelper.ShowModalWithDim(mainForm, addSupplierForm)
                    : addSupplierForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    await LoadSuppliersAsync();
                }
            }
        }

        private async void OpenEditSupplierForm(Supplier supplier)
        {
            Form mainForm = this.FindForm();
            while (mainForm != null && !(mainForm is MainForm))
            {
                mainForm = mainForm.ParentForm ?? mainForm.Owner;
            }

            using (var editSupplierForm = new AddSupplierForm(supplier))
            {
                DialogResult result = mainForm != null
                    ? FormHelper.ShowModalWithDim(mainForm, editSupplierForm)
                    : editSupplierForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    await LoadSuppliersAsync();
                }
            }
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _currentSearchTerm = txtSearch?.Text ?? "";
            await LoadSuppliersAsync();
        }

    }
}
