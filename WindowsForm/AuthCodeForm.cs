using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Model;
using HzHospitalRegister;

namespace WindowsForm
{
	public class AuthCodeForm : MetroForm
	{
		private RegisterHelper _regHelper = RegisterHelper.Instance;

		private string m_userName;

		private string m_passWd;

		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.PictureBox pbAuthCode;

		private TextBoxX tbAuthCode;

		public AuthCodeForm()
		{
			this.InitializeComponent();
		}

		public void SetLoginValue(string userName, string passWd)
		{
			this.m_userName = userName;
			this.m_passWd = passWd;
		}

		protected override void OnLoad(System.EventArgs e)
		{
			this.pbAuthCode.Image = this._regHelper.GetAuthCode();
			base.OnLoad(e);
		}

		private void pbAuthCode_Click(object sender, System.EventArgs e)
		{
			this.pbAuthCode.Image = this._regHelper.GetAuthCode();
		}

		private void tbAuthCode_TextChanged(object sender, System.EventArgs e)
		{
			if (this.tbAuthCode.Text.Length == 4)
			{
				string text = this._regHelper.Login(this.m_userName, this.m_passWd, this.tbAuthCode.Text);
				if (text.Length == 0)
				{
					base.DialogResult = System.Windows.Forms.DialogResult.Yes;
					base.Close();
					return;
				}
				if (text == "您输入的验证码有误!")
				{
					MessageBoxEx.Show(text, "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
					this.tbAuthCode.Text = string.Empty;
					this.pbAuthCode.Image = this._regHelper.GetAuthCode();
					return;
				}
				MessageBoxEx.Show(text, "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
				base.DialogResult = System.Windows.Forms.DialogResult.No;
				base.Close();
			}
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
			this.pbAuthCode = new System.Windows.Forms.PictureBox();
			this.tbAuthCode = new TextBoxX();
			((System.ComponentModel.ISupportInitialize)this.pbAuthCode).BeginInit();
			base.SuspendLayout();
			this.pbAuthCode.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.pbAuthCode.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbAuthCode.ForeColor = System.Drawing.Color.Black;
			this.pbAuthCode.Location = new System.Drawing.Point(25, 19);
			this.pbAuthCode.Name = "pbAuthCode";
			this.pbAuthCode.Size = new System.Drawing.Size(144, 50);
			this.pbAuthCode.TabIndex = 2;
			this.pbAuthCode.TabStop = false;
			this.pbAuthCode.Click += new System.EventHandler(this.pbAuthCode_Click);
			this.tbAuthCode.BackColor = System.Drawing.Color.White;
			this.tbAuthCode.Border.Class = "TextBoxBorder";
			this.tbAuthCode.Border.CornerType = eCornerType.Square;
			this.tbAuthCode.DisabledBackColor = System.Drawing.Color.White;
			this.tbAuthCode.Font = new System.Drawing.Font("SimSun", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.tbAuthCode.ForeColor = System.Drawing.Color.Black;
			this.tbAuthCode.Location = new System.Drawing.Point(189, 48);
			this.tbAuthCode.Name = "tbAuthCode";
			this.tbAuthCode.PreventEnterBeep = true;
			this.tbAuthCode.Size = new System.Drawing.Size(100, 21);
			this.tbAuthCode.TabIndex = 3;
			this.tbAuthCode.TextChanged += new System.EventHandler(this.tbAuthCode_TextChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(324, 92);
			base.Controls.Add(this.tbAuthCode);
			base.Controls.Add(this.pbAuthCode);
			this.DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AuthCodeForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "请输入验证码";
			((System.ComponentModel.ISupportInitialize)this.pbAuthCode).EndInit();
			base.ResumeLayout(false);
		}
	}
}
