using System;
using System.IO;
using System.Xml.Serialization;

namespace Utility
{
	internal class ObjectXmlSerializer
	{
		public static T LoadFromXml<T>(string fileName) where T : class
		{
			System.IO.FileStream fileStream = null;
			T result;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				result = (T)((object)xmlSerializer.Deserialize(fileStream));
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
			return result;
		}

		public static void SaveToXml<T>(string fileName, T data) where T : class
		{
			System.IO.FileStream fileStream = null;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
				xmlSerializer.Serialize(fileStream, data);
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}
	}
}
