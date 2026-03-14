namespace Crm.Service.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class InstallationPersonRelationshipController : Controller
	{
		[RenderAction("MaterialPersonItemExtensions", Priority = 30)]
		[RequiredPermission(ServicePlugin.PermissionName.ReadInstallationPersonRelationship, Group = ServicePlugin.PermissionGroup.Installation)]
		public virtual ActionResult PersonItemExtensions() => PartialView("ItemExtensions");
		
		[RenderAction("MaterialInstallationItemExtensions", Priority = 60)]
		public virtual ActionResult InstallationItemExtension() => PartialView("ItemExtensions");
		
		[RenderAction("InstallationItemTemplateActions", Priority = 60)]
		public virtual ActionResult InstallationItemTemplateActions() => PartialView("ItemTemplateActions");
		
		[RenderAction("PersonItemTemplateActions", Priority = 30)]
		[RequiredPermission(ServicePlugin.PermissionName.ReadInstallationPersonRelationship, Group = ServicePlugin.PermissionGroup.Installation)]
		public virtual ActionResult PersonItemTemplateActions() => PartialView("ItemTemplateActions");

		[RenderAction("PersonDetailsRelationshipTypeActionExtension", Priority = 30)]
		public virtual ActionResult PersonRelationshipAction() => PartialView("RelationshipAction");

		[RenderAction("PersonDetailsRelationshipTypeExtension", Priority = 30)]
		[RequiredPermission(ServicePlugin.PermissionName.ReadInstallationPersonRelationship, Group = ServicePlugin.PermissionGroup.Installation)]
		public virtual ActionResult PersonDetailsRelationshipTypeExtension() => PartialView();

		public virtual ActionResult EditTemplate() => PartialView("../Relationship/EditTemplate");
	}
}
