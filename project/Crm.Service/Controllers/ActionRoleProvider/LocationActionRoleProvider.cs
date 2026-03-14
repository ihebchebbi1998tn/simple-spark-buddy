namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Article;
	using Crm.Article.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class LocationActionRoleProvider : RoleCollectorBase
	{
		public LocationActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService
			};

			Add(PermissionGroup.WebApi, nameof(Location), roles);
			Add(ArticlePlugin.PermissionGroup.Location, PermissionName.Edit, roles);
			Add(ArticlePlugin.PermissionGroup.Location, PermissionName.Create, roles);
		}
	}
}
