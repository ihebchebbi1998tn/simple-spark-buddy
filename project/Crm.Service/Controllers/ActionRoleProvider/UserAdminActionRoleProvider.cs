namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main;

	public class UserAdminActionRoleProvider : RoleCollectorBase
	{
		public UserAdminActionRoleProvider(IPluginProvider pluginProvider) :
			base(pluginProvider)
		{
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.AddUserGroup, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.AssignLicense, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, PermissionName.Create, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.CreateUser, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, PermissionName.Edit, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.EditUser, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.EditUserGroup, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, PermissionName.Index, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListUsergroups, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.RemoveUserGroup, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.RevokeLicense, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.UserDetail, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.UserResetGeneralToken, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.UserGroupDetail, ServicePlugin.Roles.HeadOfService);
			Add(PermissionGroup.WebApi, MainPlugin.PermissionName.SetUsers, ServicePlugin.Roles.HeadOfService);
		}
	}
}
