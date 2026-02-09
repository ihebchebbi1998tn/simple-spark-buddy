namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Licensing;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	[Licensing(ModuleId = "FLD03180")]
	public class AdhocOrderActionRoleProvider : RoleCollectorBase
	{
		public AdhocOrderActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.Adhoc,
						PermissionName.Create,
							ServicePlugin.Roles.FieldService,
							ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.Adhoc, PermissionName.Create, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Create);
			AddImport(ServicePlugin.PermissionGroup.Adhoc, PermissionName.Create, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Read);
		}
	}
}
