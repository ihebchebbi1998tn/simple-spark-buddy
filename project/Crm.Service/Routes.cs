namespace Crm.Service
{

	using Crm.Library.Modularization.Registrars;
	using Crm.Library.Rest;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.AspNetCore.Routing.Constraints;

	public class ServiceRouteRegistrar : IRouteRegistrar
	{
		public virtual RoutePriority Priority
		{
			get { return RoutePriority.AboveNormal; }
		}
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
				null,
				"Crm.Service/Dispatches.{format}",
				new { plugin = "Crm.Service", controller = "ServiceOrderRest", action = "UpcomingDispatches" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Crm.Service" }
				);

			endpoints.MapControllerRoute(
				null,
				"Crm.Service/{controller}/{action}.{format}",
				new { plugin = "Crm.Service" },
				new { format = new IsJsonOrXml(), controller = new RestControllerName(), plugin = "Crm.Service" }
				);

			endpoints.MapControllerRoute(
				null,
				"Crm.Service/{controller}/{action}/{id?}",
				new { action = "Index", plugin = "Crm.Service" },
				new { plugin = "Crm.Service" }
				);
		}
	}
}
