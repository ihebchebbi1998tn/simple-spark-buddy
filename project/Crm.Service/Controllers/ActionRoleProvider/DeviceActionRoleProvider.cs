namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class DeviceActionRoleProvider : RoleCollectorBase
	{
		public DeviceActionRoleProvider(IPluginProvider pluginProvider) : base(pluginProvider)
		{
			var roles = new[] { ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice };

			Add(PermissionGroup.WebApi, nameof(Device), roles);
		}
	}
}
