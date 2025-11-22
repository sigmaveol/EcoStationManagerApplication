# UI Services Documentation

## Tổng quan

Các UI Services được thiết kế để quản lý các chức năng chung của ứng dụng một cách nhất quán và dễ bảo trì.

## 1. NavigationService

Quản lý điều hướng giữa các UserControl trong ứng dụng.

### Khởi tạo
```csharp
// Trong MainForm_Load hoặc constructor
AppServices.Navigation.Initialize(contentPanel);
```

### Sử dụng
```csharp
// Điều hướng đến một view
AppServices.Navigation.NavigateTo("dashboard");
AppServices.Navigation.NavigateTo("orders");

// Lấy view hiện tại
string currentView = AppServices.Navigation.CurrentView;

// Xóa cache của một view
AppServices.Navigation.ClearCache("dashboard");

// Xóa tất cả cache
AppServices.Navigation.ClearCache();

// Đăng ký event khi view thay đổi
AppServices.Navigation.OnViewChanged += (sender, e) =>
{
    Console.WriteLine($"View changed to: {e.ViewName}");
};
```

## 2. DialogService

Quản lý hiển thị các dialog và message box một cách nhất quán.

### Khởi tạo
```csharp
// Trong MainForm_Load hoặc constructor
AppServices.Dialog.Initialize(this); // this là MainForm
```

### Sử dụng
```csharp
// Hiển thị message box thông thường
AppServices.Dialog.ShowMessage("Thông báo", "Tiêu đề", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);

// Hiển thị dialog xác nhận
if (AppServices.Dialog.ShowConfirm("Bạn có chắc chắn?", "Xác nhận"))
{
    // Xử lý khi user chọn Yes
}

// Hiển thị các loại message phổ biến
AppServices.Dialog.ShowSuccess("Đã lưu thành công!");
AppServices.Dialog.ShowError("Đã xảy ra lỗi!");
AppServices.Dialog.ShowWarning("Cảnh báo!");

// Hiển thị exception
try
{
    // Code có thể throw exception
}
catch (Exception ex)
{
    AppServices.Dialog.ShowException(ex, "thực hiện thao tác");
}

// Hiển thị dialog tùy chỉnh
var result = AppServices.Dialog.ShowDialog(new MyCustomForm());
```

## 3. AppStateManager

Quản lý trạng thái ứng dụng (user, theme, cache...).

### Sử dụng
```csharp
// Quản lý User
AppServices.State.CurrentUser = user;
var currentUser = AppServices.State.CurrentUser;
bool isAdmin = AppServices.State.IsAdmin;
bool isManager = AppServices.State.IsManager;

// Quản lý Theme
AppServices.State.CurrentTheme = "Dark";
string theme = AppServices.State.CurrentTheme;

// Quản lý State Cache
AppServices.State.SetState("LastSearchTerm", "keyword");
var term = AppServices.State.GetState<string>("LastSearchTerm");
bool exists = AppServices.State.HasState("LastSearchTerm");
AppServices.State.RemoveState("LastSearchTerm");
AppServices.State.ClearCache();

// Application State
AppServices.State.IsInitialized = true;
AppServices.State.IsLoading = true;

// Reset tất cả
AppServices.State.Reset();

// Đăng ký events
AppServices.State.OnUserChanged += (sender, e) => { /* ... */ };
AppServices.State.OnThemeChanged += (sender, e) => { /* ... */ };
```

## 4. BaseUserControl

Base class cho tất cả UserControl trong ứng dụng.

### Sử dụng
```csharp
public class MyControl : BaseUserControl
{
    protected override void InitializeControl()
    {
        base.InitializeControl();
        // Khởi tạo control của bạn
    }
    
    protected override async void LoadDataAsync()
    {
        ShowLoading(true);
        try
        {
            // Load data
            var result = await AppServices.MyService.GetDataAsync();
            // Bind data
        }
        catch (Exception ex)
        {
            HandleError(ex, "tải dữ liệu");
        }
        finally
        {
            ShowLoading(false);
        }
    }
    
    protected override bool ValidateData()
    {
        // Validate logic
        return true;
    }
    
    protected override void Cleanup()
    {
        // Cleanup resources nếu cần
        base.Cleanup();
    }
}
```

## Best Practices

1. **Luôn khởi tạo services trong MainForm**: Đảm bảo NavigationService và DialogService được khởi tạo trước khi sử dụng.

2. **Sử dụng DialogService thay vì MessageBox trực tiếp**: Đảm bảo nhất quán và dễ bảo trì.

3. **Sử dụng AppStateManager để lưu trữ state**: Thay vì dùng biến static hoặc global.

4. **Kế thừa từ BaseUserControl**: Để có các tính năng chung như loading state, error handling.

5. **Sử dụng NavigationService**: Thay vì tự quản lý navigation trong MainForm.

