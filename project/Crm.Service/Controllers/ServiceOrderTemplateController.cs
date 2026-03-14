using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{

	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Service.Enums;

	public class ServiceOrderTemplateController : Controller
	{
		private readonly IAppSettingsProvider appSettingsProvider;

		public ServiceOrderTemplateController(IAppSettingsProvider appSettingsProvider)
		{
			this.appSettingsProvider = appSettingsProvider;
		}

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrderTemplate)]
		public virtual ActionResult Create()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderTemplateCreateForm", Priority = 500)]
		public virtual ActionResult CreateGeneral()
		{
			return PartialView();
		}


		[RenderAction("ServiceOrderTemplateCreateForm", Priority = 400)]
		public virtual ActionResult CreateExtended()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.ServiceOrderTemplate)]
		public virtual ActionResult Details()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderTemplateDetailsTab", Priority = 100)]
		public virtual ActionResult DetailsTab()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderTemplateDetailsTabHeader", Priority = 100)]
		public virtual ActionResult DetailsTabHeader()
		{
			return PartialView();
		}
		

		[RenderAction("ServiceOrderTemplateDetailsTab", Priority = 90)]
		[RequiredPermission(ServicePlugin.PermissionName.JobsTab, Group = ServicePlugin.PermissionGroup.ServiceOrderTemplate)]
		public virtual ActionResult JobsTab()
		{
			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
			if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
			{
				return PartialView();
			}

			return new EmptyResult();
		}

		[RenderAction("ServiceOrderTemplateDetailsTabHeader", Priority = 90)]
		[RequiredPermission(ServicePlugin.PermissionName.JobsTab, Group = ServicePlugin.PermissionGroup.ServiceOrderTemplate)]
		public virtual ActionResult JobsTabHeader()
		{
			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
			if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
			{
				return PartialView();
			}

			return new EmptyResult();
		}

		[RenderAction("ServiceOrderTemplateDetailsJobsTabPrimaryAction")]
		public virtual ActionResult JobsTabPrimaryAction()
		{
			return PartialView();
		}
	}
}
