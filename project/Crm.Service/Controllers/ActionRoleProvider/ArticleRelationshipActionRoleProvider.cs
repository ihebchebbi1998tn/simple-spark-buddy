using Crm.Library.Model.Authorization.PermissionIntegration;

namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Article;
	using Crm.Article.Model.Lookups;
	using Crm.Article.Model.Relationships;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Modularization.Interfaces;

	public class ArticleRelationshipActionRoleProvider : RoleCollectorBase
	{
		public ArticleRelationshipActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Create, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService);
			Add(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Edit, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService);
			Add(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Delete, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService);
			Add(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Read, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);

			Add(PermissionGroup.WebApi, nameof(ArticleRelationship), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(ArticleRelationshipType), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.FieldService);
		}
	}
}