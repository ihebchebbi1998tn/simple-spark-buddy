namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class StatisticsKeyActionRoleProvider : RoleCollectorBase
	{
		public StatisticsKeyActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{

			Add(ServicePlugin.PermissionGroup.StatisticsKey, PermissionName.View, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.StatisticsKey, PermissionName.Edit, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);

			Add(PermissionGroup.WebApi, nameof(ServiceOrderStatisticsKey), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeyProductType), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeyMainAssembly), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeySubAssembly), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeyAssemblyGroup), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeyFaultImage), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeyRemedy), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeyCause), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeyWeighting), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(StatisticsKeyCauser), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
		}
	}
}
