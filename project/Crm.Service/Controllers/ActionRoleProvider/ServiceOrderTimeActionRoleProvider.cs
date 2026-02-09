namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;

	public class ServiceOrderTimeActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderTimeActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceOrderTime,
				PermissionName.Index,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimeAdd,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimeNew,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimeEdit,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimeEdit, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimeAdd);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimeDeleteSelfCreated,
						ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimeDeleteSelfCreated, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimeEdit);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimeDelete,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimeDelete, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimeDeleteSelfCreated);

			Add(ServicePlugin.PermissionGroup.ServiceOrderTime,
						PermissionName.View,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);

			Add(PermissionGroup.WebApi, nameof(NoCausingItemPreviousSerialNoReason), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(NoCausingItemSerialNoReason), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderTime), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderTimeCategory), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderTimeLocation), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderTimePriority), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderTimeStatus), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
		}
	}
}
