using EcoStationManagerApplication.Core.Composition;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class NotificationForm : Form
    {
        private readonly IInventoryService _inventoryService;
        private DataGridView dgvAlerts;
        private Label lblTitle;
        private Button btnRefresh;
        private Button btnClose;

        public NotificationForm()
        {
            InitializeComponent();
            _inventoryService = ServiceRegistry.InventoryService;
            InitializeCustomComponents();
            LoadAlerts();
        }

        private void InitializeCustomComponents()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Cảnh báo Tồn kho";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title label
            lblTitle = new Label
            {
                Text = "Cảnh báo Sản phẩm Sắp Hết",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(400, 30),
                AutoSize = false
            };

            // Refresh button
            btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new Point(600, 20),
                Size = new Size(80, 30),
                UseVisualStyleBackColor = true
            };
            btnRefresh.Click += BtnRefresh_Click;

            // Close button
            btnClose = new Button
            {
                Text = "Đóng",
                Location = new Point(690, 20),
                Size = new Size(80, 30),
                UseVisualStyleBackColor = true
            };
            btnClose.Click += (s, e) => this.Close();

            // DataGridView for alerts
            dgvAlerts = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(750, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // Add columns
            dgvAlerts.Columns.Add("ProductName", "Tên sản phẩm");
            dgvAlerts.Columns.Add("CurrentStock", "Tồn kho hiện tại");
            dgvAlerts.Columns.Add("MinStock", "Tồn kho tối thiểu");
            dgvAlerts.Columns.Add("AlertLevel", "Mức cảnh báo");

            // Format columns
            dgvAlerts.Columns["CurrentStock"].DefaultCellStyle.Format = "N2";
            dgvAlerts.Columns["MinStock"].DefaultCellStyle.Format = "N2";
            dgvAlerts.Columns["AlertLevel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(btnClose);
            this.Controls.Add(dgvAlerts);

            this.ResumeLayout(false);
        }

        private async void LoadAlerts()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                dgvAlerts.Rows.Clear();

                var result = await _inventoryService.GetLowStockItemsAsync();
                if (result.Success && result.Data != null && result.Data.Any())
                {
                    foreach (var inventory in result.Data)
                    {
                        var productName = inventory.Product?.Name ?? "N/A";
                        var currentStock = inventory.Quantity;
                        var minStock = inventory.Product?.MinStockLevel ?? 0;
                        var alertLevel = GetAlertLevel(currentStock, minStock);

                        var row = new DataGridViewRow();
                        row.CreateCells(dgvAlerts,
                            productName,
                            currentStock,
                            minStock,
                            alertLevel
                        );

                        // Color coding
                        if (alertLevel == "Nguy cấp")
                        {
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                        }
                        else if (alertLevel == "Cảnh báo")
                        {
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                        }

                        dgvAlerts.Rows.Add(row);
                    }

                    lblTitle.Text = $"Cảnh báo Sản phẩm Sắp Hết ({result.Data.Count} sản phẩm)";
                }
                else
                {
                    lblTitle.Text = "Không có cảnh báo tồn kho";
                    var row = new DataGridViewRow();
                    row.CreateCells(dgvAlerts, "Không có sản phẩm nào sắp hết", "", "", "");
                    dgvAlerts.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải cảnh báo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private string GetAlertLevel(decimal currentStock, decimal minStock)
        {
            if (minStock == 0) return "Bình thường";

            var percentage = (currentStock / minStock) * 100;

            if (percentage <= 50)
                return "Nguy cấp";
            else if (percentage <= 80)
                return "Cảnh báo";
            else
                return "Bình thường";
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadAlerts();
        }

        public static void ShowLowStockAlerts()
        {
            var form = new NotificationForm();
            form.ShowDialog();
        }
    }
}
