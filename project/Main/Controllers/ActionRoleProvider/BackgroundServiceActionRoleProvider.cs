namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class BackgroundServiceActionRoleProvider : RoleCollectorBase
	{
		public BackgroundServiceActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.BackgroundService, PermissionName.Pause);
			AddImport(PermissionGroup.BackgroundService, PermissionName.Pause, PermissionGroup.BackgroundService, PermissionName.RunNow);
			Add(PermissionGroup.BackgroundService, PermissionName.Index);
			Add(PermissionGroup.BackgroundService, PermissionName.Read);
			Add(PermissionGroup.BackgroundService, PermissionName.RunNow);
			AddImport(PermissionGroup.BackgroundService, PermissionName.RunNow, PermissionGroup.BackgroundService, PermissionName.Read);
		}
	}
}
