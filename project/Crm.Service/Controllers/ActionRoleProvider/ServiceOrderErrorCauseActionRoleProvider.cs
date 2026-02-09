namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;

	public class ServiceOrderErrorCauseActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderErrorCauseActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorCause,
				PermissionName.Index,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorCause,
				PermissionName.View,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorCause,
				PermissionName.Create,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorCause,
				PermissionName.Edit,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorCause,
				PermissionName.Delete,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorCause,
				ServicePlugin.PermissionName.EditErrorCauseIsSuspected,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);

			Add(PermissionGroup.WebApi,
				nameof(ServiceOrderErrorCause),
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService,
				CrmPlugin.Roles.HeadOfSales,
				CrmPlugin.Roles.InternalSales,
				CrmPlugin.Roles.SalesBackOffice,
				Roles.APIUser);
		}
	}
}
