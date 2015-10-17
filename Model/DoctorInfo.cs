using System;

namespace Model
{
	public class DoctorInfo
	{
		public string Name;

		public string HospitalName;

		public string Department;

		public string HospitalId;

		public void Clear()
		{
			this.Name = (this.HospitalId = (this.Department = (this.HospitalName = string.Empty)));
		}
	}
}
