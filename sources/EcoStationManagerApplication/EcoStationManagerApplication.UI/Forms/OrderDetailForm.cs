using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using AppServices = EcoStationManagerApplication.UI.Common.AppServices;
using EcoStationManagerApplication.Common.Exporters;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
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
        private readonly IExcelExporter _excelExporter;
        private readonly IPdfExporter _pdfExporter;

        public OrderDetailForm(int orderId)
        {
            _orderId = orderId;
            _excelExporter = new ExcelExporter();
            _pdfExporter = new PdfExporter();
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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_order == null)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"ChiTietDonHang_{_order.OrderCode ?? _order.OrderId.ToString()}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    Title = "Xuất chi tiết đơn hàng ra Excel"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    // Xuất Excel với layout đầy đủ
                    ExportOrderDetailToExcel(saveDialog.FileName);

                    Cursor = Cursors.Default;
                    MessageBox.Show($"Đã xuất Excel thành công!\nFile: {saveDialog.FileName}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportOrderDetailToExcel(string filePath)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Chi tiết đơn hàng");
                int currentRow = 1;

                // Title
                worksheet.Cell(currentRow, 1).Value = "Chi tiết đơn hàng";
                worksheet.Cell(currentRow, 1).Style.Font.FontSize = 16;
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Range(currentRow, 1, currentRow, 5).Merge();
                currentRow += 2;

                // Thông tin đơn hàng
                worksheet.Cell(currentRow, 1).Value = "Mã đơn hàng:";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Value = _order.OrderCode ?? $"ORD-{_order.OrderId:D5}";
                
                worksheet.Cell(currentRow, 3).Value = "Trạng thái:";
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 4).Value = GetOrderStatusDisplay(_order.Status);
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "Nguồn đơn:";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Value = GetOrderSourceDisplay(_order.Source);
                
                worksheet.Cell(currentRow, 3).Value = "Ngày tạo:";
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 4).Value = _order.LastUpdated.ToString("dd/MM/yyyy HH:mm");
                currentRow += 2;

                // Thông tin khách hàng
                worksheet.Cell(currentRow, 1).Value = "Tên khách hàng:";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                string customerName = _order.CustomerId.HasValue && _order.Customer != null 
                    ? (_order.Customer.Name ?? "Khách lẻ") 
                    : "Khách lẻ";
                worksheet.Cell(currentRow, 2).Value = customerName;
                
                worksheet.Cell(currentRow, 3).Value = "Số điện thoại:";
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                string phone = _order.CustomerId.HasValue && _order.Customer != null 
                    ? (_order.Customer.Phone ?? "---") 
                    : "---";
                worksheet.Cell(currentRow, 4).Value = phone;
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "Địa chỉ giao hàng:";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Value = _order.Address ?? "---";
                worksheet.Range(currentRow, 2, currentRow, 4).Merge();
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "Phương thức thanh toán:";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Value = GetPaymentMethodDisplay(_order.PaymentMethod);
                currentRow += 2;

                // Chi tiết sản phẩm
                worksheet.Cell(currentRow, 1).Value = "Chi tiết sản phẩm";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Style.Font.FontSize = 12;
                currentRow++;

                // Header của bảng sản phẩm
                int headerRow = currentRow;
                worksheet.Cell(headerRow, 1).Value = "Sản phẩm";
                worksheet.Cell(headerRow, 2).Value = "Đơn giá";
                worksheet.Cell(headerRow, 3).Value = "Số lượng";
                worksheet.Cell(headerRow, 4).Value = "Giảm giá";
                worksheet.Cell(headerRow, 5).Value = "Thành tiền";

                var headerRange = worksheet.Range(headerRow, 1, headerRow, 5);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(240, 240, 240);
                headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                currentRow++;

                // Dữ liệu sản phẩm
                if (_order?.OrderDetails != null && _order.OrderDetails.Any())
                {
                    foreach (var detail in _order.OrderDetails)
                    {
                        var product = _products?.FirstOrDefault(p => p.ProductId == detail.ProductId);
                        string productName = product?.Name ?? $"Sản phẩm ID: {detail.ProductId}";
                        decimal unitPrice = detail.UnitPrice;
                        decimal quantity = detail.Quantity;
                        decimal discount = 0;
                        decimal totalPrice = quantity * unitPrice;

                        worksheet.Cell(currentRow, 1).Value = productName;
                        worksheet.Cell(currentRow, 2).Value = unitPrice;
                        worksheet.Cell(currentRow, 2).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 3).Value = quantity;
                        worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "#,##0.00";
                        worksheet.Cell(currentRow, 4).Value = discount;
                        worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(currentRow, 4).Style.Font.FontColor = ClosedXML.Excel.XLColor.Red;
                        worksheet.Cell(currentRow, 5).Value = totalPrice;
                        worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0";
                        currentRow++;
                    }
                }

                // Border cho bảng sản phẩm
                var productTableRange = worksheet.Range(headerRow, 1, currentRow - 1, 5);
                productTableRange.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                productTableRange.Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                currentRow += 2;

                // Ghi chú
                worksheet.Cell(currentRow, 1).Value = "Ghi chú:";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = _order.Note ?? "";
                worksheet.Range(currentRow, 1, currentRow, 5).Merge();
                currentRow += 2;

                // Tổng kết
                decimal totalAmount = _order.TotalAmount;
                decimal discountAmount = _order.DiscountedAmount;
                decimal finalTotal = totalAmount - discountAmount;

                worksheet.Cell(currentRow, 4).Value = "Tổng tiền hàng:";
                worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 5).Value = totalAmount;
                worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0";
                currentRow++;

                worksheet.Cell(currentRow, 4).Value = "Giảm giá:";
                worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 5).Value = discountAmount;
                worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0";
                worksheet.Cell(currentRow, 5).Style.Font.FontColor = ClosedXML.Excel.XLColor.Red;
                currentRow++;

                worksheet.Cell(currentRow, 4).Value = "Thành tiền:";
                worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 4).Style.Font.FontSize = 10;
                worksheet.Cell(currentRow, 5).Value = finalTotal;
                worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0";
                worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 5).Style.Font.FontSize = 10;

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(filePath);
            }
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (_order == null)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"ChiTietDonHang_{_order.OrderCode ?? _order.OrderId.ToString()}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Xuất chi tiết đơn hàng ra PDF"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    // Vẽ chi tiết đơn hàng và xuất PDF bằng Graphics (không dùng PdfSharp)
                    ExportOrderDetailToPdfUsingGraphics(saveDialog.FileName);

                    Cursor = Cursors.Default;
                    MessageBox.Show($"Đã xuất PDF thành công!\nFile: {saveDialog.FileName}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportOrderDetailToPdfUsingGraphics(string filePath)
        {
            // Tạo Bitmap với kích thước A4 (210mm x 297mm) ở 96 DPI
            int width = 794; // A4 width in pixels at 96 DPI
            int height = 1123; // A4 height in pixels at 96 DPI
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                    // Fonts
                    Font titleFont = new Font("Segoe UI", 16, FontStyle.Bold);
                    Font headerFont = new Font("Segoe UI", 9, FontStyle.Bold);
                    Font normalFont = new Font("Segoe UI", 9);
                    Font smallFont = new Font("Segoe UI", 8);

                    float yPos = 40;
                    float leftMargin = 40;
                    float rightMargin = 40;
                    float contentWidth = width - leftMargin - rightMargin;

                    // Title
                    string title = "Chi tiết đơn hàng";
                    SizeF titleSize = g.MeasureString(title, titleFont);
                    g.DrawString(title, titleFont, Brushes.Black, leftMargin + (contentWidth - titleSize.Width) / 2, yPos);
                    yPos += titleSize.Height + 20;

                    // Thông tin đơn hàng
                    yPos = DrawOrderInfoSection(g, leftMargin, yPos, contentWidth, headerFont, normalFont);
                    yPos += 15;

                    // Thông tin khách hàng
                    yPos = DrawCustomerInfoSection(g, leftMargin, yPos, contentWidth, headerFont, normalFont);
                    yPos += 15;

                    // Chi tiết sản phẩm
                    yPos = DrawProductDetailsSection(g, leftMargin, yPos, contentWidth, headerFont, normalFont);
                    yPos += 15;

                    // Ghi chú
                    yPos = DrawNoteSection(g, leftMargin, yPos, contentWidth, headerFont, normalFont);
                    yPos += 15;

                    // Tổng kết
                    DrawSummarySection(g, leftMargin, yPos, contentWidth, headerFont, normalFont);
                }

                // Lưu bitmap và chuyển sang PDF bằng PrintDocument
                SaveBitmapToPdf(bitmap, filePath);
            }
        }

        private float DrawOrderInfoSection(Graphics g, float x, float y, float width, Font headerFont, Font normalFont)
        {
            float currentY = y;
            float lineHeight = 20;

            // Mã đơn hàng
            g.DrawString("Mã đơn hàng:", headerFont, Brushes.Black, x, currentY);
            string orderCode = _order.OrderCode ?? $"ORD-{_order.OrderId:D5}";
            g.DrawString(orderCode, normalFont, Brushes.Black, x + 120, currentY);
            
            // Trạng thái
            g.DrawString("Trạng thái:", headerFont, Brushes.Black, x + width / 2, currentY);
            g.DrawString(GetOrderStatusDisplay(_order.Status), normalFont, Brushes.Black, x + width / 2 + 100, currentY);
            currentY += lineHeight;

            // Nguồn đơn
            g.DrawString("Nguồn đơn:", headerFont, Brushes.Black, x, currentY);
            g.DrawString(GetOrderSourceDisplay(_order.Source), normalFont, Brushes.Black, x + 120, currentY);
            
            // Ngày tạo
            g.DrawString("Ngày tạo:", headerFont, Brushes.Black, x + width / 2, currentY);
            g.DrawString(_order.LastUpdated.ToString("dd/MM/yyyy HH:mm"), normalFont, Brushes.Black, x + width / 2 + 100, currentY);
            currentY += lineHeight;

            // Đường kẻ ngang
            g.DrawLine(Pens.LightGray, x, currentY, x + width, currentY);
            currentY += 10;

            return currentY;
        }

        private float DrawCustomerInfoSection(Graphics g, float x, float y, float width, Font headerFont, Font normalFont)
        {
            float currentY = y;
            float lineHeight = 20;

            // Tên khách hàng
            g.DrawString("Tên khách hàng:", headerFont, Brushes.Black, x, currentY);
            string customerName = _order.CustomerId.HasValue && _order.Customer != null 
                ? (_order.Customer.Name ?? "Khách lẻ") 
                : "Khách lẻ";
            g.DrawString(customerName, normalFont, Brushes.Black, x + 150, currentY);
            
            // Số điện thoại
            g.DrawString("Số điện thoại:", headerFont, Brushes.Black, x + width / 2, currentY);
            string phone = _order.CustomerId.HasValue && _order.Customer != null 
                ? (_order.Customer.Phone ?? "---") 
                : "---";
            g.DrawString(phone, normalFont, Brushes.Black, x + width / 2 + 120, currentY);
            currentY += lineHeight;

            // Địa chỉ giao hàng
            g.DrawString("Địa chỉ giao hàng:", headerFont, Brushes.Black, x, currentY);
            g.DrawString(_order.Address ?? "---", normalFont, Brushes.Black, x + 150, currentY);
            currentY += lineHeight;

            // Phương thức thanh toán
            g.DrawString("Phương thức thanh toán:", headerFont, Brushes.Black, x, currentY);
            g.DrawString(GetPaymentMethodDisplay(_order.PaymentMethod), normalFont, Brushes.Black, x + 200, currentY);
            currentY += lineHeight;

            // Đường kẻ ngang
            g.DrawLine(Pens.LightGray, x, currentY, x + width, currentY);
            currentY += 10;

            return currentY;
        }

        private float DrawProductDetailsSection(Graphics g, float x, float y, float width, Font headerFont, Font normalFont)
        {
            float currentY = y;

            // Tiêu đề
            g.DrawString("Chi tiết sản phẩm", headerFont, Brushes.Black, x, currentY);
            currentY += 25;

            // Header của bảng
            float tableY = currentY;
            float col1Width = width * 0.4f; // Sản phẩm
            float col2Width = width * 0.15f; // Đơn giá
            float col3Width = width * 0.15f; // Số lượng
            float col4Width = width * 0.15f; // Giảm giá
            float col5Width = width * 0.15f; // Thành tiền

            // Vẽ header background
            g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), x, tableY, width, 25);

            // Header text
            g.DrawString("Sản phẩm", headerFont, Brushes.Black, x + 5, tableY + 5);
            g.DrawString("Đơn giá", headerFont, Brushes.Black, x + col1Width + 5, tableY + 5);
            g.DrawString("Số lượng", headerFont, Brushes.Black, x + col1Width + col2Width + 5, tableY + 5);
            g.DrawString("Giảm giá", headerFont, Brushes.Black, x + col1Width + col2Width + col3Width + 5, tableY + 5);
            g.DrawString("Thành tiền", headerFont, Brushes.Black, x + col1Width + col2Width + col3Width + col4Width + 5, tableY + 5);

            // Đường kẻ header
            g.DrawLine(Pens.Gray, x, tableY + 25, x + width, tableY + 25);
            currentY = tableY + 25;

            // Dữ liệu sản phẩm
            if (_order?.OrderDetails != null && _order.OrderDetails.Any())
            {
                int index = 0; // <--- SỬA: Khởi tạo biến đếm index
                foreach (var detail in _order.OrderDetails)
                {
                    var product = _products?.FirstOrDefault(p => p.ProductId == detail.ProductId);
                    string productName = product?.Name ?? $"Sản phẩm ID: {detail.ProductId}";
                    decimal unitPrice = detail.UnitPrice;
                    decimal quantity = detail.Quantity;
                    decimal discount = 0;
                    decimal totalPrice = quantity * unitPrice;

                    // Vẽ dòng (alternate color)
                    // <--- SỬA: Dùng biến index để kiểm tra chẵn lẻ
                    if (index % 2 == 0)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(250, 250, 250)), x, currentY, width, 20);
                    }

                    g.DrawString(productName, normalFont, Brushes.Black, x + 5, currentY + 3);
                    g.DrawString(unitPrice.ToString("N0"), normalFont, Brushes.Black, x + col1Width + 5, currentY + 3);
                    g.DrawString(quantity.ToString("N2"), normalFont, Brushes.Black, x + col1Width + col2Width + 5, currentY + 3);
                    g.DrawString(discount.ToString("N0"), normalFont, Brushes.Red, x + col1Width + col2Width + col3Width + 5, currentY + 3);
                    g.DrawString(totalPrice.ToString("N0"), normalFont, Brushes.Black, x + col1Width + col2Width + col3Width + col4Width + 5, currentY + 3);

                    // Đường kẻ dưới
                    g.DrawLine(Pens.LightGray, x, currentY + 20, x + width, currentY + 20);
                    currentY += 20;

                    index++; // <--- SỬA: Tăng biến đếm
                }
            }

            currentY += 10;
            return currentY;
        }

        // Các hàm bên dưới giữ nguyên như cũ
        private float DrawNoteSection(Graphics g, float x, float y, float width, Font headerFont, Font normalFont)
        {
            float currentY = y;

            g.DrawString("Ghi chú:", headerFont, Brushes.Black, x, currentY);
            currentY += 20;

            string note = _order.Note ?? "";
            if (!string.IsNullOrEmpty(note))
            {
                RectangleF noteRect = new RectangleF(x, currentY, width, 60);
                g.DrawString(note, normalFont, Brushes.Black, noteRect);
                currentY += 60;
            }
            else
            {
                currentY += 20;
            }

            return currentY;
        }

        private void DrawSummarySection(Graphics g, float x, float y, float width, Font headerFont, Font normalFont)
        {
            float currentY = y;
            float rightAlignX = x + width - 200;

            decimal totalAmount = _order.TotalAmount;
            decimal discountAmount = _order.DiscountedAmount;
            decimal finalTotal = totalAmount - discountAmount;

            // Tổng tiền hàng
            g.DrawString("Tổng tiền hàng:", normalFont, Brushes.Black, rightAlignX, currentY);
            g.DrawString($"{totalAmount:N0} đ", normalFont, Brushes.Black, rightAlignX + 120, currentY);
            currentY += 20;

            // Giảm giá
            g.DrawString("Giảm giá:", normalFont, Brushes.Red, rightAlignX, currentY);
            g.DrawString($"-{discountAmount:N0} đ", normalFont, Brushes.Red, rightAlignX + 120, currentY);
            currentY += 20;

            // Thành tiền (bold)
            Font boldFont = new Font(normalFont.FontFamily, normalFont.Size, FontStyle.Bold);
            g.DrawString("Thành tiền:", boldFont, Brushes.Black, rightAlignX, currentY);
            g.DrawString($"{finalTotal:N0} đ", boldFont, Brushes.Black, rightAlignX + 120, currentY);
        }

        private void SaveBitmapToPdf(Bitmap bitmap, string filePath)
        {
            // Sử dụng PrintDocument để xuất PDF
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintController = new System.Drawing.Printing.StandardPrintController();
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("A4", 794, 1123);
            printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            bool printed = false;
            printDoc.PrintPage += (sender, e) =>
            {
                if (!printed)
                {
                    e.Graphics.DrawImage(bitmap, 0, 0, e.PageBounds.Width, e.PageBounds.Height);
                    printed = true;
                    e.HasMorePages = false;
                }
            };

            // Tìm Microsoft Print to PDF hoặc PDF printer
            string pdfPrinter = null;
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                if (printer.Contains("PDF") || printer.Contains("Microsoft Print to PDF"))
                {
                    pdfPrinter = printer;
                    break;
                }
            }

            if (pdfPrinter != null)
            {
                printDoc.PrinterSettings.PrinterName = pdfPrinter;
                printDoc.PrinterSettings.PrintToFile = true;
                printDoc.PrinterSettings.PrintFileName = filePath;

                try
                {
                    printDoc.Print();
                }
                catch (Exception ex)
                {
                    // Nếu không thể in, lưu dưới dạng ảnh
                    string imagePath = Path.ChangeExtension(filePath, ".png");
                    bitmap.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                    throw new Exception($"Không thể xuất PDF. Đã lưu dưới dạng ảnh: {imagePath}. Lỗi: {ex.Message}");
                }
            }
            else
            {
                // Nếu không có PDF printer, lưu dưới dạng ảnh
                string imagePath = Path.ChangeExtension(filePath, ".png");
                bitmap.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                throw new Exception($"Không tìm thấy PDF printer. Đã lưu dưới dạng ảnh: {imagePath}");
            }
        }

        private List<ProductDetailDisplay> CreateExportData()
        {
            var exportData = new List<ProductDetailDisplay>();

            if (_order?.OrderDetails != null && _order.OrderDetails.Any())
            {
                foreach (var detail in _order.OrderDetails)
                {
                    var product = _products?.FirstOrDefault(p => p.ProductId == detail.ProductId);
                    string productName = product?.Name ?? $"Sản phẩm ID: {detail.ProductId}";

                    exportData.Add(new ProductDetailDisplay
                    {
                        ProductName = productName,
                        UnitPrice = detail.UnitPrice,
                        Quantity = detail.Quantity,
                        Discount = 0,
                        TotalPrice = detail.Quantity * detail.UnitPrice
                    });
                }
            }

            return exportData;
        }
    }
}
