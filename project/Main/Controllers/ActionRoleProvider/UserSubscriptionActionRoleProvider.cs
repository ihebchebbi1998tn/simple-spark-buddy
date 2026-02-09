namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main.Model;

	public class UserSubscriptionActionRoleProvider : RoleCollectorBase
	{
		public UserSubscriptionActionRoleProvider(IPluginProvider pluginProvider) : base(pluginProvider)
		{
			Add(PermissionGroup.WebApi, nameof(UserSubscription));
		}
	}
}
