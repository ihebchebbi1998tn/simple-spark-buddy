namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Article;
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model.Lookup;

	public class StoreActionRoleProvider : RoleCollectorBase
	{
		public StoreActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService
			};

			Add(PermissionGroup.WebApi, nameof(Store), roles);
			Add(ArticlePlugin.PermissionGroup.Store, PermissionName.View, roles);
			Add(ArticlePlugin.PermissionGroup.Store, PermissionName.Index, roles);
			Add(ArticlePlugin.PermissionGroup.Store, PermissionName.Read, roles);
			Add(ArticlePlugin.PermissionGroup.Store, PermissionName.Edit, roles);
			Add(ArticlePlugin.PermissionGroup.Store, PermissionName.Create, roles);
			Add(PermissionGroup.WebApi, nameof(StoreName), roles);

		}
	}
}
