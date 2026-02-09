namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class BackgroundServiceActionRoleProvider : RoleCollectorBase
	{
		public BackgroundServiceActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.BackgroundService, PermissionName.Pause, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.BackgroundService, PermissionName.Index, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.BackgroundService, PermissionName.Read, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.BackgroundService, PermissionName.RunNow, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}