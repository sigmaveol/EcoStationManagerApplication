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

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class OrderDetailForm : Form
    {
        private int _orderId;
        private Order _order;
        private List<Product> _products;

        public OrderDetailForm(int orderId)
        {
            _orderId = orderId;
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Chi tiết đơn hàng";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(900, 800);
            this.BackColor = Color.White;

            // Load dữ liệu
            _ = LoadOrderDataAsync();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvProducts.EnableHeadersVisualStyles = false;

            // Xóa các cột cũ nếu có
            dgvProducts.Columns.Clear();

            // Thêm cột Sản phẩm
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Sản phẩm",
                DataPropertyName = "ProductName",
                Width = 300,
                ReadOnly = true
            });

            // Thêm cột Đơn giá
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "Đơn giá",
                DataPropertyName = "UnitPrice",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // Thêm cột Số lượng
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Số lượng",
                DataPropertyName = "Quantity",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // Thêm cột Giảm giá
            var discountColumn = new DataGridViewTextBoxColumn
            {
                Name = "Discount",
                HeaderText = "Giảm giá",
                DataPropertyName = "Discount",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N0",
                    ForeColor = Color.Red,
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            };
            dgvProducts.Columns.Add(discountColumn);

            // Thêm cột Thành tiền
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalPrice",
                HeaderText = "Thành tiền",
                DataPropertyName = "TotalPrice",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
        }

        private async Task LoadOrderDataAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Load đơn hàng với chi tiết
                var orderResult = await AppServices.OrderService.GetOrderWithDetailsAsync(_orderId);
                if (!orderResult.Success || orderResult.Data == null)
                {
                    MessageBox.Show($"Không tìm thấy đơn hàng: {orderResult.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                _order = orderResult.Data;

                // Load thông tin khách hàng nếu có CustomerId
                if (_order.CustomerId.HasValue)
                {
                    var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(_order.CustomerId.Value);
                    if (customerResult.Success && customerResult.Data != null)
                    {
                        _order.Customer = customerResult.Data;
                    }
                }

                // Load danh sách sản phẩm để lấy tên
                var productsResult = await AppServices.ProductService.GetAllActiveProductsAsync();
                if (productsResult.Success && productsResult.Data != null)
                {
                    _products = productsResult.Data.ToList();
                }

                // Hiển thị dữ liệu
                DisplayOrderData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void DisplayOrderData()
        {
            if (_order == null) return;

            // Thông tin chung về đơn hàng
            lblOrderCodeValue.Text = _order.OrderCode ?? $"ORD-{_order.OrderId:D5}";
            lblStatusValue.Text = GetOrderStatusDisplay(_order.Status);
            lblSourceValue.Text = GetOrderSourceDisplay(_order.Source);
            lblCreatedDateValue.Text = _order.LastUpdated.ToString("dd/MM/yyyy HH:mm");

            // Thông tin khách hàng
            if (_order.CustomerId.HasValue && _order.Customer != null)
            {
                lblCustomerNameValue.Text = _order.Customer.Name ?? "Khách lẻ";
                lblCustomerPhoneValue.Text = _order.Customer.Phone ?? "---";
            }
            else
            {
                lblCustomerNameValue.Text = "Khách lẻ";
                lblCustomerPhoneValue.Text = "---";
            }

            lblDeliveryAddressValue.Text = _order.Address ?? "---";
            lblPaymentMethodValue.Text = GetPaymentMethodDisplay(_order.PaymentMethod);

            // Chi tiết sản phẩm
            LoadProductDetails();

            // Tổng kết
            decimal totalAmount = _order.TotalAmount;
            decimal discountAmount = _order.DiscountedAmount;
            decimal finalTotal = totalAmount - discountAmount;

            lblTotalItemsValue.Text = $"{totalAmount:N0} đ";
            lblDiscountValue.Text = $"-{discountAmount:N0} đ";
            lblFinalTotalValue.Text = $"{finalTotal:N0} đ";

            // Ghi chú
            txtNote.Text = _order.Note ?? "";
        }

        private void LoadProductDetails()
        {
            if (_order?.OrderDetails == null || !_order.OrderDetails.Any())
            {
                dgvProducts.DataSource = null;
                return;
            }

            // Tạo danh sách chi tiết với tên sản phẩm
            var productDetails = new List<ProductDetailDisplay>();

            foreach (var detail in _order.OrderDetails)
            {
                var product = _products?.FirstOrDefault(p => p.ProductId == detail.ProductId);
                string productName = product?.Name ?? $"Sản phẩm ID: {detail.ProductId}";

                productDetails.Add(new ProductDetailDisplay
                {
                    ProductName = productName,
                    UnitPrice = detail.UnitPrice,
                    Quantity = detail.Quantity,
                    Discount = 0, // Có thể tính từ order level discount hoặc detail level
                    TotalPrice = detail.Quantity * detail.UnitPrice
                });
            }

            dgvProducts.DataSource = productDetails;
            dgvProducts.Refresh();
        }

        private string GetOrderStatusDisplay(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.DRAFT:
                    return "Nháp";
                case OrderStatus.CONFIRMED:
                    return "Mới";
                case OrderStatus.PROCESSING:
                    return "Đang xử lý";
                case OrderStatus.READY:
                    return "Chuẩn bị";
                case OrderStatus.SHIPPED:
                    return "Đang giao";
                case OrderStatus.COMPLETED:
                    return "Hoàn thành";
                case OrderStatus.CANCELLED:
                    return "Đã hủy";
                default:
                    return "Không xác định";
            }
        }

        private string GetOrderSourceDisplay(OrderSource source)
        {
            switch (source)
            {
                case OrderSource.MANUAL:
                    return "Thủ công";
                case OrderSource.GOOGLEFORM:
                    return "Google Form";
                case OrderSource.EXCEL:
                    return "Excel";
                case OrderSource.EMAIL:
                    return "Email";
                default:
                    return "Khác";
            }
        }

        private string GetPaymentMethodDisplay(PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.CASH:
                    return "Tiền mặt";
                case PaymentMethod.TRANSFER:
                    return "Chuyển khoản";
                default:
                    return "Khác";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Helper class để hiển thị trong DataGridView
        private class ProductDetailDisplay
        {
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Quantity { get; set; }
            public decimal Discount { get; set; }
            public decimal TotalPrice { get; set; }
        }

        private void panelOrderInfo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
