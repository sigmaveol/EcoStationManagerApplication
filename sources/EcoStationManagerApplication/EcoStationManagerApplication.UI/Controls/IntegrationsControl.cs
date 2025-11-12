using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class IntegrationsControl : UserControl
    {
        public IntegrationsControl()
        {
            InitializeComponent();

            PopulateIntegrationsPanel();

            if (btnUpdateGoogleForms != null)
                btnUpdateGoogleForms.Click += btnUpdateGoogleForms_Click;

            if (btnConnectGoogleSheets != null)
                btnConnectGoogleSheets.Click += btnConnectGoogleSheets_Click;
        }

        // --- HÀM XỬ LÝ SỰ KIỆN ---

        private void btnUpdateGoogleForms_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cập nhật cài đặt Google Forms", "Cập nhật");
        }

        private void btnConnectGoogleSheets_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kết nối Google Sheets", "Kết nối");
        }

        // --- HÀM ĐỔ DỮ LIỆU (LOGIC) ---

        // Hàm này chứa logic (if/foreach...)
        private void PopulateIntegrationsPanel()
        {
            if (integrationsPanel == null) return;

            var googleFormsCard = CreateIntegrationCard(
                "Google Forms",
                "Thu thập đơn hàng từ khách hàng qua Google Forms",
                "https://forms.google.com/...",
                "Đã kết nối",
                true
            );
            googleFormsCard.Size = new Size(430, 300);
            googleFormsCard.Margin = new Padding(10);

            var googleSheetsCard = CreateIntegrationCard(
                "Google Sheets",
                "Đồng bộ dữ liệu đặt hàng từ Google Sheets",
                "https://docs.google.com/spreadsheets/...",
                "Chưa kết nối",
                false
            );
            googleSheetsCard.Size = new Size(430, 300);
            googleSheetsCard.Margin = new Padding(10);

            integrationsPanel.Controls.Add(googleFormsCard);
            integrationsPanel.Controls.Add(googleSheetsCard);
        }

        private Panel CreateIntegrationCard(string title, string description, string urlPlaceholder, string status, bool isConnected)
        {
            var card = new Panel();
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.None;
            card.Padding = new Padding(15);

            var titleLabel = new Label();
            titleLabel.Text = title;
            titleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(15, 15);

            var descLabel = new Label();
            descLabel.Text = description;
            descLabel.Font = new Font("Segoe UI", 9);
            descLabel.AutoSize = false;
            descLabel.Size = new Size(380, 40); 
            descLabel.Location = new Point(15, 45);

            var lblUrl = new Label();
            lblUrl.Text = title + " URL";
            lblUrl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblUrl.AutoSize = true;
            lblUrl.Location = new Point(15, 95);

            var txtUrl = new TextBox();
            txtUrl.Size = new Size(380, 25);
            txtUrl.Location = new Point(15, 120);
            txtUrl.Text = urlPlaceholder;

            var lblApiKey = new Label();
            lblApiKey.Text = "API Key (Nếu có)";
            lblApiKey.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblApiKey.AutoSize = true;
            lblApiKey.Location = new Point(15, 155);
            lblApiKey.Visible = (title == "Google Sheets");

            var txtApiKey = new TextBox();
            txtApiKey.Size = new Size(380, 25);
            txtApiKey.Location = new Point(15, 180);
            txtApiKey.UseSystemPasswordChar = true;
            txtApiKey.Visible = (title == "Google Sheets");

            var lblStatus = new Label();
            lblStatus.Text = "Trạng thái:";
            lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(15, 230);

            var statusLabel = new Label();
            statusLabel.Text = status;
            statusLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(90, 230);
            statusLabel.ForeColor = isConnected ? Color.Green : Color.Red;

            Button actionButton = new Button();
            actionButton.Size = new Size(120, 35);
            actionButton.Location = new Point(275, 225);
            actionButton.BackColor = isConnected ? Color.FromArgb(25, 118, 210) : Color.FromArgb(46, 125, 50);
            actionButton.ForeColor = Color.White;
            actionButton.FlatStyle = FlatStyle.Flat;
            actionButton.FlatAppearance.BorderSize = 0;

            if (title == "Google Forms")
            {
                actionButton.Text = "Cập nhật";
                btnUpdateGoogleForms = actionButton;
                actionButton.Click += btnUpdateGoogleForms_Click; 
            }
            else
            {
                actionButton.Text = isConnected ? "Ngắt kết nối" : "Kết nối";
                btnConnectGoogleSheets = actionButton;
                actionButton.Click += btnConnectGoogleSheets_Click;
            }

            card.Controls.Add(titleLabel);
            card.Controls.Add(descLabel);
            card.Controls.Add(lblUrl);
            card.Controls.Add(txtUrl);
            card.Controls.Add(lblApiKey);
            card.Controls.Add(txtApiKey);
            card.Controls.Add(lblStatus);
            card.Controls.Add(statusLabel);
            card.Controls.Add(actionButton);

            return card;
        }
    }
}