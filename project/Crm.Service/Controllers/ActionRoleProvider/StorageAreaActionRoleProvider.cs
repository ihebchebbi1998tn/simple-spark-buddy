namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Article.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class StorageAreaActionRoleProvider : RoleCollectorBase
	{
		public StorageAreaActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService
			};

			Add(PermissionGroup.WebApi, nameof(StorageArea), roles);
		}
	}
}
