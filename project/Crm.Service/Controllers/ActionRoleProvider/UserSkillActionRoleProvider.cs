namespace Crm.Service.Controllers.ActionRoleProvider
{
	using System.Linq;

	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Main.Model;
	using Crm.Service;

	public class UserSkillActionRoleProvider : RoleCollectorBase
	{
		public UserSkillActionRoleProvider(IPluginProvider pluginProvider) :
			base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService
			};
			Add(PermissionGroup.WebApi, nameof(UserSkill), roles);

			if (pluginProvider.ActivePluginNames.Contains("Crm.Offline"))
			{
				Add(PermissionGroup.Sync, nameof(UserSkill), roles);
			}
		}
	}
}
