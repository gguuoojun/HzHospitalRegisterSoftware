using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Utility
{
    public enum _LeapIndicator
    {
        NoWarning,
        LastMinute61,
        LastMinute59,
        Alarm
    }

    public enum _Mode
    {
        SymmetricActive,
        SymmetricPassive,
        Client,
        Server,
        Broadcast,
        Unknown
    }

    public enum _Stratum
    {
        Unspecified,
        PrimaryReference,
        SecondaryReference,
        Reserved
    }

	public class NTPClient
	{
		private struct SYSTEMTIME
		{
			public short year;

			public short month;

			public short dayOfWeek;

			public short day;

			public short hour;

			public short minute;

			public short second;

			public short milliseconds;
		}

		private const byte NTPDataLength = 48;

		private const byte offReferenceID = 12;

		private const byte offReferenceTimestamp = 16;

		private const byte offOriginateTimestamp = 24;

		private const byte offReceiveTimestamp = 32;

		private const byte offTransmitTimestamp = 40;

		private byte[] NTPData = new byte[48];

		public System.DateTime ReceptionTimestamp;

		private string TimeServer;

		public _LeapIndicator LeapIndicator
		{
			get
			{
				switch ((byte)(this.NTPData[0] >> 6))
				{
				case 0:
					return _LeapIndicator.NoWarning;
				case 1:
					return _LeapIndicator.LastMinute61;
				case 2:
					return _LeapIndicator.LastMinute59;
				}
				return _LeapIndicator.Alarm;
			}
		}

		public byte VersionNumber
		{
			get
			{
				return (byte)((this.NTPData[0] & 56) >> 3);
			}
		}

		public _Mode Mode
		{
			get
			{
				switch (this.NTPData[0] & 7)
				{
				case 0:
				case 6:
				case 7:
					return _Mode.Unknown;
				case 1:
					return _Mode.SymmetricActive;
				case 2:
					return _Mode.SymmetricPassive;
				case 3:
					return _Mode.Client;
				case 4:
					return _Mode.Server;
				case 5:
					return _Mode.Broadcast;
				}
				return _Mode.Unknown;
			}
		}

		public _Stratum Stratum
		{
			get
			{
				byte b = this.NTPData[1];
				if (b == 0)
				{
					return _Stratum.Unspecified;
				}
				if (b == 1)
				{
					return _Stratum.PrimaryReference;
				}
				if (b <= 15)
				{
					return _Stratum.SecondaryReference;
				}
				return _Stratum.Reserved;
			}
		}

		public uint PollInterval
		{
			get
			{
				return (uint)System.Math.Round(System.Math.Pow(2.0, (double)this.NTPData[2]));
			}
		}

		public double Precision
		{
			get
			{
				return 1000.0 * System.Math.Pow(2.0, (double)this.NTPData[3]);
			}
		}

		public double RootDelay
		{
			get
			{
				int num = 256 * (256 * (256 * (int)this.NTPData[4] + (int)this.NTPData[5]) + (int)this.NTPData[6]) + (int)this.NTPData[7];
				return 1000.0 * ((double)num / 65536.0);
			}
		}

		public double RootDispersion
		{
			get
			{
				int num = 256 * (256 * (256 * (int)this.NTPData[8] + (int)this.NTPData[9]) + (int)this.NTPData[10]) + (int)this.NTPData[11];
				return 1000.0 * ((double)num / 65536.0);
			}
		}

		public string ReferenceID
		{
			get
			{
				string text = "";
				switch (this.Stratum)
				{
				case _Stratum.Unspecified:
				case _Stratum.PrimaryReference:
					text += (char)this.NTPData[12];
					text += (char)this.NTPData[13];
					text += (char)this.NTPData[14];
					text += (char)this.NTPData[15];
					break;
				case _Stratum.SecondaryReference:
				{
					switch (this.VersionNumber)
					{
					case 3:
					{
						string text2 = string.Concat(new string[]
						{
							this.NTPData[12].ToString(),
							".",
							this.NTPData[13].ToString(),
							".",
							this.NTPData[14].ToString(),
							".",
							this.NTPData[15].ToString()
						});
						try
						{
							System.Net.IPHostEntry hostByAddress = System.Net.Dns.GetHostByAddress(text2);
							text = hostByAddress.HostName + " (" + text2 + ")";
							return text;
						}
						catch (System.Exception)
						{
							text = "N/A";
							return text;
						}
						
					}
					case 4:
						break;
					default:
						text = "N/A";
						return text;
					}
					System.DateTime d = this.ComputeDate(this.GetMilliSeconds(12));
					System.TimeSpan utcOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now);
					text = (d + utcOffset).ToString();
					break;
				}
				}
				return text;
			}
		}

		public System.DateTime ReferenceTimestamp
		{
			get
			{
				System.DateTime d = this.ComputeDate(this.GetMilliSeconds(16));
				System.TimeSpan utcOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now);
				return d + utcOffset;
			}
		}

		public System.DateTime OriginateTimestamp
		{
			get
			{
				return this.ComputeDate(this.GetMilliSeconds(24));
			}
		}

		public System.DateTime ReceiveTimestamp
		{
			get
			{
				System.DateTime d = this.ComputeDate(this.GetMilliSeconds(32));
				System.TimeSpan utcOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now);
				return d + utcOffset;
			}
		}

		public System.DateTime TransmitTimestamp
		{
			get
			{
				System.DateTime d = this.ComputeDate(this.GetMilliSeconds(40));
				System.TimeSpan utcOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now);
				return d + utcOffset;
			}
			set
			{
				this.SetDate(40, value);
			}
		}

		public int RoundTripDelay
		{
			get
			{
				return (int)(this.ReceiveTimestamp - this.OriginateTimestamp + (this.ReceptionTimestamp - this.TransmitTimestamp)).TotalMilliseconds;
			}
		}

		public int LocalClockOffset
		{
			get
			{
				return (int)((this.ReceiveTimestamp - this.OriginateTimestamp - (this.ReceptionTimestamp - this.TransmitTimestamp)).TotalMilliseconds / 2.0);
			}
		}

		private System.DateTime ComputeDate(ulong milliseconds)
		{
			System.TimeSpan t = System.TimeSpan.FromMilliseconds(milliseconds);
			System.DateTime dateTime = new System.DateTime(1900, 1, 1);
			dateTime += t;
			return dateTime;
		}

		private ulong GetMilliSeconds(byte offset)
		{
			ulong num = 0uL;
			ulong num2 = 0uL;
			for (int i = 0; i <= 3; i++)
			{
				num = 256uL * num + (ulong)this.NTPData[(int)offset + i];
			}
			for (int j = 4; j <= 7; j++)
			{
				num2 = 256uL * num2 + (ulong)this.NTPData[(int)offset + j];
			}
			ulong arg_5A_0 = num2 * 1000uL / 4294967296uL;
			return num * 1000uL + num2 * 1000uL / 4294967296uL;
		}

		private void SetDate(byte offset, System.DateTime date)
		{
			System.DateTime d = new System.DateTime(1900, 1, 1, 0, 0, 0);
			ulong num = (ulong)(date - d).TotalMilliseconds;
			ulong num2 = num / 1000uL;
			ulong num3 = num % 1000uL * 4294967296uL / 1000uL;
			ulong num4 = num2;
			for (int i = 3; i >= 0; i--)
			{
				this.NTPData[(int)offset + i] = (byte)(num4 % 256uL);
				num4 /= 256uL;
			}
			num4 = num3;
			for (int j = 7; j >= 4; j--)
			{
				this.NTPData[(int)offset + j] = (byte)(num4 % 256uL);
				num4 /= 256uL;
			}
		}

		private void Initialize()
		{
			this.NTPData[0] = 27;
			for (int i = 1; i < 48; i++)
			{
				this.NTPData[i] = 0;
			}
			this.TransmitTimestamp = System.DateTime.Now;
		}

		public NTPClient(string host)
		{
			this.TimeServer = host;
		}

		public void Connect()
		{
			try
			{
				System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(this.TimeServer);
				System.Net.IPEndPoint endPoint = new System.Net.IPEndPoint(hostEntry.AddressList[0], 123);
				System.Net.Sockets.UdpClient udpClient = new System.Net.Sockets.UdpClient();
                udpClient.Client.ReceiveTimeout = 1000;
                udpClient.Client.SendTimeout = 1000;
				udpClient.Connect(endPoint);
				this.Initialize();
				udpClient.Send(this.NTPData, this.NTPData.Length);
				this.NTPData = udpClient.Receive(ref endPoint);
				this.ReceptionTimestamp = System.DateTime.Now;
				if (!this.IsResponseValid())
				{
					throw new System.Exception("Invalid response from " + this.TimeServer);
				}
			}
			catch (System.Net.Sockets.SocketException ex)
			{
				throw new System.Exception(ex.Message);
			}
		}

		public bool IsResponseValid()
		{
			return this.NTPData.Length >= 48 && this.Mode == _Mode.Server;
		}

		public override string ToString()
		{
			string str = "Leap Indicator: ";
			switch (this.LeapIndicator)
			{
			case _LeapIndicator.NoWarning:
				str += "No warning";
				break;
			case _LeapIndicator.LastMinute61:
				str += "Last minute has 61 seconds";
				break;
			case _LeapIndicator.LastMinute59:
				str += "Last minute has 59 seconds";
				break;
			case _LeapIndicator.Alarm:
				str += "Alarm Condition (clock not synchronized)";
				break;
			}
			str = str + "\r\nVersion number: " + this.VersionNumber.ToString() + "\r\n";
			str += "Mode: ";
			switch (this.Mode)
			{
			case _Mode.SymmetricActive:
				str += "Symmetric Active";
				break;
			case _Mode.SymmetricPassive:
				str += "Symmetric Pasive";
				break;
			case _Mode.Client:
				str += "Client";
				break;
			case _Mode.Server:
				str += "Server";
				break;
			case _Mode.Broadcast:
				str += "Broadcast";
				break;
			case _Mode.Unknown:
				str += "Unknown";
				break;
			}
			str += "\r\nStratum: ";
			switch (this.Stratum)
			{
			case _Stratum.Unspecified:
			case _Stratum.Reserved:
				str += "Unspecified";
				break;
			case _Stratum.PrimaryReference:
				str += "Primary Reference";
				break;
			case _Stratum.SecondaryReference:
				str += "Secondary Reference";
				break;
			}
			str = str + "\r\nLocal time: " + this.TransmitTimestamp.ToString();
			str = str + "\r\nPrecision: " + this.Precision.ToString() + " ms";
			str = str + "\r\nPoll Interval: " + this.PollInterval.ToString() + " s";
			str = str + "\r\nReference ID: " + this.ReferenceID.ToString();
			str = str + "\r\nRoot Dispersion: " + this.RootDispersion.ToString() + " ms";
			str = str + "\r\nRound Trip Delay: " + this.RoundTripDelay.ToString() + " ms";
			str = str + "\r\nLocal Clock Offset: " + this.LocalClockOffset.ToString() + " ms";
			return str + "\r\n";
		}

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static extern bool SetLocalTime(ref NTPClient.SYSTEMTIME time);

		public bool SetTime(System.DateTime trts)
		{
			NTPClient.SYSTEMTIME sYSTEMTIME;
			sYSTEMTIME.year = (short)trts.Year;
			sYSTEMTIME.month = (short)trts.Month;
			sYSTEMTIME.dayOfWeek = (short)trts.DayOfWeek;
			sYSTEMTIME.day = (short)trts.Day;
			sYSTEMTIME.hour = (short)trts.Hour;
			sYSTEMTIME.minute = (short)trts.Minute;
			sYSTEMTIME.second = (short)trts.Second;
			sYSTEMTIME.milliseconds = (short)trts.Millisecond;
			return NTPClient.SetLocalTime(ref sYSTEMTIME);
		}
	}
}
