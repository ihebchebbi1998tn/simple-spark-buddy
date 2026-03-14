namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.PerDiem;
	using Crm.Service.Model;

	public class ServiceOrderExpensePostingActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderExpensePostingActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceOrderExpensePosting,
				PermissionName.Index,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

				Add(ServicePlugin.PermissionGroup.ServiceOrderExpensePosting,
				PermissionName.View,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderExpensePosting,
				PermissionName.Create,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderExpensePosting,
				PermissionName.Edit,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderExpensePosting,
				PermissionName.Delete,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			
			Add(ServicePlugin.PermissionGroup.ServiceOrderExpensePosting,
				PerDiemPlugin.PermissionName.NoMinDateLimit,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);

			Add(PermissionGroup.WebApi, nameof(ServiceOrderExpensePosting), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
		}
	}
}
