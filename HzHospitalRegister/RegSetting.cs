using System;
using System.IO;
using System.Windows.Forms;
using Utility;

namespace HzHospitalRegister
{
	public class RegSetting
	{
		private readonly string SETTING_PATH = System.Windows.Forms.Application.StartupPath + "\\Setting.data";

		public string SoundPath;

		public bool HideWindows;

		private static RegSetting _instance;

		public static RegSetting Instance
		{
			get
			{
				if (RegSetting._instance == null)
				{
					RegSetting._instance = new RegSetting();
				}
				return RegSetting._instance;
			}
		}

		private RegSetting()
		{
			this.HideWindows = false;
			this.SoundPath = System.Windows.Forms.Application.StartupPath + "\\Sound\\马里奥.wav";
		}

		public void ReadSetting()
		{
			try
			{
				if (System.IO.File.Exists(this.SETTING_PATH))
				{
					RegSetting._instance = ObjectXmlSerializer.LoadFromXml<RegSetting>(this.SETTING_PATH);
				}
			}
			catch (System.Exception err)
			{
				Log.WriteError("读取配置信息错误", err);
			}
		}

		public void WriteSetting()
		{
			try
			{
				ObjectXmlSerializer.SaveToXml<RegSetting>(this.SETTING_PATH, RegSetting._instance);
			}
			catch (System.Exception err)
			{
				Log.WriteError("写入配置信息错误", err);
			}
		}
	}
}
