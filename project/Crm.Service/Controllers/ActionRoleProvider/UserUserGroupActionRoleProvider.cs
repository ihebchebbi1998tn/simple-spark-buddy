namespace Crm.Service.Controllers.ActionRoleProvider
{
	using System.Linq;

	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Main.Model;
	using Crm.Service;

	public class UserUserGroupActionRoleProvider : RoleCollectorBase
	{
		public UserUserGroupActionRoleProvider(IPluginProvider pluginProvider) :
			base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService
			};
			Add(PermissionGroup.WebApi, nameof(UserUserGroup), roles);
		}
	}
}
