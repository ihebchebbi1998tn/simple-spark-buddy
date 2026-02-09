namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main.Model;
	using Main.Model.Lookups;

	public class LookupActionRoleProvider : RoleCollectorBase
	{
		public LookupActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.WebApi, nameof(Country), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Currency), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(InvoicingType), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(LengthUnit), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Log), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Region), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(TimeUnit), Roles.APIUser);
			Add(PermissionGroup.WebApi, MainPlugin.PermissionName.Lookup);
			Add(PermissionGroup.WebApi, nameof(Asset), Roles.APIUser);
		}
	}
}
