namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;

	public class ServiceOrderDispatchActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderDispatchActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.Dispatch, PermissionName.View, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.UpcomingDispatches, PermissionName.View, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.ScheduledDispatches, PermissionName.View, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.ClosedDispatches, PermissionName.View, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Index, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Index, ServicePlugin.PermissionGroup.ServiceOrder, PermissionName.Index);
			Add(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Index);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read, ServicePlugin.PermissionGroup.ServiceCase, PermissionName.Read);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read, ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Read);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read, ServicePlugin.PermissionGroup.ServiceOrder, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Create);
			Add(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Delete, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Delete, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Create, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.Create, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Index);
			Add(ServicePlugin.PermissionGroup.Dispatch, PermissionName.CalendarEntry, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, PermissionName.CalendarEntry, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Index);
			
			Add(nameof(ServiceOrderDispatch), MainPlugin.PermissionName.Ics, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ErrorTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ExpenseTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);

			AddImport(nameof(ServiceOrderDispatch), MainPlugin.PermissionName.Ics, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Index);
			Add(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.SeeTechnicianChoice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.CreateForOtherUsers, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.CreateForOtherUsers, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Create);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.SeeAllUsersDispatches, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.SeeAllUsersDispatches, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Index);

			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.AppointmentRequest, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.AppointmentRequest, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.AppointmentConfirmation, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.AppointmentConfirmation, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ConfirmScheduled, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ConfirmScheduled, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ConfirmReleased, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ConfirmReleased, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.Reschedule, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.Reschedule, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.RejectScheduled, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.RejectScheduled, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.RejectReleased, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.RejectReleased, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);

			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.Complete, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.Complete, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ReportPreview, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ReportPreview, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ReportRecipients, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.ReportRecipients, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.Signature, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.Signature, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.Cancel, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.Cancel, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);

			Add(PermissionGroup.MaterialDashboard, nameof(ServiceOrderDispatch), ServicePlugin.Roles.FieldService);
			
			Add(ServicePlugin.PermissionGroup.Dispatch, CrmPlugin.PermissionName.TasksBlock, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, CrmPlugin.PermissionName.TasksBlock, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.RelatedOrdersTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.Dispatch, CrmPlugin.PermissionName.DocumentsTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Dispatch, CrmPlugin.PermissionName.DocumentsTab, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read);
			Add(nameof(ServiceOrderDispatchRejectReason), ServicePlugin.PermissionName.SelectNonMobileLookupValues);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderDispatch), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderDispatchRejectReason), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderDispatchCancellationReason), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderDispatchReportRecipient));
			AddImport(PermissionGroup.WebApi, nameof(ServiceOrderDispatch), PermissionGroup.WebApi, nameof(ServiceOrderDispatchReportRecipient));
			Add(PermissionGroup.WebApi, nameof(ServiceOrderDispatchStatus), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.DownloadReport, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.AutonomousDispatchPlanning);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderTimeDispatch), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, Roles.APIUser);
		}
	}
}
