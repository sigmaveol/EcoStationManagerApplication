namespace EcoStationManagerApplication.UI.Common
{
    /// <summary>
    /// Interface cho các control cần reload dữ liệu khi được hiển thị lại.
    /// </summary>
    public interface IRefreshableControl
    {
        void RefreshData();
    }
}

