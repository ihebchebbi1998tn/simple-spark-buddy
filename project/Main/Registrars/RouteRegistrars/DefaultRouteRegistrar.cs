namespace Main.Registrars.RouteRegistrars
{
	using Crm.Library.Modularization.Registrars;
	using Crm.Library.Routing.Constraints;
	using Crm.Library.Signalr;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;

	public class DefaultRouteRegistrar : IRouteRegistrar
	{
		public virtual RoutePriority Priority
		{
			get { return RoutePriority.Low; }
		}
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
				"Home",
				"Home",
				new { plugin = "Main", controller = "Home", action = "Index" },
				new { plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				"Error",
				"Error",
				new { plugin = "Main", controller = "Error", action = "Index" },
				new { plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/{controller}/{action}/{id?}",
				new { action = "Index", plugin = "Main" },
				new { plugin = "Main" }
			);

			endpoints.MapControllerRoute(
				"Default", // Route name
				pattern: "{controller}/{action}/{id}/{pageNumber}",
				defaults: new { plugin = "Main", controller = "Home", action = "Index", id = "0", pageNumber = "" },
				constraints: new { plugin = "Main", pageNumber = new IsPageNumber(), controller = "(?!api).*" }
				);
			endpoints.MapHub<ProfilingHub>("/profilingHub");
		}
	}
}
