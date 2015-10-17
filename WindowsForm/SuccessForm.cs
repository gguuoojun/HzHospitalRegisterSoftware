using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Model;

namespace WindowsForm
{
	public class SuccessForm : MetroForm
	{
		private OrderSuccessInfo m_infoOrderSuccess;

		private System.ComponentModel.IContainer components;

		private ButtonX btnDonate;

		private ButtonX btnOK;

		private LabelX labelX1;

		private LabelX lbPasswd;

		private LabelX labelX3;

		private LabelX lbPhone;

		private LabelX labelX5;

		private LabelX labelX6;

		private LabelX lbVisitTime;

		private LabelX lbVisitNum;

		public SuccessForm(OrderSuccessInfo infoOrderSuccess)
		{
			this.InitializeComponent();
			this.m_infoOrderSuccess = infoOrderSuccess;
		}

		protected override void OnLoad(System.EventArgs e)
		{
			this.lbPasswd.Text = "<font color=\"OrangeRed\">" + this.m_infoOrderSuccess.Passwd + "</font>";
			this.lbPhone.Text = "<font color=\"OrangeRed\">" + this.m_infoOrderSuccess.Phone + "</font>";
			this.lbVisitTime.Text = "<font color=\"LightSeaGreen\">" + this.m_infoOrderSuccess.ResTime + "</font>";
			this.lbVisitNum.Text = "<font color=\"LightSeaGreen\">" + this.m_infoOrderSuccess.ResNum + "</font>";
			base.OnLoad(e);
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			base.Close();
		}

		private void btnDonate_Click(object sender, System.EventArgs e)
		{
			base.Hide();
			DonateForm donateForm = new DonateForm();
			donateForm.ShowDialog();
			donateForm.Dispose();
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
			this.btnDonate = new ButtonX();
			this.btnOK = new ButtonX();
			this.labelX1 = new LabelX();
			this.lbPasswd = new LabelX();
			this.labelX3 = new LabelX();
			this.lbPhone = new LabelX();
			this.labelX5 = new LabelX();
			this.labelX6 = new LabelX();
			this.lbVisitTime = new LabelX();
			this.lbVisitNum = new LabelX();
			base.SuspendLayout();
			this.btnDonate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnDonate.ColorTable = eButtonColor.OrangeWithBackground;
			this.btnDonate.Font = new System.Drawing.Font("Microsoft YaHei", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.btnDonate.Location = new System.Drawing.Point(143, 223);
			this.btnDonate.Name = "btnDonate";
			this.btnDonate.Size = new System.Drawing.Size(86, 34);
			this.btnDonate.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnDonate.TabIndex = 0;
			this.btnDonate.Text = "捐助软件";
			this.btnDonate.Click += new System.EventHandler(this.btnDonate_Click);
			this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnOK.ColorTable = eButtonColor.OrangeWithBackground;
			this.btnOK.Font = new System.Drawing.Font("Microsoft YaHei", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.btnOK.Location = new System.Drawing.Point(360, 223);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(86, 34);
			this.btnOK.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "不，谢谢";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.labelX1.BackColor = System.Drawing.Color.Transparent;
			this.labelX1.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX1.Location = new System.Drawing.Point(61, 29);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(75, 23);
			this.labelX1.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX1.TabIndex = 2;
			this.labelX1.Text = "取号密码：";
			this.lbPasswd.BackColor = System.Drawing.Color.Transparent;
			this.lbPasswd.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbPasswd.Font = new System.Drawing.Font("Microsoft YaHei", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.lbPasswd.ForeColor = System.Drawing.Color.OrangeRed;
			this.lbPasswd.Location = new System.Drawing.Point(143, 12);
			this.lbPasswd.Name = "lbPasswd";
			this.lbPasswd.Size = new System.Drawing.Size(152, 51);
			this.lbPasswd.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbPasswd.TabIndex = 3;
			this.lbPasswd.Text = "76548786";
			this.labelX3.BackColor = System.Drawing.Color.Transparent;
			this.labelX3.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX3.Location = new System.Drawing.Point(61, 75);
			this.labelX3.Name = "labelX3";
			this.labelX3.Size = new System.Drawing.Size(192, 23);
			this.labelX3.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX3.TabIndex = 4;
			this.labelX3.Text = "预约成功短信已经发送至手机：";
			this.lbPhone.BackColor = System.Drawing.Color.Transparent;
			this.lbPhone.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbPhone.Font = new System.Drawing.Font("Microsoft YaHei", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.lbPhone.ForeColor = System.Drawing.Color.OrangeRed;
			this.lbPhone.Location = new System.Drawing.Point(244, 55);
			this.lbPhone.Name = "lbPhone";
			this.lbPhone.Size = new System.Drawing.Size(291, 51);
			this.lbPhone.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbPhone.TabIndex = 5;
			this.lbPhone.Text = "76548786";
			this.labelX5.BackColor = System.Drawing.Color.Transparent;
			this.labelX5.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX5.Location = new System.Drawing.Point(61, 124);
			this.labelX5.Name = "labelX5";
			this.labelX5.Size = new System.Drawing.Size(192, 23);
			this.labelX5.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX5.TabIndex = 6;
			this.labelX5.Text = "您的就诊时间：";
			this.labelX6.BackColor = System.Drawing.Color.Transparent;
			this.labelX6.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX6.Location = new System.Drawing.Point(61, 175);
			this.labelX6.Name = "labelX6";
			this.labelX6.Size = new System.Drawing.Size(192, 23);
			this.labelX6.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX6.TabIndex = 7;
			this.labelX6.Text = "您的就诊序号：";
			this.lbVisitTime.BackColor = System.Drawing.Color.Transparent;
			this.lbVisitTime.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbVisitTime.Font = new System.Drawing.Font("Microsoft YaHei", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.lbVisitTime.ForeColor = System.Drawing.Color.LightSeaGreen;
			this.lbVisitTime.Location = new System.Drawing.Point(172, 104);
			this.lbVisitTime.Name = "lbVisitTime";
			this.lbVisitTime.Size = new System.Drawing.Size(332, 51);
			this.lbVisitTime.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbVisitTime.TabIndex = 8;
			this.lbVisitTime.Text = "76548786";
			this.lbVisitNum.BackColor = System.Drawing.Color.Transparent;
			this.lbVisitNum.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbVisitNum.Font = new System.Drawing.Font("Microsoft YaHei", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.lbVisitNum.ForeColor = System.Drawing.Color.LightSeaGreen;
			this.lbVisitNum.Location = new System.Drawing.Point(172, 161);
			this.lbVisitNum.Name = "lbVisitNum";
			this.lbVisitNum.Size = new System.Drawing.Size(332, 51);
			this.lbVisitNum.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbVisitNum.TabIndex = 9;
			this.lbVisitNum.Text = "76548786";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(600, 276);
			base.Controls.Add(this.lbVisitNum);
			base.Controls.Add(this.lbVisitTime);
			base.Controls.Add(this.labelX6);
			base.Controls.Add(this.labelX5);
			base.Controls.Add(this.lbPhone);
			base.Controls.Add(this.labelX3);
			base.Controls.Add(this.lbPasswd);
			base.Controls.Add(this.labelX1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnDonate);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SuccessForm";
			base.ShowIcon = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "恭喜您已预约成功！";
			base.ResumeLayout(false);
		}
	}
}
