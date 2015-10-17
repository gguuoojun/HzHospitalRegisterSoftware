using log4net;
using System;

namespace HzHospitalRegister
{
	internal class Log
	{
		private static readonly ILog m_logInfo = LogManager.GetLogger("LogInfo");

		private static readonly ILog m_logError = LogManager.GetLogger("LogError");

		public static void WriteInfo(string info)
		{
			if (Log.m_logInfo != null && Log.m_logInfo.IsInfoEnabled)
			{
				Log.m_logInfo.Info(info);
			}
		}

		public static void WriteInfo(string info, System.Exception err)
		{
			if (Log.m_logInfo != null && Log.m_logInfo.IsInfoEnabled)
			{
				Log.m_logInfo.Info(info, err);
			}
		}

		public static void WriteError(string error)
		{
			if (Log.m_logError != null && Log.m_logError.IsErrorEnabled)
			{
				Log.m_logError.Error(error);
			}
		}

		public static void WriteError(string error, System.Exception err)
		{
			if (Log.m_logError != null && Log.m_logError.IsErrorEnabled)
			{
				Log.m_logError.Error(error, err);
			}
		}
	}
}
