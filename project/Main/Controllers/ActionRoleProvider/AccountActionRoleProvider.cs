namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class AccountActionRoleProvider : RoleCollectorBase
	{
		public AccountActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.UserAccount, PermissionName.Settings);
			Add(MainPlugin.PermissionGroup.UserAccount, MainPlugin.PermissionName.UpdateStatus);
		}
	}
}
