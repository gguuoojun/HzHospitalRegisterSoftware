using System;
using System.Collections.Generic;

namespace Model
{
	public class OrderInfo
	{
		public UserInfo User;

		public DoctorInfo Doctor;

		public string VisitDate;

		public string Fee;

		public string CheckOrderPost;

		public System.Collections.Generic.List<VisitTime> VisitTimes;

		public ResponseReuslt ResResult;

		public OrderInfo()
		{
			this.VisitTimes = new System.Collections.Generic.List<VisitTime>();
			this.Doctor = new DoctorInfo();
			this.User = new UserInfo();
		}

		public void Clear()
		{
			this.VisitTimes.Clear();
			this.VisitDate = string.Empty;
			this.Fee = string.Empty;
			this.Doctor.Clear();
			this.CheckOrderPost = string.Empty;
		}
	}
}
