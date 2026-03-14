namespace Crm.Service.Team
{
	using Crm.Library.Modularization.Registrars;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;

	public class Routes : IRouteRegistrar
	{
		public virtual RoutePriority Priority => RoutePriority.Normal;
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
				null,
				"Crm.Service.Team/{controller}/{action}/{id?}",
				new
				{
					action = "Index",
					plugin = "Crm.Service.Team"
				},
				new { plugin = "Crm.Service.Team" }
			);
		}
	}
}
