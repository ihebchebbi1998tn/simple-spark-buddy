namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class CategoryActionRoleProvider : RoleCollectorBase
	{
		public CategoryActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Index);
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Delete);
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Edit);
		}
	}
}
