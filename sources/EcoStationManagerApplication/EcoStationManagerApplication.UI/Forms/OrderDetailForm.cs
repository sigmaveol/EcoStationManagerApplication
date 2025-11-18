using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using AppServices = EcoStationManagerApplication.UI.Common.AppServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class OrderDetailForm : Form
    {
        private int _orderId;

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
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(800, 700);

            InitializeDataGridView();
            _ = LoadOrderDataAsync();
        }

        private void InitializeDataGridView()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;

            // Clear existing columns
            dgvProducts.Columns.Clear();

            // Product column
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Sản phẩm",
                DataPropertyName = "ProductName",
                Width = 250,
                ReadOnly = true
            });

            // Unit price column
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

            // Quantity column
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Số lượng",
                DataPropertyName = "Quantity",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // Discount column (will be styled red)
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
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.Red
                }
            };
            dgvProducts.Columns.Add(discountColumn);

            // Total column
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Total",
                HeaderText = "Thành tiền",
                DataPropertyName = "Total",
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

                // Load order with details
                var orderResult = await AppServices.OrderService.GetOrderWithDetailsAsync(_orderId);
                if (!orderResult.Success || orderResult.Data == null)
                {
                    MessageBox.Show($"Không thể tải thông tin đơn hàng: {orderResult.Message}", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                var order = orderResult.Data;
                LoadOrderInfo(order);
                LoadCustomerInfo(order);
                await LoadProductDetailsAsync(order);
                LoadSummary(order);
                LoadNotes(order);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void LoadOrderInfo(Order order)
        {
            lblOrderCode.Text = order.OrderCode ?? $"DH{order.OrderId:D6}";
            lblStatus.Text = FormatOrderStatus(order.Status);
            lblSource.Text = FormatOrderSource(order.Source);
            lblCreatedDate.Text = order.LastUpdated.ToString("dd/MM/yyyy HH:mm");
        }

        private void LoadCustomerInfo(Order order)
        {
            // Load customer if exists
            if (order.CustomerId.HasValue && order.Customer != null)
            {
                lblCustomerName.Text = order.Customer.Name ?? "Không có";
                lblPhone.Text = order.Customer.Phone ?? "Không có";
            }
            else
            {
                // Try to load customer separately if not loaded
                _ = LoadCustomerAsync(order.CustomerId);
            }

            lblAddress.Text = !string.IsNullOrWhiteSpace(order.Address) ? order.Address : "Không có";
            lblPaymentMethod.Text = FormatPaymentMethod(order.PaymentMethod);
        }

        private async Task LoadCustomerAsync(int? customerId)
        {
            if (!customerId.HasValue)
            {
                lblCustomerName.Text = "Khách lẻ";
                lblPhone.Text = "Không có";
                return;
            }

            try
            {
                var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(customerId.Value);
                if (customerResult.Success && customerResult.Data != null)
                {
                    var customer = customerResult.Data;
                    lblCustomerName.Text = customer.Name ?? "Không có";
                    lblPhone.Text = customer.Phone ?? "Không có";
                }
                else
                {
                    lblCustomerName.Text = "Không tìm thấy";
                    lblPhone.Text = "Không có";
                }
            }
            catch
            {
                lblCustomerName.Text = "Không tìm thấy";
                lblPhone.Text = "Không có";
            }
        }

        private async Task LoadProductDetailsAsync(Order order)
        {
            if (order.OrderDetails == null || !order.OrderDetails.Any())
            {
                dgvProducts.DataSource = null;
                return;
            }

            // Load all products at once for better performance
            var productIds = order.OrderDetails.Select(od => od.ProductId).Distinct().ToList();
            var products = new Dictionary<int, string>();

            foreach (var productId in productIds)
            {
                try
                {
                    var productResult = await AppServices.ProductService.GetProductByIdAsync(productId);
                    if (productResult.Success && productResult.Data != null)
                    {
                        products[productId] = productResult.Data.Name;
                    }
                    else
                    {
                        products[productId] = "Không xác định";
                    }
                }
                catch
                {
                    products[productId] = "Không xác định";
                }
            }

            // Create display items
            var productDetails = order.OrderDetails.Select(detail => new ProductDetailDisplay
            {
                ProductName = products.ContainsKey(detail.ProductId) ? products[detail.ProductId] : "Không xác định",
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                Discount = 0, // OrderDetail doesn't have discount field, using order-level discount
                Total = detail.Quantity * detail.UnitPrice
            }).ToList();

            dgvProducts.DataSource = productDetails;
            
            // Apply red color to discount column cells
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.Cells["Discount"] != null)
                {
                    row.Cells["Discount"].Style.ForeColor = Color.Red;
                }
            }
        }

        private void LoadSummary(Order order)
        {
            decimal subtotal = order.OrderDetails?.Sum(od => od.Quantity * od.UnitPrice) ?? 0;
            decimal discount = order.DiscountedAmount;
            decimal total = order.TotalAmount;

            lblSubtotal.Text = $"{subtotal:N0}đ";
            lblDiscount.Text = $"-{discount:N0}đ";
            lblTotal.Text = $"{total:N0}đ";
        }

        private void LoadNotes(Order order)
        {
            txtNotes.Text = !string.IsNullOrWhiteSpace(order.Note) ? order.Note : "";
        }

        private string FormatOrderStatus(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.DRAFT:
                    return "Nháp";
                case OrderStatus.CONFIRMED:
                    return "Đã xác nhận";
                case OrderStatus.PROCESSING:
                    return "Đang xử lý";
                case OrderStatus.READY:
                    return "Sẵn sàng";
                case OrderStatus.SHIPPED:
                    return "Đã giao";
                case OrderStatus.COMPLETED:
                    return "Hoàn thành";
                case OrderStatus.CANCELLED:
                    return "Đã hủy";
                default:
                    return status.ToString();
            }
        }

        private string FormatOrderSource(OrderSource source)
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
                    return source.ToString();
            }
        }

        private string FormatPaymentMethod(PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.CASH:
                    return "Tiền mặt";
                case PaymentMethod.TRANSFER:
                    return "Chuyển khoản";
                default:
                    return method.ToString();
            }
        }

        private void btnCloseX_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Helper class for displaying product details in DataGridView
        private class ProductDetailDisplay
        {
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Quantity { get; set; }
            public decimal Discount { get; set; }
            public decimal Total { get; set; }
        }
    }
}
