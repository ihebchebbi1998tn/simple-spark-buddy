namespace Main.Model
{
	using System;
	using System.Globalization;

	public class NumberingSequence
	{
		public virtual string SequenceName { get; set; }
		public virtual long? MaxLow { get; set; }
		public virtual long LastNumber { get; set; }
		public virtual string Prefix { get; set; }
		public virtual string Format { get; set; }
		public virtual string Suffix { get; set; }

		public override string ToString()
		{
			return String.Format("{0}{1}{2}", Prefix, (LastNumber * (MaxLow ?? 1)).ToString(Format, CultureInfo.InvariantCulture), Suffix);
		}
	}
}
