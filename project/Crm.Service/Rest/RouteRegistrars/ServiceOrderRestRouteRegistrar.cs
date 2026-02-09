namespace Crm.Service.Rest.RouteRegistrars
{

	using Crm.Library.Modularization.Registrars;
	using Crm.Library.Rest;
	using Crm.Library.Routing.Constraints;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.AspNetCore.Routing.Constraints;

	public class ServiceOrderRestRouteRegistrar : IRouteRegistrar
	{
		public virtual RoutePriority Priority
		{
			get { return RoutePriority.Important; }
		}
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
				null,
				"ServiceOrders/{serviceOrderNo}/Dispatches.{format}",
				new { plugin = "Crm.Service", controller = "ServiceOrderRest", action = "ListDispatches" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Crm.Service" }
				);

			endpoints.MapControllerRoute(
				null,
				"ServiceOrders/{serviceOrderNo}/Dispatches.{format}",
				new { plugin = "Crm.Service", controller = "ServiceOrderRest", action = "CreateDispatch" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("POST"), plugin = "Crm.Service" }
				);

			endpoints.MapControllerRoute(
				null,
				"ServiceOrders/{serviceOrderNo}/Dispatches.{format}",
				new { plugin = "Crm.Service", controller = "ServiceOrderRest", action = "UpdateDispatch" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("PUT"), plugin = "Crm.Service" }
				);

			endpoints.MapControllerRoute(
				null,
				"ServiceOrders/{serviceOrderNo}/Dispatches/{dispatchId}",
				new { plugin = "Crm.Service", controller = "ServiceOrderRest", action = "DeleteDispatch" },
				new { dispatchId = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("DELETE"), plugin = "Crm.Service" }
				);
		}
	}
}
