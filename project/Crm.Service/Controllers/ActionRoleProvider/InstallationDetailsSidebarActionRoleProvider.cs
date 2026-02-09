namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main;

	public class InstallationDetailsSidebarActionRoleProvider : RoleCollectorBase
	{
		public InstallationDetailsSidebarActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ImportPositionsFromCsv, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.DocumentArchive, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.DocumentArchive, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.DetailsTechnicianInfo, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.DetailsTechnicianInfo, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
		}
	}
}
