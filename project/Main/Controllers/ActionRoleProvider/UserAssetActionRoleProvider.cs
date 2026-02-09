namespace Main.Controllers.ActionRoleProvider
{
	using System.Linq;

	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Main.Model;

	public class UserAssetActionRoleProvider : RoleCollectorBase
	{
		public UserAssetActionRoleProvider(IPluginProvider pluginProvider) :
			base(pluginProvider)
		{
			Add(nameof(UserAsset), PermissionName.Create);
			Add(nameof(UserAsset), PermissionName.Edit);
			Add(nameof(UserAsset), PermissionName.Index);
			Add(nameof(UserAsset), PermissionName.View);
			Add(PermissionGroup.WebApi, nameof(UserAsset));

			if (pluginProvider.ActivePluginNames.Contains("Crm.Offline"))
			{
				Add(PermissionGroup.Sync, nameof(UserAsset));
				AddImport(PermissionGroup.Sync, nameof(UserAsset), PermissionGroup.WebApi, nameof(UserAsset));
			}
		}
	}
}
