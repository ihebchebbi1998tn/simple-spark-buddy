namespace Crm.Service.Controllers.ActionRoleProvider
{
	using System.Linq;

	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class ErpIntegrationActionRoleProvider : RoleCollectorBase
	{
		public ErpIntegrationActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			if (pluginProvider.ActivePluginNames.Contains("Main.Replication"))
			{
				Add(ServicePlugin.PermissionGroup.Replication,
					PermissionName.Import,
					ServicePlugin.Roles.HeadOfService,
					ServicePlugin.Roles.ServiceBackOffice);
				Add(ServicePlugin.PermissionGroup.Replication,
					PermissionName.Export,
					ServicePlugin.Roles.HeadOfService,
					ServicePlugin.Roles.ServiceBackOffice);
			}
		}
	}
}
