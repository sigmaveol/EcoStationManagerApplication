using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class InventoryControl : UserControl
    {
        private List<Inventory> inventories;
        private List<Variant> variants;
        private List<Station> stations;
        private List<Batch> batches;
        private string searchTerm = "";
        private string selectedStation = "all";

        public InventoryControl()
        {
            InitializeComponent();
            LoadData();
            InitializeControls();
        }

        private void LoadData()
        {
            inventories = InventoryMockData.GetInventories();
            variants = InventoryMockData.GetVariants();
            stations = InventoryMockData.GetStations();
            batches = InventoryMockData.GetBatches();
        }

        private void InitializeControls()
        {
            // Initialize DataGridView
            InitializeDataGridView();

            // Initialize station filter
            comboBoxStation.Items.Add("Tất cả trạm");
            foreach (var station in stations)
            {
                comboBoxStation.Items.Add(station.Name);
            }
            comboBoxStation.SelectedIndex = 0;

            // Bind data
            BindData();
            UpdateStatistics();
        }

        private void InitializeDataGridView()
        {

            dataGridViewInventory.Columns.Clear();

            dataGridViewInventory.Columns.Clear();
            dataGridViewInventory.Columns.Add("SKU", "SKU");
            dataGridViewInventory.Columns.Add("ProductName", "Tên sản phẩm");
            dataGridViewInventory.Columns.Add("Station", "Trạm");
            dataGridViewInventory.Columns.Add("Batch", "Lô hàng");
            dataGridViewInventory.Columns.Add("CurrentStock", "Tồn kho");
            dataGridViewInventory.Columns.Add("ReservedStock", "Đã đặt");
            dataGridViewInventory.Columns.Add("AvailableStock", "Có thể bán");
            dataGridViewInventory.Columns.Add("StockLevel", "Mức tồn");

            // Set column widths
            dataGridViewInventory.Columns["SKU"].Width = 120;
            dataGridViewInventory.Columns["ProductName"].Width = 200;
            dataGridViewInventory.Columns["Station"].Width = 120;
            dataGridViewInventory.Columns["Batch"].Width = 100;
            dataGridViewInventory.Columns["CurrentStock"].Width = 80;
            dataGridViewInventory.Columns["ReservedStock"].Width = 80;
            dataGridViewInventory.Columns["AvailableStock"].Width = 80;
            dataGridViewInventory.Columns["StockLevel"].Width = 150;

            // Set alignment
            dataGridViewInventory.Columns["CurrentStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewInventory.Columns["ReservedStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewInventory.Columns["AvailableStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void BindData()
        {
            try
            {
                var filteredInventories = inventories.Where(inv =>
                {
                    var variant = variants.FirstOrDefault(v => v.VariantId == inv.VariantId);
                    var matchSearch = variant?.Name.ToLower().Contains(searchTerm.ToLower()) == true ||
                                     variant?.SKU.ToLower().Contains(searchTerm.ToLower()) == true;
                    var matchStation = selectedStation == "all" || GetStationName(inv.StationId) == selectedStation;
                    return matchSearch && matchStation;
                }).ToList();

                dataGridViewInventory.Rows.Clear();

                foreach (var inv in filteredInventories)
                {
                    var variant = GetVariantInfo(inv.VariantId);
                    var available = inv.CurrentStock - inv.ReservedStock;
                    var stockLevel = GetStockLevel(inv.CurrentStock, inv.ReservedStock);

                    int rowIndex = dataGridViewInventory.Rows.Add(
                        variant?.SKU,
                        variant?.Name,
                        GetStationName(inv.StationId),
                        GetBatchNo(inv.BatchId),
                        inv.CurrentStock,
                        inv.ReservedStock,
                        available,
                        $"{stockLevel.Percentage:F0}% - {GetStockLevelText(stockLevel.Level)}"
                    );

                    // Set colors
                    dataGridViewInventory.Rows[rowIndex].Cells["ReservedStock"].Style.ForeColor = Color.Orange;
                    dataGridViewInventory.Rows[rowIndex].Cells["AvailableStock"].Style.ForeColor = stockLevel.Color;

                    if (stockLevel.Level == "low")
                    {
                        dataGridViewInventory.Rows[rowIndex].Cells["StockLevel"].Style.ForeColor = Color.Red;
                        dataGridViewInventory.Rows[rowIndex].Cells["StockLevel"].Style.Font = new Font(dataGridViewInventory.Font, FontStyle.Bold);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics()
        {
            lblTotalStock.Text = inventories.Sum(inv => inv.CurrentStock).ToString() + " đơn vị";
            lblReservedStock.Text = inventories.Sum(inv => inv.ReservedStock).ToString() + " đơn vị";
            lblAvailableStock.Text = (inventories.Sum(inv => inv.CurrentStock) - inventories.Sum(inv => inv.ReservedStock)).ToString() + " đơn vị";

            // Count low stock items
            int lowStockCount = inventories.Count(inv =>
            {
                var available = inv.CurrentStock - inv.ReservedStock;
                return available <= 50; // min stock
            });
            lblLowStockCount.Text = lowStockCount.ToString() + " mặt hàng";
        }

        private Variant GetVariantInfo(int variantId)
        {
            return variants.FirstOrDefault(v => v.VariantId == variantId);
        }

        private string GetStationName(int stationId)
        {
            return stations.FirstOrDefault(s => s.StationId == stationId)?.Name ?? "N/A";
        }

        private string GetBatchNo(int? batchId)
        {
            if (!batchId.HasValue) return "N/A";
            return batches.FirstOrDefault(b => b.BatchId == batchId.Value)?.BatchNo ?? "N/A";
        }

        private (string Level, Color Color, double Percentage) GetStockLevel(int current, int reserved)
        {
            int available = current - reserved;
            int minStock = 50;
            int maxStock = 500;

            double percentage = (available / (double)maxStock) * 100;

            if (available <= minStock)
                return ("low", Color.Red, percentage);
            if (available >= maxStock * 0.8)
                return ("high", Color.Green, percentage);
            return ("normal", Color.Blue, percentage);
        }

        private string GetStockLevelText(string level)
        {
            switch (level)
            {
                case "low":
                    return "Thấp";
                case "high":
                    return "Cao";
                default:
                    return "Bình thường";
            }
        }

        #region Event Handlers
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTerm = txtSearch.Text;
            BindData();
        }

        private void comboBoxStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxStation.SelectedIndex == 0)
                selectedStation = "all";
            else
                selectedStation = comboBoxStation.SelectedItem.ToString();

            BindData();
        }
        #endregion
    }

    #region Data Models
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int VariantId { get; set; }
        public int StationId { get; set; }
        public int? BatchId { get; set; }
        public int CurrentStock { get; set; }
        public int ReservedStock { get; set; }
    }

    public class Batch
    {
        public int BatchId { get; set; }
        public string BatchNo { get; set; }
    }

    public static class InventoryMockData
    {
        public static List<Inventory> GetInventories()
        {
            return new List<Inventory>
            {
                new Inventory { InventoryId = 1, VariantId = 1, StationId = 1, BatchId = 1, CurrentStock = 150, ReservedStock = 25 },
                new Inventory { InventoryId = 2, VariantId = 2, StationId = 1, BatchId = 1, CurrentStock = 80, ReservedStock = 15 },
                new Inventory { InventoryId = 3, VariantId = 3, StationId = 2, BatchId = 2, CurrentStock = 45, ReservedStock = 10 },
                new Inventory { InventoryId = 4, VariantId = 1, StationId = 2, BatchId = 2, CurrentStock = 200, ReservedStock = 30 },
                new Inventory { InventoryId = 5, VariantId = 2, StationId = 3, BatchId = null, CurrentStock = 30, ReservedStock = 5 }
            };
        }

        public static List<Variant> GetVariants()
        {
            return new List<Variant>
            {
                new Variant { VariantId = 1, SKU = "SP001-S", Name = "Áo thun cotton - Size S" },
                new Variant { VariantId = 2, SKU = "SP001-M", Name = "Áo thun cotton - Size M" },
                new Variant { VariantId = 3, SKU = "SP002-32", Name = "Quần jeans - Size 32" }
            };
        }

        public static List<Station> GetStations()
        {
            return new List<Station>
            {
                new Station { StationId = 1, Name = "Trạm Hà Nội" },
                new Station { StationId = 2, Name = "Trạm Hồ Chí Minh" },
                new Station { StationId = 3, Name = "Trạm Đà Nẵng" }
            };
        }

        public static List<Batch> GetBatches()
        {
            return new List<Batch>
            {
                new Batch { BatchId = 1, BatchNo = "LÔ-2024-001" },
                new Batch { BatchId = 2, BatchNo = "LÔ-2024-002" },
                new Batch { BatchId = 3, BatchNo = "LÔ-2024-003" }
            };
        }
    }
    #endregion
}