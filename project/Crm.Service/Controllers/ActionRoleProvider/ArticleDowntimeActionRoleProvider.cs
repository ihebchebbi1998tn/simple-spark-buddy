namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class ArticleDowntimeActionRoleProvider : RoleCollectorBase
	{

		public ArticleDowntimeActionRoleProvider(IPluginProvider pluginProvider) : base(pluginProvider)
		{
			var roles = new [] { ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, Roles.APIUser };
			Add(PermissionGroup.WebApi, nameof(ArticleDowntime), roles);
			Add(PermissionGroup.WebApi, nameof(ArticleDowntimeReason), roles);
		}
	}
}
