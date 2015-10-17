using System;

namespace Utility
{
	public class BeijingTime
	{
		private const string HOST = "ntp.sjtu.edu.cn";

		private static BeijingTime _instance;

		private NTPClient _client;

		private System.TimeSpan _tsClock = new System.TimeSpan(0L);

		private bool _IsConnect;

		public bool IsConnect
		{
			get
			{
				return this._IsConnect;
			}
		}

		public System.DateTime BeijingTimeNow
		{
			get
			{
				return System.DateTime.Now.Add(this._tsClock);
			}
		}

		public static BeijingTime Instance
		{
			get
			{
				if (BeijingTime._instance == null)
				{
					BeijingTime._instance = new BeijingTime();
				}
				return BeijingTime._instance;
			}
		}

		private BeijingTime()
		{
			this._client = new NTPClient("ntp.sjtu.edu.cn");
		}

		public bool SetLocalTime(System.DateTime dtLocal)
		{
			return this._client.SetTime(dtLocal);
		}

		public bool Connect()
		{
			bool result;
			try
			{
				this._client.Connect();
				this._IsConnect = true;
				this._tsClock = new System.TimeSpan((long)this._client.LocalClockOffset);
				result = true;
			}
			catch (System.Exception)
			{
				this._IsConnect = false;
				result = false;
			}
			return result;
		}
	}
}
