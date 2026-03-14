namespace Crm.Service.Team.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class UserAdminActionRoleProvider : RoleCollectorBase
	{
		public UserAdminActionRoleProvider(IPluginProvider pluginProvider)
			:
			base(pluginProvider)
		{
			Add(PermissionGroup.UserAdmin,
				ServiceTeamPlugin.PermissionName.ListServiceTeams,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
