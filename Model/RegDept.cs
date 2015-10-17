using System;
using System.Collections.Generic;
using Utility;

namespace Model
{
	public class RegDept
	{
		public int RealCount;

		public System.Collections.Generic.List<RegDoctor> ListDoctors;

		public ResponseReuslt ResResult;

		public string[] Dates = new string[8];

		public RegDept()
		{
			this.ListDoctors = new System.Collections.Generic.List<RegDoctor>();
		}

		public void AddDoctor(RegDoctor doctor)
		{
			if (this.RealCount < this.ListDoctors.Count)
			{
				this.ListDoctors[this.RealCount].Name = doctor.Name;
				for (int i = 0; i < doctor.Values.Length; i++)
				{
					this.ListDoctors[this.RealCount].Values[i] = doctor.Values[i];
					this.ListDoctors[this.RealCount].ToolTipTexts[i] = doctor.ToolTipTexts[i];
					this.ListDoctors[this.RealCount].Tags[i] = doctor.Tags[i];
				}
			}
			else
			{
				this.ListDoctors.Add((RegDoctor)doctor.Clone());
			}
			this.RealCount++;
		}

		public void Reset()
		{
			this.RealCount = 0;
		}
	}
}
