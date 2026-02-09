namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;

	using PermissionGroup = MainPlugin.PermissionGroup;

	public class SiteActionRoleProvider : RoleCollectorBase
	{
		public SiteActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Site, PermissionName.Settings);
			Add(PermissionGroup.Site, PermissionName.SetTheme);
			Add(PermissionGroup.Site, PermissionName.SetProperties);
			Add(PermissionGroup.Site, PermissionName.SaveReportSettings);
			Add(PermissionGroup.Site, MainPlugin.PermissionName.SetMinPasswordStrength);
			Add(PermissionGroup.Site, MainPlugin.PermissionName.SetMultiFactorAuthenticationMode);
			Add(PermissionGroup.ThirdPartySoftware, PermissionName.View);
			Add(Crm.Library.Model.Authorization.PermissionGroup.WebApi, nameof(Site));
			Add(Crm.Library.Model.Authorization.PermissionGroup.WebApi, PermissionName.Settings);
		}
	}
}
