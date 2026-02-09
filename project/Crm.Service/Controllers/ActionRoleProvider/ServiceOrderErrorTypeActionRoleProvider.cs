namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;

	public class ServiceOrderErrorTypeActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderErrorTypeActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorType,
				PermissionName.Index,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorType,
				PermissionName.View,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorType,
				PermissionName.Create,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorType,
				PermissionName.Edit,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorType,
				PermissionName.Delete,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderErrorType,
				ServicePlugin.PermissionName.EditErrorTypeIsSuspected,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);

			Add(PermissionGroup.WebApi,
				nameof(ServiceOrderErrorType),
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
