using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class CardControl : UserControl
    {
        public CardControl()
        {
            InitializeComponent();
            //this.Size = this.panelCard.Size;
        }

        #region Events

        public event EventHandler ValueChanged;
        public event EventHandler TitleChanged;
        public event EventHandler IconChanged;

        #endregion

        #region Properties

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    labelTitle.Text = value;
                    TitleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    labelValue.Text = value;
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private string _subInfo;
        public string SubInfo
        {
            get => _subInfo;
            set
            {
                if (_subInfo != value)
                {
                    _subInfo = value;
                    labelSubInfo.Text = value;
                }
            }
        }

        private string _change;
        public string Change
        {
            get => _change;
            set
            {
                if (_change != value)
                {
                    _change = value;
                    labelChange.Text = value;
                }
            }
        }

        public Color ChangeColor
        {
            get => labelChange.ForeColor;
            set => labelChange.ForeColor = value;
        }

        private Image _icon;
        public Image Icon
        {
            get => _icon;
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    pictureIcon.Image = value;
                    IconChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Color CardColor
        {
            get {return panelCard.FillColor;}
            set { panelCard.FillColor = value; }
        }

        public Color TitleColor
        {
            get => labelTitle.ForeColor;
            set => labelTitle.ForeColor = value;
        }

        public Color ValueColor
        {
            get => labelValue.ForeColor;
            set => labelValue.ForeColor = value;
        }

        public Color SubInfoColor
        {
            get => labelSubInfo.ForeColor;
            set => labelSubInfo.ForeColor = value;
        }

        public Font TitleFont
        {
            get => labelTitle.Font;
            set => labelTitle.Font = value;
        }

        public Font ValueFont
        {
            get => labelValue.Font;
            set => labelValue.Font = value;
        }

        public Font SubInfoFont
        {
            get => labelSubInfo.Font;
            set => labelSubInfo.Font = value;
        }

        public Font ChangeFont
        {
            get => labelChange.Font;
            set => labelChange.Font = value;
        }

#endregion

        #region Methods

        public void SetPositiveChange(string changeText)
        {
            Change = changeText;
            ChangeColor = Color.Green;
        }

        public void SetNegativeChange(string changeText)
        {
            Change = changeText;
            ChangeColor = Color.Red;
        }

        public void SetNeutralChange(string changeText)
        {
            Change = changeText;
            ChangeColor = Color.Gray;
        }

        public void SetIconColor(Color color)
        {
            pictureIcon.FillColor = color;
        }

        public void SetCardStyle(Color backgroundColor, Color textColor)
        {
            CardColor = backgroundColor;
            TitleColor = textColor;
            ValueColor = textColor;
            SubInfoColor = textColor;
        }

        public void SetData(string title, string value, string subInfo = "", string change = "")
        {
            Title = title;
            Value = value;
            SubInfo = subInfo;
            Change = change;
        }

        #endregion
    }
}
