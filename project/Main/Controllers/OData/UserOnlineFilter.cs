namespace Main.Controllers.OData
{
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Signalr;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;

	public class UserOnlineFilter : IODataQueryFunction, IDependency
	{
		private static readonly MethodInfo filterByOnlineStatusInfo = typeof(UserOnlineFilter).GetMethod(nameof(FilterByOnlineStatus), BindingFlags.Instance | BindingFlags.NonPublic);
		private readonly ISignalrProfiler signalrProfiler;
		public UserOnlineFilter(ISignalrProfiler signalrProfiler)
		{
			this.signalrProfiler = signalrProfiler;
		}
		protected virtual IQueryable<T> FilterByOnlineStatus<T>(IQueryable<T> query)
			where T : User
		{
			var connectedUsers = signalrProfiler.ConnectedUsers.Select(x => x.Id).ToArray();
			return query.Where(x => connectedUsers.Contains(x.Id));
		}
		public virtual IQueryable<T> Apply<T, TRest>([FromQuery] ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(User).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}

			var filterByOnlineStatus = options.Request.GetQueryParameter("filterByOnlineStatus")?.Trim();
			if (string.IsNullOrEmpty(filterByOnlineStatus))
			{
				return query;
			}

			return (IQueryable<T>)filterByOnlineStatusInfo.MakeGenericMethod(typeof(T)).Invoke(this, new object[] { query });
		}
	}
}
