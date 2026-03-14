namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;

	using Main;

	public class DashboardActionRoleProvider : RoleCollectorBase
	{
		public DashboardActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Dashboard, PermissionName.View, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(PermissionGroup.Dashboard, PermissionName.Index, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			
			Add(PermissionGroup.MaterialDashboard, PermissionName.View, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.MaterialDashboard, PermissionName.Index, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.MaterialDashboard, PermissionName.Calendar, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.MaterialDashboard, PermissionName.FilterableCalendar, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);

			Add(MainPlugin.PermissionGroup.TopSearch, PermissionName.View, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(MainPlugin.PermissionGroup.TopSearch, nameof(ServiceOrderDispatch), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(MainPlugin.PermissionGroup.TopSearch, nameof(ServiceOrderHead), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(MainPlugin.PermissionGroup.TopSearch, nameof(ServiceCase), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(MainPlugin.PermissionGroup.TopSearch, nameof(Installation), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(MainPlugin.PermissionGroup.TopSearch, nameof(ServiceContract), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
		}
	}
}
