using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;
	using Crm.Library.Helper;

	using Main;

	using PermissionGroup = ServicePlugin.PermissionGroup;

	[Authorize]
	public class InstallationController : Controller
	{
		[RenderAction("InstallationDetailsTopMenu")]
		public virtual ActionResult EditVisibilityTopMenu()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult DetailsTab()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult DetailsTabHeader()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Installation)]
		public virtual ActionResult DetailsTemplate()
		{
			return PartialView();
		}
		
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.DocumentArchive)]
		public virtual ActionResult DocumentAttributeEditTemplate()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTab", Priority = 50)]
		[RequiredPermission(ServicePlugin.PermissionName.DocumentArchive, Group = PermissionGroup.Installation)]
		public virtual ActionResult DocumentsTab()
		{
			return PartialView("DocumentsTab");
		}

		[RenderAction("InstallationDetailsMaterialTabHeader", Priority = 50)]
		[RequiredPermission(ServicePlugin.PermissionName.DocumentArchive, Group = PermissionGroup.Installation)]
		public virtual ActionResult DocumentsTabHeader()
		{
			return PartialView("DocumentsTabHeader");
		}

		[RenderAction("InstallationDocumentsTabPrimaryAction")]
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.DocumentArchive)]
		public virtual ActionResult DocumentsTabPrimaryAction()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.Installation)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTab", Priority = 80)]
		[RequiredPermission(CrmPlugin.PermissionName.NotesTab, Group = PermissionGroup.Installation)]
		public virtual ActionResult NotesTab()
		{
			return PartialView("MaterialNotesTab");
		}

		[RenderAction("InstallationDetailsMaterialTabHeader", Priority = 80)]
		[RequiredPermission(CrmPlugin.PermissionName.NotesTab, Group = PermissionGroup.Installation)]
		public virtual ActionResult NotesTabHeader()
		{
			return PartialView("MaterialNotesTabHeader");
		}

		[RenderAction("InstallationDetailsMaterialTab", Priority = 90)]
		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.InstallationPosition)]
		public virtual ActionResult PositionsTab()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTabHeader", Priority = 90)]
		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.InstallationPosition)]
		public virtual ActionResult PositionsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTab", Priority = 70)]
		[RequiredPermission(CrmPlugin.PermissionName.RelationshipsTab, Group = PermissionGroup.Installation)]
		public virtual ActionResult RelationshipsTab()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTabHeader", Priority = 70)]
		[RequiredPermission(CrmPlugin.PermissionName.RelationshipsTab, Group = PermissionGroup.Installation)]
		public virtual ActionResult RelationshipsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTab", Priority = 95)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceCasesTab, Group = PermissionGroup.Installation)]
		public virtual ActionResult ServiceCasesTab()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTabHeader", Priority = 95)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceCasesTab, Group = PermissionGroup.Installation)]
		public virtual ActionResult ServiceCasesTabHeader()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTab", Priority = 93)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceOrdersTab, Group = PermissionGroup.Installation)]
		public virtual ActionResult ServiceOrdersTab()
		{
			return PartialView();
		}

		[RenderAction("InstallationDetailsMaterialTabHeader", Priority = 93)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceOrdersTab, Group = PermissionGroup.Installation)]
		public virtual ActionResult ServiceOrdersTabHeader()
		{
			return PartialView();
		}
		[RenderAction("MaterialTitleResource", Priority = 900)]
		public virtual ActionResult MaterialTitleResourceInstallationCss() => Content(Url.CssResource("Crm.Service","installationCss"));
		[RenderAction("InstallationInstallationPositionTemplate", Priority = 9000)]
		public virtual ActionResult InstallationPositionTemplate() => PartialView();
	}
}
