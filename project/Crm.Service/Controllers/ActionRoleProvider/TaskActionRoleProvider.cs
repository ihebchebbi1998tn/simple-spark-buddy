namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main;

	public class TaskActionRoleProvider : RoleCollectorBase
	{
		public TaskActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(CrmPlugin.PermissionGroup.Task, PermissionName.View, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Task, PermissionName.Index, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Task, PermissionName.Read, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Task, PermissionName.Create, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Task, PermissionName.Edit, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Task, PermissionName.CalendarEntry, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Task, MainPlugin.PermissionName.Ics, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Task, MainPlugin.PermissionName.Complete, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Task, CrmPlugin.PermissionName.SeeAllUsersTasks, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
