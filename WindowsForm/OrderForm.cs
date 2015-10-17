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
	public class OrderForm : MetroForm
	{
		private OrderInfo m_infoOrder;

		private RegisterHelper m_regHelper = RegisterHelper.Instance;

		private string m_selectValue = string.Empty;

		private string m_numId = string.Empty;

		private string m_orderPost = string.Empty;

		private System.ComponentModel.IContainer components;

		private LabelX labelX1;

		private LabelX labelX2;

		private System.Windows.Forms.PictureBox pbAuthCode;

		private TextBoxX tbOrderAuthCode;

		private LabelX labelX3;

		private LabelX labelX4;

		private LabelX labelX5;

		private LabelX labelX6;

		private ListBoxAdv lbxRegTime;

		private LabelX lbDoctorName;

		private LabelX lbHospital;

		private LabelX lbUserName;

		private LabelX lbPhone;

		private LabelX lbCardId;

		private LabelX lbFee;

		private LabelX labelX7;

		private LabelX lbVisitTime;

		private LabelX lbMessage;

		private LabelX labelX8;

		public OrderForm(OrderInfo infoOrder)
		{
			this.InitializeComponent();
			this.m_infoOrder = infoOrder;
		}

		private void OrderForm_Load(object sender, System.EventArgs e)
		{
			this.lbDoctorName.Text = this.m_infoOrder.Doctor.Name;
			this.lbHospital.Text = this.m_infoOrder.Doctor.HospitalName + "  " + this.m_infoOrder.Doctor.Department;
			this.lbUserName.Text = this.m_infoOrder.User.Name;
			this.lbPhone.Text = this.m_infoOrder.User.PhoneNumber;
			this.lbCardId.Text = this.m_infoOrder.User.CardId;
			this.lbVisitTime.Text = this.m_infoOrder.VisitDate;
			this.lbFee.Text = "<font color=\"OrangeRed\">" + this.m_infoOrder.Fee + "</font>";
			foreach (VisitTime current in this.m_infoOrder.VisitTimes)
			{
				ListBoxItem listBoxItem = new ListBoxItem();
				listBoxItem.Text = string.Format("序号:{0}  时间:{1}", current.Index, current.Time);
				listBoxItem.Tag = current;
				this.lbxRegTime.Items.Add(listBoxItem);
			}
		}

		private void tbOrderAuthCode_TextChanged(object sender, System.EventArgs e)
		{
			if (this.tbOrderAuthCode.Text.Length == 5)
			{
				this.lbMessage.Text = string.Empty;
				if (this.m_regHelper.CheckOrderCode(this.m_infoOrder.Doctor.HospitalId, this.m_numId, this.tbOrderAuthCode.Text) != ResponseReuslt.SUCCESS)
				{
					this.lbMessage.Text = "验证码输入错误";
					this.tbOrderAuthCode.Text = string.Empty;
					this.pbAuthCode.Image = this.m_regHelper.GetOrderCode(this.m_infoOrder.Doctor.HospitalId, this.m_numId);
					return;
				}
				this.lbMessage.Text = "提交中...";
				OrderSuccessInfo orderSuccessInfo = this.m_regHelper.OrderSave(this.m_orderPost + "code=" + this.tbOrderAuthCode.Text);
				if (orderSuccessInfo.ResResult != ResponseReuslt.SUCCESS)
				{
					this.lbMessage.Text = "验证码校验失败";
					this.tbOrderAuthCode.Text = string.Empty;
					this.pbAuthCode.Image = this.m_regHelper.GetOrderCode(this.m_infoOrder.Doctor.HospitalId, this.m_numId);
					return;
				}
				base.Hide();
				this.lbMessage.Text = "提交成功";
				SuccessForm successForm = new SuccessForm(orderSuccessInfo);
				successForm.ShowDialog();
				successForm.Dispose();
				base.Close();
			}
		}

		private void tbOrderAuthCode_Enter(object sender, System.EventArgs e)
		{
			if (this.lbxRegTime.SelectedIndex >= 0 && this.m_selectValue != this.m_numId)
			{
				this.m_orderPost = this.m_regHelper.CheckOrder(this.m_infoOrder, (VisitTime)((ListBoxItem)this.lbxRegTime.SelectedItem).Tag);
				if (this.m_orderPost.Length == 0)
				{
					this.lbMessage.Text = "订单验证失败,请刷新验证码";
					return;
				}
				this.tbOrderAuthCode.Text = string.Empty;
				this.lbMessage.Text = string.Empty;
				this.pbAuthCode.Image = this.m_regHelper.GetOrderCode(this.m_infoOrder.Doctor.HospitalId, this.m_numId);
				this.m_selectValue = this.m_numId;
			}
		}

		private void pbAuthCode_Click(object sender, System.EventArgs e)
		{
			if (this.lbxRegTime.SelectedIndex < 0)
			{
				this.lbMessage.Text = "请选择具体时间后再点击刷新验证码";
				return;
			}
			this.lbMessage.Text = string.Empty;
			if (this.m_selectValue != this.m_numId)
			{
				this.m_orderPost = this.m_regHelper.CheckOrder(this.m_infoOrder, (VisitTime)((ListBoxItem)this.lbxRegTime.SelectedItem).Tag);
				if (this.m_orderPost.Length == 0)
				{
					this.lbMessage.Text = "订单验证失败,请刷新验证码";
					return;
				}
				this.m_selectValue = this.m_numId;
			}
			this.pbAuthCode.Image = this.m_regHelper.GetOrderCode(this.m_infoOrder.Doctor.HospitalId, this.m_numId);
		}

		private void lbxRegTime_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.m_numId = ((VisitTime)((ListBoxItem)this.lbxRegTime.SelectedItem).Tag).NumId;
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
			this.labelX1 = new LabelX();
			this.labelX2 = new LabelX();
			this.pbAuthCode = new System.Windows.Forms.PictureBox();
			this.tbOrderAuthCode = new TextBoxX();
			this.labelX3 = new LabelX();
			this.labelX4 = new LabelX();
			this.labelX5 = new LabelX();
			this.labelX6 = new LabelX();
			this.lbxRegTime = new ListBoxAdv();
			this.lbDoctorName = new LabelX();
			this.lbHospital = new LabelX();
			this.lbUserName = new LabelX();
			this.lbPhone = new LabelX();
			this.lbCardId = new LabelX();
			this.lbFee = new LabelX();
			this.labelX7 = new LabelX();
			this.lbVisitTime = new LabelX();
			this.lbMessage = new LabelX();
			this.labelX8 = new LabelX();
			((System.ComponentModel.ISupportInitialize)this.pbAuthCode).BeginInit();
			base.SuspendLayout();
			this.labelX1.BackColor = System.Drawing.Color.Transparent;
			this.labelX1.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX1.ForeColor = System.Drawing.Color.Black;
			this.labelX1.Location = new System.Drawing.Point(36, 69);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(75, 23);
			this.labelX1.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX1.TabIndex = 0;
			this.labelX1.Text = "就诊人：";
			this.labelX2.BackColor = System.Drawing.Color.Transparent;
			this.labelX2.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX2.ForeColor = System.Drawing.Color.Black;
			this.labelX2.Location = new System.Drawing.Point(28, 399);
			this.labelX2.Name = "labelX2";
			this.labelX2.Size = new System.Drawing.Size(75, 23);
			this.labelX2.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX2.TabIndex = 3;
			this.labelX2.Text = "验证码：";
			this.pbAuthCode.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.pbAuthCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pbAuthCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbAuthCode.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbAuthCode.ForeColor = System.Drawing.Color.Black;
			this.pbAuthCode.Location = new System.Drawing.Point(191, 380);
			this.pbAuthCode.Name = "pbAuthCode";
			this.pbAuthCode.Size = new System.Drawing.Size(142, 53);
			this.pbAuthCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbAuthCode.TabIndex = 4;
			this.pbAuthCode.TabStop = false;
			this.pbAuthCode.Click += new System.EventHandler(this.pbAuthCode_Click);
			this.tbOrderAuthCode.BackColor = System.Drawing.Color.White;
			this.tbOrderAuthCode.Border.Class = "TextBoxBorder";
			this.tbOrderAuthCode.Border.CornerType = eCornerType.Square;
			this.tbOrderAuthCode.DisabledBackColor = System.Drawing.Color.White;
			this.tbOrderAuthCode.ForeColor = System.Drawing.Color.Black;
			this.tbOrderAuthCode.Location = new System.Drawing.Point(92, 398);
			this.tbOrderAuthCode.Name = "tbOrderAuthCode";
			this.tbOrderAuthCode.PreventEnterBeep = true;
			this.tbOrderAuthCode.Size = new System.Drawing.Size(93, 23);
			this.tbOrderAuthCode.TabIndex = 5;
			this.tbOrderAuthCode.TextChanged += new System.EventHandler(this.tbOrderAuthCode_TextChanged);
			this.tbOrderAuthCode.Enter += new System.EventHandler(this.tbOrderAuthCode_Enter);
			this.labelX3.BackColor = System.Drawing.Color.Transparent;
			this.labelX3.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX3.ForeColor = System.Drawing.Color.Black;
			this.labelX3.Location = new System.Drawing.Point(321, 69);
			this.labelX3.Name = "labelX3";
			this.labelX3.Size = new System.Drawing.Size(75, 23);
			this.labelX3.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX3.TabIndex = 6;
			this.labelX3.Text = "证件号：";
			this.labelX4.BackColor = System.Drawing.Color.Transparent;
			this.labelX4.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX4.ForeColor = System.Drawing.Color.Black;
			this.labelX4.Location = new System.Drawing.Point(27, 98);
			this.labelX4.Name = "labelX4";
			this.labelX4.Size = new System.Drawing.Size(84, 23);
			this.labelX4.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX4.TabIndex = 7;
			this.labelX4.Text = "联系电话：";
			this.labelX5.BackColor = System.Drawing.Color.Transparent;
			this.labelX5.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX5.ForeColor = System.Drawing.Color.Black;
			this.labelX5.Location = new System.Drawing.Point(323, 98);
			this.labelX5.Name = "labelX5";
			this.labelX5.Size = new System.Drawing.Size(72, 23);
			this.labelX5.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX5.TabIndex = 8;
			this.labelX5.Text = "挂号费：";
			this.labelX6.BackColor = System.Drawing.Color.Transparent;
			this.labelX6.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX6.ForeColor = System.Drawing.Color.Black;
			this.labelX6.Location = new System.Drawing.Point(36, 16);
			this.labelX6.Name = "labelX6";
			this.labelX6.Size = new System.Drawing.Size(84, 23);
			this.labelX6.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX6.TabIndex = 9;
			this.labelX6.Text = "预约医生：";
			this.lbxRegTime.AutoScroll = true;
			this.lbxRegTime.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.lbxRegTime.BackgroundStyle.Class = "ListBoxAdv";
			this.lbxRegTime.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbxRegTime.ContainerControlProcessDialogKey = true;
			this.lbxRegTime.DragDropSupport = true;
			this.lbxRegTime.ForeColor = System.Drawing.Color.Black;
			this.lbxRegTime.Location = new System.Drawing.Point(24, 174);
			this.lbxRegTime.Name = "lbxRegTime";
			this.lbxRegTime.Size = new System.Drawing.Size(581, 200);
			this.lbxRegTime.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbxRegTime.TabIndex = 11;
			this.lbxRegTime.Text = "listBoxAdv1";
			this.lbxRegTime.SelectedIndexChanged += new System.EventHandler(this.lbxRegTime_SelectedIndexChanged);
			this.lbDoctorName.BackColor = System.Drawing.Color.Transparent;
			this.lbDoctorName.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDoctorName.ForeColor = System.Drawing.Color.Black;
			this.lbDoctorName.Location = new System.Drawing.Point(111, 16);
			this.lbDoctorName.Name = "lbDoctorName";
			this.lbDoctorName.Size = new System.Drawing.Size(127, 23);
			this.lbDoctorName.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbDoctorName.TabIndex = 12;
			this.lbHospital.BackColor = System.Drawing.Color.Transparent;
			this.lbHospital.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbHospital.ForeColor = System.Drawing.Color.Black;
			this.lbHospital.Location = new System.Drawing.Point(111, 42);
			this.lbHospital.Name = "lbHospital";
			this.lbHospital.Size = new System.Drawing.Size(464, 23);
			this.lbHospital.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbHospital.TabIndex = 13;
			this.lbUserName.BackColor = System.Drawing.Color.Transparent;
			this.lbUserName.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbUserName.ForeColor = System.Drawing.Color.Black;
			this.lbUserName.Location = new System.Drawing.Point(111, 69);
			this.lbUserName.Name = "lbUserName";
			this.lbUserName.Size = new System.Drawing.Size(127, 23);
			this.lbUserName.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbUserName.TabIndex = 14;
			this.lbPhone.BackColor = System.Drawing.Color.Transparent;
			this.lbPhone.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbPhone.ForeColor = System.Drawing.Color.Black;
			this.lbPhone.Location = new System.Drawing.Point(111, 98);
			this.lbPhone.Name = "lbPhone";
			this.lbPhone.Size = new System.Drawing.Size(127, 23);
			this.lbPhone.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbPhone.TabIndex = 15;
			this.lbCardId.BackColor = System.Drawing.Color.Transparent;
			this.lbCardId.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbCardId.ForeColor = System.Drawing.Color.Black;
			this.lbCardId.Location = new System.Drawing.Point(384, 69);
			this.lbCardId.Name = "lbCardId";
			this.lbCardId.Size = new System.Drawing.Size(191, 23);
			this.lbCardId.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbCardId.TabIndex = 16;
			this.lbFee.BackColor = System.Drawing.Color.Transparent;
			this.lbFee.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbFee.Font = new System.Drawing.Font("Microsoft YaHei", 10.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.lbFee.ForeColor = System.Drawing.Color.Black;
			this.lbFee.Location = new System.Drawing.Point(384, 98);
			this.lbFee.Name = "lbFee";
			this.lbFee.Size = new System.Drawing.Size(127, 23);
			this.lbFee.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbFee.TabIndex = 17;
			this.labelX7.BackColor = System.Drawing.Color.Transparent;
			this.labelX7.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX7.ForeColor = System.Drawing.Color.Black;
			this.labelX7.Location = new System.Drawing.Point(27, 127);
			this.labelX7.Name = "labelX7";
			this.labelX7.Size = new System.Drawing.Size(84, 23);
			this.labelX7.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX7.TabIndex = 18;
			this.labelX7.Text = "就诊时间：";
			this.lbVisitTime.BackColor = System.Drawing.Color.Transparent;
			this.lbVisitTime.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbVisitTime.ForeColor = System.Drawing.Color.Black;
			this.lbVisitTime.Location = new System.Drawing.Point(111, 126);
			this.lbVisitTime.Name = "lbVisitTime";
			this.lbVisitTime.Size = new System.Drawing.Size(194, 23);
			this.lbVisitTime.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbVisitTime.TabIndex = 19;
			this.lbMessage.BackColor = System.Drawing.Color.Transparent;
			this.lbMessage.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbMessage.ForeColor = System.Drawing.Color.Black;
			this.lbMessage.Location = new System.Drawing.Point(339, 389);
			this.lbMessage.Name = "lbMessage";
			this.lbMessage.Size = new System.Drawing.Size(274, 35);
			this.lbMessage.Style = eDotNetBarStyle.StyleManagerControlled;
			this.lbMessage.TabIndex = 20;
			this.labelX8.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX8.Location = new System.Drawing.Point(25, 151);
			this.labelX8.Name = "labelX8";
			this.labelX8.Size = new System.Drawing.Size(280, 23);
			this.labelX8.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX8.TabIndex = 22;
			this.labelX8.Text = "<font color=\"Red\">（请在以下时间列表中选择后填写验证码）</font>";
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(625, 438);
			base.Controls.Add(this.labelX8);
			base.Controls.Add(this.lbMessage);
			base.Controls.Add(this.lbVisitTime);
			base.Controls.Add(this.labelX7);
			base.Controls.Add(this.lbFee);
			base.Controls.Add(this.lbCardId);
			base.Controls.Add(this.lbPhone);
			base.Controls.Add(this.lbUserName);
			base.Controls.Add(this.lbHospital);
			base.Controls.Add(this.lbDoctorName);
			base.Controls.Add(this.lbxRegTime);
			base.Controls.Add(this.labelX6);
			base.Controls.Add(this.labelX5);
			base.Controls.Add(this.labelX4);
			base.Controls.Add(this.labelX3);
			base.Controls.Add(this.tbOrderAuthCode);
			base.Controls.Add(this.pbAuthCode);
			base.Controls.Add(this.labelX2);
			base.Controls.Add(this.labelX1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("SimSun", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "OrderForm";
			base.ShowIcon = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "订单信息";
			base.Load += new System.EventHandler(this.OrderForm_Load);
			((System.ComponentModel.ISupportInitialize)this.pbAuthCode).EndInit();
			base.ResumeLayout(false);
		}
	}
}
