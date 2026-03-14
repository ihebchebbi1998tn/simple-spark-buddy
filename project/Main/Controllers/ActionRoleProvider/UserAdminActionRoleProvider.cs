namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using LMobile.Unicore;

	using User = Crm.Library.Model.User;

	public class UserAdminActionRoleProvider : RoleCollectorBase
	{
		public UserAdminActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.AddRole);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.AddUserGroup);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.AssignRole);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.AssignSkill);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.AssignUserGroup);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.CreateRole);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.CreateUser);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.CreateUserGroup);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.DeleteRole);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.DeleteUser);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.DeleteUserGroup);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.EditRole);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.EditUser);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.EditUserGroup);
			Add(PermissionGroup.UserAdmin, PermissionName.Create);
			Add(PermissionGroup.UserAdmin, PermissionName.Delete);
			Add(PermissionGroup.UserAdmin, PermissionName.Edit);
			Add(PermissionGroup.UserAdmin, PermissionName.Get);
			Add(PermissionGroup.UserAdmin, PermissionName.Index);
			Add(PermissionGroup.UserAdmin, PermissionName.Read);
			AddImport(PermissionGroup.UserAdmin, PermissionName.Index, PermissionGroup.WebApi, nameof(User));
			AddImport(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListRoles, PermissionGroup.WebApi, nameof(PermissionSchemaRole));
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListPermissions);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListRoles);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListUsergroups);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.RefreshUserCache);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.RemoveRole);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.RemoveUserGroup);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.RequestLocalDatabase);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.RequestLocalStorage);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ResetUserPassword);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.SaveUserGroup);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.SendJavaScriptCommand);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.SetLogLevel);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.SignalR);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ToggleActive);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.UserDetail);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.UserResetGeneralToken);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ViewLocalDatabaseLog);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ViewLocalStorageLog);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.AssignLicense, MainPlugin.Roles.LicenseManagement);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.RevokeLicense, MainPlugin.Roles.LicenseManagement);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ManageMultiFactorAuthentication);
			Add(PermissionGroup.UserAdmin, MainPlugin.PermissionName.ResetUserTotp);

			AddImport(PermissionGroup.WebApi, nameof(Usergroup), PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListUsergroups);
			Add(PermissionGroup.WebApi, MainPlugin.PermissionName.SetUsers);
		}
	}
}
