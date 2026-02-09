namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class RecentPagesActionRoleProvider : RoleCollectorBase
    {
	    public RecentPagesActionRoleProvider(IPluginProvider pluginProvider)
            : base(pluginProvider)
        {
            Add(PermissionGroup.WebApi, nameof(RecentPage));
        }
    }
}
