namespace Crm.Service.Team.Controllers.ActionRoleProvider
{
	using Crm.Article;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Team.Model;

	public class ArticleActionRoleProvider : RoleCollectorBase
	{
		public ArticleActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[]
			{
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				Roles.APIUser
			};
			Add(PermissionGroup.WebApi, nameof(ArticleUserGroupRelationship), roles);
			AddImport(PermissionGroup.WebApi, nameof(ArticleUserGroupRelationship), ArticlePlugin.PermissionGroup.Article, PermissionName.Read);
			Add(PermissionGroup.Sync, nameof(ArticleUserGroupRelationship), roles);
			AddImport(PermissionGroup.Sync, nameof(ArticleUserGroupRelationship), PermissionGroup.WebApi, nameof(ArticleUserGroupRelationship));
		}
	}
}
