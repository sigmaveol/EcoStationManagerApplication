using EcoStationManagerApplication.UI.Properties;
using System;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class SystemSettingsControl
    {
        #region General Settings Methods

        private void LoadGeneralSettings()
        {
            try
            {
                Settings.Default.Reload();
                txtCompanyName.Text = Settings.Default.CompanyName ?? "";
                txtCompanyAddress.Text = Settings.Default.CompanyAddress ?? "";
                txtCompanyPhone.Text = Settings.Default.CompanyPhone ?? "";
                txtCompanyEmail.Text = Settings.Default.CompanyEmail ?? "";
                txtTaxCode.Text = Settings.Default.TaxCode ?? "";

                SetComboBoxItem(comboBoxStartTime, Settings.Default.BusinessStartTime, 14);
                SetComboBoxItem(comboBoxEndTime, Settings.Default.BusinessEndTime, 34);
                SetComboBoxItem(comboBoxCurrency, Settings.Default.Currency, 0);

                toggleAutoBackup.Checked = Settings.Default.AutoBackupEnabled;
                numericBackupDays.Value = Settings.Default.BackupIntervalDays > 0 ? Settings.Default.BackupIntervalDays : 7;
                UpdateGeneralPlaceholders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải cài đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetDefaultGeneralSettings();
                UpdateGeneralPlaceholders();
            }
        }

        private void SetComboBoxItem(ComboBox comboBox, string value, int defaultIndex)
        {
            if (!string.IsNullOrEmpty(value) && comboBox.Items.Contains(value))
                comboBox.SelectedItem = value;
            else
                comboBox.SelectedIndex = defaultIndex;
        }

        private void SetDefaultGeneralSettings()
        {
            txtCompanyName.Text = "Công ty TNHH EcoStation";
            txtCompanyAddress.Text = "123 Đường ABC, Quận 1, TP.HCM";
            txtCompanyPhone.Text = "(+84) 28 1234 5678";
            txtCompanyEmail.Text = "contact@ecostation.com";
            txtTaxCode.Text = "0123456789";
            comboBoxStartTime.SelectedIndex = 14; // 07:00
            comboBoxEndTime.SelectedIndex = 34; // 17:00
            comboBoxCurrency.SelectedIndex = 0; // VND
            toggleAutoBackup.Checked = true;
            numericBackupDays.Value = 7;
        }

        private void SaveGeneralSettings()
        {
            try
            {
                Settings.Default.CompanyName = txtCompanyName.Text;
                Settings.Default.CompanyAddress = txtCompanyAddress.Text;
                Settings.Default.CompanyPhone = txtCompanyPhone.Text;
                Settings.Default.CompanyEmail = txtCompanyEmail.Text;
                Settings.Default.TaxCode = txtTaxCode.Text;
                Settings.Default.BusinessStartTime = comboBoxStartTime.SelectedItem?.ToString();
                Settings.Default.BusinessEndTime = comboBoxEndTime.SelectedItem?.ToString();
                Settings.Default.Currency = comboBoxCurrency.SelectedItem?.ToString();
                Settings.Default.AutoBackupEnabled = toggleAutoBackup.Checked;
                Settings.Default.BackupIntervalDays = (int)numericBackupDays.Value;
                Settings.Default.Save();

                MessageBox.Show("Đã lưu cài đặt chung thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGeneralSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cài đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateGeneralSettings()
        {
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên công ty", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (comboBoxStartTime.SelectedIndex >= comboBoxEndTime.SelectedIndex)
            {
                MessageBox.Show("Thời gian kết thúc phải sau thời gian bắt đầu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void SetupPlaceholderBehavior()
        {
            txtCompanyName.TextChanged -= TxtCompanyName_TextChanged;
            txtCompanyName.TextChanged += TxtCompanyName_TextChanged;
            UpdateGeneralPlaceholders();
        }

        private void TxtCompanyName_TextChanged(object sender, EventArgs e)
        {
            UpdateGeneralPlaceholders();
        }

        private void UpdateGeneralPlaceholders()
        {
            bool showPlaceholders = string.IsNullOrWhiteSpace(txtCompanyName.Text);

            txtCompanyName.PlaceholderText = showPlaceholders ? "Nhập tên công ty" : string.Empty;
            txtCompanyAddress.PlaceholderText = showPlaceholders ? "Nhập địa chỉ công ty" : string.Empty;
            txtCompanyPhone.PlaceholderText = showPlaceholders ? "Nhập số điện thoại liên hệ" : string.Empty;
            txtCompanyEmail.PlaceholderText = showPlaceholders ? "Nhập email liên hệ" : string.Empty;
            txtTaxCode.PlaceholderText = showPlaceholders ? "Nhập mã số thuế" : string.Empty;
        }

        private void SetGeneralInputsEnabled(bool enabled)
        {
            txtCompanyName.ReadOnly = !enabled;
            txtCompanyAddress.ReadOnly = !enabled;
            txtCompanyPhone.ReadOnly = !enabled;
            txtCompanyEmail.ReadOnly = !enabled;
            txtTaxCode.ReadOnly = !enabled;
            comboBoxStartTime.Enabled = enabled;
            comboBoxEndTime.Enabled = enabled;
            comboBoxCurrency.Enabled = enabled;
            toggleAutoBackup.Enabled = enabled;
            numericBackupDays.Enabled = enabled;
            btnSaveGeneral.Enabled = enabled;
            btnChangePin.Enabled = enabled;
        }

        #endregion

        #region General Settings Event Handlers

        private void btnSaveGeneral_Click(object sender, EventArgs e)
        {
            if (EnsureCanEdit() && ValidateGeneralSettings())
                SaveGeneralSettings();
        }

        #endregion
    }
}

