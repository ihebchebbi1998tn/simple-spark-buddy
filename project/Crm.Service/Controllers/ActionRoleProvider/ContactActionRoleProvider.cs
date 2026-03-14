namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main;

	public class ContactActionRoleProvider : RoleCollectorBase
	{
		public ContactActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Contact, CrmPlugin.PermissionName.CreateTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.Contact, MainPlugin.PermissionName.ExportAsCsv, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.Contact, MainPlugin.PermissionName.Merge, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
