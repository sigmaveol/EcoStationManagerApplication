using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using EcoStationManagerApplication.Core.Composition;
using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class IntegrationsControl : UserControl
    {
        private bool isGoogleSheetsConnected;

        public IntegrationsControl()
        {
            InitializeComponent();
            InitializeBusinessLogic();
        }

        private void InitializeBusinessLogic()
        {
            InitializeEventHandlers();
            InitializeTimer();
            if (googleFormsStatusValue != null)
            {
                googleFormsStatusValue.Text = "Sắp ra mắt";
                googleFormsStatusValue.ForeColor = Color.Gray;
            }
            if (statusLabelGmail != null)
            {
                statusLabelGmail.Text = "Sắp ra mắt";
                statusLabelGmail.ForeColor = Color.Gray;
            }
            _ = LoadSavedGoogleSheetsConfigAsync();
        }

        private void InitializeEventHandlers()
        {
            btnUpdateGoogleForms.Click += btnUpdateGoogleForms_Click;
            btnConnectGoogleSheets.Click += btnConnectGoogleSheets_Click;
            btnTestGoogleSheets.Click += btnTestGoogleSheets_Click;
            cmbGoogleSheetsInterval.SelectedIndexChanged += cmbGoogleSheetsInterval_SelectedIndexChanged;
            btnTestGmail.Click += btnTestGmail_Click;
            btnConnectGmail.Click += btnConnectGmail_Click;
        }

        private void InitializeTimer()
        {
            googleSheetsTimer.Tick += async (s, e) => await SyncGoogleSheetsAsync();
            if (cmbGoogleSheetsInterval != null && int.TryParse(cmbGoogleSheetsInterval.SelectedItem?.ToString(), out var minutes))
            {
                googleSheetsTimer.Interval = minutes * 60 * 1000;
            }
            else
            {
                googleSheetsTimer.Interval = 5 * 60 * 1000;
            }
        }

        private void cmbGoogleSheetsInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cmbGoogleSheetsInterval.SelectedItem?.ToString(), out var minutes))
            {
                googleSheetsTimer.Interval = minutes * 60 * 1000;
            }
        }

        // --- EVENT HANDLERS ---
        private void btnUpdateGoogleForms_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng Google Forms sẽ được phát triển ở phiên bản sắp tới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private async void btnConnectGoogleSheets_Click(object sender, EventArgs e)
        {
            if (!isGoogleSheetsConnected)
            {
                await ConnectGoogleSheetsAsync();
            }
            else
            {
                DisconnectGoogleSheets();
            }
        }

        private async void btnTestGoogleSheets_Click(object sender, EventArgs e)
        {
            await TestGoogleSheetsConnectionAsync();
        }

        private async void btnTestGmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng Gmail sẽ được phát triển ở phiên bản sắp tới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void btnConnectGmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng Gmail sẽ được phát triển ở phiên bản sắp tới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // --- BUSINESS LOGIC METHODS ---
        private async Task ConnectGoogleSheetsAsync()
        {
            var url = txtGoogleSheetsUrl?.Text?.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Vui lòng nhập URL Google Sheets", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!url.Contains("docs.google.com/spreadsheets/d/"))
            {
                MessageBox.Show("URL Google Sheets chưa hợp lệ. Hãy dán liên kết đầy đủ dạng https://docs.google.com/spreadsheets/d/{ID}/edit#gid={GID}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var online = await IsOnlineAsync();
            if (!online)
            {
                MessageBox.Show("Không có kết nối internet. Vui lòng kiểm tra mạng và thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var spreadsheetId = ExtractSpreadsheetId(url);
            var sheetName = ExtractSheetGid(url);
            var apiKey = txtGoogleSheetsApiKey?.Text?.Trim();
            var gi = ServiceRegistry.GoogleIntegrationService;
            _ = await gi.GetOrCreateActiveConfigAsync(url, spreadsheetId, apiKey, sheetName);

            isGoogleSheetsConnected = true;
            UpdateGoogleSheetsUI();
            await SyncGoogleSheetsAsync();
            googleSheetsTimer.Start();
        }

        private void DisconnectGoogleSheets()
        {
            isGoogleSheetsConnected = false;
            googleSheetsTimer.Stop();
            UpdateGoogleSheetsUI();
        }

        private void UpdateGoogleSheetsUI()
        {
            btnConnectGoogleSheets.Text = isGoogleSheetsConnected ? "Ngắt kết nối" : "Kết nối";
            statusLabelGoogleSheets.Text = isGoogleSheetsConnected ? "Đã kết nối" : "Chưa kết nối";
            statusLabelGoogleSheets.ForeColor = isGoogleSheetsConnected ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            if (txtGoogleSheetsUrl != null) txtGoogleSheetsUrl.Enabled = !isGoogleSheetsConnected;
            if (txtGoogleSheetsApiKey != null) txtGoogleSheetsApiKey.Enabled = !isGoogleSheetsConnected;
            if (!isGoogleSheetsConnected && lblLastSyncGoogleSheets != null && string.IsNullOrWhiteSpace(lblLastSyncGoogleSheets.Text))
                lblLastSyncGoogleSheets.Text = "-";
        }

        private async Task TestGoogleSheetsConnectionAsync()
        {
            try
            {
                var online = await IsOnlineAsync();
                if (!online)
                {
                    MessageBox.Show("Không có kết nối internet. Vui lòng kiểm tra mạng và thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var url = txtGoogleSheetsUrl?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(url))
                {
                    MessageBox.Show("Vui lòng nhập URL Google Sheets", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!url.Contains("docs.google.com/spreadsheets/d/"))
                {
                    MessageBox.Show("URL Google Sheets chưa hợp lệ. Hãy dán liên kết đầy đủ dạng https://docs.google.com/spreadsheets/d/{ID}/edit#gid={GID}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var csvUrl = BuildCsvUrl(url);
                using (var http = new HttpClient())
                {
                    var response = await http.GetAsync(csvUrl, HttpCompletionOption.ResponseHeadersRead);
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Kết nối thất bại hoặc URL không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
                    var okType = contentType.Contains("csv") || contentType.Contains("excel") || contentType.Contains("text");
                    var sample = await response.Content.ReadAsByteArrayAsync();
                    var okData = sample != null && sample.Length > 0;
                    var headerInfo = "";
                    if (okData)
                    {
                        var text = Encoding.UTF8.GetString(sample);
                        var firstLine = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                        if (!string.IsNullOrEmpty(firstLine))
                        {
                            var cols = firstLine.Split(',');
                            headerInfo = "\nCột: " + string.Join(", ", cols.Take(6)) + (cols.Length > 6 ? " ..." : "");
                        }
                    }

                    if (okType && okData)
                    {
                        MessageBox.Show("Kết nối thành công và truy cập được dữ liệu" + headerInfo, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Kết nối thành công nhưng dữ liệu không hợp lệ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SyncGoogleSheetsAsync()
        {
            try
            {
                var online = await IsOnlineAsync();
                if (!online)
                {
                    return;
                }
                var inputUrl = txtGoogleSheetsUrl?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(inputUrl)) return;

                var csvUrl = BuildCsvUrl(inputUrl);
                using (var http = new HttpClient())
                {
                    var response = await http.GetAsync(csvUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Không tải được dữ liệu từ Google Sheets", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var tempPath = Path.Combine(Path.GetTempPath(), "gsync_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
                    File.WriteAllBytes(tempPath, bytes);

                    var import = ServiceRegistry.ImportService;
                    var validate = await import.ValidateImportFileAsync(tempPath);
                    if (!validate.Success)
                    {
                        MessageBox.Show("File CSV không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var result = await import.ImportOrdersFromFileAsync(tempPath, OrderSource.EXCEL);
                    if (result.Success)
                    {
                        var spreadsheetId = ExtractSpreadsheetId(inputUrl);
                        var sheetName = ExtractSheetGid(inputUrl);
                        var apiKey = txtGoogleSheetsApiKey?.Text?.Trim();
                        var gi = ServiceRegistry.GoogleIntegrationService;
                        var cfgRes = await gi.GetOrCreateActiveConfigAsync(inputUrl, spreadsheetId, apiKey, sheetName);
                        if (cfgRes.Success)
                        {
                            var cfg = cfgRes.Data;
                            await gi.UpdateLastSyncAsync(cfg.IntegrationId, DateTime.Now);
                            if (result.Data?.CreatedOrderIds?.Count == result.Data?.CreatedRowIndexes?.Count)
                            {
                                var pairs = result.Data.CreatedOrderIds.Select((orderId, idx) => (result.Data.CreatedRowIndexes[idx], orderId)).ToList();
                                await gi.SaveOrderMappingsAsync(cfg.IntegrationId, pairs);
                            }
                        }

                        MessageBox.Show("Đồng bộ Google Sheets thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblLastSyncGoogleSheets.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    }
                    else
                    {
                        MessageBox.Show(result.Message ?? "Đồng bộ thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string BuildCsvUrl(string inputUrl)
        {
            try
            {
                var uri = new Uri(inputUrl);
                var path = uri.AbsolutePath;
                var idx = path.IndexOf("/d/");
                if (idx >= 0)
                {
                    var after = path.Substring(idx + 3);
                    var parts = after.Split('/');
                    var id = parts[0];
                    var gid = "0";
                    var frag = uri.Fragment;
                    if (!string.IsNullOrWhiteSpace(frag))
                    {
                        var key = "gid=";
                        var pos = frag.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                        if (pos >= 0)
                        {
                            var g = frag.Substring(pos + key.Length);
                            if (g.StartsWith("#")) g = g.Substring(1);
                            gid = g;
                        }
                    }
                    var query = uri.Query;
                    if (gid == "0" && !string.IsNullOrWhiteSpace(query))
                    {
                        var key = "gid=";
                        var qPos = query.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                        if (qPos >= 0)
                        {
                            var g = query.Substring(qPos + key.Length);
                            var amp = g.IndexOf('&');
                            if (amp >= 0) g = g.Substring(0, amp);
                            gid = g;
                        }
                    }
                    return "https://docs.google.com/spreadsheets/d/" + id + "/export?format=csv&gid=" + gid;
                }
            }
            catch { }
            return inputUrl;
        }

        private string ExtractSpreadsheetId(string inputUrl)
        {
            try
            {
                var uri = new Uri(inputUrl);
                var path = uri.AbsolutePath;
                var idx = path.IndexOf("/d/");
                if (idx >= 0)
                {
                    var after = path.Substring(idx + 3);
                    var parts = after.Split('/');
                    return parts[0];
                }
            }
            catch { }
            return string.Empty;
        }

        private string ExtractSheetGid(string inputUrl)
        {
            try
            {
                var uri = new Uri(inputUrl);
                var key = "gid=";
                var frag = uri.Fragment;
                if (!string.IsNullOrWhiteSpace(frag))
                {
                    var pos = frag.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                    if (pos >= 0)
                    {
                        var g = frag.Substring(pos + key.Length);
                        if (g.StartsWith("#")) g = g.Substring(1);
                        return "gid-" + g;
                    }
                }
                var query = uri.Query;
                if (!string.IsNullOrWhiteSpace(query))
                {
                    var qPos = query.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                    if (qPos >= 0)
                    {
                        var g = query.Substring(qPos + key.Length);
                        var amp = g.IndexOf('&');
                        if (amp >= 0) g = g.Substring(0, amp);
                        return "gid-" + g;
                    }
                }
            }
            catch { }
            return null;
        }

        private async Task<bool> IsOnlineAsync()
        {
            try
            {
                using (var http = new HttpClient { Timeout = TimeSpan.FromMilliseconds(2500) })
                {
                    var res1 = await http.GetAsync("https://docs.google.com");
                    if (res1.IsSuccessStatusCode) return true;
                    var res2 = await http.GetAsync("https://www.google.com");
                    return res2.IsSuccessStatusCode;
                }
            }
            catch { return false; }
        }

        private async Task LoadSavedGoogleSheetsConfigAsync()
        {
            try
            {
                var gi = ServiceRegistry.GoogleIntegrationService;
                var res = await gi.GetLatestActiveConfigAsync();
                if (res.Success && res.Data != null)
                {
                    if (txtGoogleSheetsUrl != null) txtGoogleSheetsUrl.Text = res.Data.SheetUrl ?? string.Empty;
                    if (txtGoogleSheetsApiKey != null) txtGoogleSheetsApiKey.Text = res.Data.ApiKey ?? string.Empty;
                    if (res.Data.LastSyncTime.HasValue && lblLastSyncGoogleSheets != null)
                        lblLastSyncGoogleSheets.Text = res.Data.LastSyncTime.Value.ToString("dd/MM/yyyy HH:mm");
                }
            }
            catch { }
        }

        private void googleSheetsCard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
