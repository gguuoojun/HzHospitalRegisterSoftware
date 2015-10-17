using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using WindowsForm.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsForm
{
	public class DonateForm : MetroForm
	{
		private System.ComponentModel.IContainer components;

		private ButtonX btnClose;

		private LabelX labelX1;

		private System.Windows.Forms.PictureBox pictureBox1;

		private LabelX labelX2;

		public DonateForm()
		{
			this.InitializeComponent();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			base.Close();
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
			this.btnClose = new ButtonX();
			this.labelX1 = new LabelX();
			this.labelX2 = new LabelX();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnClose.ColorTable = eButtonColor.OrangeWithBackground;
			this.btnClose.Location = new System.Drawing.Point(268, 361);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 26);
			this.btnClose.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "谢谢支持";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			this.labelX1.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX1.Font = new System.Drawing.Font("SimSun", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.labelX1.Location = new System.Drawing.Point(26, 13);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(573, 49);
			this.labelX1.TabIndex = 1;
			this.labelX1.Text = "支付宝手机钱包扫描以下二维码捐助";
			this.labelX2.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX2.Font = new System.Drawing.Font("SimSun", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.labelX2.ForeColor = System.Drawing.SystemColors.Desktop;
			this.labelX2.Location = new System.Drawing.Point(44, 322);
			this.labelX2.Name = "labelX2";
			this.labelX2.Size = new System.Drawing.Size(203, 39);
			this.labelX2.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX2.TabIndex = 3;
			this.labelX2.Text = "<font color=\"DarkOliveGreen\">作者QQ：364250783</font>";
            this.pictureBox1.Image = HzHospitalRegister.Properties.Resources.alipay;
			this.pictureBox1.Location = new System.Drawing.Point(198, 64);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(250, 250);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(625, 396);
			base.Controls.Add(this.labelX2);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.labelX1);
			base.Controls.Add(this.btnClose);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DonateForm";
			base.ShowIcon = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "捐助软件";
			((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
