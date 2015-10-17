using System;

namespace Model
{
	public class RegDoctor : System.ICloneable
	{
		public string Name;

		public string[] Values;

		public string[] Tags;

		public string[] ToolTipTexts;

		public object Clone()
		{
			RegDoctor regDoctor = (RegDoctor)base.MemberwiseClone();
			regDoctor.Tags = new string[this.Tags.Length];
			regDoctor.ToolTipTexts = new string[this.ToolTipTexts.Length];
			regDoctor.Values = new string[this.Values.Length];
			System.Array.Copy(this.Values, regDoctor.Values, this.Values.Length);
			System.Array.Copy(this.ToolTipTexts, regDoctor.ToolTipTexts, this.ToolTipTexts.Length);
			System.Array.Copy(this.Tags, regDoctor.Tags, this.Tags.Length);
			return regDoctor;
		}
	}
}
