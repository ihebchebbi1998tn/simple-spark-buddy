namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class InstallationCompanyRelationshipController : Controller
	{
		[RenderAction("MaterialCompanyItemExtensions", Priority = 30)]
		[RequiredPermission(ServicePlugin.PermissionName.ReadInstallationCompanyRelationship, Group = ServicePlugin.PermissionGroup.Installation)]
		public virtual ActionResult CompanyItemExtensions() => PartialView("ItemExtensions");

		[RenderAction("MaterialInstallationItemExtensions", Priority = 60)]
		public virtual ActionResult InstallationItemExtension() => PartialView("ItemExtensions");

		[RenderAction("InstallationItemTemplateActions", Priority = 60)]
		public virtual ActionResult InstallationItemTemplateActions() => PartialView("ItemTemplateActions");

		[RenderAction("CompanyItemTemplateActions", Priority = 30)]
		[RequiredPermission(ServicePlugin.PermissionName.ReadInstallationCompanyRelationship, Group = ServicePlugin.PermissionGroup.Installation)]
		public virtual ActionResult CompanyItemTemplateActions() => PartialView("ItemTemplateActions");

		[RenderAction("CompanyDetailsRelationshipTypeActionExtension", Priority = 30)]
		public virtual ActionResult CompanyRelationshipAction() => PartialView("RelationshipAction");

		[RenderAction("CompanyDetailsRelationshipTypeExtension", Priority = 30)]
		[RequiredPermission(ServicePlugin.PermissionName.ReadInstallationCompanyRelationship, Group = ServicePlugin.PermissionGroup.Installation)]
		public virtual ActionResult CompanyDetailsRelationshipTypeExtension() => PartialView();

		public virtual ActionResult EditTemplate() => PartialView("../Relationship/EditTemplate");
	}
}
