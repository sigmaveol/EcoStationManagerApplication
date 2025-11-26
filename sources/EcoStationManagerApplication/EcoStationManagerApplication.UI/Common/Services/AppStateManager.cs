using EcoStationManagerApplication.Common.Config;
using EcoStationManagerApplication.Models.DTOs;
using System;
using System.Collections.Generic;

namespace EcoStationManagerApplication.UI.Common.Services
{
    /// <summary>
    /// Quản lý trạng thái ứng dụng (user, theme, settings, cache...)
    /// </summary>
    public class AppStateManager
    {
        private static AppStateManager _instance;
        private Dictionary<string, object> _stateCache;
        private UserDTO _currentUser;
        private string _currentTheme;

        private AppStateManager()
        {
            _stateCache = new Dictionary<string, object>();
            LoadTheme();
        }

        public static AppStateManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppStateManager();
                return _instance;
            }
        }

        #region User Management

        /// <summary>
        /// User hiện tại đang đăng nhập
        /// </summary>
        public UserDTO CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnUserChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Kiểm tra user có quyền admin không
        /// </summary>
        public bool IsAdmin => CurrentUser?.Role == Models.Enums.UserRole.ADMIN;

        /// <summary>
        /// Kiểm tra user có quyền manager không
        /// </summary>
        public bool IsManager => CurrentUser?.Role == Models.Enums.UserRole.MANAGER;

        /// <summary>
        /// Event được gọi khi user thay đổi
        /// </summary>
        public event EventHandler OnUserChanged;

        #endregion

        #region Theme Management

        /// <summary>
        /// Theme hiện tại
        /// </summary>
        public string CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _currentTheme = value;
                SaveTheme();
                OnThemeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Load theme từ config
        /// </summary>
        private void LoadTheme()
        {
            try
            {
                var uiConfig = ConfigManager.GetUIConfig();
                _currentTheme = uiConfig?.GunaTheme ?? "Light";
            }
            catch
            {
                _currentTheme = "Light";
            }
        }

        /// <summary>
        /// Lưu theme vào config
        /// </summary>
        private void SaveTheme()
        {
            // TODO: Implement save theme to config
            // Có thể lưu vào Settings hoặc database
        }

        /// <summary>
        /// Event được gọi khi theme thay đổi
        /// </summary>
        public event EventHandler OnThemeChanged;

        #endregion

        #region State Cache

        /// <summary>
        /// Lưu giá trị vào cache
        /// </summary>
        public void SetState<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            _stateCache[key] = value;
        }

        /// <summary>
        /// Lấy giá trị từ cache
        /// </summary>
        public T GetState<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            if (_stateCache.TryGetValue(key, out var value) && value is T)
            {
                return (T)value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Xóa giá trị khỏi cache
        /// </summary>
        public void RemoveState(string key)
        {
            if (!string.IsNullOrEmpty(key))
                _stateCache.Remove(key);
        }

        /// <summary>
        /// Xóa tất cả cache
        /// </summary>
        public void ClearCache()
        {
            _stateCache.Clear();
        }

        /// <summary>
        /// Kiểm tra key có tồn tại trong cache không
        /// </summary>
        public bool HasState(string key)
        {
            return !string.IsNullOrEmpty(key) && _stateCache.ContainsKey(key);
        }

        #endregion

        #region Application State

        /// <summary>
        /// Trạng thái ứng dụng đã khởi tạo chưa
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Trạng thái đang tải dữ liệu
        /// </summary>
        public bool IsLoading { get; set; }

        /// <summary>
        /// Reset tất cả trạng thái
        /// </summary>
        public void Reset()
        {
            _currentUser = null;
            _stateCache.Clear();
            IsInitialized = false;
            IsLoading = false;
        }

        #endregion
    }
}

