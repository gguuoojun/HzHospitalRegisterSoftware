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
			this.lbMessage = new LabelX();
			this.btnOK = new ButtonX();
			this.lkMail = new System.Windows.Forms.LinkLabel();
			base.SuspendLayout();
			this.lbMessage.BackColor = System.Drawing.Color.Transparent;
			this.lbMessage.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbMessage.Font = new System.Drawing.Font("SimSun", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.lbMessage.ForeColor = System.Drawing.Color.Black;
			this.lbMessage.Location = new System.Drawing.Point(12, 22);
			this.lbMessage.Name = "lbMessage";
			this.lbMessage.Size = new System.Drawing.Size(371, 133);
			this.lbMessage.TabIndex = 0;
			this.lbMessage.Text = "<div>杭州预约挂号辅助软件</div><div> </div><div>软件版本: V1.0.0</div><div></div><div>版权所有:Copyright© 2015 日行一米</div><div></div><div></div><div> </div><div>本软件是免费软件,使用过程中如果什么问题,请发送</div><div>内容到以下邮箱</div>";
			this.lbMessage.TextLineAlignment = System.Drawing.StringAlignment.Near;
			this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnOK.ColorTable = eButtonColor.OrangeWithBackground;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(162, 186);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "确定";
			this.lkMail.AccessibleRole = System.Windows.Forms.AccessibleRole.Document;
			this.lkMail.AutoSize = true;
			this.lkMail.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.lkMail.Font = new System.Drawing.Font("SimSun", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.lkMail.ForeColor = System.Drawing.Color.Black;
			this.lkMail.LinkColor = System.Drawing.Color.IndianRed;
			this.lkMail.Location = new System.Drawing.Point(98, 157);
			this.lkMail.Name = "lkMail";
			this.lkMail.Size = new System.Drawing.Size(200, 16);
			this.lkMail.TabIndex = 2;
			this.lkMail.TabStop = true;
			this.lkMail.Text = "onemetersupport@yeah.net";
			this.lkMail.Click += new System.EventHandler(this.lkMail_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(395, 217);
			base.Controls.Add(this.lkMail);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.lbMessage);
			this.DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "关于";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
