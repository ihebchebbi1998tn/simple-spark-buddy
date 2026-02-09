namespace Main.Rest.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Rest;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	using Microsoft.AspNetCore.Mvc;

	public class PermissionRestController : RestController<Permission>
	{
		private readonly IAccessControlManager accessControlManager;

		// Methods
		[RequiredRole(RoleName.Administrator)]
		public virtual ActionResult List()
		{
			var permissions = accessControlManager.ListPermissionSchemaPermissions(UnicoreDefaults.DefaultPermissionSchema);
			return Rest(permissions, "Permissions");
		}

		// Constructor
		public PermissionRestController(RestTypeProvider restTypeProvider, IAccessControlManager accessControlManager)
			: base(restTypeProvider)
		{
			this.accessControlManager = accessControlManager;
		}
	}
}
