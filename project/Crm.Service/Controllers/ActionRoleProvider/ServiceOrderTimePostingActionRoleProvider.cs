namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.PerDiem;
	using Crm.Service.Model;

	using Main;

	public class ServiceOrderTimePostingActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderTimePostingActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceOrderTimePosting,
				PermissionName.Index,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrderTimePosting,
				PermissionName.CalendarEntry,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrderTimePosting, PermissionName.CalendarEntry, ServicePlugin.PermissionGroup.ServiceOrderTimePosting, PermissionName.Index);

			Add(ServicePlugin.PermissionGroup.ServiceOrderTimePosting,
				ServicePlugin.PermissionName.DispatchRelink,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimePostingPrePlannedAdd,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingPrePlannedAdd, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingAdd);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimePostingPrePlannedEdit,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingPrePlannedEdit, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingsEdit);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimePostingPrePlannedEditJob,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingPrePlannedEditJob, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingPrePlannedEdit);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimePostingPrePlannedRemove,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingPrePlannedRemove, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingRemove);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimePostingAdd,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimePostingsEdit,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingsEdit, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingAdd);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.SinglePostingEdit,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimePostingSave,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.TimePostingRemove,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingRemove, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.TimePostingsEdit);

			Add(PermissionGroup.WebApi, nameof(ServiceOrderTimePosting), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(ServicePlugin.PermissionGroup.ServiceOrderTimePosting, PerDiemPlugin.PermissionName.NoMinDateLimit, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditTimePostingWhenDispatchClosed, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceOrder,
					ServicePlugin.PermissionName.CreateAllUsersTimePostings,
					ServicePlugin.Roles.HeadOfService,
					ServicePlugin.Roles.ServiceBackOffice,
					ServicePlugin.Roles.InternalService);
		}
	}
}
