namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.PerDiem;
	using Crm.PerDiem.Model;
	using Crm.PerDiem.Model.Lookups;

	using Main;
	using Main.Model.Lookups;

	public class UserTimeEntryActionRoleProvider : RoleCollectorBase
	{
		public UserTimeEntryActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var serviceRoleNames = new[] { ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService };

			Add(nameof(PerDiemReportStatus), PerDiemPlugin.PermissionName.SelectNonMobileLookupValues, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, PermissionName.Create, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, PermissionName.Edit, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, PermissionName.Delete, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, PermissionName.View, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, PermissionName.Index, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, PermissionName.CalendarEntry, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, PerDiemPlugin.PermissionName.SeeAllUsersTimeEntries, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, PerDiemPlugin.PermissionName.NoMinDateLimit, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PerDiemPlugin.PermissionGroup.TimeEntry, MainPlugin.PermissionName.DownloadAsPdf, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			Add(nameof(TimeEntryType), PerDiemPlugin.PermissionName.SelectNonMobileLookupValues, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(UserTimeEntry), serviceRoleNames);
			Add(PermissionGroup.WebApi, nameof(TimeEntryType), serviceRoleNames);
			Add(PermissionGroup.WebApi, nameof(TravelType), serviceRoleNames);
			Add(PermissionGroup.WebApi, nameof(DriverType), serviceRoleNames);
			Add(PermissionGroup.WebApi, nameof(DistanceUnit), serviceRoleNames);
			Add(PermissionGroup.WebApi, nameof(CostCenter), serviceRoleNames);
		}
	}
}
