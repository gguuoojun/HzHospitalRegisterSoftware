using System.Threading;
using System;

namespace Utility
{
	public class BeijingTime
	{
        private const string HOST = "cn.ntp.org.cn";

		private static BeijingTime _instance;

		private NTPClient _client;

		private System.TimeSpan _tsClock = new System.TimeSpan(0L);

		private bool _IsConnect;

        private Thread syncThread;
        private bool bIsExitThread = false;       //是否退出线程
        private AutoResetEvent autoEvent = new AutoResetEvent(false);

        public delegate void NTPServerTimeConnected(bool isConnected, TimeSpan tsClock);
        public event NTPServerTimeConnected NTPServerTimeConnectedEventHander;

        public bool IsConnect
        {
            get
            {
                return this._IsConnect;
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
            this._client = new NTPClient(HOST);
            syncThread = new Thread(new ThreadStart(SyncTime));
            syncThread.IsBackground = true;
            syncThread.Start();
		}

		public bool SetLocalTime(System.DateTime dtLocal)
		{
			return this._client.SetTime(dtLocal);
		}

        public void GetNtpTime()
        {
            autoEvent.Set();
        }

		private void Connect()
		{
			try
			{
				this._client.Connect();
				this._IsConnect = true;
				this._tsClock = new System.TimeSpan((long)this._client.LocalClockOffset);
			}
			catch (System.Exception)
			{
				this._IsConnect = false;
			}
		}

        /// <summary>
        /// 获取NTP服务器端时间
        /// </summary>
        private void SyncTime()
        {
            while(!bIsExitThread)
            {
                autoEvent.WaitOne();
                if (bIsExitThread)
                {
                    return;
                }

                Connect();

                if (NTPServerTimeConnectedEventHander != null)
                {
                    NTPServerTimeConnectedEventHander(_IsConnect, _tsClock);
                }
            }           
        }

        public void Close()
        {
            if (syncThread != null)
            {
                bIsExitThread = true;
                autoEvent.Set();
                syncThread = null;
            }
        }
	}
}
