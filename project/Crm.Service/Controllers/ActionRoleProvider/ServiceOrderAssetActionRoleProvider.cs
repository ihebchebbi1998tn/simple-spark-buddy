namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main.Model.Lookups;

	public class ServiceOrderAssetActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderAssetActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.AddAsset,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.AddAsset, ServicePlugin.PermissionGroup.ServiceOrder, PermissionName.Read);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.RemoveAsset,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.RemoveAsset, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.AddAsset);

			Add(PermissionGroup.WebApi, nameof(Asset),
				ServicePlugin.Roles.FieldService,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService);
		}
	}
}
