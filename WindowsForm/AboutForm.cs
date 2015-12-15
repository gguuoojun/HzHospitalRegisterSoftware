using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsForm
{
	public class AboutForm : MetroForm
	{
		private System.ComponentModel.IContainer components;

		private LabelX lbMessage;

		private ButtonX btnOK;

		private System.Windows.Forms.LinkLabel lkMail;

		public AboutForm()
		{
			this.InitializeComponent();
		}

		private void lkMail_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.Clipboard.SetDataObject(this.lkMail.Text);
			MessageBoxEx.Show("已复制到剪贴板", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.lbMessage = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.lkMail = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lbMessage
            // 
            this.lbMessage.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbMessage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbMessage.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMessage.ForeColor = System.Drawing.Color.Black;
            this.lbMessage.Location = new System.Drawing.Point(12, 22);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(371, 133);
            this.lbMessage.TabIndex = 0;
            this.lbMessage.Text = "<div>杭州预约挂号辅助软件</div><div> </div><div>软件版本: V1.1.0</div><div></div><div>版权所有:Copy" +
    "right© 2015 日行一米</div><div></div><div></div><div> </div><div>本软件是免费软件,使用过程中如果什么问" +
    "题,请发送</div><div>内容到以下邮箱</div>";
            this.lbMessage.TextLineAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(162, 186);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            // 
            // lkMail
            // 
            this.lkMail.AccessibleRole = System.Windows.Forms.AccessibleRole.Document;
            this.lkMail.AutoSize = true;
            this.lkMail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.lkMail.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lkMail.ForeColor = System.Drawing.Color.Black;
            this.lkMail.LinkColor = System.Drawing.Color.IndianRed;
            this.lkMail.Location = new System.Drawing.Point(98, 157);
            this.lkMail.Name = "lkMail";
            this.lkMail.Size = new System.Drawing.Size(200, 16);
            this.lkMail.TabIndex = 2;
            this.lkMail.TabStop = true;
            this.lkMail.Text = "onemetersupport@yeah.net";
            this.lkMail.Click += new System.EventHandler(this.lkMail_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 217);
            this.Controls.Add(this.lkMail);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbMessage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "关于";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
