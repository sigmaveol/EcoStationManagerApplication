using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StationsControl : UserControl
    {
        private List<Stationss> stations;
        private List<Tank> tanks;
        private Stationss selectedStation;

        // Colors
        private Color primaryColor = Color.FromArgb(31, 107, 59); // #1f6b3b
        private Color primaryHoverColor = Color.FromArgb(33, 140, 73); // #218c49

        public StationsControl()
        {
            InitializeComponent();
            LoadData();
            InitializeControls();
        }

        private void LoadData()
        {
            stations = StationsMockData.GetStations();
            tanks = StationsMockData.GetTanks();
        }

        private void InitializeControls()
        {
            // Initialize styles
            InitializeStyles();

            // Load stations as cards
            LoadStationCards();

            // Show first station by default
            if (stations.Count > 0)
            {
                SelectStation(stations[0]);
            }
        }

        private void InitializeStyles()
        {
            // Set button colors
            btnAddStation.FillColor = primaryColor;
            btnAddStation.HoverState.FillColor = primaryHoverColor;
            btnAddTank.FillColor = primaryColor;
            btnAddTank.HoverState.FillColor = primaryHoverColor;
        }

        private void LoadStationCards()
        {
            flowLayoutStations.Controls.Clear();

            foreach (var station in stations)
            {
                var stationCard = CreateStationCard(station);
                flowLayoutStations.Controls.Add(stationCard);
            }
        }

        private Guna2Panel CreateStationCard(Stationss station)
        {
            var card = new Guna2Panel()
            {
                Size = new Size(280, 120),
                BorderRadius = 10,
                BorderColor = Color.FromArgb(229, 231, 235),
                BorderThickness = 1,
                Margin = new Padding(10),
                Cursor = Cursors.Hand,
                Tag = station
            };

            // Card content
            var layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(15)
            };

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Station name with icon
            var namePanel = new Panel() { Height = 25, Dock = DockStyle.Top };
            var icon = new PictureBox()
            {
                Image = SystemIcons.Shield.ToBitmap(), // Using system icon as fallback
                Size = new Size(16, 16),
                Location = new Point(0, 4),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            var nameLabel = new Label()
            {
                Text = station.Name,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Location = new Point(25, 3),
                AutoSize = true
            };
            namePanel.Controls.AddRange(new Control[] { icon, nameLabel });

            // Station code
            var codeLabel = new Label()
            {
                Text = $"Mã: ST{station.StationId.ToString().PadLeft(3, '0')}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 0)
            };

            // Station type
            var typeBadge = new Guna2Chip()
            {
                Text = GetStationTypeText(station.StationType),
                FillColor = Color.FromArgb(239, 246, 255),
                ForeColor = Color.FromArgb(37, 99, 235),
                BorderThickness = 0,
                Size = new Size(100, 25),
                Margin = new Padding(0, 5, 0, 0),
                Font = new Font("Segoe UI", 8)
            };

            // Status
            var statusPanel = new Panel() { Height = 25, Dock = DockStyle.Top, Margin = new Padding(0, 5, 0, 0) };
            var statusBadge = new Guna2Chip()
            {
                Text = station.IsActive ? "Hoạt động" : "Tạm ngưng",
                FillColor = station.IsActive ? Color.FromArgb(220, 252, 231) : Color.FromArgb(254, 226, 226),
                ForeColor = station.IsActive ? Color.FromArgb(22, 163, 74) : Color.FromArgb(239, 68, 68),
                BorderThickness = 0,
                Size = new Size(80, 22),
                Font = new Font("Segoe UI", 8)
            };
            statusPanel.Controls.Add(statusBadge);

            layout.Controls.Add(namePanel, 0, 0);
            layout.Controls.Add(codeLabel, 0, 1);
            layout.Controls.Add(typeBadge, 0, 2);
            layout.Controls.Add(statusPanel, 0, 3);

            card.Controls.Add(layout);

            // Click event
            card.Click += (s, e) => SelectStation(station);

            return card;
        }

        private void SelectStation(Stationss station)
        {
            selectedStation = station;

            // Update UI to show selected state
            //foreach (Control control in flowLayoutStations.Controls)
            //{
            //    if (control is Guna2Panel card)
            //    {
            //        var cardStation = card.Tag as Station;
            //        card.FillColor = cardStation?.StationId == station.StationId ?
            //            Color.FromArgb(240, 253, 244) : Color.White;
            //    }
            //}

            // Load station details
            LoadStationDetails(station);
            LoadStationTanks(station.StationId);
        }

        private void LoadStationDetails(Stationss station)
        {
            lblStationName.Text = station.Name;
            lblStationCode.Text = $"ST{station.StationId.ToString().PadLeft(3, '0')}";
            lblStationType.Text = GetStationTypeText(station.StationType);
            lblStationAddress.Text = station.Address;
            lblStationPhone.Text = station.Phone;
            lblManager.Text = station.Manager;
            lblCapacity.Text = $"{station.Capacity} kg";

            // Update status
            var statusBadge = station.IsActive ?
                new { Text = "Hoạt động", Color = Color.FromArgb(34, 197, 94) } :
                new { Text = "Tạm ngưng", Color = Color.FromArgb(239, 68, 68) };

            lblStatus.Text = statusBadge.Text;
            lblStatus.ForeColor = statusBadge.Color;
        }

        private void LoadStationTanks(int stationId)
        {
            flowLayoutTanks.Controls.Clear();

            var stationTanks = tanks.Where(t => t.StationId == stationId).ToList();

            foreach (var tank in stationTanks)
            {
                var tankCard = CreateTankCard(tank);
                flowLayoutTanks.Controls.Add(tankCard);
            }

            // Update tanks count
            lblTanksCount.Text = $"{stationTanks.Count} bồn";
        }

        private Guna2Panel CreateTankCard(Tank tank)
        {
            var card = new Guna2Panel()
            {
                Size = new Size(300, 180),
                BorderRadius = 10,
                BorderColor = Color.FromArgb(229, 231, 235),
                BorderThickness = 1,
                Margin = new Padding(10),
                Padding = new Padding(15)
            };

            var fillPercentage = (tank.CurrentLevel / tank.Capacity) * 100;
            var isLowLevel = fillPercentage < 30;

            // Tank header
            var headerPanel = new Panel() { Height = 30, Dock = DockStyle.Top };
            var icon = new PictureBox()
            {
                Image = SystemIcons.Application.ToBitmap(), // Using system icon as fallback
                Size = new Size(16, 16),
                Location = new Point(0, 7),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            var nameLabel = new Label()
            {
                Text = tank.Name,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Location = new Point(25, 5),
                AutoSize = true
            };
            headerPanel.Controls.AddRange(new Control[] { icon, nameLabel });

            // Tank code
            var codeLabel = new Label()
            {
                Text = $"Mã: TANK{tank.TankId.ToString().PadLeft(3, '0')}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(0, 35)
            };

            // Material
            var materialLabel = new Label()
            {
                Text = $"Chất liệu: {GetMaterialText(tank.Material)}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(0, 55)
            };

            // Capacity
            var capacityLabel = new Label()
            {
                Text = $"Dung tích: {tank.Capacity} {tank.Unit}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(0, 75)
            };

            // Current level with progress bar
            var levelPanel = new Panel() { Height = 40, Location = new Point(0, 95), Width = 270 };

            var levelLabel = new Label()
            {
                Text = $"{tank.CurrentLevel} {tank.Unit} ({fillPercentage:F0}%)",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = isLowLevel ? Color.FromArgb(249, 115, 22) : Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            if (isLowLevel)
            {
                var warningIcon = new PictureBox()
                {
                    Image = SystemIcons.Warning.ToBitmap(),
                    Size = new Size(14, 14),
                    Location = new Point(levelLabel.Right + 5, 2),
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                levelPanel.Controls.Add(warningIcon);
            }

            // Progress bar - Sử dụng Guna2ProgressBar đúng cách
            var progressBar = new Guna2ProgressBar()
            {
                Location = new Point(0, 20),
                Size = new Size(270, 8),
                Value = (int)fillPercentage,
                FillColor = isLowLevel ? Color.FromArgb(249, 115, 22) : Color.FromArgb(59, 130, 246),
                // BackgroundColor property doesn't exist in Guna2ProgressBar, using alternative
                BorderRadius = 4
            };

            // Set background color using reflection or alternative method
            progressBar.BackColor = Color.FromArgb(229, 231, 235);

            levelPanel.Controls.Add(levelLabel);
            levelPanel.Controls.Add(progressBar);

            // Status
            var statusPanel = new Panel() { Height = 25, Location = new Point(0, 140), Width = 270 };
            var statusBadge = new Guna2Chip()
            {
                Text = GetTankStatusText(tank.Status),
                FillColor = GetTankStatusColor(tank.Status),
                ForeColor = Color.White,
                BorderThickness = 0,
                Size = new Size(80, 22),
                Font = new Font("Segoe UI", 8)
            };
            statusPanel.Controls.Add(statusBadge);

            // Cleaning info
            var cleaningLabel = new Label()
            {
                Text = $"Vệ sinh: {tank.LastCleanDate:dd/MM/yyyy} → {tank.NextCleanDate:dd/MM/yyyy}",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(156, 163, 175),
                AutoSize = true,
                Location = new Point(85, 3)
            };
            statusPanel.Controls.Add(cleaningLabel);

            card.Controls.Add(headerPanel);
            card.Controls.Add(codeLabel);
            card.Controls.Add(materialLabel);
            card.Controls.Add(capacityLabel);
            card.Controls.Add(levelPanel);
            card.Controls.Add(statusPanel);

            return card;
        }

        #region Helper Methods
        private string GetStationTypeText(string type)
        {
            var types = new Dictionary<string, string>
            {
                { "warehouse", "Kho trung tâm" },
                { "refill", "Trạm refill" },
                { "hybrid", "Kho & Refill" },
                { "other", "Khác" }
            };
            return types.ContainsKey(type) ? types[type] : type;
        }

        private string GetTankStatusText(string status)
        {
            var texts = new Dictionary<string, string>
            {
                { "active", "Hoạt động" },
                { "maintenance", "Bảo trì" },
                { "outoforder", "Hỏng" }
            };
            return texts.ContainsKey(status) ? texts[status] : status;
        }

        private Color GetTankStatusColor(string status)
        {
            var colors = new Dictionary<string, Color>
            {
                { "active", Color.FromArgb(34, 197, 94) },
                { "maintenance", Color.FromArgb(156, 163, 175) },
                { "outoforder", Color.FromArgb(239, 68, 68) }
            };
            return colors.ContainsKey(status) ? colors[status] : Color.Gray;
        }

        private string GetMaterialText(string material)
        {
            var materials = new Dictionary<string, string>
            {
                { "glass", "Thủy tinh" },
                { "plastic", "Nhựa" },
                { "metal", "Kim loại" }
            };
            return materials.ContainsKey(material) ? materials[material] : material;
        }
        #endregion

        #region Event Handlers
        private void btnAddStation_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng thêm trạm mới đang được phát triển", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAddTank_Click(object sender, EventArgs e)
        {
            if (selectedStation == null)
            {
                MessageBox.Show("Vui lòng chọn một trạm trước", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show($"Chức năng thêm bồn cho trạm {selectedStation.Name} đang được phát triển", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditStation_Click(object sender, EventArgs e)
        {
            if (selectedStation == null)
            {
                MessageBox.Show("Vui lòng chọn một trạm", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show($"Chức năng chỉnh sửa trạm {selectedStation.Name} đang được phát triển", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }

    #region Data Models
    public class Stationss
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public string StationType { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Manager { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }

    public class Tank
    {
        public int TankId { get; set; }
        public string Name { get; set; }
        public int StationId { get; set; }
        public string Material { get; set; }
        public decimal Capacity { get; set; }
        public decimal CurrentLevel { get; set; }
        public string Unit { get; set; }
        public DateTime LastCleanDate { get; set; }
        public DateTime NextCleanDate { get; set; }
        public string Status { get; set; }
    }

    public static class StationsMockData
    {
        public static List<Stationss> GetStations()
        {
            return new List<Stationss>
            {
                new Stationss {
                    StationId = 1,
                    Name = "Trạm Hà Nội",
                    StationType = "warehouse",
                    Address = "123 Nguyễn Huệ, Q.1, Hà Nội",
                    Phone = "(+84) 24 1234 5678",
                    Manager = "Nguyễn Văn A",
                    Capacity = 10000,
                    IsActive = true
                },
                new Stationss {
                    StationId = 2,
                    Name = "Trạm Hồ Chí Minh",
                    StationType = "hybrid",
                    Address = "456 Lê Lợi, Q.1, TP.HCM",
                    Phone = "(+84) 28 8765 4321",
                    Manager = "Trần Thị B",
                    Capacity = 15000,
                    IsActive = true
                },
                new Stationss {
                    StationId = 3,
                    Name = "Trạm Đà Nẵng",
                    StationType = "refill",
                    Address = "789 Hải Phòng, Q. Hải Châu, Đà Nẵng",
                    Phone = "(+84) 236 123 456",
                    Manager = "Lê Văn C",
                    Capacity = 8000,
                    IsActive = false
                }
            };
        }

        public static List<Tank> GetTanks()
        {
            return new List<Tank>
            {
                new Tank {
                    TankId = 1,
                    Name = "Bồn chính",
                    StationId = 1,
                    Material = "plastic",
                    Capacity = 5000,
                    CurrentLevel = 3200,
                    Unit = "kg",
                    LastCleanDate = DateTime.Now.AddDays(-15),
                    NextCleanDate = DateTime.Now.AddDays(15),
                    Status = "active"
                },
                new Tank {
                    TankId = 2,
                    Name = "Bồn phụ",
                    StationId = 1,
                    Material = "metal",
                    Capacity = 3000,
                    CurrentLevel = 800,
                    Unit = "kg",
                    LastCleanDate = DateTime.Now.AddDays(-5),
                    NextCleanDate = DateTime.Now.AddDays(25),
                    Status = "active"
                },
                new Tank {
                    TankId = 3,
                    Name = "Bồn trung tâm",
                    StationId = 2,
                    Material = "glass",
                    Capacity = 8000,
                    CurrentLevel = 6500,
                    Unit = "kg",
                    LastCleanDate = DateTime.Now.AddDays(-10),
                    NextCleanDate = DateTime.Now.AddDays(20),
                    Status = "maintenance"
                }
            };
        }
    }
    #endregion
}