using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class AddProductForm : Form
    {
        private string _selectedImagePath = null;
        private int? _productId = null; // null = Add mode, có giá trị = Edit mode
        private bool _isEditMode => _productId.HasValue;
        private string _currentImagePath = null; // Đường dẫn ảnh hiện tại trong Resources
        private bool _imageDeleted = false; // Đánh dấu đã xóa ảnh

        /// <summary>
        /// Constructor cho chế độ Thêm mới
        /// </summary>
        public AddProductForm() : this(null)
        {
        }

        /// <summary>
        /// Constructor cho chế độ Chỉnh sửa
        /// </summary>
        /// <param name="productId">ID sản phẩm cần chỉnh sửa. Null = chế độ thêm mới</param>
        public AddProductForm(int? productId)
        {
            _productId = productId;
            InitializeComponent();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            // Ẩn label lỗi ban đầu
            labelError.Text = "";
            labelError.Visible = false;

            // Load ProductType combobox
            LoadProductTypes();

            // Load Categories combobox
            await LoadCategories();

            // Set default values
            txtMinStockLevel.Text = "15";
            chkIsActive.Checked = true;

            // Thiết lập chế độ Add hoặc Edit
            if (_isEditMode)
            {
                // Chế độ Edit: Load dữ liệu sản phẩm
                this.Text = "Chỉnh sửa sản phẩm";
                lblTitle.Text = "Chỉnh sửa sản phẩm";
                btnSave.Text = "Cập nhật";
                // Chỉ hiển thị nút Xóa nếu user là Admin
                btnDelete.Visible = AppUserContext.IsAdmin;
                txtSKU.ReadOnly = true; // Không cho sửa SKU khi edit
                await LoadProductData(_productId.Value);
            }
            else
            {
                // Chế độ Add
                this.Text = "Thêm sản phẩm mới";
                lblTitle.Text = "Thêm sản phẩm mới";
                btnSave.Text = "Lưu";
                btnDelete.Visible = false; // Ẩn nút Xóa khi Add mode
                txtSKU.ReadOnly = false;
            }
        }

        private async Task LoadProductData(int productId)
        {
            try
            {
                var productResult = await AppServices.ProductService.GetProductByIdAsync(productId);
                if (productResult.Success && productResult.Data != null)
                {
                    var product = productResult.Data;

                    // Load dữ liệu vào form
                    txtSKU.Text = product.Sku ?? "";
                    txtName.Text = product.Name;
                    txtUnit.Text = product.Unit;
                    txtPrice.Text = product.Price.ToString("N0");
                    txtMinStockLevel.Text = product.MinStockLevel.ToString("N0");
                    chkIsActive.Checked = product.IsActive == ActiveStatus.ACTIVE;

                    // Set ProductType - đảm bảo combobox đã có items
                    if (cmbProductType.Items.Count == 0)
                    {
                        LoadProductTypes();
                    }
                    
                    bool found = false;
                    for (int i = 0; i < cmbProductType.Items.Count; i++)
                    {
                        try
                        {
                            var item = (dynamic)cmbProductType.Items[i];
                            if (item.Value == product.ProductType)
                            {
                                cmbProductType.SelectedIndex = i;
                                found = true;
                                System.Diagnostics.Debug.WriteLine($"ProductType loaded: {product.ProductType} at index {i}");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error comparing ProductType at index {i}: {ex.Message}");
                        }
                    }
                    
                    // Nếu không tìm thấy, set về default (PACKED = index 0)
                    if (!found)
                    {
                        cmbProductType.SelectedIndex = 0;
                        System.Diagnostics.Debug.WriteLine($"ProductType {product.ProductType} not found, using default PACKED");
                    }

                    // Set Category
                    if (product.CategoryId.HasValue)
                    {
                        for (int i = 0; i < cmbCategory.Items.Count; i++)
                        {
                            var item = (dynamic)cmbCategory.Items[i];
                            if (item.Value == product.CategoryId.Value)
                            {
                                cmbCategory.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    // Load hình ảnh nếu có
                    await LoadProductImage(productId, product.Sku);
                }
                else
                {
                    UIHelper.ShowErrorMessage("Không thể tải thông tin sản phẩm.");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải thông tin sản phẩm");
            }
        }

        private async Task LoadProductImage(int productId, string sku)
        {
            try
            {
                // Tìm ảnh trong thư mục Resources/Products
                string imageDirectory = Path.Combine(Application.StartupPath, "Resources", "Products");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                    return;
                }

                // Tìm ảnh theo productId hoặc SKU
                string[] imageExtensions = { "*.jpg", "*.jpeg", "*.png", "*.gif" };
                string imagePath = null;

                // Ưu tiên tìm theo productId
                foreach (var ext in imageExtensions)
                {
                    string pattern = $"{productId}{ext.Replace("*", "")}";
                    var files = Directory.GetFiles(imageDirectory, pattern, SearchOption.TopDirectoryOnly);
                    if (files.Length > 0)
                    {
                        imagePath = files[0];
                        break;
                    }
                }

                // Nếu không tìm thấy, tìm theo SKU
                if (string.IsNullOrEmpty(imagePath) && !string.IsNullOrEmpty(sku))
                {
                    foreach (var ext in imageExtensions)
                    {
                        string pattern = $"{sku}{ext.Replace("*", "")}";
                        var files = Directory.GetFiles(imageDirectory, pattern, SearchOption.TopDirectoryOnly);
                        if (files.Length > 0)
                        {
                            imagePath = files[0];
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    _currentImagePath = imagePath;
                    pictureBoxProduct.Image = Image.FromFile(imagePath);
                    pictureBoxProduct.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi nếu không tìm thấy ảnh
                System.Diagnostics.Debug.WriteLine($"Error loading product image: {ex.Message}");
            }
        }

        private void LoadProductTypes()
        {
            cmbProductType.Items.Clear();
            cmbProductType.Items.Add(new { Text = "Hàng đóng chai, sản phẩm đóng gói", Value = ProductType.PACKED });
            cmbProductType.Items.Add(new { Text = "Hàng refill từ bồn", Value = ProductType.REFILLED });
            cmbProductType.Items.Add(new { Text = "Khác", Value = ProductType.OTHER });
            
            cmbProductType.DisplayMember = "Text";
            cmbProductType.ValueMember = "Value";
            cmbProductType.SelectedIndex = 0; // Mặc định PACKED
        }

        private async Task LoadCategories()
        {
            try
            {
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add(new { Text = "-- Chọn danh mục --", Value = (int?)null });

                // Load từ database
                var categoriesResult = await AppServices.CategoryService.GetActiveCategoriesAsync();
                if (categoriesResult.Success && categoriesResult.Data != null)
                {
                    foreach (var category in categoriesResult.Data)
                    {
                        cmbCategory.Items.Add(new { Text = category.Name, Value = (int?)category.CategoryId });
                    }
                }

                cmbCategory.DisplayMember = "Text";
                cmbCategory.ValueMember = "Value";
                cmbCategory.SelectedIndex = 0; // Mặc định không chọn
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải danh sách danh mục");
            }
        }

        private void AddProductForm_Load(object sender, EventArgs e)
        {
            // Focus vào ô tên sản phẩm
            txtSKU.Focus();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = openFileDialog.FileName;
                    try
                    {
                        pictureBoxProduct.Image = Image.FromFile(_selectedImagePath);
                        pictureBoxProduct.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        UIHelper.ShowErrorMessage($"Không thể tải hình ảnh: {ex.Message}");
                        _selectedImagePath = null;
                    }
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            // Xác nhận xóa ảnh
            if (pictureBoxProduct.Image != null || !string.IsNullOrEmpty(_currentImagePath))
            {
                var confirmResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa ảnh này không?",
                    "Xác nhận xóa ảnh",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult != DialogResult.Yes)
                    return;
            }

            // QUAN TRỌNG: Dispose image từ PictureBox TRƯỚC khi xóa file
            // Điều này giải phóng lock trên file
            if (pictureBoxProduct.Image != null)
            {
                var oldImage = pictureBoxProduct.Image;
                pictureBoxProduct.Image = null; // Set null trước
                oldImage.Dispose(); // Dispose để giải phóng file handle
            }

            // Đợi một chút để đảm bảo file được giải phóng hoàn toàn
            System.Threading.Thread.Sleep(100);
            Application.DoEvents(); // Process pending events

            // Xóa ảnh hiện tại trong Resources nếu có (sử dụng helper method)
            if (!string.IsNullOrEmpty(_currentImagePath) && File.Exists(_currentImagePath))
            {
                DeleteImageFileWithRetry(_currentImagePath);
            }

            // Reset các biến
            _selectedImagePath = null;
            _currentImagePath = null;
            
            // Đánh dấu đã xóa ảnh (để khi lưu sẽ không copy ảnh mới)
            _imageDeleted = true;
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            // Validate Tên sản phẩm (bắt buộc)

            if (string.IsNullOrWhiteSpace(txtSKU.Text))
            {
                ShowError("Vui lòng nhập mã SKU sản phẩm");
                txtSKU.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowError("Vui lòng nhập tên sản phẩm.");
                txtName.Focus();
                return false;
            }

            

            if (txtName.Text.Length > 255)
            {
                ShowError("Tên sản phẩm không được vượt quá 255 ký tự.");
                txtName.Focus();
                return false;
            }

            // Validate Đơn vị tính (bắt buộc)
            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                ShowError("Vui lòng nhập đơn vị tính.");
                txtUnit.Focus();
                return false;
            }

            // Validate Giá bán (bắt buộc, > 0)
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                ShowError("Vui lòng nhập giá bán.");
                txtPrice.Focus();
                return false;
            }

            // Parse giá bán (hỗ trợ định dạng Việt Nam)
            string priceText = txtPrice.Text.Trim().Replace(" ", "");
            decimal price;
            
            // Xử lý tương tự như trong SaveProduct
            if (priceText.Contains(".") && priceText.Contains(","))
            {
                priceText = priceText.Replace(".", "").Replace(",", ".");
            }
            else if (priceText.Contains("."))
            {
                var parts = priceText.Split('.');
                if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length == 3))
                {
                    priceText = priceText.Replace(".", "");
                }
            }
            else if (priceText.Contains(","))
            {
                var parts = priceText.Split(',');
                if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length == 3))
                {
                    priceText = priceText.Replace(",", "");
                }
                else
                {
                    priceText = priceText.Replace(",", ".");
                }
            }
            
            if (!decimal.TryParse(priceText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out price) || price <= 0)
            {
                ShowError("Giá bán phải là số lớn hơn 0.");
                txtPrice.Focus();
                return false;
            }

            // Kiểm tra giá tối đa: DECIMAL(10,2) = tối đa 99,999,999.99
            if (price > 99999999.99m)
            {
                ShowError("Giá bán không được vượt quá 99,999,999.99");
                txtPrice.Focus();
                return false;
            }

            // Validate Ngưỡng tồn kho (nếu có nhập) - xử lý tương tự như giá
            if (!string.IsNullOrWhiteSpace(txtMinStockLevel.Text))
            {
                string minStockText = txtMinStockLevel.Text.Trim().Replace(" ", "");
                decimal minStock;
                
                // Xử lý định dạng tương tự giá
                if (minStockText.Contains(".") && minStockText.Contains(","))
                {
                    minStockText = minStockText.Replace(".", "").Replace(",", ".");
                }
                else if (minStockText.Contains("."))
                {
                    var parts = minStockText.Split('.');
                    if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length == 3))
                    {
                        minStockText = minStockText.Replace(".", "");
                    }
                }
                else if (minStockText.Contains(","))
                {
                    var parts = minStockText.Split(',');
                    if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length == 3))
                    {
                        minStockText = minStockText.Replace(",", "");
                    }
                    else
                    {
                        minStockText = minStockText.Replace(",", ".");
                    }
                }
                
                if (!decimal.TryParse(minStockText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out minStock) || minStock < 0)
                {
                    ShowError("Ngưỡng tồn kho tối thiểu phải là số lớn hơn hoặc bằng 0.");
                    txtMinStockLevel.Focus();
                    return false;
                }

                // Kiểm tra min stock level tối đa: DECIMAL(10,2) = tối đa 99,999,999.99
                if (minStock > 99999999.99m)
                {
                    ShowError("Ngưỡng tồn kho tối thiểu không được vượt quá 99,999,999.99");
                    txtMinStockLevel.Focus();
                    return false;
                }
            }

            return true;
        }

        private void ShowError(string message)
        {
            labelError.Text = message;
            labelError.Visible = true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            await SaveProduct(false);
        }

        private async Task SaveProduct(bool saveAndNew)
        {
            if (!ValidateForm())
                return;

            // Kiểm tra SKU trùng (chỉ khi Add mode hoặc SKU thay đổi)
            if (!string.IsNullOrWhiteSpace(txtSKU.Text))
            {
                var skuCheckResult = await AppServices.ProductService.IsSkuExistsAsync(
                    txtSKU.Text.Trim(),
                    _isEditMode ? _productId : null
                );
                if (skuCheckResult.Success && skuCheckResult.Data)
                {
                    ShowError($"Mã SKU '{txtSKU.Text.Trim()}' đã tồn tại trong hệ thống.");
                    txtSKU.Focus();
                    return;
                }
            }

            // Disable controls during save
            SetControlsEnabled(false);
            btnSave.Text = _isEditMode ? "Đang cập nhật..." : "Đang lưu...";

            try
            {
                // Tạo Product entity
                // Xử lý Image: chỉ lưu tên file hoặc null, không lưu đường dẫn đầy đủ
                string imageFileName = null;
                if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                {
                    // Chỉ lưu tên file (không lưu đường dẫn đầy đủ)
                    imageFileName = Path.GetFileName(_selectedImagePath);
                    // Giới hạn độ dài để phù hợp với VARCHAR(255)
                    if (imageFileName.Length > 255)
                        imageFileName = imageFileName.Substring(0, 255);
                }

                // Parse giá bán (hỗ trợ định dạng Việt Nam: dấu chấm là phân cách hàng nghìn, dấu phẩy là phân cách thập phân)
                string priceText = txtPrice.Text.Trim();
                // Loại bỏ khoảng trắng
                priceText = priceText.Replace(" ", "");
                
                // Xử lý định dạng Việt Nam: 111.111 hoặc 111,111 hoặc 111.111,5
                // Nếu có cả dấu chấm và dấu phẩy -> dấu chấm là hàng nghìn, dấu phẩy là thập phân
                // Nếu chỉ có dấu chấm -> có thể là hàng nghìn (111.111) hoặc thập phân (111.5)
                // Nếu chỉ có dấu phẩy -> có thể là hàng nghìn (111,111) hoặc thập phân (111,5)
                
                decimal price;
                if (priceText.Contains(".") && priceText.Contains(","))
                {
                    // Có cả 2: dấu chấm = hàng nghìn, dấu phẩy = thập phân
                    // Ví dụ: 111.111,5 -> 111111.5
                    priceText = priceText.Replace(".", "").Replace(",", ".");
                }
                else if (priceText.Contains("."))
                {
                    // Chỉ có dấu chấm: kiểm tra xem là hàng nghìn hay thập phân
                    // Nếu có nhiều hơn 1 dấu chấm -> hàng nghìn (111.111.111)
                    // Nếu chỉ có 1 dấu chấm và có 3 chữ số sau -> hàng nghìn (111.111)
                    // Ngược lại -> thập phân (111.5)
                    var parts = priceText.Split('.');
                    if (parts.Length > 2)
                    {
                        // Nhiều dấu chấm -> hàng nghìn: 111.111.111
                        priceText = priceText.Replace(".", "");
                    }
                    else if (parts.Length == 2 && parts[1].Length == 3)
                    {
                        // 1 dấu chấm, 3 chữ số sau -> hàng nghìn: 111.111
                        priceText = priceText.Replace(".", "");
                    }
                    // Ngược lại giữ nguyên (thập phân): 111.5
                }
                else if (priceText.Contains(","))
                {
                    // Chỉ có dấu phẩy: thường là thập phân trong định dạng VN
                    // Nhưng cũng có thể là hàng nghìn nếu có nhiều dấu phẩy
                    var parts = priceText.Split(',');
                    if (parts.Length > 2)
                    {
                        // Nhiều dấu phẩy -> hàng nghìn: 111,111,111
                        priceText = priceText.Replace(",", "");
                    }
                    else if (parts.Length == 2 && parts[1].Length == 3)
                    {
                        // 1 dấu phẩy, 3 chữ số sau -> hàng nghìn: 111,111
                        priceText = priceText.Replace(",", "");
                    }
                    else
                    {
                        // Thập phân: 111,5 -> 111.5
                        priceText = priceText.Replace(",", ".");
                    }
                }
                
                if (!decimal.TryParse(priceText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out price))
                {
                    ShowError("Giá bán không hợp lệ. Vui lòng kiểm tra lại.");
                    return;
                }

                // Kiểm tra giá tối đa: DECIMAL(10,2) = tối đa 99,999,999.99
                if (price > 99999999.99m)
                {
                    ShowError("Giá bán không được vượt quá 99,999,999.99");
                    txtPrice.Focus();
                    return;
                }

                // Parse min stock level (xử lý tương tự như giá)
                decimal minStockLevel = 15; // Default
                if (!string.IsNullOrWhiteSpace(txtMinStockLevel.Text))
                {
                    string minStockText = txtMinStockLevel.Text.Trim().Replace(" ", "");
                    
                    // Xử lý định dạng tương tự giá
                    if (minStockText.Contains(".") && minStockText.Contains(","))
                    {
                        minStockText = minStockText.Replace(".", "").Replace(",", ".");
                    }
                    else if (minStockText.Contains("."))
                    {
                        var parts = minStockText.Split('.');
                        if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length == 3))
                        {
                            minStockText = minStockText.Replace(".", "");
                        }
                    }
                    else if (minStockText.Contains(","))
                    {
                        var parts = minStockText.Split(',');
                        if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length == 3))
                        {
                            minStockText = minStockText.Replace(",", "");
                        }
                        else
                        {
                            minStockText = minStockText.Replace(",", ".");
                        }
                    }
                    
                    if (!decimal.TryParse(minStockText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out minStockLevel))
                    {
                        ShowError("Ngưỡng tồn kho tối thiểu không hợp lệ. Vui lòng kiểm tra lại.");
                        return;
                    }

                    // Kiểm tra min stock level tối đa: DECIMAL(10,2) = tối đa 99,999,999.99
                    if (minStockLevel > 99999999.99m)
                    {
                        ShowError("Ngưỡng tồn kho tối thiểu không được vượt quá 99,999,999.99");
                        txtMinStockLevel.Focus();
                        return;
                    }
                }

                // Lấy ProductType từ combobox - đảm bảo luôn có giá trị hợp lệ
                ProductType productType = ProductType.PACKED; // Default value
                if (cmbProductType.SelectedItem != null)
                {
                    try
                    {
                        var selectedValue = ((dynamic)cmbProductType.SelectedItem).Value;
                        if (selectedValue is ProductType)
                        {
                            productType = (ProductType)selectedValue;
                        }
                        else if (selectedValue != null)
                        {
                            // Thử parse nếu không phải ProductType trực tiếp
                            if (Enum.TryParse<ProductType>(selectedValue.ToString(), true, out var parsedType))
                            {
                                productType = parsedType;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error getting ProductType from combobox: {ex.Message}");
                        // Sử dụng default value (PACKED)
                    }
                }
                else if (cmbProductType.SelectedIndex >= 0 && cmbProductType.SelectedIndex < cmbProductType.Items.Count)
                {
                    // Nếu SelectedItem null nhưng có SelectedIndex, lấy từ Items
                    try
                    {
                        var item = (dynamic)cmbProductType.Items[cmbProductType.SelectedIndex];
                        if (item.Value is ProductType)
                        {
                            productType = (ProductType)item.Value;
                        }
                    }
                    catch
                    {
                        // Sử dụng default
                    }
                }

                var product = new Product
                {
                    Sku = string.IsNullOrWhiteSpace(txtSKU.Text) ? null : txtSKU.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Image = imageFileName, // Chỉ lưu tên file, không lưu đường dẫn đầy đủ
                    ProductType = productType, // Đảm bảo luôn có giá trị hợp lệ
                    Unit = txtUnit.Text.Trim(),
                    Price = price,
                    MinStockLevel = minStockLevel,
                    CategoryId = cmbCategory.SelectedItem != null && ((dynamic)cmbCategory.SelectedItem).Value != null
                        ? ((dynamic)cmbCategory.SelectedItem).Value
                        : null,
                    IsActive = chkIsActive.Checked ? ActiveStatus.ACTIVE : ActiveStatus.INACTIVE
                };

                // Debug log để kiểm tra
                System.Diagnostics.Debug.WriteLine($"Saving product - ProductType: {product.ProductType} ({product.ProductType.ToString()})");

                // Set ProductId nếu là Edit mode
                if (_isEditMode)
                {
                    product.ProductId = _productId.Value;
                }
                else
                {
                    product.CreatedDate = DateTime.Now;
                }

                // Validate product
                var validateResult = await AppServices.ProductService.ValidateProductAsync(product);
                if (!validateResult.Success)
                {
                    ShowError(validateResult.Message ?? "Dữ liệu sản phẩm không hợp lệ.");
                return;
            }

                // Lưu hoặc cập nhật sản phẩm
                bool success = false;
                string message = "";

                if (_isEditMode)
                {
                    // Update mode
                    var updateResult = await AppServices.ProductService.UpdateProductAsync(product);
                    success = updateResult.Success;
                    message = updateResult.Message ?? "Cập nhật sản phẩm thành công!";
                }
                else
                {
                    // Create mode
                    var createResult = await AppServices.ProductService.CreateProductAsync(product);
                    success = createResult.Success;
                    message = createResult.Message ?? "Thêm sản phẩm thành công!";
                    
                    if (success)
                    {
                        _productId = createResult.Data; // Lưu ProductId mới tạo
                    }
                }
                
                if (success)
                {
                    // Xử lý hình ảnh
                    if (_imageDeleted && _isEditMode)
                    {
                        // Nếu đã xóa ảnh trong Edit mode, đảm bảo file đã bị xóa
                        // (đã xóa trong btnRemoveImage_Click)
                    }
                    else if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                    {
                        // Lưu hình ảnh mới vào Resources/Products
                        await SaveProductImage(_productId.Value, product.Sku);
                    }

                    UIHelper.ShowSuccessMessage(message);

                    if (saveAndNew && !_isEditMode)
                    {
                        // Reset form để thêm mới (chỉ khi Add mode)
                        ResetForm();
                        _productId = null; // Reset về Add mode
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    ShowError(message ?? (_isEditMode ? "Không thể cập nhật sản phẩm. Vui lòng thử lại." : "Không thể thêm sản phẩm. Vui lòng thử lại."));
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, _isEditMode ? "cập nhật sản phẩm" : "thêm sản phẩm");
                ShowError($"Đã xảy ra lỗi khi {(_isEditMode ? "cập nhật" : "thêm")} sản phẩm. Vui lòng thử lại.");
            }
            finally
            {
                SetControlsEnabled(true);
                btnSave.Text = _isEditMode ? "Cập nhật" : "Lưu";
            }
        }

        private async Task SaveProductImage(int productId, string sku)
        {
            try
            {
                // Tạo thư mục Resources/Products nếu chưa có
                string imageDirectory = Path.Combine(Application.StartupPath, "Resources", "Products");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                // Xác định tên file (ưu tiên dùng SKU, nếu không có thì dùng productId)
                string fileName = !string.IsNullOrEmpty(sku) ? sku : $"product_{productId}";
                string extension = Path.GetExtension(_selectedImagePath);
                string targetPath = Path.Combine(imageDirectory, $"{fileName}{extension}");

                // Xóa ảnh cũ nếu có (dispose image trước nếu đang được hiển thị)
                if (!string.IsNullOrEmpty(_currentImagePath) && File.Exists(_currentImagePath) && _currentImagePath != targetPath)
                {
                    // Dispose image từ PictureBox nếu đang hiển thị file này
                    if (pictureBoxProduct.Image != null)
                    {
                        var oldImage = pictureBoxProduct.Image;
                        pictureBoxProduct.Image = null;
                        oldImage.Dispose();
                        System.Threading.Thread.Sleep(50);
                    }
                    
                    // Xóa file với retry logic
                    DeleteImageFileWithRetry(_currentImagePath);
                }

                // Copy ảnh mới vào thư mục Resources
                File.Copy(_selectedImagePath, targetPath, true);
                _currentImagePath = targetPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving product image: {ex.Message}");
                // Không throw exception để không làm gián đoạn việc lưu sản phẩm
            }
        }

        private void ResetForm()
        {
            txtSKU.Text = "";
            txtName.Text = "";
            txtUnit.Text = "";
            txtPrice.Text = "";
            txtMinStockLevel.Text = "15";
            cmbProductType.SelectedIndex = 0;
            cmbCategory.SelectedIndex = 0;
            chkIsActive.Checked = true;
            pictureBoxProduct.Image = null;
            _selectedImagePath = null;
            _currentImagePath = null;
            _imageDeleted = false;
            labelError.Text = "";
            labelError.Visible = false;
            txtName.Focus();
        }

        /// <summary>
        /// Xóa file ảnh với retry logic để tránh lỗi file đang được sử dụng
        /// </summary>
        private void DeleteImageFileWithRetry(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return;

            int retryCount = 3;
            bool deleted = false;

            for (int i = 0; i < retryCount && !deleted; i++)
            {
                try
                {
                    File.Delete(filePath);
                    deleted = true;
                }
                catch (IOException ex) when (i < retryCount - 1)
                {
                    // File đang được sử dụng, đợi và thử lại
                    System.Threading.Thread.Sleep(200);
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    // Lỗi khác, log và dừng
                    System.Diagnostics.Debug.WriteLine($"Error deleting image file: {ex.Message}");
                    break;
                }
            }

            if (!deleted)
            {
                System.Diagnostics.Debug.WriteLine($"Could not delete image file after {retryCount} attempts: {filePath}");
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtSKU.Enabled = enabled;
            txtName.Enabled = enabled;
            cmbProductType.Enabled = enabled;
            txtUnit.Enabled = enabled;
            txtPrice.Enabled = enabled;
            txtMinStockLevel.Enabled = enabled;
            cmbCategory.Enabled = enabled;
            chkIsActive.Enabled = enabled;
            btnSelectImage.Enabled = enabled;
            btnRemoveImage.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnDelete.Enabled = enabled;
            btnCancel.Enabled = enabled;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_isEditMode || !_productId.HasValue)
                return;

            // Xác nhận xóa
            var confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sản phẩm này không?\n\nLưu ý: Sản phẩm sẽ bị vô hiệu hóa (không hiển thị trong danh sách).",
                "Xác nhận xóa sản phẩm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult != DialogResult.Yes)
                return;

            // Disable controls during delete
            SetControlsEnabled(false);
            btnDelete.Text = "Đang xóa...";

            try
            {
                // Xóa sản phẩm (soft delete)
                var deleteResult = await AppServices.ProductService.DeleteProductAsync(_productId.Value);

                if (deleteResult.Success)
                {
                    // Xóa ảnh trong Resources nếu có (dispose image trước)
                    if (!string.IsNullOrEmpty(_currentImagePath) && File.Exists(_currentImagePath))
                    {
                        // Dispose image từ PictureBox trước
                        if (pictureBoxProduct.Image != null)
                        {
                            var oldImage = pictureBoxProduct.Image;
                            pictureBoxProduct.Image = null;
                            oldImage.Dispose();
                            System.Threading.Thread.Sleep(50);
                        }
                        
                        // Xóa file với retry logic
                        DeleteImageFileWithRetry(_currentImagePath);
                    }

                    UIHelper.ShowSuccessMessage(deleteResult.Message ?? "Xóa sản phẩm thành công!");
            this.DialogResult = DialogResult.OK;
            this.Close();
                }
                else
                {
                    UIHelper.ShowErrorMessage(deleteResult.Message ?? "Không thể xóa sản phẩm. Vui lòng thử lại.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "xóa sản phẩm");
                UIHelper.ShowErrorMessage("Đã xảy ra lỗi khi xóa sản phẩm. Vui lòng thử lại.");
            }
            finally
            {
                SetControlsEnabled(true);
                btnDelete.Text = "Xóa sản phẩm";
            }
        }

        private void chkIsActive_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle active/inactive status
            // Checked = ACTIVE, Unchecked = INACTIVE
            // Logic này đã được xử lý trong SaveProduct method
            // Chỉ cần đảm bảo checkbox hoạt động đúng
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
