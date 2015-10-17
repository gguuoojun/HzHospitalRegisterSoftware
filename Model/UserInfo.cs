using System;

namespace Model
{
	public class UserInfo
	{
		public string Name = string.Empty;

		public string CardId = string.Empty;

		public string PhoneNumber = string.Empty;

		public string Credibility = string.Empty;

		public void Clear()
		{
			this.Name = string.Empty;
			this.CardId = string.Empty;
			this.PhoneNumber = string.Empty;
			this.Credibility = string.Empty;
		}
	}
}
