namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;

	using Main;

	public class GenericListActionRoleProvider : RoleCollectorBase
	{
		public GenericListActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var genericListTypes = new[]
			{
				nameof(Installation),
				nameof(ReplenishmentOrderItem),
				nameof(ServiceCase),
				nameof(ServiceContract),
				nameof(ServiceObject),
				nameof(ServiceOrderHead),
				nameof(ServiceOrderDispatch)
			};

			foreach (var permissionGroup in genericListTypes)
			{
				Add(permissionGroup, MainPlugin.PermissionName.ExportAsCsv, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
				Add(permissionGroup, MainPlugin.PermissionName.Ics, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
				Add(permissionGroup, MainPlugin.PermissionName.Rss, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			}
		}
	}
}
