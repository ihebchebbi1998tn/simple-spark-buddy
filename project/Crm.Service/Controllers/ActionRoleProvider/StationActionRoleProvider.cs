namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using System.Linq;

	using Crm.Model;

	public class StationActionRoleProvider : RoleCollectorBase
	{
		public StationActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService
			};

			Add(nameof(Station), PermissionName.Create, ServicePlugin.Roles.HeadOfService);
			Add(nameof(Station), PermissionName.Edit, ServicePlugin.Roles.HeadOfService);
			Add(nameof(Station), PermissionName.Index, ServicePlugin.Roles.HeadOfService);
			Add(nameof(Station), PermissionName.View, ServicePlugin.Roles.HeadOfService);
			Add(nameof(Station), PermissionName.Delete, ServicePlugin.Roles.HeadOfService);


			Add(PermissionGroup.WebApi, nameof(Station), roles);

			if (pluginProvider.ActivePluginNames.Contains("Crm.Offline"))
			{
				Add(PermissionGroup.Sync, nameof(Station), new[] {ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService });
				AddImport(PermissionGroup.Sync, nameof(Station), PermissionGroup.WebApi, nameof(Station));
			}
		}
	}
}
