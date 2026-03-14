namespace Crm.Service.Controllers;

using Crm.Article;
using Crm.Library.Model;
using Crm.Library.Modularization;
using Microsoft.AspNetCore.Mvc;

public class ArticleController : Controller
{
	[RenderAction("ArticleMaterialDetailsTabExtensions")]
	public virtual ActionResult ArticleMaterialDetailsTabExtensions() => PartialView();

	[RenderAction("ArticleDetailsRelationshipTypeExtension")]
	public virtual ActionResult ArticleDetailsRelationshipTypeExtension() => PartialView();

	
	[RenderAction("ArticleDetailsMaterialTabHeader")]
	[RequiredPermission(ServicePlugin.PermissionName.InstallationsTab, Group = ArticlePlugin.PermissionGroup.Article)]
	public virtual ActionResult InstallationsTabHeader() => PartialView();

	[RenderAction("ArticleDetailsMaterialTab")]
	[RequiredPermission(ServicePlugin.PermissionName.InstallationsTab, Group = ArticlePlugin.PermissionGroup.Article)]
	public virtual ActionResult InstallationsTab() =>  PartialView();
}
