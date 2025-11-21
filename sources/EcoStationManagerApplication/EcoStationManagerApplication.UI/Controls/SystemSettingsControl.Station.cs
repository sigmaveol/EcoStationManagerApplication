using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SystemSettingsControl
    {
        #region Station Settings Methods

        private void LoadStationSettings()
        {
            listBoxStations.Items.Clear();
            if (stations != null && stations.Count > 0)
            {
                foreach (var station in stations)
                    listBoxStations.Items.Add($"{station.Name}");

                listBoxStations.SelectedIndex = 0;
                if (_canEdit) btnAddStation.Enabled = false;

                // Hiển thị label cảnh báo thay vì thêm vào listBox
                if (labelStationWarning != null)
                {
                    labelStationWarning.Visible = true;
                }
            }
            else
            {
                listBoxStations.Items.Add("Chưa có trạm nào. Vui lòng thêm trạm mới.");
                if (_canEdit) btnAddStation.Enabled = true;
            }
            LoadSelectedStationDetails();
        }

        private async void LoadSelectedStationDetails()
        {
            if (stations == null || stations.Count == 0)
            {
                ClearStationFields();
                _isCreatingNewStation = false;
                return;
            }

            if (listBoxStations.SelectedIndex >= 0 && listBoxStations.SelectedIndex < stations.Count)
            {
                _isCreatingNewStation = false;
                var station = stations[listBoxStations.SelectedIndex];

                txtStationName.Text = station.Name ?? "";
                txtStationAddress.Text = station.Address ?? "";
                txtStationPhone.Text = station.Phone ?? "";
                comboBoxStationType.SelectedIndex = GetStationTypeIndex(station.StationType ?? "refill");
                toggleStationActive.Checked = station.IsActive;
                labelCreatedDateValue.Text = station.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                labelUpdatedDateValue.Text = station.UpdatedDate.ToString("dd/MM/yyyy HH:mm");

                await SetManagerComboBox(station.Manager);
            }
        }

        private void ClearStationFields()
        {
            txtStationName.Text = "";
            txtStationAddress.Text = "";
            txtStationPhone.Text = "";
            comboBoxManager.SelectedIndex = -1;
            comboBoxStationType.SelectedIndex = -1;
            toggleStationActive.Checked = true;
            labelCreatedDateValue.Text = "--/--/--";
            labelUpdatedDateValue.Text = "--/--/--";
        }

        private async Task SetManagerComboBox(int? managerId)
        {
            if (!managerId.HasValue)
            {
                comboBoxManager.SelectedIndex = 0;
                return;
            }

            for (int i = 0; i < comboBoxManager.Items.Count; i++)
            {
                string itemText = comboBoxManager.Items[i].ToString();
                if (_managerComboBoxMapping.ContainsKey(itemText) && _managerComboBoxMapping[itemText] == managerId.Value)
                {
                    comboBoxManager.SelectedIndex = i;
                    return;
                }
            }

            await LoadManagerComboBox();
            for (int i = 0; i < comboBoxManager.Items.Count; i++)
            {
                string itemText = comboBoxManager.Items[i].ToString();
                if (_managerComboBoxMapping.ContainsKey(itemText) && _managerComboBoxMapping[itemText] == managerId.Value)
                {
                    comboBoxManager.SelectedIndex = i;
                    break;
                }
            }
        }

        private int GetStationTypeIndex(string stationType)
        {
            switch ((stationType ?? "refill").ToLower())
            {
                case "warehouse": return 0;
                case "refill": return 1;
                case "hybrid": return 2;
                case "other": return 3;
                default: return 1;
            }
        }

        private string GetStationTypeValue(int index)
        {
            string[] types = { "warehouse", "refill", "hybrid", "other" };
            return index >= 0 && index < types.Length ? types[index] : "refill";
        }

        private int? GetSelectedManagerId()
        {
            if (comboBoxManager.SelectedIndex > 0 && comboBoxManager.SelectedIndex < comboBoxManager.Items.Count)
            {
                string selectedText = comboBoxManager.Items[comboBoxManager.SelectedIndex].ToString();
                if (_managerComboBoxMapping.ContainsKey(selectedText) && _managerComboBoxMapping[selectedText] > 0)
                {
                    return _managerComboBoxMapping[selectedText];
                }
            }
            return null;
        }

        private async void SaveStationSettings()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtStationName.Text.Trim()))
                {
                    MessageBox.Show("Tên trạm không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStationName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStationAddress.Text.Trim()))
                {
                    MessageBox.Show("Địa chỉ trạm không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStationAddress.Focus();
                    return;
                }

                if (_isCreatingNewStation)
                {
                    await CreateNewStation();
                }
                else
                {
                    await UpdateExistingStation();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thông tin trạm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CreateNewStation()
        {
            // Kiểm tra SelectedIndex hợp lệ
            int stationTypeIndex = comboBoxStationType.SelectedIndex;
            if (stationTypeIndex < 0 || stationTypeIndex >= comboBoxStationType.Items.Count)
            {
                MessageBox.Show("Vui lòng chọn loại trạm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxStationType.Focus();
                return;
            }
            
            var newStation = new Station
            {
                Name = txtStationName.Text.Trim(),
                Address = txtStationAddress.Text.Trim(),
                Phone = txtStationPhone.Text.Trim(),
                StationType = GetStationTypeValue(stationTypeIndex),
                Manager = GetSelectedManagerId(),
                IsActive = toggleStationActive.Checked,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            var result = await AppServices.StationService.CreateStationAsync(newStation);
            if (result.Success)
            {
                MessageBox.Show($"Đã tạo trạm mới thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _isCreatingNewStation = false;
                await LoadData();
                LoadStationSettings();

                var newStationInList = stations?.FirstOrDefault(s => s.StationId == result.Data);
                if (newStationInList != null)
                {
                    listBoxStations.SelectedIndex = stations.IndexOf(newStationInList);
                    LoadSelectedStationDetails();
                }
            }
            else
            {
                MessageBox.Show($"Lỗi khi tạo trạm mới: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateExistingStation()
        {
            if (stations == null || stations.Count == 0)
            {
                MessageBox.Show("Chưa có trạm nào để lưu. Vui lòng thêm trạm mới trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBoxStations.SelectedIndex < 0 || listBoxStations.SelectedIndex >= stations.Count)
                return;

            // Kiểm tra SelectedIndex hợp lệ cho StationType
            int stationTypeIndex = comboBoxStationType.SelectedIndex;
            if (stationTypeIndex < 0 || stationTypeIndex >= comboBoxStationType.Items.Count)
            {
                MessageBox.Show("Vui lòng chọn loại trạm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxStationType.Focus();
                return;
            }

            var station = stations[listBoxStations.SelectedIndex];
            station.Name = txtStationName.Text.Trim();
            station.Address = txtStationAddress.Text.Trim();
            station.Phone = txtStationPhone.Text.Trim();
            station.StationType = GetStationTypeValue(stationTypeIndex);
            station.Manager = GetSelectedManagerId();
            station.IsActive = toggleStationActive.Checked;

            var result = await AppServices.StationService.UpdateStationAsync(station);
            if (result.Success)
            {
                MessageBox.Show("Đã lưu thông tin trạm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadData();
                LoadStationSettings();
            }
            else
            {
                MessageBox.Show($"Lỗi khi lưu thông tin trạm: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateStationSettings()
        {
            if (string.IsNullOrWhiteSpace(txtStationName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên trạm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtStationAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ trạm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void SetStationInputsEnabled(bool enabled)
        {
            txtStationName.ReadOnly = !enabled;
            txtStationAddress.ReadOnly = !enabled;
            txtStationPhone.ReadOnly = !enabled;
            comboBoxManager.Enabled = enabled;
            comboBoxStationType.Enabled = enabled;
            toggleStationActive.Enabled = enabled;
            listBoxStations.Enabled = true;
            btnSaveStation.Enabled = enabled;
            btnAddStation.Enabled = enabled;
        }

        #endregion

        #region Station Settings Event Handlers

        private void btnSaveStation_Click(object sender, EventArgs e)
        {
            if (EnsureCanEdit() && ValidateStationSettings())
                SaveStationSettings();
        }

        private void listBoxStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedStationDetails();
        }

        private async void btnAddStation_Click(object sender, EventArgs e)
        {
            if (!EnsureCanEdit()) return;

            try
            {
                var existingStationsResult = await AppServices.StationService.GetAllStationsAsync();
                if (existingStationsResult?.Success == true && existingStationsResult.Data?.Any() == true)
                {
                    MessageBox.Show("Hiện tại chỉ cho phép tạo một trạm. Tính năng này sẽ phát triển ở phiên bản sau.\n\nVui lòng cập nhật trạm hiện có thay vì tạo mới.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _isCreatingNewStation = true;
                ClearStationFields();
                comboBoxStationType.SelectedIndex = 1;
                comboBoxManager.SelectedIndex = 0;
                listBoxStations.Items.Clear();
                listBoxStations.Items.Add("Đang tạo trạm mới...");
                SetStationInputsEnabled(true);
                btnSaveStation.Enabled = true;
                btnAddStation.Enabled = false;
                txtStationName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo form thêm trạm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isCreatingNewStation = false;
            }
        }

        #endregion
    }
}

