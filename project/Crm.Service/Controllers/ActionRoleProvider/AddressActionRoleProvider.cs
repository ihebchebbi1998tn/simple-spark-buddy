namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service;

	using Main;

	public class AddressActionRoleProvider : RoleCollectorBase
	{
		public AddressActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(CrmPlugin.PermissionGroup.Address, PermissionName.Create, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Address, PermissionName.Edit, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(CrmPlugin.PermissionGroup.Address, PermissionName.Delete, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Address, MainPlugin.PermissionName.ExportAsVcf, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
