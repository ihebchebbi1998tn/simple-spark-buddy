namespace Main.Flow.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main.Flow.Model;

	public class FlowActionRoleProvider : RoleCollectorBase
	{
		public FlowActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.WebApi, nameof(FlowItem));
			Add(PermissionGroup.WebApi, nameof(FlowRule));
		}
	}
}
