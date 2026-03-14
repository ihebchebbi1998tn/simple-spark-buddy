using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{

	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;

	using Main;

	public class DocumentAttributeListExtensionController : Controller
	{
		[RequiredPermission(PermissionName.Index, Group = CrmPlugin.PermissionGroup.DocumentAttribute)]
		[RenderAction("DocumentAttributeListFilterTemplate")]
		public virtual ActionResult JobFilterTemplate()
		{
			return PartialView();
		}
	}
}
