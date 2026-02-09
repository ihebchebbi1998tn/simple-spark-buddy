namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main;

	public class CategoryActionRoleProvider : RoleCollectorBase
	{
		public CategoryActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Index, ServicePlugin.Roles.ServiceBackOffice);
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Delete, ServicePlugin.Roles.ServiceBackOffice);
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Edit, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
