namespace Main.Rest.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Rest;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	using Microsoft.AspNetCore.Mvc;

	public class RoleRestController : RestController<PermissionSchemaRole>
	{
		private readonly IAccessControlManager accessControlManager;

		[RequiredRole(RoleName.Administrator)]
		public virtual ActionResult List()
		{
			var permissionSchema = accessControlManager.FindPermissionSchema(UnicoreDefaults.DefaultPermissionSchema);
			var roles = accessControlManager.ListPermissionSchemaRoles(permissionSchema.UId);
			return Rest(roles, "Roles");
		}

		// Constructor
		public RoleRestController(RestTypeProvider restTypeProvider, IAccessControlManager accessControlManager)
			: base(restTypeProvider)
		{
			this.accessControlManager = accessControlManager;
		}
	}
}
