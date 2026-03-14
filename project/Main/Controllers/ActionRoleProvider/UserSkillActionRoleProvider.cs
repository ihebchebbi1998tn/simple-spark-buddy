namespace Main.Controllers.ActionRoleProvider
{
	using System.Linq;

	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Main.Model;

	public class UserSkillActionRoleProvider : RoleCollectorBase
	{
		public UserSkillActionRoleProvider(IPluginProvider pluginProvider) :
			base(pluginProvider)
		{
			Add(nameof(UserSkill), PermissionName.Create);
			Add(nameof(UserSkill), PermissionName.Edit);
			Add(nameof(UserSkill), PermissionName.Index);
			Add(nameof(UserSkill), PermissionName.View);
			Add(PermissionGroup.WebApi, nameof(UserSkill));

			if (pluginProvider.ActivePluginNames.Contains("Crm.Offline"))
			{
				Add(PermissionGroup.Sync, nameof(UserSkill));
				AddImport(PermissionGroup.Sync, nameof(UserSkill), PermissionGroup.WebApi, nameof(UserSkill));
			}
		}
	}
}
