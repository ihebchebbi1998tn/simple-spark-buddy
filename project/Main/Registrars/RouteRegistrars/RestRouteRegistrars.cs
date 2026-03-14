namespace Main.Registrars.RouteRegistrars
{
	using Crm.Library.Modularization.Registrars;
	using Crm.Library.Rest;
	using Crm.Library.Routing.Constraints;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.AspNetCore.Routing.Constraints;

	public class RestRouteRegistrars : IRouteRegistrar
	{
		public virtual RoutePriority Priority
		{
			get { return RoutePriority.AboveNormal; }
		}
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{



			#region User

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}.{format}",
				new { controller = "UserRest", action = "Get", plugin = "Main" },
				new { format = new IsJsonOrXml(), id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}",
				new { controller = "UserRest", action = "Delete", plugin = "Main" },
				new { id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("DELETE"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Users.{format}",
				new { controller = "UserRest", action = "Update", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("PUT"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Users.{format}",
				new { controller = "UserRest", action = "Create", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("POST"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Users.{format}",
				new { controller = "UserRest", action = "List", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}/Permissions.{format}",
				new { controller = "UserRest", action = "ListPermissions", plugin = "Main" },
				new { format = new IsJsonOrXml(), id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}/Roles.{format}",
				new { controller = "UserRest", action = "ListRoles", plugin = "Main" },
				new { format = new IsJsonOrXml(), id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}/Roles/{roleId}",
				new { controller = "UserRest", action = "AddRole", plugin = "Main" },
				new { id = new IsGuid(), roleId = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("PUT"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}/Roles/{roleId}",
				new { controller = "UserRest", action = "RemoveRole", plugin = "Main" },
				new { id = new IsGuid(), roleId = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("DELETE"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}/Usergroups.{format}",
				new { controller = "UserRest", action = "ListUsergroups", plugin = "Main" },
				new { format = new IsJsonOrXml(), id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}/Usergroups/{usergroupId}",
				new { controller = "UserRest", action = "AddUsergroup", plugin = "Main" },
				new { id = new IsGuid(), usergroupId = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("PUT"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Users/{id}/Usergroups/{usergroupId}",
				new { controller = "UserRest", action = "RemoveUsergroup", plugin = "Main" },
				new { id = new IsGuid(), usergroupId = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("DELETE"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
					null,
					"Main/Users/CurrentUser.{format}",
					new { controller = "UserRest", action = "CurrentUser", plugin = "Main" },
					new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
					);

			endpoints.MapControllerRoute(
					null,
					"Main/Users/SetDeviceToken/{deviceToken}",
					new { controller = "UserRest", action = "SetDeviceToken", plugin = "Main" },
					new { httpMethod = new HttpMethodRouteConstraint("POST"), plugin = "Main" }
					);

			#endregion User

			#region Permissions

			endpoints.MapControllerRoute(
				null,
				"Main/Permissions.{format}",
				new { controller = "PermissionRest", action = "List", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);

			#endregion Permissions

			#region Roles

			endpoints.MapControllerRoute(
				null,
				"Main/Roles.{format}",
				new { controller = "RoleRest", action = "Create", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("POST"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Roles.{format}",
				new { controller = "RoleRest", action = "Update", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("PUT"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Roles/{id}",
				new { controller = "RoleRest", action = "Delete", plugin = "Main" },
				new { id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("DELETE"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Roles.{format}",
				new { controller = "RoleRest", action = "List", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);

			#endregion Roles

			#region Usergroups

			endpoints.MapControllerRoute(
				null,
				"Main/Usergroups.{format}",
				new { controller = "UsergroupRest", action = "Create", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("POST"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Usergroups.{format}",
				new { controller = "UsergroupRest", action = "Update", plugin = "Main" },
				new { format = new IsJsonOrXml(), id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("PUT"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Usergroups/{id}",
				new { controller = "UsergroupRest", action = "Delete", plugin = "Main" },
				new { id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("DELETE"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Usergroups/{id}/Users.{format}",
				new { controller = "UsergroupRest", action = "GetUsers", plugin = "Main" },
				new { id = new IsGuid(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);

			endpoints.MapControllerRoute(
				null,
				"Main/Usergroups.{format}",
				new { controller = "UsergroupRest", action = "List", plugin = "Main" },
				new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
				);
			#endregion Usergroups

			#region Licensing
			endpoints.MapControllerRoute(
					null,
					"Main/Licensing.{format}",
					new { controller = "Licensing", action = "Get", plugin = "Main" },
					new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Main" }
					);
			#endregion Licensing

			#region Default routes
			endpoints.MapControllerRoute(
				null,
				"Main/{controller}/{action}/{id}.{format}",
				new { plugin = "Main" },
				new { plugin = "Main", format = new IsJsonOrXml() }
				);
			endpoints.MapControllerRoute(
				null,
				"Main/{controller}/{id}.{format}",
				new { plugin = "Main", action = "Get" },
				new { plugin = "Main", format = new IsJsonOrXml() }
				);
			endpoints.MapControllerRoute(
				null,
				"Main/NumberingSequences.{format}",
				new { controller = "NumberingSequence", plugin = "Main", action = "List" },
				new { plugin = "Main", format = new IsJsonOrXml() }
				);
			endpoints.MapControllerRoute(
				null,
				"Main/Resources.{format}",
				new { controller = "Resource", plugin = "Main", action = "List" },
				new { plugin = "Main", format = new IsJsonOrXml() }
				);
			endpoints.MapControllerRoute(
				null,
				"Main/Settings.{format}",
				new { controller = "Setting", plugin = "Main", action = "List" },
				new { plugin = "Main", format = new IsJsonOrXml() }
				);
			endpoints.MapControllerRoute(
				null,
				"Main/{controller}.{format}",
				new { plugin = "Main", action = "Get" },
				new { plugin = "Main", format = new IsJsonOrXml() }
				);
			#endregion
		}
	}
}
