namespace Sms.Scheduler.Team.Controllers.ActionRoleProviders
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Team.Model;

	public class SchedulerActionRoleProvider : RoleCollectorBase
	{
		public SchedulerActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var readOnlyRoles = new[] { SchedulerPlugin.Roles.ServicePlanner, SchedulerPlugin.Roles.Disponent, SchedulerPlugin.Roles.SchedulerViewer };

			Add(PermissionGroup.WebApi, nameof(ArticleUserGroupRelationship), readOnlyRoles);
		}
	}
}
