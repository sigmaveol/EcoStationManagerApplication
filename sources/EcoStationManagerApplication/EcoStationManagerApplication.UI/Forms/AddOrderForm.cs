using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using AppServices = EcoStationManagerApplication.UI.Common.AppServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EcoStationManagerApplication.UI.Common;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class AddOrderForm : Form
    {
        // Danh sách sản phẩm đã chọn
        private List<OrderDetailItem> _selectedProducts = new List<OrderDetailItem>();

        // Danh sách khách hàng để tìm kiếm
        private List<Customer> _customers = new List<Customer>();
        private List<Product> _products = new List<Product>();

        // TextBox cho khách hàng và SĐT
        // txtCustomer và txtPhone đã được khai báo trong Designer (partial class)
        
        // Flag để tránh vòng lặp khi tự động điền
        private bool _isUpdatingCustomerName = false;
        private System.Windows.Forms.Timer _phoneSearchTimer;

        public AddOrderForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Tạo đơn hàng mới";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(800, 700);

            // Khởi tạo controls
            InitializeControls();

            // Load dữ liệu
            _ = LoadCustomersAsync();
            _ = LoadProductsAsync();
            InitializeDataGridView();
            SetupComboBoxes();
        }

        private void InitializeControls()
        {
            // Đăng ký sự kiện cho các TextBox đã có từ Designer
            txtCustomer.TextChanged += txtCustomer_TextChanged;
            txtPhone.TextChanged += txtPhone_TextChanged;
            
            // Khởi tạo timer cho tìm kiếm SĐT
            _phoneSearchTimer = new System.Windows.Forms.Timer();
            _phoneSearchTimer.Interval = 300; // 300ms delay
            _phoneSearchTimer.Tick += PhoneSearchTimer_Tick;
        }

        private void PhoneSearchTimer_Tick(object sender, EventArgs e)
        {
            _phoneSearchTimer.Stop();
            
            if (_isUpdatingCustomerName)
                return;

            string phoneInput = NormalizePhone(txtPhone.Text);
            if (string.IsNullOrWhiteSpace(phoneInput))
            {
                return;
            }

            // Tìm khách hàng theo SĐT (đã normalize)
            var customer = _customers.FirstOrDefault(c =>
            {
                if (c.Phone == null)
                    return false;
                
                string normalizedDbPhone = NormalizePhone(c.Phone);
                return normalizedDbPhone.Equals(phoneInput, StringComparison.OrdinalIgnoreCase);
            });

            if (customer != null)
            {
                _isUpdatingCustomerName = true;
                txtCustomer.Text = customer.Name;
                _isUpdatingCustomerName = false;
            }
        }

        private void SetupComboBoxes()
        {
            // Nguồn đơn hàng
            cmbSource.Items.Clear();
            cmbSource.Items.Add("Thủ công");
            cmbSource.Items.Add("Google Form");
            cmbSource.Items.Add("Excel");
            cmbSource.Items.Add("Email");
            cmbSource.SelectedIndex = 0;

            // Phương thức thanh toán
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.Add("Tiền mặt");
            cmbPaymentMethod.Items.Add("Chuyển khoản");
            cmbPaymentMethod.SelectedIndex = 0;
        }

        private void InitializeDataGridView()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;

            // Xóa các cột cũ nếu có
            dgvProducts.Columns.Clear();

            // Thêm cột
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "ProductName",
                Width = 250,
                ReadOnly = true
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Số lượng",
                DataPropertyName = "Quantity",
                Width = 100,
                ReadOnly = true
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "Đơn giá",
                DataPropertyName = "UnitPrice",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalPrice",
                HeaderText = "Thành tiền",
                DataPropertyName = "TotalPrice",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            // Cột xóa
            var deleteColumn = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Xóa",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 60
            };
            dgvProducts.Columns.Add(deleteColumn);
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                // Load tất cả khách hàng để tìm kiếm
                var result = await AppServices.CustomerService.SearchCustomersAsync("");
                if (result.Success && result.Data != null)
                {
                    _customers = result.Data.ToList();

                    // Thiết lập AutoComplete cho TextBox khách hàng
                    SetupCustomerAutoComplete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupCustomerAutoComplete()
        {
            // Tạo AutoCompleteStringCollection từ danh sách khách hàng
            var autoCompleteSource = new AutoCompleteStringCollection();

            foreach (var customer in _customers)
            {
                string displayText = $"{customer.Name} - {customer.Phone ?? ""}";
                autoCompleteSource.Add(displayText);
            }

            // Cấu hình AutoComplete cho TextBox
            txtCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtCustomer.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCustomer.AutoCompleteCustomSource = autoCompleteSource;
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var result = await AppServices.ProductService.GetAllActiveProductsAsync();
                if (result.Success && result.Data != null)
                {
                    _products = result.Data.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            // Có thể thêm logic tìm kiếm real-time nếu cần
            // Hiện tại đã có AutoComplete nên không cần xử lý thêm
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            // Tự động điền tên khách hàng khi nhập SĐT
            if (_isUpdatingCustomerName)
                return;

            // Dừng timer cũ nếu có
            _phoneSearchTimer.Stop();
            
            // Khởi động lại timer (debounce)
            _phoneSearchTimer.Start();
        }

        private string NormalizePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            // Loại bỏ tất cả khoảng trắng, dấu gạch ngang, dấu ngoặc đơn
            return phone.Trim()
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(".", "");
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            using (var addProductForm = new AddProductToOrderForm(_products))
            {
                if (addProductForm.ShowDialog() == DialogResult.OK)
                {
                    var selectedProduct = addProductForm.SelectedProduct;
                    var quantity = addProductForm.Quantity;

                    if (selectedProduct != null && quantity > 0)
                    {
                        // Kiểm tra sản phẩm đã tồn tại chưa
                        var existing = _selectedProducts.FirstOrDefault(p => p.ProductId == selectedProduct.ProductId);
                        if (existing != null)
                        {
                            existing.Quantity += quantity;
                            existing.TotalPrice = existing.Quantity * existing.UnitPrice;
                        }
                        else
                        {
                            _selectedProducts.Add(new OrderDetailItem
                            {
                                ProductId = selectedProduct.ProductId,
                                ProductName = selectedProduct.Name,
                                Quantity = quantity,
                                UnitPrice = selectedProduct.Price,
                                TotalPrice = quantity * selectedProduct.Price
                            });
                        }

                        RefreshProductGrid();
                    }
                }
            }
        }

        private void RefreshProductGrid()
        {
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = _selectedProducts;
            dgvProducts.Refresh();

            // Tính tổng tiền
            decimal total = _selectedProducts.Sum(p => p.TotalPrice);
            lblTotalAmount.Text = $"Tổng tiền: {total:N0} đ";
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvProducts.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                var item = _selectedProducts[e.RowIndex];
                _selectedProducts.Remove(item);
                RefreshProductGrid();
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Validation - chỉ sản phẩm là bắt buộc
            if (_selectedProducts.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đơn hàng
            _ = CreateOrderAsync();
        }

        private async Task CreateOrderAsync()
        {
            try
            {
                btnCreate.Enabled = false;
                Cursor = Cursors.WaitCursor;

                // Tìm khách hàng - ưu tiên tìm theo SĐT, sau đó mới tìm theo tên
                int? customerId = null;
                string phoneInput = txtPhone.Text.Trim();
                string customerNameInput = txtCustomer.Text.Trim();

                // Ưu tiên tìm theo SĐT
                if (!string.IsNullOrWhiteSpace(phoneInput))
                {
                    customerId = await FindCustomerIdByPhoneAsync(phoneInput);
                }

                // Nếu không tìm thấy theo SĐT, tìm theo tên
                if (customerId == null && !string.IsNullOrWhiteSpace(customerNameInput))
                {
                    customerId = await FindCustomerIdByNameAsync(customerNameInput);
                }

                // TẠO ĐƠN HÀNG (cho phép CustomerId = null cho khách vãng lai)
                var order = new Order
                {
                    CustomerId = customerId, // Có thể null cho khách vãng lai
                    Source = GetOrderSource(),
                    PaymentMethod = GetPaymentMethod(),
                    PaymentStatus = PaymentStatus.UNPAID,
                    Address = txtAddress.Text.Trim(), // Có thể null
                    Note = txtNote.Text.Trim(),
                    Status = OrderStatus.DRAFT,
                    TotalAmount = _selectedProducts.Sum(p => p.TotalPrice),
                    LastUpdated = DateTime.Now,
                    UserId = AppUserContext.CurrentUserId,
                };

                // ... (phần còn lại của hàm giữ nguyên) ...
                var orderDetails = _selectedProducts.Select(p => new OrderDetail
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice
                }).ToList();

                var result = await AppServices.OrderService.CreateOrderAsync(order, orderDetails);

                if (result.Success)
                {
                    MessageBox.Show($"Tạo đơn hàng thành công!\nMã đơn: {result.Data}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Lỗi tạo đơn hàng: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCreate.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private async Task<int?> FindCustomerIdByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            try
            {
                // Tìm trong danh sách đã load
                var cachedCustomer = _customers.FirstOrDefault(c =>
                    c.Phone != null && c.Phone.Equals(phone, StringComparison.OrdinalIgnoreCase));

                if (cachedCustomer != null)
                    return cachedCustomer.CustomerId;

                // Nếu không tìm thấy trong cache, có thể tìm kiếm bằng service
                // (Tùy chọn - nếu bạn muốn tìm kiếm real-time từ database)
                return null;
            }
            catch
            {
                return null;
            }
        }

        private async Task<int?> FindCustomerIdByNameAsync(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return null;

            try
            {
                // Kịch bản 1: Người dùng chọn từ AutoComplete (chuỗi chính xác)
                var cachedCustomer = _customers.FirstOrDefault(c =>
                    $"{c.Name} - {c.Phone ?? ""}" == customerName);

                if (cachedCustomer != null)
                    return cachedCustomer.CustomerId;

                // Kịch bản 2: Người dùng gõ TÊN chính xác (nhưng không chọn từ AutoComplete)
                cachedCustomer = _customers.FirstOrDefault(c =>
                    c.Name != null && c.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase));

                if (cachedCustomer != null)
                    return cachedCustomer.CustomerId;

                return null;
            }
            catch
            {
                return null;
            }
        }

        private OrderSource GetOrderSource()
        {
            // Kiểm tra SelectedIndex hợp lệ
            int selectedIndex = cmbSource.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= cmbSource.Items.Count)
            {
                return OrderSource.MANUAL; // Default value
            }
            
            switch (selectedIndex)
            {
                case 0: return OrderSource.MANUAL;
                case 1: return OrderSource.GOOGLEFORM;
                case 2: return OrderSource.EXCEL;
                case 3: return OrderSource.EMAIL;
                default: return OrderSource.MANUAL;
            }
        }

        private PaymentMethod GetPaymentMethod()
        {
            // Kiểm tra SelectedIndex hợp lệ
            int selectedIndex = cmbPaymentMethod.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= cmbPaymentMethod.Items.Count)
            {
                return PaymentMethod.CASH; // Default value
            }
            
            return selectedIndex == 0 ? PaymentMethod.CASH : PaymentMethod.TRANSFER;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AddOrderForm_Load(object sender, EventArgs e)
        {
            // Đã xử lý trong InitializeForm
        }

        // Helper class cho OrderDetail trong DataGridView
        private class OrderDetailItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
        }

        // Các sự kiện khác
        private void txtNote_TextChanged(object sender, EventArgs e) { }

        private void lblCustomer_Click(object sender, EventArgs e)
        {

        }
    }

    // Extension method cho string comparison (để hỗ trợ .Contains với StringComparison)
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
    }

    // Form popup để thêm sản phẩm
    public class AddProductToOrderForm : Form
    {
        private ComboBox cmbProduct;
        private NumericUpDown numQuantity;
        private Button btnOK;
        private Button btnCancel;
        private Label lblProduct;
        private Label lblQuantity;
        private List<Product> _products;

        public Product SelectedProduct { get; private set; }
        public decimal Quantity { get; private set; }

        public AddProductToOrderForm(List<Product> products)
        {
            _products = products;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Thêm sản phẩm";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblProduct = new Label
            {
                Text = "Sản phẩm *",
                Location = new Point(20, 20),
                AutoSize = true
            };

            cmbProduct = new ComboBox
            {
                Location = new Point(20, 45),
                Width = 340,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // SỬA Ở ĐÂY: Dùng DataSource thay vì Items.Add
            cmbProduct.DataSource = _products;          // 1. Gán nguồn dữ liệu là list sản phẩm
            cmbProduct.DisplayMember = "Name";          // 2. Chỉ định thuộc tính để HIỂN THỊ
            cmbProduct.ValueMember = "ProductId";       // 3. Chỉ định thuộc tính GIÁ TRỊ (để lấy ID)

            if (cmbProduct.Items.Count > 0)
                cmbProduct.SelectedIndex = 0;

            lblQuantity = new Label
            {
                Text = "Số lượng *",
                Location = new Point(20, 85),
                AutoSize = true
            };

            numQuantity = new NumericUpDown
            {
                Location = new Point(20, 110),
                Width = 150,
                Minimum = 0.01m,
                DecimalPlaces = 2,
                Value = 1
            };

            btnOK = new Button
            {
                Text = "Thêm",
                Location = new Point(200, 110),
                Size = new Size(75, 30),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(285, 110),
                Size = new Size(75, 30),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.Add(lblProduct);
            this.Controls.Add(cmbProduct);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(numQuantity);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            SelectedProduct = cmbProduct.SelectedItem as Product;
            Quantity = numQuantity.Value;
        }
    }
}