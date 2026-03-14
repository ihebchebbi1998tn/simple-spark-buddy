namespace Crm.Service.Controllers.ActionRoleProvider
{
	using System.Linq;

	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Main.Model;
	using Crm.Service;

	public class UserAssetActionRoleProvider : RoleCollectorBase
	{
		public UserAssetActionRoleProvider(IPluginProvider pluginProvider) :
			base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService
			};
			Add(PermissionGroup.WebApi, nameof(UserAsset), roles);

			if (pluginProvider.ActivePluginNames.Contains("Crm.Offline"))
			{
				Add(PermissionGroup.Sync, nameof(UserAsset), roles);
			}
		}
	}
}
