using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ServiceOrderExpensePostingController : Controller
	{

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrderExpensePosting)]
		[RenderAction("ServiceOrderExpensePostingsTabPrimaryAction")]
		public virtual ActionResult PrimaryActionAddServiceOrderExpensePosting() => PartialView();

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrderExpensePosting)]
		public virtual ActionResult EditTemplate() => PartialView();

		[RequiredPermission(PermissionName.View, Group = ServicePlugin.PermissionGroup.ServiceOrderExpensePosting)]
		[RenderAction("PerDiemReportOverviewEntry", Priority = 100)]
		public virtual ActionResult PerDiemReportOverviewEntry()
		{
			return PartialView();
		}

		[RenderAction("ExpenseTemplateTableColumns")]
		public virtual ActionResult TemplateTableColumns()
		{
			return PartialView();
		}

		[RenderAction("ExpenseTemplateActions", Priority = 50)]
		public virtual ActionResult TemplateActionViewDispatch()
		{
			return PartialView();
		}

		[AllowAnonymous]
		[RenderAction("PerDiemReport", Priority = 50)]
		public virtual ActionResult PerDiemReportAttachments()
		{
			return PartialView();
		}

	}
}
