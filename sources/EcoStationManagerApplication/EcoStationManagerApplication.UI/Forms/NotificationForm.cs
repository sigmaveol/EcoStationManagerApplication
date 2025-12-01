using EcoStationManagerApplication.Core.Composition;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.UI.Common;
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

        public NotificationForm()
        {
            InitializeComponent();
            _inventoryService = AppServices.InventoryService;
            LoadAlerts();
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
                        var productName = string.IsNullOrWhiteSpace(inventory.ProductName) ? (inventory.Product?.Name ?? "N/A") : inventory.ProductName;
                        var currentStock = inventory.Quantity;
                        var minStock = inventory.MinStockLevel > 0 ? inventory.MinStockLevel : (inventory.Product?.MinStockLevel ?? 0);
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

        public static void ShowLowStockAlerts()
        {
            var form = new NotificationForm();
            var parent = Form.ActiveForm;
            if (parent != null)
            {
                FormHelper.ShowModalWithDim(parent, form);
            }
            else
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAlerts();
        }
    }
}