namespace Main.SearchCriteria
{
	using System;

	using Crm.Library.Model;

	public abstract class TimeSpanSearchCriteria
	{
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public bool NoDeterminedTimespan { get; set; }
		public TimeSpanItem SelectedTime { get; set; }

		public static TimeSpanItem[] PastTimeSpanItems
		{
			get
			{
				return new[]
					{
						TimeSpanItem.All,
						TimeSpanItem.LastMonth,
						TimeSpanItem.ThisMonth,
						TimeSpanItem.ThisYear,
						TimeSpanItem.LastYear,
						TimeSpanItem.FromTo
					};
			}
		}

		public static TimeSpanItem[] AllTimeSpanItems
		{
			get
			{
				return new[]
					{
						TimeSpanItem.All,
						TimeSpanItem.LastMonth,
						TimeSpanItem.Next3Months,
						TimeSpanItem.ThisMonth,
						TimeSpanItem.NextMonth,
						TimeSpanItem.ThisYear,
						TimeSpanItem.LastYear,
						TimeSpanItem.FromTo
					};
			}
		}
	}
}
