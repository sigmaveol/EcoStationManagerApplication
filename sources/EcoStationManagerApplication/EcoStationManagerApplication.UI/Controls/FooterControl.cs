using System.ComponentModel;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class FooterControl : UserControl
    {
        public FooterControl()
        {
            InitializeComponent();
            UpdateFooterInfo();
        }

        private string _version = "1.0.0";
        private string _buildNumber = "2025.11.27";
        private int _copyrightYear = 2025;
        private string _companyName = "Green Core Tech";
        private string _supportEmail = "hungminhtobe@gmail.com";
        private string _appName = "EcoStation Manager";

        [Category("Footer Settings")]
        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                UpdateFooterInfo();
            }
        }

        [Category("Footer Settings")]
        public string BuildNumber
        {
            get => _buildNumber;
            set
            {
                _buildNumber = value;
                UpdateFooterInfo();
            }
        }

        [Category("Footer Settings")]
        public int CopyrightYear
        {
            get => _copyrightYear;
            set
            {
                _copyrightYear = value;
                UpdateFooterInfo();
            }
        }

        [Category("Footer Settings")]
        public string CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                UpdateFooterInfo();
            }
        }

        [Category("Footer Settings")]
        public string SupportEmail
        {
            get => _supportEmail;
            set
            {
                _supportEmail = value;
                UpdateFooterInfo();
            }
        }

        [Category("Footer Settings")]
        public string ApplicationName
        {
            get => _appName;
            set
            {
                _appName = value;
                UpdateFooterInfo();
            }
        }

        private void UpdateFooterInfo()
        {
            lblAppName.Text = _appName;
            lblCopyright.Text = $"© {_copyrightYear} {_companyName}. All rights reserved.";
            lblVersion.Text = $"Phiên bản: {_version} | Build: {_buildNumber}";
            lblSupportInfo.Text = $"Hỗ trợ kỹ thuật: {_supportEmail}";
        }

        public void SetFooterInfo(string version, string buildNumber, int copyrightYear,
                                string companyName, string supportEmail, string appName)
        {
            _version = version;
            _buildNumber = buildNumber;
            _copyrightYear = copyrightYear;
            _companyName = companyName;
            _supportEmail = supportEmail;
            _appName = appName;

            UpdateFooterInfo();
        }
    }
}