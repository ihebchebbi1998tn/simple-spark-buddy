using Crm.Library.Modularization.Registrars;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Main.Flow
{
	public class Routes : IRouteRegistrar
	{
		public virtual RoutePriority Priority => RoutePriority.Normal;

		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
				null,
				"Main.Flow/{controller}/{action}/{id?}",
				new { action = "Index", plugin = "Main.Flow" },
				new { plugin = "Main.Flow" }
				);
		}
	}
}
