namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Article.Model;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceRoleProvider : RoleCollectorBase
	{
		public ServiceRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Login, PermissionName.MaterialClientOnline, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(PermissionGroup.WebApi, nameof(Location), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(MonitoringDataType), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(RdsPpStructure), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(RecentPage), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(Site), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(Store), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(StorageArea), Roles.APIUser);
		}
	}
}
