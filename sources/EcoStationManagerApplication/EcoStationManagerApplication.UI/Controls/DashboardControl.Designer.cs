using EcoStationManagerApplication.UI.Common;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Controls
{
    partial class DashboardControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.guna2PanelContent = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.panelRecentActivities = new Guna.UI2.WinForms.Guna2Panel();
            this.flowLayoutRecent = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRecent = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.panelTodoList = new Guna.UI2.WinForms.Guna2Panel();
            this.flowLayoutTodo = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTodo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.flowLayoutPanelCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardControl1 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardControl2 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardControl3 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.cardControl4 = new EcoStationManagerApplication.UI.Controls.CardControl();
            this.guna2PanelContent.SuspendLayout();
            this.tableLayoutPanelBottom.SuspendLayout();
            this.panelRecentActivities.SuspendLayout();
            this.panelTodoList.SuspendLayout();
            this.flowLayoutPanelCards.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2PanelContent
            // 
            this.guna2PanelContent.AutoScroll = true;
            this.guna2PanelContent.Controls.Add(this.tableLayoutPanelBottom);
            this.guna2PanelContent.Controls.Add(this.flowLayoutPanelCards);
            this.guna2PanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelContent.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(240)))));
            this.guna2PanelContent.Location = new System.Drawing.Point(0, 0);
            this.guna2PanelContent.Name = "guna2PanelContent";
            this.guna2PanelContent.Padding = new System.Windows.Forms.Padding(20);
            this.guna2PanelContent.Size = new System.Drawing.Size(1500, 800);
            this.guna2PanelContent.TabIndex = 8;
            // 
            // tableLayoutPanelBottom
            // 
            this.tableLayoutPanelBottom.AutoSize = true;
            this.tableLayoutPanelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelBottom.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelBottom.ColumnCount = 2;
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.Controls.Add(this.panelRecentActivities, 0, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.panelTodoList, 1, 0);
            this.tableLayoutPanelBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelBottom.Location = new System.Drawing.Point(20, 296);
            this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
            this.tableLayoutPanelBottom.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanelBottom.RowCount = 1;
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(1460, 125);
            this.tableLayoutPanelBottom.TabIndex = 3;
            // 
            // panelRecentActivities
            // 
            this.panelRecentActivities.AutoSize = true;
            this.panelRecentActivities.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRecentActivities.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panelRecentActivities.BorderRadius = 20;
            this.panelRecentActivities.BorderThickness = 1;
            this.panelRecentActivities.Controls.Add(this.flowLayoutRecent);
            this.panelRecentActivities.Controls.Add(this.lblRecent);
            this.panelRecentActivities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRecentActivities.FillColor = System.Drawing.Color.White;
            this.panelRecentActivities.Location = new System.Drawing.Point(20, 20);
            this.panelRecentActivities.Margin = new System.Windows.Forms.Padding(10);
            this.panelRecentActivities.Name = "panelRecentActivities";
            this.panelRecentActivities.Padding = new System.Windows.Forms.Padding(15);
            this.panelRecentActivities.Size = new System.Drawing.Size(700, 85);
            this.panelRecentActivities.TabIndex = 4;
            // 
            // flowLayoutRecent
            // 
            this.flowLayoutRecent.AutoScroll = true;
            this.flowLayoutRecent.AutoSize = true;
            this.flowLayoutRecent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutRecent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutRecent.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutRecent.Location = new System.Drawing.Point(15, 60);
            this.flowLayoutRecent.Name = "flowLayoutRecent";
            this.flowLayoutRecent.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutRecent.Size = new System.Drawing.Size(670, 10);
            this.flowLayoutRecent.TabIndex = 2;
            this.flowLayoutRecent.WrapContents = false;
            // 
            // lblRecent
            // 
            this.lblRecent.AutoSize = false;
            this.lblRecent.BackColor = System.Drawing.Color.Transparent;
            this.lblRecent.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRecent.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblRecent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblRecent.Location = new System.Drawing.Point(15, 15);
            this.lblRecent.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.lblRecent.Name = "lblRecent";
            this.lblRecent.Size = new System.Drawing.Size(670, 45);
            this.lblRecent.TabIndex = 1;
            this.lblRecent.Text = "Hoạt động gần đây";
            this.lblRecent.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelTodoList
            // 
            this.panelTodoList.AutoSize = true;
            this.panelTodoList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTodoList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panelTodoList.BorderRadius = 20;
            this.panelTodoList.BorderThickness = 1;
            this.panelTodoList.Controls.Add(this.flowLayoutTodo);
            this.panelTodoList.Controls.Add(this.lblTodo);
            this.panelTodoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTodoList.FillColor = System.Drawing.Color.White;
            this.panelTodoList.Location = new System.Drawing.Point(740, 20);
            this.panelTodoList.Margin = new System.Windows.Forms.Padding(10);
            this.panelTodoList.Name = "panelTodoList";
            this.panelTodoList.Padding = new System.Windows.Forms.Padding(15);
            this.panelTodoList.Size = new System.Drawing.Size(700, 85);
            this.panelTodoList.TabIndex = 5;
            // 
            // flowLayoutTodo
            // 
            this.flowLayoutTodo.AutoScroll = true;
            this.flowLayoutTodo.AutoSize = true;
            this.flowLayoutTodo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutTodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutTodo.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutTodo.Location = new System.Drawing.Point(15, 60);
            this.flowLayoutTodo.Name = "flowLayoutTodo";
            this.flowLayoutTodo.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutTodo.Size = new System.Drawing.Size(670, 10);
            this.flowLayoutTodo.TabIndex = 2;
            this.flowLayoutTodo.WrapContents = false;
            // 
            // lblTodo
            // 
            this.lblTodo.AutoSize = false;
            this.lblTodo.BackColor = System.Drawing.Color.Transparent;
            this.lblTodo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTodo.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTodo.Location = new System.Drawing.Point(15, 15);
            this.lblTodo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.lblTodo.Name = "lblTodo";
            this.lblTodo.Size = new System.Drawing.Size(670, 45);
            this.lblTodo.TabIndex = 1;
            this.lblTodo.Text = "Danh sách công việc";
            this.lblTodo.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanelCards
            // 
            this.flowLayoutPanelCards.AutoSize = true;
            this.flowLayoutPanelCards.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelCards.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanelCards.Controls.Add(this.cardControl1);
            this.flowLayoutPanelCards.Controls.Add(this.cardControl2);
            this.flowLayoutPanelCards.Controls.Add(this.cardControl3);
            this.flowLayoutPanelCards.Controls.Add(this.cardControl4);
            this.flowLayoutPanelCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelCards.Location = new System.Drawing.Point(20, 20);
            this.flowLayoutPanelCards.Margin = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.flowLayoutPanelCards.Name = "flowLayoutPanelCards";
            this.flowLayoutPanelCards.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanelCards.Size = new System.Drawing.Size(1460, 276);
            this.flowLayoutPanelCards.TabIndex = 2;
            // 
            // cardControl1
            // 
            this.cardControl1.BackColor = System.Drawing.Color.Transparent;
            this.cardControl1.CardColor = System.Drawing.Color.White;
            this.cardControl1.Change = "+ 12%";
            this.cardControl1.ChangeColor = System.Drawing.Color.Green;
            this.cardControl1.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardControl1.Icon = null;
            this.cardControl1.Location = new System.Drawing.Point(13, 13);
            this.cardControl1.MaximumSize = new System.Drawing.Size(300, 250);
            this.cardControl1.Name = "cardControl1";
            this.cardControl1.Padding = new System.Windows.Forms.Padding(10);
            this.cardControl1.Size = new System.Drawing.Size(284, 250);
            this.cardControl1.SubInfo = "23 đơn hàng";
            this.cardControl1.SubInfoColor = System.Drawing.Color.Gray;
            this.cardControl1.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardControl1.TabIndex = 0;
            this.cardControl1.Title = "Doanh thu hôm nay";
            this.cardControl1.TitleColor = System.Drawing.Color.Gray;
            this.cardControl1.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardControl1.Value = "12,000,000đ";
            this.cardControl1.ValueColor = System.Drawing.Color.Black;
            this.cardControl1.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardControl2
            // 
            this.cardControl2.BackColor = System.Drawing.Color.Transparent;
            this.cardControl2.CardColor = System.Drawing.Color.White;
            this.cardControl2.Change = null;
            this.cardControl2.ChangeColor = System.Drawing.Color.Green;
            this.cardControl2.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardControl2.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardControl2.Icon = null;
            this.cardControl2.Location = new System.Drawing.Point(303, 13);
            this.cardControl2.MaximumSize = new System.Drawing.Size(300, 250);
            this.cardControl2.Name = "cardControl2";
            this.cardControl2.Padding = new System.Windows.Forms.Padding(10);
            this.cardControl2.Size = new System.Drawing.Size(284, 250);
            this.cardControl2.SubInfo = "cần đặt hàng ngay";
            this.cardControl2.SubInfoColor = System.Drawing.Color.Gray;
            this.cardControl2.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardControl2.TabIndex = 1;
            this.cardControl2.Title = "Tồn kho thấp";
            this.cardControl2.TitleColor = System.Drawing.Color.Gray;
            this.cardControl2.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardControl2.Value = "3 mặt hàng";
            this.cardControl2.ValueColor = System.Drawing.Color.Black;
            this.cardControl2.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardControl3
            // 
            this.cardControl3.BackColor = System.Drawing.Color.Transparent;
            this.cardControl3.CardColor = System.Drawing.Color.White;
            this.cardControl3.Change = "+5%";
            this.cardControl3.ChangeColor = System.Drawing.Color.Green;
            this.cardControl3.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardControl3.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardControl3.Icon = null;
            this.cardControl3.Location = new System.Drawing.Point(593, 13);
            this.cardControl3.MaximumSize = new System.Drawing.Size(300, 250);
            this.cardControl3.Name = "cardControl3";
            this.cardControl3.Padding = new System.Windows.Forms.Padding(10);
            this.cardControl3.Size = new System.Drawing.Size(284, 250);
            this.cardControl3.SubInfo = "Tháng này";
            this.cardControl3.SubInfoColor = System.Drawing.Color.Gray;
            this.cardControl3.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardControl3.TabIndex = 2;
            this.cardControl3.Title = "Khách hàng mới";
            this.cardControl3.TitleColor = System.Drawing.Color.Gray;
            this.cardControl3.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardControl3.Value = "18";
            this.cardControl3.ValueColor = System.Drawing.Color.Black;
            this.cardControl3.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // cardControl4
            // 
            this.cardControl4.BackColor = System.Drawing.Color.Transparent;
            this.cardControl4.CardColor = System.Drawing.Color.White;
            this.cardControl4.Change = null;
            this.cardControl4.ChangeColor = System.Drawing.Color.Green;
            this.cardControl4.ChangeFont = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardControl4.Cursor = System.Windows.Forms.Cursors.Default;
            this.cardControl4.Icon = null;
            this.cardControl4.Location = new System.Drawing.Point(883, 13);
            this.cardControl4.MaximumSize = new System.Drawing.Size(300, 250);
            this.cardControl4.Name = "cardControl4";
            this.cardControl4.Padding = new System.Windows.Forms.Padding(10);
            this.cardControl4.Size = new System.Drawing.Size(284, 250);
            this.cardControl4.SubInfo = "Cần thu hồi";
            this.cardControl4.SubInfoColor = System.Drawing.Color.Gray;
            this.cardControl4.SubInfoFont = new System.Drawing.Font("Segoe UI", 9F);
            this.cardControl4.TabIndex = 3;
            this.cardControl4.Title = "Bao bì chưa thu hồi";
            this.cardControl4.TitleColor = System.Drawing.Color.Gray;
            this.cardControl4.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardControl4.Value = "15 chiếc";
            this.cardControl4.ValueColor = System.Drawing.Color.Black;
            this.cardControl4.ValueFont = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // 
            // DashboardControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.guna2PanelContent);
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(1500, 800);
            this.guna2PanelContent.ResumeLayout(false);
            this.guna2PanelContent.PerformLayout();
            this.tableLayoutPanelBottom.ResumeLayout(false);
            this.tableLayoutPanelBottom.PerformLayout();
            this.panelRecentActivities.ResumeLayout(false);
            this.panelRecentActivities.PerformLayout();
            this.panelTodoList.ResumeLayout(false);
            this.panelTodoList.PerformLayout();
            this.flowLayoutPanelCards.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel guna2PanelContent;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCards;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottom;
        private Guna2Panel panelRecentActivities;
        private Guna2HtmlLabel lblRecent;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutRecent;
        private Guna2Panel panelTodoList;
        private Guna2HtmlLabel lblTodo;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutTodo;
        private CardControl cardControl1;
        private CardControl cardControl2;
        private CardControl cardControl3;
        private CardControl cardControl4;
    }
}