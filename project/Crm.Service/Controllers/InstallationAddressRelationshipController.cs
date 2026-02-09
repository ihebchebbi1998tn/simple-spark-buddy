using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;

	using Main;

	using Microsoft.AspNetCore.Authorization;
	using PermissionGroup = ServicePlugin.PermissionGroup;

	[Authorize]
	public class InstallationAddressRelationshipController : Controller
	{
		[RequiredPermission(CrmPlugin.PermissionName.EditAddressRelationship, Group = PermissionGroup.Installation)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView("../Relationship/EditTemplate");
		}
	}
}
