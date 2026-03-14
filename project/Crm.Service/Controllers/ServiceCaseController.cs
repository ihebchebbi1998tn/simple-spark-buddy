namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Service.Model;

	using Main;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ServiceCaseController : Controller
	{
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult AddToServiceOrder()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsTopMenu")]
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult AddToServiceOrderTopMenu()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult CreateServiceOrderTemplate()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsTopMenu")]
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult CreateServiceOrderTopMenu()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsTopMenu")]
		public virtual ActionResult EditVisibilityTopMenu()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult DetailsTab()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult DetailsTabHeader()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult DetailsTemplate()
		{
			return PartialView();
		}

		[RenderAction("MaterialServiceCaseHeaderExtensions", Priority = 50)]
		public virtual ActionResult DropboxBlock() => PartialView("ContactDetailsDropboxBlock", typeof(ServiceCase));

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseCreateForm", Priority = 100)]
		public virtual ActionResult CreateTemplateMainData()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseCreateForm", Priority = 50)]
		public virtual ActionResult CreateTemplateExtendedData()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseCreateForm", Priority = 25)]
		public virtual ActionResult CreateTemplateVisibility()
		{
			return PartialView();
		}

		[RenderAction("LookupBasicInformation")]
		public virtual ActionResult LookupBasicInformation() => PartialView();

		[RenderAction("LookupEditBasicInformation")]
		public virtual ActionResult LookupEditBasicInformation() => PartialView();

		[RequiredPermission(MainPlugin.PermissionName.SetStatus, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult SetStatusTemplate()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTab", Priority = 80)]
		[RequiredPermission(CrmPlugin.PermissionName.NotesTab, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult NotesTab()
		{
			return PartialView("MaterialNotesTab");
		}

		[RenderAction("ServiceCaseDetailsMaterialTabHeader", Priority = 80)]
		[RequiredPermission(CrmPlugin.PermissionName.NotesTab, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult NotesTabHeader()
		{
			return PartialView("MaterialNotesTabHeader");
		}

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.DocumentArchive)]
		public virtual ActionResult DocumentAttributeEditTemplate()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTab", Priority = 50)]
		[RequiredPermission(ServicePlugin.PermissionName.DocumentArchive, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult DocumentsTab()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTabHeader", Priority = 50)]
		[RequiredPermission(ServicePlugin.PermissionName.DocumentArchive, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult DocumentsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTabHeader", Priority = 85)]
		public virtual ActionResult ErrorTabHeader()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTab", Priority = 85)]
	
		public virtual ActionResult ErrorTab()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTab", Priority = 80)]
		[RequiredPermission(ServicePlugin.PermissionName.InstallationsTab, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult InstallationTab()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDetailsMaterialTabHeader", Priority = 80)]
		[RequiredPermission(ServicePlugin.PermissionName.InstallationsTab, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult InstallationTabHeader()
		{
			return PartialView();
		}

		[RenderAction("ServiceCaseDocumentsTabPrimaryAction")]
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.DocumentArchive)]
		public virtual ActionResult DocumentsTabPrimaryAction()
		{
			return PartialView();
		}
		
		[RenderAction("ServiceCaseDetailsMaterialTab", Priority = 30)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceOrdersTab, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult ServiceOrdersTab()
		{
			return PartialView();
		}
        
		[RenderAction("ServiceCaseDetailsMaterialTabHeader", Priority = 30)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceOrdersTab, Group = ServicePlugin.PermissionGroup.ServiceCase)]
		public virtual ActionResult ServiceOrdersTabHeader()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Create, Group = CrmPlugin.PermissionGroup.Person)]
		public virtual ActionResult AddContact()
		{
			return PartialView();
		}
	}
}
