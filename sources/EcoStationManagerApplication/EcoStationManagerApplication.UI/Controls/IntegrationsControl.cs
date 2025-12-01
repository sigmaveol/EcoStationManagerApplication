using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            MessageBox.Show("Cập nhật cài đặt Google Forms", "Cập nhật");
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
            try
            {
                var server = txtGmailServer?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(server))
                {
                    MessageBox.Show("Vui lòng nhập IMAP Server (ví dụ: imap.gmail.com)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(server, 993);
                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(8));
                    var finished = await Task.WhenAny(connectTask, timeoutTask);
                    if (finished == timeoutTask || !client.Connected)
                    {
                        MessageBox.Show("Không thể kết nối đến IMAP server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show("Kết nối IMAP thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConnectGmail_Click(object sender, EventArgs e)
        {
            var connected = string.Equals(statusLabelGmail?.Text, "Đã kết nối", StringComparison.OrdinalIgnoreCase);
            if (!connected)
            {
                statusLabelGmail.Text = "Đã kết nối";
                statusLabelGmail.ForeColor = System.Drawing.Color.Green;
                btnConnectGmail.Text = "Ngắt kết nối";
            }
            else
            {
                statusLabelGmail.Text = "Chưa kết nối";
                statusLabelGmail.ForeColor = System.Drawing.Color.Red;
                btnConnectGmail.Text = "Kết nối";
            }
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
        }

        private async Task TestGoogleSheetsConnectionAsync()
        {
            try
            {
                var url = txtGoogleSheetsUrl?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(url))
                {
                    MessageBox.Show("Vui lòng nhập URL Google Sheets", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    return "https://docs.google.com/spreadsheets/d/" + id + "/export?format=csv&gid=" + gid;
                }
            }
            catch { }
            return inputUrl;
        }

        private void googleSheetsCard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}