using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ServiceCaseTemplateController : Controller
	{
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceCaseTemplate)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseTemplateDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult DetailsTab()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseTemplateDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult DetailsTabHeader()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceCaseTemplate)]
		public virtual ActionResult DetailsTemplate()
		{
			return PartialView();
		}
	}
}
