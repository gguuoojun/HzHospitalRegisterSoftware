using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Metro.ColorTables;
using DevComponents.Editors;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Utility;
using Model;
using HzHospitalRegister;

namespace WindowsForm
{
    public delegate void Action();

	public class MainForm : MetroAppForm
	{   
		private RegisterHelper _regHelper = RegisterHelper.Instance;

		private System.TimeSpan m_spanServer;

		private BeijingTime m_bjTime = BeijingTime.Instance;

		private System.Threading.Thread m_tdManager;

		private System.Diagnostics.Process _process;

		//private bool m_bIsRefreshTime;

		private System.ComponentModel.IContainer components;

		private StyleManager styleManager;

		private ComboItem comboItem1;

		private ComboItem comboItem2;

		private ComboItem comboItem3;

		private ComboItem comboItem4;

		private ComboItem comboItem5;

		private ComboItem comboItem6;

		private ComboItem comboItem7;

		private ComboItem comboItem8;

		private ComboItem comboItem9;

		private ComboItem comboItem10;

		private ComboItem comboItem11;

		private ComboItem comboItem12;

		private ComboItem comboItem13;

		private ComboItem comboItem14;

		private ComboItem comboItem15;

		private ComboItem comboItem31;

		private ComboItem comboItem16;

		private ComboItem comboItem17;

		private ComboItem comboItem18;

		private ComboItem comboItem19;

		private ComboItem comboItem20;

		private ComboItem comboItem21;

		private ComboItem comboItem22;

		private ComboItem comboItem23;

		private ComboItem comboItem24;

		private ComboItem comboItem25;

		private ComboItem comboItem26;

		private ComboItem comboItem27;

		private ComboItem comboItem28;

		private ComboItem comboItem29;

		private ComboItem comboItem30;

		private ComboItem comboItem32;

		private ComboItem comboItem33;

		private ComboItem comboItem34;

		private ComboItem comboItem35;

		private ComboItem comboItem36;

		private ComboItem comboItem37;

		private ComboItem comboItem38;

		private ComboItem comboItem39;

		private ComboItem comboItem40;

		private ComboItem comboItem41;

		private ComboItem comboItem42;

		private ComboItem comboItem43;

		private ComboItem comboItem44;

		private ComboItem comboItem45;

		private ComboItem comboItem46;

		private ComboItem comboItem47;

		private ComboItem comboItem48;

		private ComboItem comboItem49;

		private ComboItem comboItem50;

		private ComboItem comboItem51;

		private ComboItem comboItem52;

		private ComboItem comboItem53;

		private ComboItem comboItem54;

		private ComboItem comboItem55;

		private MetroShell metroShell1;

		private MetroShell metroShell2;

		private MetroTabPanel metroTabPanel1;

		private MetroTabItem metroTabItem1;

		private MetroStatusBar metroStatusBar1;

		private RegisterControl registerControl;

		private LabelX lbPassword;

		private LabelX lbuser;

		private TextBoxX tbPasswd;

		private TextBoxX tbUser;

		private ButtonX btnRegister;

		private ButtonX btnLogin;

		private LabelX lbCredibility;

		private LabelX lbName;

		private ButtonItem btnItemDonate;

		private LabelItem labelItem1;

		private LabelItem lbItemTime;

		private LabelItem lbItemSpanTime;

		private System.Windows.Forms.NotifyIcon notifyIcon1;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		private System.Windows.Forms.ToolStripMenuItem mstripShow;

		private System.Windows.Forms.ToolStripMenuItem mstripExit;

		private LabelX labelX1;

		private System.Windows.Forms.LinkLabel lnbHelp;

		public MainForm()
		{
			this.InitializeComponent();
		}

		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
			this.Init();
			MessageBoxEx.UseSystemLocalizedString = true;
		}

		private void Init()
		{
			this._process = System.Diagnostics.Process.GetCurrentProcess();
			RegSetting.Instance.ReadSetting();
			this.m_tdManager = new System.Threading.Thread(new System.Threading.ThreadStart(this.tmManager));
			this.m_tdManager.IsBackground = true;
			this.m_tdManager.Start();
		}

		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			if (!(this.btnLogin.Text == "登陆"))
			{
				this._regHelper.Logout();
				this.lbuser.Text = "账户名";
				this.lbPassword.Text = "密码";
				this.tbPasswd.Text = string.Empty;
				this.lbPassword.Visible = true;
				this.lbuser.Visible = true;
				this.tbPasswd.Visible = true;
				this.tbUser.Visible = true;
				this.lbCredibility.Visible = false;
				this.lbName.Visible = false;
				this.btnLogin.Text = "登陆";
				return;
			}
			if (!this.IsValidLogin())
			{
				return;
			}
			AuthCodeForm authCodeForm = new AuthCodeForm();
			authCodeForm.SetLoginValue(this.tbUser.Text, this.tbPasswd.Text);
			if (authCodeForm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
			{
				UserInfo userInfo = this._regHelper.GetUserInfo();
				this.btnLogin.Text = "注销";
				this.lbName.Visible = true;
				this.lbCredibility.Visible = true;
				this.lbName.Text = "姓  名:" + userInfo.Name;
				this.lbCredibility.Text = "信誉度:" + userInfo.Credibility;
				this.tbPasswd.Visible = false;
				this.tbUser.Visible = false;
				this.lbuser.Visible = false;
				this.lbPassword.Visible = false;
			}
			authCodeForm.Dispose();
		}

		private void btnRegister_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.zj12580.cn/patient/reg1");
		}

		private bool IsValidLogin()
		{
			if (this.tbUser.Text.Length == 0)
			{
				MessageBoxEx.Show("账户名输入不能为空!", "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
				return false;
			}
			if (this.tbPasswd.Text.Length == 0)
			{
				MessageBoxEx.Show("密码输入不能为空!", "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
				return false;
			}
			if (!System.Text.RegularExpressions.Regex.IsMatch(this.tbUser.Text, "^(^\\d{15}$|^\\d{18}$|^\\d{17}(\\d|X|x))$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
			{
				MessageBoxEx.Show("请输入正确的身份证号码！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
				return false;
			}
			return true;
		}

		private void btnItemContactMe_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("mailto:onemetersupport@yeah.net");
		}

		private void btnItemDonate_Click(object sender, System.EventArgs e)
		{
			DonateForm donateForm = new DonateForm();
			donateForm.ShowDialog();
			donateForm.Dispose();
		}

		private void RefreshTime()
		{
			this.lbItemTime.Text = "时间获取中...";
			this.lbItemSpanTime.Text = string.Empty;
			this.m_spanServer = System.TimeSpan.Zero;
			if (this.m_bjTime.Connect())
			{
				this.m_spanServer = this.m_bjTime.BeijingTimeNow - System.DateTime.Now;
				this.registerControl.SetSpanTime(this.m_spanServer);
				if (this.m_spanServer.TotalSeconds > 0.0)
				{
					this.lbItemSpanTime.Text = string.Format("慢{0:f6}秒", this.m_spanServer.TotalSeconds);
				}
				else
				{
					this.lbItemSpanTime.Text = string.Format("快{0:f6}秒", System.Math.Abs(this.m_spanServer.TotalSeconds));
				}
			}			
		}

		private void tmManager()
		{
			base.Invoke(new Action(delegate
			{
				this.RefreshTime();
			}));
			try
			{
				while (true)
				{
                    if (!base.IsDisposed)
                    {
                        base.Invoke(new Action(delegate
                        {
                            if (this.m_bjTime.IsConnect)
                            {

                                this.lbItemTime.Text = System.DateTime.Now.Add(this.m_spanServer).ToString();
                            }
                            else
                            {
                                this.lbItemTime.Text = "时间获取失败(点击以重新获取)";
                            }
                            this.metroStatusBar1.Refresh();
                        }));
                    }
					
					System.Threading.Thread.Sleep(1000);
				}
			}
			catch (System.Exception err)
			{
				Log.WriteError("貌似是窗口资源释放了线程还在访问，程序可以休息了", err);
			}
		}

		private void lbItemTime_Click(object sender, System.EventArgs e)
		{
			this.RefreshTime();
			//this.m_bIsRefreshTime = true;
		}

		private void metroShell2_SettingsButtonClick(object sender, System.EventArgs e)
		{
			SettingForm settingForm = new SettingForm();
			settingForm.ShowDialog();
			settingForm.Dispose();
		}

		private void metroShell2_HelpButtonClick(object sender, System.EventArgs e)
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.ShowDialog();
			aboutForm.Dispose();
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			if (RegSetting.Instance.HideWindows)
			{
				base.Hide();
				e.Cancel = true;
				return;
			}
			base.OnClosing(e);
		}

		private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.ShowForm();
		}

		private void metroShell2_SizeChanged(object sender, System.EventArgs e)
		{
			if (base.WindowState == System.Windows.Forms.FormWindowState.Minimized)
			{
				base.Hide();
			}
		}

		private void mstripExit_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.Application.Exit();
		}

		private void mstripShow_Click(object sender, System.EventArgs e)
		{
			this.ShowForm();
		}

		private void ShowForm()
		{
			System.IntPtr intPtr = Win32Api.FindWindow(null, "杭州预约挂号辅助软件");
			Win32Api.ShowWindow(intPtr, 9u);
			Win32Api.SetForegroundWindow(intPtr);
		}

		private void lnbHelp_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.zj12580.cn/help/queryInfo?ptypeid=&helpid=128");
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.styleManager = new DevComponents.DotNetBar.StyleManager(this.components);
            this.comboItem31 = new DevComponents.Editors.ComboItem();
            this.comboItem14 = new DevComponents.Editors.ComboItem();
            this.comboItem15 = new DevComponents.Editors.ComboItem();
            this.comboItem16 = new DevComponents.Editors.ComboItem();
            this.comboItem17 = new DevComponents.Editors.ComboItem();
            this.comboItem18 = new DevComponents.Editors.ComboItem();
            this.comboItem19 = new DevComponents.Editors.ComboItem();
            this.comboItem20 = new DevComponents.Editors.ComboItem();
            this.comboItem21 = new DevComponents.Editors.ComboItem();
            this.comboItem22 = new DevComponents.Editors.ComboItem();
            this.comboItem23 = new DevComponents.Editors.ComboItem();
            this.comboItem24 = new DevComponents.Editors.ComboItem();
            this.comboItem25 = new DevComponents.Editors.ComboItem();
            this.comboItem26 = new DevComponents.Editors.ComboItem();
            this.comboItem27 = new DevComponents.Editors.ComboItem();
            this.comboItem28 = new DevComponents.Editors.ComboItem();
            this.comboItem29 = new DevComponents.Editors.ComboItem();
            this.comboItem30 = new DevComponents.Editors.ComboItem();
            this.comboItem32 = new DevComponents.Editors.ComboItem();
            this.comboItem33 = new DevComponents.Editors.ComboItem();
            this.comboItem34 = new DevComponents.Editors.ComboItem();
            this.comboItem35 = new DevComponents.Editors.ComboItem();
            this.comboItem36 = new DevComponents.Editors.ComboItem();
            this.comboItem37 = new DevComponents.Editors.ComboItem();
            this.comboItem38 = new DevComponents.Editors.ComboItem();
            this.comboItem39 = new DevComponents.Editors.ComboItem();
            this.comboItem40 = new DevComponents.Editors.ComboItem();
            this.comboItem41 = new DevComponents.Editors.ComboItem();
            this.comboItem42 = new DevComponents.Editors.ComboItem();
            this.comboItem43 = new DevComponents.Editors.ComboItem();
            this.comboItem44 = new DevComponents.Editors.ComboItem();
            this.comboItem45 = new DevComponents.Editors.ComboItem();
            this.comboItem46 = new DevComponents.Editors.ComboItem();
            this.comboItem47 = new DevComponents.Editors.ComboItem();
            this.comboItem48 = new DevComponents.Editors.ComboItem();
            this.comboItem49 = new DevComponents.Editors.ComboItem();
            this.comboItem50 = new DevComponents.Editors.ComboItem();
            this.comboItem51 = new DevComponents.Editors.ComboItem();
            this.comboItem52 = new DevComponents.Editors.ComboItem();
            this.comboItem53 = new DevComponents.Editors.ComboItem();
            this.comboItem54 = new DevComponents.Editors.ComboItem();
            this.comboItem55 = new DevComponents.Editors.ComboItem();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.comboItem10 = new DevComponents.Editors.ComboItem();
            this.comboItem11 = new DevComponents.Editors.ComboItem();
            this.comboItem12 = new DevComponents.Editors.ComboItem();
            this.comboItem13 = new DevComponents.Editors.ComboItem();
            this.metroShell1 = new DevComponents.DotNetBar.Metro.MetroShell();
            this.metroShell2 = new DevComponents.DotNetBar.Metro.MetroShell();
            this.metroTabPanel1 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.lnbHelp = new System.Windows.Forms.LinkLabel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.lbCredibility = new DevComponents.DotNetBar.LabelX();
            this.lbName = new DevComponents.DotNetBar.LabelX();
            this.btnRegister = new DevComponents.DotNetBar.ButtonX();
            this.btnLogin = new DevComponents.DotNetBar.ButtonX();
            this.tbPasswd = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lbPassword = new DevComponents.DotNetBar.LabelX();
            this.lbuser = new DevComponents.DotNetBar.LabelX();
            this.metroTabItem1 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroStatusBar1 = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.lbItemTime = new DevComponents.DotNetBar.LabelItem();
            this.lbItemSpanTime = new DevComponents.DotNetBar.LabelItem();
            this.btnItemDonate = new DevComponents.DotNetBar.ButtonItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mstripShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mstripExit = new System.Windows.Forms.ToolStripMenuItem();
            this.registerControl = new WindowsForm.RegisterControl();
            this.metroShell2.SuspendLayout();
            this.metroTabPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager
            // 
            this.styleManager.ManagerColorTint = System.Drawing.Color.White;
            this.styleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.OfficeMobile2014;
            this.styleManager.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254))))), System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(143))))));
            // 
            // comboItem31
            // 
            this.comboItem31.Text = "Default";
            // 
            // comboItem14
            // 
            this.comboItem14.Text = "VisualStudio2012Light";
            // 
            // comboItem15
            // 
            this.comboItem15.Text = "VisualStudio2012Dark";
            // 
            // comboItem16
            // 
            this.comboItem16.Text = "WashedWhite";
            // 
            // comboItem17
            // 
            this.comboItem17.Text = "WashedBlue";
            // 
            // comboItem18
            // 
            this.comboItem18.Text = "BlackClouds";
            // 
            // comboItem19
            // 
            this.comboItem19.Text = "BlackLilac";
            // 
            // comboItem20
            // 
            this.comboItem20.Text = "BlackMaroon";
            // 
            // comboItem21
            // 
            this.comboItem21.Text = "BlackSky";
            // 
            // comboItem22
            // 
            this.comboItem22.Text = "Blue";
            // 
            // comboItem23
            // 
            this.comboItem23.Text = "BlueishBrown";
            // 
            // comboItem24
            // 
            this.comboItem24.Text = "Bordeaux";
            // 
            // comboItem25
            // 
            this.comboItem25.Text = "Brown";
            // 
            // comboItem26
            // 
            this.comboItem26.Text = "Cherry";
            // 
            // comboItem27
            // 
            this.comboItem27.Text = "DarkBlue";
            // 
            // comboItem28
            // 
            this.comboItem28.Text = "DarkBrown";
            // 
            // comboItem29
            // 
            this.comboItem29.Text = "DarkPurple";
            // 
            // comboItem30
            // 
            this.comboItem30.Text = "DarkRed";
            // 
            // comboItem32
            // 
            this.comboItem32.Text = "EarlyMaroon";
            // 
            // comboItem33
            // 
            this.comboItem33.Text = "EarlyOrange";
            // 
            // comboItem34
            // 
            this.comboItem34.Text = "EarlyRed";
            // 
            // comboItem35
            // 
            this.comboItem35.Text = "Espresso";
            // 
            // comboItem36
            // 
            this.comboItem36.Text = "ForestGreen";
            // 
            // comboItem37
            // 
            this.comboItem37.Text = "GrayOrange";
            // 
            // comboItem38
            // 
            this.comboItem38.Text = "Green";
            // 
            // comboItem39
            // 
            this.comboItem39.Text = "Latte";
            // 
            // comboItem40
            // 
            this.comboItem40.Text = "LatteDarkSteel";
            // 
            // comboItem41
            // 
            this.comboItem41.Text = "LatteRed";
            // 
            // comboItem42
            // 
            this.comboItem42.Text = "Magenta";
            // 
            // comboItem43
            // 
            this.comboItem43.Text = "MaroonSilver";
            // 
            // comboItem44
            // 
            this.comboItem44.Text = "NapaRed";
            // 
            // comboItem45
            // 
            this.comboItem45.Text = "Orange";
            // 
            // comboItem46
            // 
            this.comboItem46.Text = "PowderRed";
            // 
            // comboItem47
            // 
            this.comboItem47.Text = "Purple";
            // 
            // comboItem48
            // 
            this.comboItem48.Text = "Red";
            // 
            // comboItem49
            // 
            this.comboItem49.Text = "RedAmplified";
            // 
            // comboItem50
            // 
            this.comboItem50.Text = "RetroBlue";
            // 
            // comboItem51
            // 
            this.comboItem51.Text = "Rust";
            // 
            // comboItem52
            // 
            this.comboItem52.Text = "SilverBlues";
            // 
            // comboItem53
            // 
            this.comboItem53.Text = "SilverGreen";
            // 
            // comboItem54
            // 
            this.comboItem54.Text = "SimplyBlue";
            // 
            // comboItem55
            // 
            this.comboItem55.Text = "SkyGreen";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "Office2007Blue";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "Office2007Silver";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "Office2007Black";
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "Office2007VistaGlass";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "Office2010Silver";
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "Office2010Blue";
            // 
            // comboItem7
            // 
            this.comboItem7.Text = "Office2010Black";
            // 
            // comboItem8
            // 
            this.comboItem8.Text = "Windows7Blue";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "VisualStudio2010Blue";
            // 
            // comboItem10
            // 
            this.comboItem10.Text = "Office2013";
            // 
            // comboItem11
            // 
            this.comboItem11.Text = "VisualStudio2012Light";
            // 
            // comboItem12
            // 
            this.comboItem12.Text = "VisualStudio2012Dark";
            // 
            // comboItem13
            // 
            this.comboItem13.Text = "OfficeMobile2014";
            // 
            // metroShell1
            // 
            this.metroShell1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroShell1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShell1.ForeColor = System.Drawing.Color.Black;
            this.metroShell1.HelpButtonText = null;
            this.metroShell1.Location = new System.Drawing.Point(0, 0);
            this.metroShell1.Name = "metroShell1";
            this.metroShell1.Size = new System.Drawing.Size(200, 100);
            this.metroShell1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShell1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShell1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShell1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShell1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShell1.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShell1.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShell1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShell1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShell1.SystemText.QatDialogOkButton = "OK";
            this.metroShell1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShell1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShell1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShell1.TabIndex = 0;
            this.metroShell1.TabStripFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // metroShell2
            // 
            this.metroShell2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            // 
            // 
            // 
            this.metroShell2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShell2.CaptionVisible = true;
            this.metroShell2.Controls.Add(this.metroTabPanel1);
            this.metroShell2.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroShell2.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.metroShell2.ForeColor = System.Drawing.Color.Black;
            this.metroShell2.HelpButtonText = "关于";
            this.metroShell2.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.metroTabItem1});
            this.metroShell2.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.metroShell2.Location = new System.Drawing.Point(1, 1);
            this.metroShell2.Margin = new System.Windows.Forms.Padding(2);
            this.metroShell2.MouseWheelTabScrollEnabled = false;
            this.metroShell2.Name = "metroShell2";
            this.metroShell2.SettingsButtonText = "设置";
            this.metroShell2.Size = new System.Drawing.Size(1034, 132);
            this.metroShell2.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShell2.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShell2.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShell2.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShell2.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShell2.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShell2.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShell2.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShell2.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShell2.SystemText.QatDialogOkButton = "OK";
            this.metroShell2.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell2.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShell2.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShell2.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell2.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShell2.TabIndex = 0;
            this.metroShell2.TabStripFont = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroShell2.Text = "metroShell2";
            this.metroShell2.UseCustomizeDialog = false;
            this.metroShell2.SettingsButtonClick += new System.EventHandler(this.metroShell2_SettingsButtonClick);
            this.metroShell2.HelpButtonClick += new System.EventHandler(this.metroShell2_HelpButtonClick);
            this.metroShell2.SizeChanged += new System.EventHandler(this.metroShell2_SizeChanged);
            // 
            // metroTabPanel1
            // 
            this.metroTabPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel1.Controls.Add(this.lnbHelp);
            this.metroTabPanel1.Controls.Add(this.labelX1);
            this.metroTabPanel1.Controls.Add(this.lbCredibility);
            this.metroTabPanel1.Controls.Add(this.lbName);
            this.metroTabPanel1.Controls.Add(this.btnRegister);
            this.metroTabPanel1.Controls.Add(this.btnLogin);
            this.metroTabPanel1.Controls.Add(this.tbPasswd);
            this.metroTabPanel1.Controls.Add(this.tbUser);
            this.metroTabPanel1.Controls.Add(this.lbPassword);
            this.metroTabPanel1.Controls.Add(this.lbuser);
            this.metroTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel1.Location = new System.Drawing.Point(0, 63);
            this.metroTabPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.metroTabPanel1.Name = "metroTabPanel1";
            this.metroTabPanel1.Padding = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.metroTabPanel1.Size = new System.Drawing.Size(1034, 69);
            // 
            // 
            // 
            this.metroTabPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel1.TabIndex = 1;
            // 
            // lnbHelp
            // 
            this.lnbHelp.AutoSize = true;
            this.lnbHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.lnbHelp.ForeColor = System.Drawing.Color.Black;
            this.lnbHelp.Location = new System.Drawing.Point(360, 44);
            this.lnbHelp.Name = "lnbHelp";
            this.lnbHelp.Size = new System.Drawing.Size(119, 14);
            this.lnbHelp.TabIndex = 9;
            this.lnbHelp.TabStop = true;
            this.lnbHelp.Text = "信誉度等常见问题";
            this.lnbHelp.Click += new System.EventHandler(this.lnbHelp_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(360, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(583, 23);
            this.labelX1.TabIndex = 8;
            this.labelX1.Text = "声明：本软件使用过程中不记录用户名和密码，同时也不以任何形式保存用户名和密码。";
            // 
            // lbCredibility
            // 
            this.lbCredibility.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbCredibility.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbCredibility.ForeColor = System.Drawing.Color.Black;
            this.lbCredibility.Location = new System.Drawing.Point(132, 39);
            this.lbCredibility.Name = "lbCredibility";
            this.lbCredibility.Size = new System.Drawing.Size(111, 23);
            this.lbCredibility.TabIndex = 7;
            this.lbCredibility.Visible = false;
            // 
            // lbName
            // 
            this.lbName.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbName.ForeColor = System.Drawing.Color.Black;
            this.lbName.Location = new System.Drawing.Point(132, 11);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(111, 23);
            this.lbName.TabIndex = 6;
            this.lbName.Visible = false;
            // 
            // btnRegister
            // 
            this.btnRegister.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRegister.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRegister.Location = new System.Drawing.Point(254, 41);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(75, 23);
            this.btnRegister.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnRegister.TabIndex = 5;
            this.btnRegister.Text = "免费注册";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLogin.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLogin.Location = new System.Drawing.Point(254, 10);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登陆";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbPasswd
            // 
            this.tbPasswd.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.tbPasswd.Border.Class = "TextBoxBorder";
            this.tbPasswd.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbPasswd.DisabledBackColor = System.Drawing.Color.White;
            this.tbPasswd.ForeColor = System.Drawing.Color.Black;
            this.tbPasswd.Location = new System.Drawing.Point(86, 40);
            this.tbPasswd.Name = "tbPasswd";
            this.tbPasswd.PasswordChar = '*';
            this.tbPasswd.PreventEnterBeep = true;
            this.tbPasswd.Size = new System.Drawing.Size(151, 23);
            this.tbPasswd.TabIndex = 3;
            // 
            // tbUser
            // 
            this.tbUser.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.tbUser.Border.Class = "TextBoxBorder";
            this.tbUser.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbUser.DisabledBackColor = System.Drawing.Color.White;
            this.tbUser.ForeColor = System.Drawing.Color.Black;
            this.tbUser.Location = new System.Drawing.Point(86, 10);
            this.tbUser.Name = "tbUser";
            this.tbUser.PreventEnterBeep = true;
            this.tbUser.Size = new System.Drawing.Size(151, 23);
            this.tbUser.TabIndex = 2;
            this.tbUser.WatermarkText = "请输入身份证";
            // 
            // lbPassword
            // 
            this.lbPassword.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbPassword.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbPassword.ForeColor = System.Drawing.Color.Black;
            this.lbPassword.Location = new System.Drawing.Point(21, 40);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(59, 23);
            this.lbPassword.TabIndex = 1;
            this.lbPassword.Text = "密 码";
            // 
            // lbuser
            // 
            this.lbuser.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbuser.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbuser.ForeColor = System.Drawing.Color.Black;
            this.lbuser.Location = new System.Drawing.Point(21, 10);
            this.lbuser.Name = "lbuser";
            this.lbuser.Size = new System.Drawing.Size(59, 23);
            this.lbuser.TabIndex = 0;
            this.lbuser.Text = "账户名";
            // 
            // metroTabItem1
            // 
            this.metroTabItem1.Checked = true;
            this.metroTabItem1.Name = "metroTabItem1";
            this.metroTabItem1.Panel = this.metroTabPanel1;
            this.metroTabItem1.Text = "个人中心";
            // 
            // metroStatusBar1
            // 
            this.metroStatusBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            // 
            // 
            // 
            this.metroStatusBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroStatusBar1.ContainerControlProcessDialogKey = true;
            this.metroStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroStatusBar1.DragDropSupport = true;
            this.metroStatusBar1.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroStatusBar1.ForeColor = System.Drawing.Color.Black;
            this.metroStatusBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.lbItemTime,
            this.lbItemSpanTime,
            this.btnItemDonate});
            this.metroStatusBar1.Location = new System.Drawing.Point(1, 642);
            this.metroStatusBar1.Margin = new System.Windows.Forms.Padding(2);
            this.metroStatusBar1.Name = "metroStatusBar1";
            this.metroStatusBar1.Size = new System.Drawing.Size(1034, 32);
            this.metroStatusBar1.TabIndex = 1;
            this.metroStatusBar1.Text = "metroStatusBar1";
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "北京时间:";
            // 
            // lbItemTime
            // 
            this.lbItemTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbItemTime.Name = "lbItemTime";
            this.lbItemTime.Text = "网络连接失败";
            this.lbItemTime.Click += new System.EventHandler(this.lbItemTime_Click);
            // 
            // lbItemSpanTime
            // 
            this.lbItemSpanTime.Name = "lbItemSpanTime";
            // 
            // btnItemDonate
            // 
            this.btnItemDonate.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.btnItemDonate.Name = "btnItemDonate";
            this.btnItemDonate.Text = "捐助软件";
            this.btnItemDonate.Click += new System.EventHandler(this.btnItemDonate_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "杭州预约挂号辅助软件";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mstripShow,
            this.mstripExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 48);
            // 
            // mstripShow
            // 
            this.mstripShow.Name = "mstripShow";
            this.mstripShow.Size = new System.Drawing.Size(136, 22);
            this.mstripShow.Text = "打开主程序";
            this.mstripShow.Click += new System.EventHandler(this.mstripShow_Click);
            // 
            // mstripExit
            // 
            this.mstripExit.Name = "mstripExit";
            this.mstripExit.Size = new System.Drawing.Size(136, 22);
            this.mstripExit.Text = "退出";
            this.mstripExit.Click += new System.EventHandler(this.mstripExit_Click);
            // 
            // registerControl
            // 
            this.registerControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.registerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerControl.ForeColor = System.Drawing.Color.Black;
            this.registerControl.Location = new System.Drawing.Point(1, 133);
            this.registerControl.Name = "registerControl";
            this.registerControl.Size = new System.Drawing.Size(1034, 509);
            this.registerControl.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 675);
            this.Controls.Add(this.registerControl);
            this.Controls.Add(this.metroStatusBar1);
            this.Controls.Add(this.metroShell2);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.SystemMenuClose = "关闭";
            this.SystemMenuMaximize = "最大化";
            this.SystemMenuMinimize = "最小化";
            this.SystemMenuMove = "移动";
            this.SystemMenuRestore = "还原";
            this.SystemMenuSize = "大小";
            this.Text = "杭州预约挂号辅助软件";
            this.metroShell2.ResumeLayout(false);
            this.metroShell2.PerformLayout();
            this.metroTabPanel1.ResumeLayout(false);
            this.metroTabPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
	}
}
