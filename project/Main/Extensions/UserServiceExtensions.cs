namespace Main.Extensions
{
	using System;

	using Crm.Library.Services.Interfaces;

	public static class UserServiceExtensions
	{
		/// <summary>
		/// Evaluates the local DateTime from a UtcDateTime for the current user.
		/// </summary>
		/// <param name="userService">IUserService</param>
		/// <param name="utcTime">UtcTime</param>
		/// <returns>Local DateTime</returns>
		public static DateTime GetLocalDateTimeForCurrentUser(this IUserService userService, DateTime utcTime)
		{
			var currentUser = userService.CurrentUser;
			return utcTime.GetLocalDateTimeFor(currentUser);
		}
		public static DateTime GetLocalDateTimeForCurrentUser(this IUserService userService, DateTime? utcTime)
		{
			var updatedTime = utcTime ?? DateTime.UtcNow;
			return userService.GetLocalDateTimeForCurrentUser(updatedTime);
		}
	}
}
