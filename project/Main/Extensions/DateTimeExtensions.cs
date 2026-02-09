namespace Main.Extensions
{
	using System;

	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model;

	using Main.Model.Lookups;

	public static class DateTimeExtensions
	{
		/// <summary>
		/// Evaluates the local DateTime from a UtcDateTime for the passed user.
		/// </summary>
		/// <param name="utcTime">UtcTime</param>
		/// <param name="user">User for which the local time should be evaluated</param>
		/// <returns>Local DateTime</returns>
		public static DateTime GetLocalDateTimeFor(this DateTime utcTime, User user)
		{
			var timeZoneInfo = user?.TimeZoneInfo ?? TimeZoneInfo.Local;
			return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
		}
		public static DateTime AddTimeSpan(this DateTime dateTime, int count, string timeUnit)
		{
			switch (timeUnit)
			{
				case TimeUnit.YearKey:
					return dateTime.AddYears(count);
				case TimeUnit.QuarterKey:
					return dateTime.AddMonths(3 * count);
				case TimeUnit.MonthKey:
					return dateTime.AddMonths(count);
				case TimeUnit.WeekKey:
					return dateTime.AddDays(7 * count);
				case TimeUnit.DayKey:
					return dateTime.AddDays(count);
				case TimeUnit.HourKey:
					return dateTime.AddHours(count);
				case TimeUnit.MinuteKey:
					return dateTime.AddMinutes(count);
				case TimeUnit.SecondKey:
					return dateTime.AddSeconds(count);
				case TimeUnit.MillisecondKey:
					return dateTime.AddMilliseconds(count);
				default:
					throw new ApplicationException("This code should be unreachable.");
			}
		}
		public static string TimeAgo(this IResourceManager resourceManager, DateTime date)
		{
			return TimeAgo(x => resourceManager.GetTranslation(x), date);
		}
		private static string TimeAgo(Func<string, string> localize, DateTime date)
		{
			var timeSince = DateTime.Now.Subtract(date);

			if (timeSince.TotalMilliseconds < 1)
			{
				return localize("TimeAgoNotYet");
			}
			if (timeSince.TotalSeconds < 1.0)
			{
				return localize("TimeAgoRightNow");
			}
			if (timeSince.TotalSeconds == 1)
			{
				return localize("TimeAgoOneSecond");
			}
			if (timeSince.TotalMinutes < 1)
			{
				return string.Format(localize("TimeAgoSeconds"), timeSince.Seconds); // "{0} seconds ago"
			}
			if (timeSince.TotalMinutes < 2)
			{
				return localize("TimeAgoOneMinute");
			}
			if (timeSince.TotalMinutes < 60)
			{
				return string.Format(localize("TimeAgoMinutes"), timeSince.Minutes); // "{0} minutes ago"
			}
			if (timeSince.TotalMinutes < 120)
			{
				return localize("TimeAgoOneHour");
			}
			if (timeSince.TotalHours < 24)
			{
				return string.Format(localize("TimeAgoHours"), timeSince.Hours); // "{0} hours ago"
			}
			if (timeSince.TotalDays == 1)
			{
				return localize("TimeAgoYesterday");
			}
			if (timeSince.TotalDays < 7)
			{
				return string.Format(localize("TimeAgoDays"), timeSince.Days); //"{0} days ago"
			}
			if (timeSince.TotalDays < 14)
			{
				return localize("TimeAgoLastWeek");
			}
			if (timeSince.TotalDays < 21)
			{
				return localize("TimeAgoTwoWeeksAgo");
			}
			if (timeSince.TotalDays < 28)
			{
				return localize("TimeAgoThreeWeeksAgo");
			}
			if (timeSince.TotalDays < 60)
			{
				return localize("TimeAgoLastMonth");
			}
			if (timeSince.TotalDays < 365)
			{
				return string.Format(localize("TimeAgoMonths"), Math.Round(timeSince.TotalDays / 30)); // "{0} months ago"
			}

			//last but not least...
			return timeSince.TotalDays < 730
							? localize("TimeAgoLastYear")
							: string.Format(localize("TimeAgoYears"), Math.Round(timeSince.TotalDays / 365)); // "{0} years ago"
		}
	}
}
