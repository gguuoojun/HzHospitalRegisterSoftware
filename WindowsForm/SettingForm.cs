using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Model;
using HzHospitalRegister;

namespace WindowsForm
{
	public class SettingForm : MetroForm
	{
		private RegSetting m_regSetting = RegSetting.Instance;

		private System.ComponentModel.IContainer components;

		private ButtonX btnOK;

		private ButtonX btnCancel;

		private TextBoxX tbSoundPath;

		private GroupPanel groupPanel1;

		private LabelX labelX1;

		private GroupPanel groupPanel2;

		private System.Windows.Forms.RadioButton rbHide;

		private System.Windows.Forms.RadioButton rbExit;

		private ButtonX btnBrowse;

		public SettingForm()
		{
			this.InitializeComponent();
		}

		protected override void OnLoad(System.EventArgs e)
		{
			this.tbSoundPath.Text = this.m_regSetting.SoundPath;
			this.rbExit.Checked = !this.m_regSetting.HideWindows;
			this.rbHide.Checked = this.m_regSetting.HideWindows;
			base.OnLoad(e);
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.m_regSetting.HideWindows = this.rbHide.Checked;
			this.m_regSetting.SoundPath = this.tbSoundPath.Text;
			this.m_regSetting.WriteSetting();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			base.Close();
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			if (this.tbSoundPath.Text.Length == 0)
			{
				openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
			}
			else
			{
				openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.tbSoundPath.Text);
			}
			openFileDialog.Filter = "Wav文件(*.wav)|*.wav";
			openFileDialog.RestoreDirectory = true;
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.tbSoundPath.Text = openFileDialog.FileName;
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
			this.btnOK = new ButtonX();
			this.btnCancel = new ButtonX();
			this.tbSoundPath = new TextBoxX();
			this.groupPanel1 = new GroupPanel();
			this.labelX1 = new LabelX();
			this.groupPanel2 = new GroupPanel();
			this.rbExit = new System.Windows.Forms.RadioButton();
			this.rbHide = new System.Windows.Forms.RadioButton();
			this.btnBrowse = new ButtonX();
			this.groupPanel1.SuspendLayout();
			this.groupPanel2.SuspendLayout();
			base.SuspendLayout();
			this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnOK.ColorTable = eButtonColor.OrangeWithBackground;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(245, 238);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "确定";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnCancel.ColorTable = eButtonColor.OrangeWithBackground;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(364, 238);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "取消";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.tbSoundPath.BackColor = System.Drawing.Color.White;
			this.tbSoundPath.Border.Class = "TextBoxBorder";
			this.tbSoundPath.Border.CornerType = eCornerType.Square;
			this.tbSoundPath.DisabledBackColor = System.Drawing.Color.White;
			this.tbSoundPath.ForeColor = System.Drawing.Color.Black;
			this.tbSoundPath.Location = new System.Drawing.Point(83, 9);
			this.tbSoundPath.Name = "tbSoundPath";
			this.tbSoundPath.PreventEnterBeep = true;
			this.tbSoundPath.ReadOnly = true;
			this.tbSoundPath.Size = new System.Drawing.Size(297, 21);
			this.tbSoundPath.TabIndex = 2;
			this.groupPanel1.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.groupPanel1.CanvasColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.groupPanel1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			this.groupPanel1.Controls.Add(this.btnBrowse);
			this.groupPanel1.Controls.Add(this.labelX1);
			this.groupPanel1.Controls.Add(this.tbSoundPath);
			this.groupPanel1.DisabledBackColor = System.Drawing.Color.Empty;
			this.groupPanel1.Location = new System.Drawing.Point(12, 12);
			this.groupPanel1.Name = "groupPanel1";
			this.groupPanel1.Size = new System.Drawing.Size(438, 76);
			this.groupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
			this.groupPanel1.Style.BackColorGradientAngle = 90;
			this.groupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
			this.groupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
			this.groupPanel1.Style.BorderBottomWidth = 1;
			this.groupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
			this.groupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
			this.groupPanel1.Style.BorderLeftWidth = 1;
			this.groupPanel1.Style.BorderRight = eStyleBorderType.Solid;
			this.groupPanel1.Style.BorderRightWidth = 1;
			this.groupPanel1.Style.BorderTop = eStyleBorderType.Solid;
			this.groupPanel1.Style.BorderTopWidth = 1;
			this.groupPanel1.Style.CornerDiameter = 4;
			this.groupPanel1.Style.CornerType = eCornerType.Rounded;
			this.groupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
			this.groupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
			this.groupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
			this.groupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
			this.groupPanel1.TabIndex = 3;
			this.groupPanel1.Text = "声音提醒";
			this.labelX1.BackColor = System.Drawing.Color.Transparent;
			this.labelX1.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX1.ForeColor = System.Drawing.Color.Black;
			this.labelX1.Location = new System.Drawing.Point(15, 9);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(68, 23);
			this.labelX1.Style = eDotNetBarStyle.StyleManagerControlled;
			this.labelX1.TabIndex = 3;
			this.labelX1.Text = "声音路径：";
			this.groupPanel2.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.groupPanel2.CanvasColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.groupPanel2.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			this.groupPanel2.Controls.Add(this.rbHide);
			this.groupPanel2.Controls.Add(this.rbExit);
			this.groupPanel2.DisabledBackColor = System.Drawing.Color.Empty;
			this.groupPanel2.Location = new System.Drawing.Point(12, 102);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.Size = new System.Drawing.Size(438, 100);
			this.groupPanel2.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
			this.groupPanel2.Style.BackColorGradientAngle = 90;
			this.groupPanel2.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
			this.groupPanel2.Style.BorderBottom = eStyleBorderType.Solid;
			this.groupPanel2.Style.BorderBottomWidth = 1;
			this.groupPanel2.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
			this.groupPanel2.Style.BorderLeft = eStyleBorderType.Solid;
			this.groupPanel2.Style.BorderLeftWidth = 1;
			this.groupPanel2.Style.BorderRight = eStyleBorderType.Solid;
			this.groupPanel2.Style.BorderRightWidth = 1;
			this.groupPanel2.Style.BorderTop = eStyleBorderType.Solid;
			this.groupPanel2.Style.BorderTopWidth = 1;
			this.groupPanel2.Style.CornerDiameter = 4;
			this.groupPanel2.Style.CornerType = eCornerType.Rounded;
			this.groupPanel2.Style.TextColorSchemePart = eColorSchemePart.PanelText;
			this.groupPanel2.Style.TextLineAlignment = eStyleTextAlignment.Near;
			this.groupPanel2.StyleMouseDown.CornerType = eCornerType.Square;
			this.groupPanel2.StyleMouseOver.CornerType = eCornerType.Square;
			this.groupPanel2.TabIndex = 4;
			this.groupPanel2.Text = "窗口关闭";
			this.rbExit.AutoSize = true;
			this.rbExit.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.rbExit.ForeColor = System.Drawing.Color.Black;
			this.rbExit.Location = new System.Drawing.Point(15, 16);
			this.rbExit.Name = "rbExit";
			this.rbExit.Size = new System.Drawing.Size(95, 16);
			this.rbExit.TabIndex = 0;
			this.rbExit.Text = "直接退出程序";
			this.rbExit.UseVisualStyleBackColor = false;
			this.rbHide.AutoSize = true;
			this.rbHide.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			this.rbHide.Checked = true;
			this.rbHide.ForeColor = System.Drawing.Color.Black;
			this.rbHide.Location = new System.Drawing.Point(15, 48);
			this.rbHide.Name = "rbHide";
			this.rbHide.Size = new System.Drawing.Size(83, 16);
			this.rbHide.TabIndex = 1;
			this.rbHide.TabStop = true;
			this.rbHide.Text = "隐藏到托盘";
			this.rbHide.UseVisualStyleBackColor = false;
			this.btnBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnBrowse.ColorTable = eButtonColor.OrangeWithBackground;
			this.btnBrowse.Location = new System.Drawing.Point(387, 8);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(37, 23);
			this.btnBrowse.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnBrowse.TabIndex = 4;
			this.btnBrowse.Text = "...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(462, 279);
			base.Controls.Add(this.groupPanel2);
			base.Controls.Add(this.groupPanel1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			this.DoubleBuffered = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SettingForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "设置";
			this.groupPanel1.ResumeLayout(false);
			this.groupPanel2.ResumeLayout(false);
			this.groupPanel2.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
