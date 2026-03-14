using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{

	using Crm.Library.Model;
	using Crm.Library.Modularization;

	using Main;

	public class CompanyController : Controller
	{
		[RenderAction("CompanyDetailsMaterialTab", Priority = 25)]
		[RequiredPermission(ServicePlugin.PermissionName.InstallationsTab, Group = CrmPlugin.PermissionGroup.Company)]
		public virtual ActionResult InstallationsTab()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 10)]
		[RequiredPermission(ServicePlugin.PermissionName.InstallationsTab, Group = CrmPlugin.PermissionGroup.Company)]
		public virtual ActionResult InstallationsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTab", Priority = 20)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceCasesTab, Group = CrmPlugin.PermissionGroup.Company)]
		public virtual ActionResult ServiceCasesTab()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 20)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceCasesTab, Group = CrmPlugin.PermissionGroup.Company)]
		public virtual ActionResult ServiceCasesTabHeader()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTab", Priority = 5)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceContractsTab, Group = CrmPlugin.PermissionGroup.Company)]
		public virtual ActionResult ServiceContractsTab()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 10)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceContractsTab, Group = CrmPlugin.PermissionGroup.Company)]
		public virtual ActionResult ServiceContractsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTab", Priority = 15)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceOrdersTab, Group = CrmPlugin.PermissionGroup.Company)]
		public virtual ActionResult ServiceOrdersTab()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 15)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceOrdersTab, Group = CrmPlugin.PermissionGroup.Company)]
		public virtual ActionResult ServiceOrdersTabHeader()
		{
			return PartialView();
		}
	}
}
