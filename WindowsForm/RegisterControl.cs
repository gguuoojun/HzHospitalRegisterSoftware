using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using Model;
using HzHospitalRegister;

namespace WindowsForm
{
	public class RegisterControl : System.Windows.Forms.UserControl
	{
		private const string RESULT_SUCCESS = "success";

		private const string NON_SELECT_VALUE = "-1";

		private const string NON_SELECT_TXT = "请选择";

		private const string GET_SUCCESS = "(提前7天下午14:00放号，取号时间请以接收的短信为准)";

		private const string Get_SUCCESS2 = "(提前7天下午15:00放号，取号时间请以接收的短信为准)";

		private const string GET_FAIL = "信息读取有误：可能网路连接失败，或者不存在该科室信息";

		private const string Get_NON_INFO = "该科室暂无排班";

		private RegisterHelper m_regHelper = RegisterHelper.Instance;

		private HtmlAgilityPack.HtmlDocument _htmlDoc = new HtmlAgilityPack.HtmlDocument();

		private bool m_bIsProvinceHos = true;

		private bool m_bIsSuccessGetTicket;

		private bool m_bIsStopRefresh = true;

		private bool m_bIsHaveTickets;

		private System.TimeSpan m_spanBeijingTime;

		private System.TimeSpan m_spanRegTime;

		private System.Threading.Thread m_tdRefresh;

		private System.Threading.ManualResetEvent m_ResetEvent = new System.Threading.ManualResetEvent(false);

		private System.Collections.Generic.List<SelectDoctor> m_allSelectDoctors = new System.Collections.Generic.List<SelectDoctor>();

		private System.Media.SoundPlayer m_sdPlayer = new System.Media.SoundPlayer();

		private System.ComponentModel.IContainer components;

		private PanelEx panelEx1;

		private PanelEx panelEx2;

		private ButtonX btnRefresh;

		private LabelX labelX1;

		private ComboBoxEx cmbArea;

		private ComboBoxEx cmbHospital;

		private LabelX labelX2;

		private LabelX labelX3;

		private ComboBoxEx cmbDepartment;

		private DataGridViewX dataGridViewX1;

		private PanelEx panelInfo;

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

		private LabelX lbDate2;

		private LabelX lbDate1;

		private LabelX labelX5;

		private LabelX lbDate8;

		private LabelX lbDate7;

		private LabelX lbDate6;

		private LabelX lbDate5;

		private LabelX lbDate3;

		private LabelX lbDate4;

		private CheckBoxX chkWaitforTime;

		private DataGridViewLabelXColumn colDoctor;

		private DataGridViewButtonXColumn colAnteMeridiem1;

		private DataGridViewButtonXColumn colPostMeridiem1;

		private DataGridViewButtonXColumn colAnteMeridiem2;

		private DataGridViewButtonXColumn colPostMeridiem2;

		private DataGridViewButtonXColumn colAnteMeridiem3;

		private DataGridViewButtonXColumn colPostMeridiem3;

		private DataGridViewButtonXColumn colAnteMeridiem4;

		private DataGridViewButtonXColumn colPostMeridiem4;

		private DataGridViewButtonXColumn colAnteMeridiem5;

		private DataGridViewButtonXColumn colPostMeridiem5;

		private DataGridViewButtonXColumn colAnteMeridiem6;

		private DataGridViewButtonXColumn colPostMeridiem6;

		private DataGridViewButtonXColumn colAnteMeridiem7;

		private DataGridViewButtonXColumn colPostMeridiem7;

		private DataGridViewButtonXColumn colAnteMeridiem8;

		private DataGridViewButtonXColumn colPostMeridiem8;

		private PanelEx plPriority;

		private SwitchButton btnSwitch;

		private LabelX labelX4;

		private ItemPanel itemPrioritys;

		private System.Windows.Forms.ToolTip toolTip1;

		public RegisterControl()
		{
			this.InitializeComponent();
		}

		public void SetSpanTime(System.TimeSpan spanTime)
		{
			this.m_spanBeijingTime = spanTime;
		}

		protected override void OnLoad(System.EventArgs e)
		{
			this.InitAreaControl();
			this.InitDateLable();
			this.InitRefreshThread();
			this.toolTip1.SetToolTip(this.itemPrioritys, "请拖动表格中第七天的预约信息到这里");
			base.OnLoad(e);
		}

		private void InitRefreshThread()
		{
			this.m_tdRefresh = new System.Threading.Thread(new System.Threading.ThreadStart(this.RefreshThread));
			this.m_tdRefresh.IsBackground = true;
			this.m_tdRefresh.Start();
		}

		private void InitAreaControl()
		{
			this.cmbArea.Items.Clear();
			ComboItem comboItem = new ComboItem();
			comboItem.Text = "请选择";
			comboItem.Value = "-1";
			this.cmbArea.Items.Add(comboItem);
			IntegratedArea area = this.m_regHelper.GetArea();
			if (area != null && area.result == "success")
			{
				foreach (area current in area.data.areaList)
				{
					ComboItem comboItem2 = new ComboItem();
					comboItem2.Text = current.areaName;
					comboItem2.Value = current.areaCode;
					this.cmbArea.Items.Add(comboItem2);
				}
			}
			this.cmbArea.SelectedIndex = 0;
		}

		private void cmbArea_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.cmbHospital.Items.Clear();
			ComboItem comboItem = new ComboItem();
			comboItem.Text = "请选择";
			comboItem.Value = "-1";
			this.cmbHospital.Items.Add(comboItem);
			ComboItem comboItem2 = (ComboItem)this.cmbArea.SelectedItem;
			if (comboItem2.Text == "省直")
			{
				this.m_spanRegTime = new System.TimeSpan(15, 0, 0);
				this.m_bIsProvinceHos = true;
			}
			else
			{
				this.m_spanRegTime = new System.TimeSpan(14, 0, 0);
				this.m_bIsProvinceHos = false;
			}
			if (comboItem2.Value.ToString() != "-1")
			{
				IntergetedHospital hispital = this.m_regHelper.GetHispital(comboItem2.Value.ToString());
				if (hispital != null && hispital.result == "success")
				{
					foreach (HospitalInfo current in hispital.data.hos)
					{
						ComboItem comboItem3 = new ComboItem();
						comboItem3.Text = current.aliasName;
						comboItem3.Value = current.hosCode;
						this.cmbHospital.Items.Add(comboItem3);
					}
				}
			}
			this.cmbHospital.SelectedIndex = 0;
		}

		private void cmbHospital_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.cmbDepartment.Items.Clear();
			ComboItem comboItem = new ComboItem();
			comboItem.Text = "请选择";
			comboItem.Value = "-1";
			this.cmbDepartment.Items.Add(comboItem);
			ComboItem comboItem2 = (ComboItem)this.cmbHospital.SelectedItem;
			if (comboItem2.Value.ToString() != "-1")
			{
				IntergretedDepartment department = this.m_regHelper.GetDepartment(comboItem2.Value.ToString());
				if (department != null && department.result == "success")
				{
					foreach (DeptInfo current in department.data.dept)
					{
						ComboItem comboItem3 = new ComboItem();
						comboItem3.Text = current.deptName;
						this.cmbDepartment.Items.Add(comboItem3);
					}
				}
			}
			this.cmbDepartment.SelectedIndex = 0;
		}

		private void cmbDepartment_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.m_allSelectDoctors.Clear();
			this.chkWaitforTime.Checked = false;
			this.itemPrioritys.Items.Clear();
			this.RefreshRegisterInfo();
		}

		private void RefreshThread()
		{
			double total;
			while (true)
			{
				this.m_ResetEvent.WaitOne();
				total = (System.DateTime.Now.Add(this.m_spanBeijingTime) - System.DateTime.Today).TotalMilliseconds - this.m_spanRegTime.TotalMilliseconds;
				float count = 1000f;
				if (total < 0.0)
				{
					if (base.IsDisposed)
					{
						break;
					}
					base.Invoke(new Action(delegate
					{
						this.btnRefresh.Text = string.Format("  停止<div align=\"center\"><font face=\"Times New Roman\" size=\"12\" color=\"red\">{0:f3}</font></div>", -total / 1000.0);
						this.btnRefresh.Refresh();
					}));
					System.Threading.Thread.Sleep(100);
				}
				else if (total <= 0.0 && total > -500.0)
				{
					this.RefreshRegisterInfoInThread();
					if (!this.m_bIsSuccessGetTicket)
					{
						if (base.IsDisposed)
						{
							return;
						}
                        base.Invoke(new Action(delegate
						{
							this.btnRefresh.Text = string.Format("  停止<div align=\"center\"><font face=\"Times New Roman\" size=\"12\" color=\"red\">{0:f3}</font></div>", -total / 1000.0);
						}));
						System.Threading.Thread.Sleep(200);
					}
				}
				else if (total < 300000.0)
				{
					this.RefreshRegisterInfoInThread();
					if (!this.m_bIsSuccessGetTicket)
					{
						for (int i = 0; i < 10; i++)
						{
							if (base.IsDisposed)
							{
								return;
							}
							if (this.m_bIsStopRefresh)
							{
								break;
							}
							base.Invoke(new Action(delegate
							{
								this.btnRefresh.Text = string.Format("  停止<div align=\"center\"><font face=\"Times New Roman\" size=\"12\" color=\"red\">{0:f1}</font></div>", -count / 1000f);
								this.btnRefresh.Refresh();
							}));
							count -= 100f;
							System.Threading.Thread.Sleep(100);
						}
					}
				}
				else
				{
					this.RefreshRegisterInfoInThread();
					if (!this.m_bIsSuccessGetTicket)
					{
						count = 3000f;
						for (int j = 0; j < 30; j++)
						{
							if (base.IsDisposed)
							{
								return;
							}
							if (this.m_bIsStopRefresh)
							{
								break;
							}
							base.Invoke(new Action(delegate
							{
								this.btnRefresh.Text = string.Format("  停止<div align=\"center\"><font face=\"Times New Roman\" size=\"12\" color=\"red\">{0:f1}</font></div>", count / 1000f);
								this.btnRefresh.Refresh();
							}));
							count -= 100f;
							System.Threading.Thread.Sleep(100);
						}
					}
				}
			}
		}

		private void RefreshRegisterInfoInThread()
		{
			if (!base.IsDisposed)
			{
				base.Invoke(new Action(delegate
				{
					this.RefreshRegisterInfo();
				}));
			}
		}

		private void RefreshRegisterInfo()
		{
			this.dataGridViewX1.Rows.Clear();
			ComboItem comboItem = (ComboItem)this.cmbHospital.SelectedItem;
			ComboItem comboItem2 = (ComboItem)this.cmbDepartment.SelectedItem;
			if (comboItem2 == null || comboItem.Value.ToString() == "-1" || comboItem2.Text == "请选择")
			{
				return;
			}
			this.RefreshDatagridView(this.m_regHelper.GetDepartmenInfo(comboItem.Value.ToString(), comboItem2.Text));
		}

		private void RefreshDatagridView(RegDept regDept)
		{
			this.btnRefresh.Text = "刷新";
			this.btnRefresh.Refresh();
			this.m_bIsSuccessGetTicket = false;
			this.m_bIsHaveTickets = false;
			this.panelInfo.Text = (this.m_bIsProvinceHos ? "(提前7天下午15:00放号，取号时间请以接收的短信为准)" : "(提前7天下午14:00放号，取号时间请以接收的短信为准)");
			if (regDept.ResResult != ResponseReuslt.SUCCESS)
			{
				this.panelInfo.Text = "信息读取有误：可能网路连接失败，或者不存在该科室信息";
				return;
			}
			if (regDept.RealCount == 0)
			{
				this.panelInfo.Text = "该科室暂无排班";
				return;
			}
			this.GrabTicket(regDept);
			if (this.lbDate1.Text != regDept.Dates[0])
			{
				this.lbDate1.Text = regDept.Dates[0];
				this.lbDate2.Text = regDept.Dates[1];
				this.lbDate3.Text = regDept.Dates[2];
				this.lbDate4.Text = regDept.Dates[3];
				this.lbDate5.Text = regDept.Dates[4];
				this.lbDate6.Text = regDept.Dates[5];
				this.lbDate7.Text = regDept.Dates[6];
				this.lbDate8.Text = regDept.Dates[7];
			}
			for (int i = 0; i < regDept.RealCount; i++)
			{
				int num = 0;
				int index = this.dataGridViewX1.Rows.Add();
				RegDoctor regDoctor = regDept.ListDoctors[i];
				System.Windows.Forms.DataGridViewCell dataGridViewCell = this.dataGridViewX1.Rows[index].Cells[num];
				dataGridViewCell.Value = regDoctor.Name;
				num++;
				for (int j = 0; j < 16; j++)
				{
					if (regDoctor.Values[j] != null)
					{
						dataGridViewCell = this.dataGridViewX1.Rows[index].Cells[num];
						dataGridViewCell.Value = regDoctor.Values[j];
						dataGridViewCell.ToolTipText = regDoctor.ToolTipTexts[j];
						dataGridViewCell.Tag = regDoctor.Tags[j];
						if (j >= 14)
						{
							this.m_bIsHaveTickets = true;
						}
					}
					num++;
				}
			}
		}

		private void GrabTicket(RegDept regDept)
		{
			if (this.chkWaitforTime.Checked && this.m_allSelectDoctors.Count > 0)
			{
				foreach (SelectDoctor current in this.m_allSelectDoctors)
				{
					object obj = regDept.ListDoctors[current.RowsIndex].Tags[current.CellIndex];
					if (obj != null)
					{
						OrderInfo queryRegTime = this.m_regHelper.GetQueryRegTime((string)obj);
						if (queryRegTime.ResResult == ResponseReuslt.SUCCESS)
						{
							this.m_bIsSuccessGetTicket = true;
							this.m_ResetEvent.Reset();
							this.m_sdPlayer.SoundLocation = RegSetting.Instance.SoundPath;
							this.m_sdPlayer.Play();
							OrderForm orderForm = new OrderForm(queryRegTime);
							orderForm.TopMost = true;
							orderForm.ShowDialog();
							orderForm.Dispose();
							this.cmbArea.Enabled = true;
							this.cmbDepartment.Enabled = true;
							this.cmbHospital.Enabled = true;
							break;
						}
						if (queryRegTime.ResResult == ResponseReuslt.NON_LOGIN)
						{
							this.m_bIsSuccessGetTicket = true;
							this.m_ResetEvent.Reset();
							MessageBoxEx.Show("请先登录后再预约", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
							this.cmbArea.Enabled = true;
							this.cmbDepartment.Enabled = true;
							this.cmbHospital.Enabled = true;
							break;
						}
					}
				}
			}
		}

		private void InitDateLable()
		{
			System.DateTime now = System.DateTime.Now;
			this.lbDate1.Text = now.ToString("MM/dd\ndddd");
			this.lbDate2.Text = now.AddDays(1.0).ToString("MM/dd\ndddd");
			this.lbDate3.Text = now.AddDays(2.0).ToString("MM/dd\ndddd");
			this.lbDate4.Text = now.AddDays(3.0).ToString("MM/dd\ndddd");
			this.lbDate5.Text = now.AddDays(4.0).ToString("MM/dd\ndddd");
			this.lbDate6.Text = now.AddDays(5.0).ToString("MM/dd\ndddd");
			this.lbDate7.Text = now.AddDays(6.0).ToString("MM/dd\ndddd");
			this.lbDate8.Text = now.AddDays(7.0).ToString("MM/dd\ndddd");
		}

		private void RegisterControl_SizeChanged(object sender, System.EventArgs e)
		{
			this.tableLayoutPanel1.ColumnStyles[0].Width = (float)this.dataGridViewX1.Columns[0].Width;
			for (int i = 1; i < 9; i++)
			{
				this.tableLayoutPanel1.ColumnStyles[i].Width = (float)(this.dataGridViewX1.Columns[(i - 1) * 2 + 1].Width + this.dataGridViewX1.Columns[i * 2].Width);
			}
		}

		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			if (!this.m_bIsStopRefresh)
			{
				this.m_ResetEvent.Reset();
				this.btnRefresh.Text = "刷新";
				this.cmbArea.Enabled = true;
				this.cmbDepartment.Enabled = true;
				this.cmbHospital.Enabled = true;
				this.m_bIsStopRefresh = true;
				return;
			}
			if (!this.chkWaitforTime.Checked)
			{
				this.RefreshRegisterInfo();
				return;
			}
			if (!this.m_regHelper.IsLogin)
			{
				MessageBoxEx.Show("请先登录后再预约", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
				return;
			}
			this.m_bIsStopRefresh = false;
			this.cmbArea.Enabled = false;
			this.cmbDepartment.Enabled = false;
			this.cmbHospital.Enabled = false;
			this.m_ResetEvent.Set();
		}

		private void cmbArea_DropDown(object sender, System.EventArgs e)
		{
			if (this.cmbArea.Items.Count == 1 && this.m_regHelper.GetArea() == null)
			{
				MessageBoxEx.Show("网络连接失败,无法获取地区信息！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
			}
		}

		private void chkWaitforTime_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!this.chkWaitforTime.Checked)
			{
				this.m_ResetEvent.Reset();
				if (!this.m_bIsStopRefresh)
				{
					this.m_bIsStopRefresh = true;
					this.btnRefresh.Text = "刷新";
					this.btnRefresh.Refresh();
					this.cmbArea.Enabled = true;
					this.cmbDepartment.Enabled = true;
					this.cmbHospital.Enabled = true;
				}
			}
			else if (!this.m_bIsHaveTickets)
			{
				MessageBoxEx.Show(this.lbDate8.Text + "没有可用的医源,请选择其他部门", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
				this.chkWaitforTime.Checked = false;
				return;
			}
			this.btnSwitch.Enabled = this.chkWaitforTime.Checked;
			this.plPriority.Visible = this.chkWaitforTime.Checked;
			this.btnSwitch.Value = this.chkWaitforTime.Checked;
		}

		private void btnSwitch_ValueChanged(object sender, System.EventArgs e)
		{
			this.plPriority.Visible = this.btnSwitch.Value;
		}

		private void itemPrioritys_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			e.Effect = System.Windows.Forms.DragDropEffects.Copy;
		}

		private void itemPrioritys_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			System.Windows.Forms.DataGridViewCell dataGridViewCell = e.Data.GetData(typeof(DataGridViewButtonXCell)) as System.Windows.Forms.DataGridViewCell;
			if (dataGridViewCell != null)
			{
				this.AddPriorityItem(dataGridViewCell);
			}
		}

		private void AddPriorityItem(System.Windows.Forms.DataGridViewCell cell)
		{
			if (!this.m_allSelectDoctors.Exists((SelectDoctor T) => T.CellIndex == cell.ColumnIndex - 1 && T.RowsIndex == cell.RowIndex))
			{
				SelectDoctor selectDoctor = new SelectDoctor
				{
					CellIndex = cell.ColumnIndex - 1,
					RowsIndex = cell.RowIndex,
					Name = this.dataGridViewX1.Rows[cell.RowIndex].Cells[0].Value + " " + this.dataGridViewX1.Columns[cell.ColumnIndex].HeaderText
				};
				this.m_allSelectDoctors.Add(selectDoctor);
				LabelItem labelItem = new LabelItem();
				labelItem.Text = selectDoctor.Name;
				labelItem.Font = new System.Drawing.Font("宋体", 9.5f, System.Drawing.FontStyle.Underline);
				labelItem.Cursor = System.Windows.Forms.Cursors.Hand;
				this.itemPrioritys.Items.Add(labelItem);
				labelItem.Tag = selectDoctor;
				this.itemPrioritys.Refresh();
			}
		}

		private void dataGridViewX1_CellMouseDown(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex <= 14 || e.RowIndex < 0)
			{
				return;
			}
			System.Windows.Forms.DataGridViewCell dataGridViewCell = this.dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex];
			if (this.chkWaitforTime.Checked && dataGridViewCell.ToolTipText != null && dataGridViewCell.ToolTipText.Length > 0)
			{
				this.itemPrioritys.DoDragDrop(dataGridViewCell, System.Windows.Forms.DragDropEffects.All);
			}
		}

		private void itemPrioritys_ItemDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			LabelItem labelItem = sender as LabelItem;
			if (labelItem != null)
			{
				this.itemPrioritys.Items.Remove(labelItem);
				this.itemPrioritys.Cursor = System.Windows.Forms.Cursors.Default;
				this.itemPrioritys.Refresh();
				if (labelItem.Tag != null)
				{
					this.m_allSelectDoctors.Remove((SelectDoctor)labelItem.Tag);
				}
			}
		}

		private void dataGridViewX1_CellContentDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex <= 0 || e.RowIndex < 0)
			{
				return;
			}
			System.Windows.Forms.DataGridViewCell dataGridViewCell = this.dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex];
			if (dataGridViewCell != null && dataGridViewCell.Tag != null)
			{
				if (!this.m_regHelper.IsLogin)
				{
					MessageBoxEx.Show("请先登录后再预约", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
					return;
				}
				OrderInfo queryRegTime = this.m_regHelper.GetQueryRegTime((string)dataGridViewCell.Tag);
				if (queryRegTime.ResResult == ResponseReuslt.SUCCESS)
				{
					OrderForm orderForm = new OrderForm(queryRegTime);
					orderForm.ShowDialog();
					orderForm.Dispose();
					return;
				}
				if (queryRegTime.ResResult == ResponseReuslt.NON_LOGIN)
				{
					MessageBoxEx.Show("请先登录后再预约", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
				}
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.panelEx1 = new PanelEx();
			this.dataGridViewX1 = new DataGridViewX();
			this.colDoctor = new DataGridViewLabelXColumn();
			this.colAnteMeridiem1 = new DataGridViewButtonXColumn();
			this.colPostMeridiem1 = new DataGridViewButtonXColumn();
			this.colAnteMeridiem2 = new DataGridViewButtonXColumn();
			this.colPostMeridiem2 = new DataGridViewButtonXColumn();
			this.colAnteMeridiem3 = new DataGridViewButtonXColumn();
			this.colPostMeridiem3 = new DataGridViewButtonXColumn();
			this.colAnteMeridiem4 = new DataGridViewButtonXColumn();
			this.colPostMeridiem4 = new DataGridViewButtonXColumn();
			this.colAnteMeridiem5 = new DataGridViewButtonXColumn();
			this.colPostMeridiem5 = new DataGridViewButtonXColumn();
			this.colAnteMeridiem6 = new DataGridViewButtonXColumn();
			this.colPostMeridiem6 = new DataGridViewButtonXColumn();
			this.colAnteMeridiem7 = new DataGridViewButtonXColumn();
			this.colPostMeridiem7 = new DataGridViewButtonXColumn();
			this.colAnteMeridiem8 = new DataGridViewButtonXColumn();
			this.colPostMeridiem8 = new DataGridViewButtonXColumn();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lbDate8 = new LabelX();
			this.lbDate7 = new LabelX();
			this.lbDate6 = new LabelX();
			this.lbDate5 = new LabelX();
			this.lbDate3 = new LabelX();
			this.lbDate1 = new LabelX();
			this.labelX5 = new LabelX();
			this.lbDate2 = new LabelX();
			this.lbDate4 = new LabelX();
			this.panelInfo = new PanelEx();
			this.plPriority = new PanelEx();
			this.itemPrioritys = new ItemPanel();
			this.labelX4 = new LabelX();
			this.panelEx2 = new PanelEx();
			this.btnSwitch = new SwitchButton();
			this.chkWaitforTime = new CheckBoxX();
			this.cmbDepartment = new ComboBoxEx();
			this.labelX3 = new LabelX();
			this.cmbHospital = new ComboBoxEx();
			this.labelX2 = new LabelX();
			this.cmbArea = new ComboBoxEx();
			this.labelX1 = new LabelX();
			this.btnRefresh = new ButtonX();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panelEx1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dataGridViewX1).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.plPriority.SuspendLayout();
			this.panelEx2.SuspendLayout();
			base.SuspendLayout();
			this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
			this.panelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			this.panelEx1.Controls.Add(this.dataGridViewX1);
			this.panelEx1.Controls.Add(this.tableLayoutPanel1);
			this.panelEx1.Controls.Add(this.panelInfo);
			this.panelEx1.Controls.Add(this.plPriority);
			this.panelEx1.Controls.Add(this.panelEx2);
			this.panelEx1.DisabledBackColor = System.Drawing.Color.Empty;
			this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelEx1.Location = new System.Drawing.Point(0, 0);
			this.panelEx1.Name = "panelEx1";
			this.panelEx1.Size = new System.Drawing.Size(1082, 504);
			this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
			this.panelEx1.Style.Border = eBorderType.SingleLine;
			this.panelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
			this.panelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
			this.panelEx1.Style.GradientAngle = 90;
			this.panelEx1.TabIndex = 1;
			this.dataGridViewX1.AllowDrop = true;
			this.dataGridViewX1.AllowUserToAddRows = false;
			this.dataGridViewX1.AllowUserToDeleteRows = false;
			this.dataGridViewX1.AllowUserToResizeColumns = false;
			this.dataGridViewX1.BackgroundColor = System.Drawing.Color.FromArgb(254, 254, 254);
			dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Font = new System.Drawing.Font("SimSun", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewX1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridViewX1.ColumnHeadersHeight = 30;
			this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
			{
				this.colDoctor,
				this.colAnteMeridiem1,
				this.colPostMeridiem1,
				this.colAnteMeridiem2,
				this.colPostMeridiem2,
				this.colAnteMeridiem3,
				this.colPostMeridiem3,
				this.colAnteMeridiem4,
				this.colPostMeridiem4,
				this.colAnteMeridiem5,
				this.colPostMeridiem5,
				this.colAnteMeridiem6,
				this.colPostMeridiem6,
				this.colAnteMeridiem7,
				this.colPostMeridiem7,
				this.colAnteMeridiem8,
				this.colPostMeridiem8
			});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewX1.EnableHeadersVisualStyles = false;
			this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(200, 200, 200);
			this.dataGridViewX1.Location = new System.Drawing.Point(0, 158);
			this.dataGridViewX1.MultiSelect = false;
			this.dataGridViewX1.Name = "dataGridViewX1";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewX1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridViewX1.RowHeadersVisible = false;
			this.dataGridViewX1.RowHeadersWidth = 30;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dataGridViewX1.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridViewX1.RowTemplate.Height = 50;
			this.dataGridViewX1.Size = new System.Drawing.Size(1082, 346);
			this.dataGridViewX1.TabIndex = 2;
			this.dataGridViewX1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellContentDoubleClick);
			this.dataGridViewX1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewX1_CellMouseDown);
			this.colDoctor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colDoctor.DataPropertyName = "Doctor";
			this.colDoctor.FillWeight = 94.38452f;
			this.colDoctor.HeaderText = "医生";
			this.colDoctor.MinimumWidth = 100;
			this.colDoctor.Name = "colDoctor";
			this.colDoctor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colDoctor.Text = "";
			this.colDoctor.TextAlignment = System.Drawing.StringAlignment.Center;
			this.colDoctor.WordWrap = true;
			this.colAnteMeridiem1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colAnteMeridiem1.ColorTable = eButtonColor.Office2007WithBackground;
			this.colAnteMeridiem1.DataPropertyName = "AnteMeridiem1";
			this.colAnteMeridiem1.FillWeight = 94.38452f;
			this.colAnteMeridiem1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.colAnteMeridiem1.HeaderText = "上午";
			this.colAnteMeridiem1.Name = "colAnteMeridiem1";
			this.colAnteMeridiem1.ReadOnly = true;
			this.colAnteMeridiem1.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colAnteMeridiem1.Text = null;
			this.colPostMeridiem1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPostMeridiem1.ColorTable = eButtonColor.Office2007WithBackground;
			this.colPostMeridiem1.DataPropertyName = "PostMeridiem1";
			this.colPostMeridiem1.FillWeight = 94.38452f;
			this.colPostMeridiem1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.colPostMeridiem1.HeaderText = "下午";
			this.colPostMeridiem1.Name = "colPostMeridiem1";
			this.colPostMeridiem1.ReadOnly = true;
			this.colPostMeridiem1.SplitButton = true;
			this.colPostMeridiem1.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colPostMeridiem1.Text = null;
			this.colAnteMeridiem2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colAnteMeridiem2.ColorTable = eButtonColor.Office2007WithBackground;
			this.colAnteMeridiem2.DataPropertyName = "AnteMeridiem2";
			this.colAnteMeridiem2.FillWeight = 94.38452f;
			this.colAnteMeridiem2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.colAnteMeridiem2.HeaderText = "上午";
			this.colAnteMeridiem2.Name = "colAnteMeridiem2";
			this.colAnteMeridiem2.ReadOnly = true;
			this.colAnteMeridiem2.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colAnteMeridiem2.Text = null;
			this.colPostMeridiem2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPostMeridiem2.ColorTable = eButtonColor.Office2007WithBackground;
			this.colPostMeridiem2.DataPropertyName = "PostMeridiem2";
			this.colPostMeridiem2.FillWeight = 94.38452f;
			this.colPostMeridiem2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.colPostMeridiem2.HeaderText = "下午";
			this.colPostMeridiem2.Name = "colPostMeridiem2";
			this.colPostMeridiem2.ReadOnly = true;
			this.colPostMeridiem2.Text = null;
			this.colAnteMeridiem3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colAnteMeridiem3.ColorTable = eButtonColor.Office2007WithBackground;
			this.colAnteMeridiem3.DataPropertyName = "AnteMeridiem3";
			this.colAnteMeridiem3.FillWeight = 94.38452f;
			this.colAnteMeridiem3.HeaderText = "上午";
			this.colAnteMeridiem3.Name = "colAnteMeridiem3";
			this.colAnteMeridiem3.ReadOnly = true;
			this.colAnteMeridiem3.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colAnteMeridiem3.Text = null;
			this.colPostMeridiem3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPostMeridiem3.ColorTable = eButtonColor.Office2007WithBackground;
			this.colPostMeridiem3.DataPropertyName = "PostMeridiem3";
			this.colPostMeridiem3.FillWeight = 94.38452f;
			this.colPostMeridiem3.HeaderText = "下午";
			this.colPostMeridiem3.Name = "colPostMeridiem3";
			this.colPostMeridiem3.ReadOnly = true;
			this.colPostMeridiem3.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colPostMeridiem3.Text = null;
			this.colAnteMeridiem4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colAnteMeridiem4.ColorTable = eButtonColor.Office2007WithBackground;
			this.colAnteMeridiem4.DataPropertyName = "AnteMeridiem4";
			this.colAnteMeridiem4.FillWeight = 94.38452f;
			this.colAnteMeridiem4.HeaderText = "上午";
			this.colAnteMeridiem4.Name = "colAnteMeridiem4";
			this.colAnteMeridiem4.ReadOnly = true;
			this.colAnteMeridiem4.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colAnteMeridiem4.Text = null;
			this.colPostMeridiem4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPostMeridiem4.ColorTable = eButtonColor.Office2007WithBackground;
			this.colPostMeridiem4.DataPropertyName = "PostMeridiem4";
			this.colPostMeridiem4.FillWeight = 94.38452f;
			this.colPostMeridiem4.HeaderText = "下午";
			this.colPostMeridiem4.Name = "colPostMeridiem4";
			this.colPostMeridiem4.ReadOnly = true;
			this.colPostMeridiem4.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colPostMeridiem4.Text = null;
			this.colAnteMeridiem5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colAnteMeridiem5.ColorTable = eButtonColor.Office2007WithBackground;
			this.colAnteMeridiem5.DataPropertyName = "AnteMeridiem5";
			this.colAnteMeridiem5.FillWeight = 94.38452f;
			this.colAnteMeridiem5.HeaderText = "上午";
			this.colAnteMeridiem5.Name = "colAnteMeridiem5";
			this.colAnteMeridiem5.ReadOnly = true;
			this.colAnteMeridiem5.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colAnteMeridiem5.Text = null;
			this.colPostMeridiem5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPostMeridiem5.ColorTable = eButtonColor.Office2007WithBackground;
			this.colPostMeridiem5.DataPropertyName = "PostMeridiem5";
			this.colPostMeridiem5.FillWeight = 94.38452f;
			this.colPostMeridiem5.HeaderText = "下午";
			this.colPostMeridiem5.Name = "colPostMeridiem5";
			this.colPostMeridiem5.ReadOnly = true;
			this.colPostMeridiem5.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colPostMeridiem5.Text = null;
			this.colAnteMeridiem6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colAnteMeridiem6.ColorTable = eButtonColor.Office2007WithBackground;
			this.colAnteMeridiem6.DataPropertyName = "AnteMeridiem6";
			this.colAnteMeridiem6.FillWeight = 94.38452f;
			this.colAnteMeridiem6.HeaderText = "上午";
			this.colAnteMeridiem6.Name = "colAnteMeridiem6";
			this.colAnteMeridiem6.ReadOnly = true;
			this.colAnteMeridiem6.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colAnteMeridiem6.Text = null;
			this.colPostMeridiem6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPostMeridiem6.ColorTable = eButtonColor.Office2007WithBackground;
			this.colPostMeridiem6.DataPropertyName = "PostMeridiem6";
			this.colPostMeridiem6.FillWeight = 94.38452f;
			this.colPostMeridiem6.HeaderText = "下午";
			this.colPostMeridiem6.Name = "colPostMeridiem6";
			this.colPostMeridiem6.ReadOnly = true;
			this.colPostMeridiem6.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colPostMeridiem6.Text = null;
			this.colAnteMeridiem7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colAnteMeridiem7.ColorTable = eButtonColor.Office2007WithBackground;
			this.colAnteMeridiem7.DataPropertyName = "AnteMeridiem7";
			this.colAnteMeridiem7.FillWeight = 94.38452f;
			this.colAnteMeridiem7.HeaderText = "上午";
			this.colAnteMeridiem7.Name = "colAnteMeridiem7";
			this.colAnteMeridiem7.ReadOnly = true;
			this.colAnteMeridiem7.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colAnteMeridiem7.Text = null;
			this.colPostMeridiem7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPostMeridiem7.ColorTable = eButtonColor.Office2007WithBackground;
			this.colPostMeridiem7.DataPropertyName = "PostMeridiem7";
			this.colPostMeridiem7.FillWeight = 94.38452f;
			this.colPostMeridiem7.HeaderText = "下午";
			this.colPostMeridiem7.Name = "colPostMeridiem7";
			this.colPostMeridiem7.ReadOnly = true;
			this.colPostMeridiem7.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colPostMeridiem7.Text = null;
			this.colAnteMeridiem8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colAnteMeridiem8.ColorTable = eButtonColor.Office2007WithBackground;
			this.colAnteMeridiem8.DataPropertyName = "AnteMeridiem8";
			this.colAnteMeridiem8.FillWeight = 94.38452f;
			this.colAnteMeridiem8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.colAnteMeridiem8.HeaderText = "上午";
			this.colAnteMeridiem8.Name = "colAnteMeridiem8";
			this.colAnteMeridiem8.ReadOnly = true;
			this.colAnteMeridiem8.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colAnteMeridiem8.Text = null;
			this.colPostMeridiem8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPostMeridiem8.ColorTable = eButtonColor.Office2007WithBackground;
			this.colPostMeridiem8.DataPropertyName = "PostMeridiem8";
			this.colPostMeridiem8.FillWeight = 94.38452f;
			this.colPostMeridiem8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.colPostMeridiem8.HeaderText = "下午";
			this.colPostMeridiem8.Name = "colPostMeridiem8";
			this.colPostMeridiem8.ReadOnly = true;
			this.colPostMeridiem8.Style = eDotNetBarStyle.StyleManagerControlled;
			this.colPostMeridiem8.Text = null;
			this.tableLayoutPanel1.CausesValidation = false;
			this.tableLayoutPanel1.ColumnCount = 9;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 922f));
			this.tableLayoutPanel1.Controls.Add(this.lbDate8, 8, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbDate7, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbDate6, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbDate5, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbDate3, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbDate1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelX5, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbDate2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbDate4, 4, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 123);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1082, 35);
			this.tableLayoutPanel1.TabIndex = 0;
			this.lbDate8.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDate8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDate8.Location = new System.Drawing.Point(163, 3);
			this.lbDate8.Name = "lbDate8";
			this.lbDate8.Size = new System.Drawing.Size(916, 29);
			this.lbDate8.TabIndex = 9;
			this.lbDate8.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lbDate7.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDate7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDate7.Location = new System.Drawing.Point(143, 3);
			this.lbDate7.Name = "lbDate7";
			this.lbDate7.Size = new System.Drawing.Size(14, 29);
			this.lbDate7.TabIndex = 8;
			this.lbDate7.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lbDate6.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDate6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDate6.Location = new System.Drawing.Point(123, 3);
			this.lbDate6.Name = "lbDate6";
			this.lbDate6.Size = new System.Drawing.Size(14, 29);
			this.lbDate6.TabIndex = 7;
			this.lbDate6.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lbDate5.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDate5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDate5.Location = new System.Drawing.Point(103, 3);
			this.lbDate5.Name = "lbDate5";
			this.lbDate5.Size = new System.Drawing.Size(14, 29);
			this.lbDate5.TabIndex = 6;
			this.lbDate5.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lbDate3.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDate3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDate3.Location = new System.Drawing.Point(63, 3);
			this.lbDate3.Name = "lbDate3";
			this.lbDate3.Size = new System.Drawing.Size(14, 29);
			this.lbDate3.TabIndex = 5;
			this.lbDate3.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lbDate1.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDate1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDate1.Location = new System.Drawing.Point(23, 3);
			this.lbDate1.Name = "lbDate1";
			this.lbDate1.Size = new System.Drawing.Size(14, 29);
			this.lbDate1.TabIndex = 3;
			this.lbDate1.TextAlignment = System.Drawing.StringAlignment.Center;
			this.labelX5.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelX5.Location = new System.Drawing.Point(3, 3);
			this.labelX5.Name = "labelX5";
			this.labelX5.Size = new System.Drawing.Size(14, 29);
			this.labelX5.TabIndex = 2;
			this.labelX5.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lbDate2.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDate2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDate2.Location = new System.Drawing.Point(43, 3);
			this.lbDate2.Name = "lbDate2";
			this.lbDate2.Size = new System.Drawing.Size(14, 29);
			this.lbDate2.TabIndex = 0;
			this.lbDate2.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lbDate4.BackgroundStyle.CornerType = eCornerType.Square;
			this.lbDate4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDate4.Location = new System.Drawing.Point(83, 3);
			this.lbDate4.Name = "lbDate4";
			this.lbDate4.Size = new System.Drawing.Size(14, 29);
			this.lbDate4.TabIndex = 4;
			this.lbDate4.TextAlignment = System.Drawing.StringAlignment.Center;
			this.panelInfo.CanvasColor = System.Drawing.SystemColors.Control;
			this.panelInfo.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			this.panelInfo.DisabledBackColor = System.Drawing.Color.Empty;
			this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelInfo.Location = new System.Drawing.Point(0, 87);
			this.panelInfo.Name = "panelInfo";
			this.panelInfo.Size = new System.Drawing.Size(1082, 36);
			this.panelInfo.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelInfo.Style.BackColor1.Color = System.Drawing.Color.Transparent;
			this.panelInfo.Style.Font = new System.Drawing.Font("SimSun", 10.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.panelInfo.Style.ForeColor.Color = System.Drawing.Color.FromArgb(192, 0, 0);
			this.panelInfo.Style.GradientAngle = 90;
			this.panelInfo.Style.WordWrap = true;
			this.panelInfo.TabIndex = 3;
			this.plPriority.CanvasColor = System.Drawing.SystemColors.Control;
			this.plPriority.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			this.plPriority.Controls.Add(this.itemPrioritys);
			this.plPriority.Controls.Add(this.labelX4);
			this.plPriority.DisabledBackColor = System.Drawing.Color.Empty;
			this.plPriority.Dock = System.Windows.Forms.DockStyle.Top;
			this.plPriority.Location = new System.Drawing.Point(0, 50);
			this.plPriority.Name = "plPriority";
			this.plPriority.Size = new System.Drawing.Size(1082, 37);
			this.plPriority.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.plPriority.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
			this.plPriority.Style.Border = eBorderType.SingleLine;
			this.plPriority.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
			this.plPriority.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
			this.plPriority.Style.GradientAngle = 90;
			this.plPriority.TabIndex = 0;
			this.plPriority.Visible = false;
			this.itemPrioritys.AllowDrop = true;
			this.itemPrioritys.BackgroundStyle.Class = "ItemPanel";
			this.itemPrioritys.BackgroundStyle.CornerType = eCornerType.Square;
			this.itemPrioritys.BackgroundStyle.Description = "这是一个测试";
			this.itemPrioritys.BackgroundStyle.Name = "这是一个测试2";
			this.itemPrioritys.ContainerControlProcessDialogKey = true;
			this.itemPrioritys.Dock = System.Windows.Forms.DockStyle.Fill;
			this.itemPrioritys.DragDropSupport = true;
			this.itemPrioritys.ItemSpacing = 5;
			this.itemPrioritys.Location = new System.Drawing.Point(97, 0);
			this.itemPrioritys.MultiLine = true;
			this.itemPrioritys.Name = "itemPrioritys";
			this.itemPrioritys.Size = new System.Drawing.Size(985, 37);
			this.itemPrioritys.Style = eDotNetBarStyle.StyleManagerControlled;
			this.itemPrioritys.TabIndex = 1;
			this.itemPrioritys.Text = "测试下";
			this.itemPrioritys.UseNativeDragDrop = true;
			this.itemPrioritys.ItemDoubleClick += new System.Windows.Forms.MouseEventHandler(this.itemPrioritys_ItemDoubleClick);
			this.itemPrioritys.DragDrop += new System.Windows.Forms.DragEventHandler(this.itemPrioritys_DragDrop);
			this.itemPrioritys.DragEnter += new System.Windows.Forms.DragEventHandler(this.itemPrioritys_DragEnter);
			this.labelX4.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX4.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelX4.Location = new System.Drawing.Point(0, 0);
			this.labelX4.Name = "labelX4";
			this.labelX4.Size = new System.Drawing.Size(97, 37);
			this.labelX4.TabIndex = 0;
			this.labelX4.Text = "医生优先级：";
			this.labelX4.TextAlignment = System.Drawing.StringAlignment.Center;
			this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
			this.panelEx2.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
			this.panelEx2.Controls.Add(this.btnSwitch);
			this.panelEx2.Controls.Add(this.chkWaitforTime);
			this.panelEx2.Controls.Add(this.cmbDepartment);
			this.panelEx2.Controls.Add(this.labelX3);
			this.panelEx2.Controls.Add(this.cmbHospital);
			this.panelEx2.Controls.Add(this.labelX2);
			this.panelEx2.Controls.Add(this.cmbArea);
			this.panelEx2.Controls.Add(this.labelX1);
			this.panelEx2.Controls.Add(this.btnRefresh);
			this.panelEx2.DisabledBackColor = System.Drawing.Color.Empty;
			this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelEx2.Location = new System.Drawing.Point(0, 0);
			this.panelEx2.Name = "panelEx2";
			this.panelEx2.Size = new System.Drawing.Size(1082, 50);
			this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelEx2.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
			this.panelEx2.Style.Border = eBorderType.SingleLine;
			this.panelEx2.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
			this.panelEx2.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
			this.panelEx2.Style.GradientAngle = 90;
			this.panelEx2.TabIndex = 0;
			this.btnSwitch.BackgroundStyle.CornerType = eCornerType.Square;
			this.btnSwitch.Enabled = false;
			this.btnSwitch.Location = new System.Drawing.Point(800, 24);
			this.btnSwitch.Name = "btnSwitch";
			this.btnSwitch.OffText = "隐藏优先级";
			this.btnSwitch.OnText = "展开优先级";
			this.btnSwitch.Size = new System.Drawing.Size(110, 22);
			this.btnSwitch.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnSwitch.TabIndex = 8;
			this.btnSwitch.ValueChanged += new System.EventHandler(this.btnSwitch_ValueChanged);
			this.chkWaitforTime.BackgroundStyle.CornerType = eCornerType.Square;
			this.chkWaitforTime.FocusCuesEnabled = false;
			this.chkWaitforTime.Location = new System.Drawing.Point(801, 1);
			this.chkWaitforTime.Name = "chkWaitforTime";
			this.chkWaitforTime.Size = new System.Drawing.Size(109, 23);
			this.chkWaitforTime.Style = eDotNetBarStyle.StyleManagerControlled;
			this.chkWaitforTime.TabIndex = 6;
			this.chkWaitforTime.Text = "静候放票";
			this.chkWaitforTime.CheckedChanged += new System.EventHandler(this.chkWaitforTime_CheckedChanged);
			this.cmbDepartment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbDepartment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbDepartment.DisplayMember = "Text";
			this.cmbDepartment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbDepartment.FormattingEnabled = true;
			this.cmbDepartment.ItemHeight = 15;
			this.cmbDepartment.Location = new System.Drawing.Point(545, 16);
			this.cmbDepartment.Name = "cmbDepartment";
			this.cmbDepartment.Size = new System.Drawing.Size(238, 21);
			this.cmbDepartment.Style = eDotNetBarStyle.StyleManagerControlled;
			this.cmbDepartment.TabIndex = 5;
			this.cmbDepartment.SelectedIndexChanged += new System.EventHandler(this.cmbDepartment_SelectedIndexChanged);
			this.labelX3.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX3.Location = new System.Drawing.Point(504, 16);
			this.labelX3.Name = "labelX3";
			this.labelX3.Size = new System.Drawing.Size(49, 23);
			this.labelX3.TabIndex = 4;
			this.labelX3.Text = "科室：";
			this.cmbHospital.DisplayMember = "Text";
			this.cmbHospital.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbHospital.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbHospital.FormattingEnabled = true;
			this.cmbHospital.ItemHeight = 15;
			this.cmbHospital.Location = new System.Drawing.Point(206, 15);
			this.cmbHospital.Name = "cmbHospital";
			this.cmbHospital.Size = new System.Drawing.Size(272, 21);
			this.cmbHospital.Style = eDotNetBarStyle.StyleManagerControlled;
			this.cmbHospital.TabIndex = 2;
			this.cmbHospital.SelectedIndexChanged += new System.EventHandler(this.cmbHospital_SelectedIndexChanged);
			this.labelX2.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX2.Location = new System.Drawing.Point(167, 15);
			this.labelX2.Name = "labelX2";
			this.labelX2.Size = new System.Drawing.Size(49, 23);
			this.labelX2.TabIndex = 3;
			this.labelX2.Text = "医院：";
			this.cmbArea.DisplayMember = "Text";
			this.cmbArea.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbArea.FormattingEnabled = true;
			this.cmbArea.ItemHeight = 15;
			this.cmbArea.Location = new System.Drawing.Point(62, 15);
			this.cmbArea.Name = "cmbArea";
			this.cmbArea.Size = new System.Drawing.Size(79, 21);
			this.cmbArea.Style = eDotNetBarStyle.StyleManagerControlled;
			this.cmbArea.TabIndex = 1;
			this.cmbArea.DropDown += new System.EventHandler(this.cmbArea_DropDown);
			this.cmbArea.SelectedIndexChanged += new System.EventHandler(this.cmbArea_SelectedIndexChanged);
			this.labelX1.BackgroundStyle.CornerType = eCornerType.Square;
			this.labelX1.Location = new System.Drawing.Point(19, 15);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(49, 23);
			this.labelX1.TabIndex = 1;
			this.labelX1.Text = "地区：";
			this.btnRefresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnRefresh.ColorTable = eButtonColor.OrangeWithBackground;
			this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnRefresh.Font = new System.Drawing.Font("SimSun", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.btnRefresh.Location = new System.Drawing.Point(992, 0);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(90, 50);
			this.btnRefresh.Style = eDotNetBarStyle.StyleManagerControlled;
			this.btnRefresh.TabIndex = 0;
			this.btnRefresh.Text = "刷新";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.panelEx1);
			base.Name = "RegisterControl";
			base.Size = new System.Drawing.Size(1082, 504);
			base.SizeChanged += new System.EventHandler(this.RegisterControl_SizeChanged);
			this.panelEx1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dataGridViewX1).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.plPriority.ResumeLayout(false);
			this.panelEx2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
