namespace Sms.Scheduler
{

	using Crm.Library.Modularization.Registrars;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;

	public class Routes : IRouteRegistrar
	{
		public virtual RoutePriority Priority
		{
			get { return RoutePriority.AboveNormal; }
		}
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
				null,
				"Sms.Scheduler/{controller}/{action}/{id?}",
				new { action = "DetailsTemplate", plugin = "Sms.Scheduler" },
				new { plugin = "Sms.Scheduler" }
			);
		}
	}
}
