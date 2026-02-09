namespace Main.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[TimeUnit]", "TimeUnitId")]
	public class TimeUnit : EntityLookup<string>, ILookupWithTimeUnitsPerYear
	{
		[LookupProperty(Shared = true)]
		public virtual int? TimeUnitsPerYear { get; set; }

		public const string YearKey = "Year";
		public const string MonthKey = "Month";
		public const string QuarterKey = "Quarter";
		public const string WeekKey = "Week";
		public const string DayKey = "Day";
		public const string HourKey = "Hour";
		public const string MinuteKey = "Minute";
		public const string SecondKey = "Second";
		public const string MillisecondKey = "Millisecond";
	}
}
