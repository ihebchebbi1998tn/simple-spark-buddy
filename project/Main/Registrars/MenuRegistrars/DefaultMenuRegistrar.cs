namespace Main.Registrars.MenuRegistrars
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;

	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Menu;

	using Main.Model;
	using Main.Rest.Model;

	public class DefaultMenuRegistrar : IMenuRegistrar<MaterialMainMenu>, IMenuRegistrar<MaterialUserProfileMenu>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void Initialize(MenuProvider<MaterialUserProfileMenu> menuProvider)
		{
			menuProvider.Register(null, "Settings", url: "~/Main/Account/UserProfile", iconClass: "zmdi zmdi-account-circle", priority: Int32.MaxValue - 100);
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			menuProvider.Register(null, "Dashboard", url: "~/Main/Dashboard/IndexTemplate", iconClass: "zmdi zmdi-home", priority: 1700);
			menuProvider.AddPermission(null, "Dashboard", PermissionGroup.MaterialDashboard, PermissionName.View);

			menuProvider.Register(null, "Administration", iconClass: "zmdi zmdi-settings", priority: 300, htmlAttributes: new Dictionary<string, object> { { "data-bind", "if: !window.Helper.Offline || window.Helper.Offline.status == 'online'" } });
			menuProvider.Register("Administration", "UserAdministrationTitle", url: "~/Main/UserList/IndexTemplate", priority: 1400);
			menuProvider.AddPermission("Administration", "UserAdministrationTitle", PermissionGroup.UserAdmin, PermissionName.Index);
			menuProvider.Register("Administration", "UserGroups", url: "~/Main/UserGroupList/IndexTemplate", iconClass: "zmdi zmdi-alert-circle-o", priority: 1300);
			menuProvider.AddPermission("Administration", "UserGroups", PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListUsergroups);
			menuProvider.Register("Administration", "RolesAndPermissionsTitle", url: "~/Main/PermissionSchemaRoleList/IndexTemplate", priority: 1200);
			menuProvider.AddPermission("Administration", "RolesAndPermissionsTitle", PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListRoles);
			menuProvider.Register("Administration", "BackgroundServices", iconClass: "zmdi zmdi-alarm zmdi-hc-fw", url: "~/Main/BackgroundService/IndexTemplate", priority: 1100);
			menuProvider.AddPermission("Administration", "BackgroundServices", PermissionGroup.WebApiRead, nameof(MainPlugin.PermissionGroup.BackgroundService));
			menuProvider.Register("Administration", "Logs", url: "~/Main/LogList/IndexTemplate", iconClass: "zmdi zmdi-alert-circle-o", priority: 1000);
			menuProvider.AddPermission("Administration", "Logs", PermissionGroup.WebApiRead, nameof(Log));
			menuProvider.Register("Administration", "Transactions", iconClass: "zmdi zmdi-view-list", url: "~/Main/PostingList/IndexTemplate", priority: 900);
			menuProvider.AddPermission("Administration", "Transactions", PermissionGroup.WebApiRead, nameof(Posting));
			menuProvider.Register("Administration", "EmailMessages", iconClass: "zmdi zmdi-email", url: "~/Main/MessageList/IndexTemplate", priority: 700);
			menuProvider.AddPermission("Administration", "EmailMessages", PermissionGroup.WebApiRead, nameof(Message));
			menuProvider.Register("Administration", "DocumentGeneration", iconClass: "zmdi zmdi-email", url: "~/Main/DocumentGeneratorList/IndexTemplate", priority: 500);
			menuProvider.AddPermission("Administration", "DocumentGeneration", PermissionGroup.WebApiRead, nameof(DocumentGeneratorEntry));
			menuProvider.Register("Administration", "Lookups", url: "~/Main/LookupList/IndexTemplate", priority: 400);
			menuProvider.AddPermission("Administration", "Lookups", PermissionGroup.WebApiRead, MainPlugin.PermissionName.Lookup);
			menuProvider.Register("Administration", "SiteSettings", url: "~/Main/Site/Details", priority: 100);
			menuProvider.AddPermission("Administration", "SiteSettings", MainPlugin.PermissionGroup.Site, PermissionName.Settings);
			menuProvider.AddPermission("Administration", "SiteSettings", PermissionGroup.WebApiRead, PermissionName.Settings);

			menuProvider.Register("Help", "ThirdPartySoftware", url: "~/Main/Site/ThirdPartySoftware", priority: 200);
			menuProvider.AddPermission("Help", "ThirdPartySoftware", MainPlugin.PermissionGroup.ThirdPartySoftware, PermissionName.View);
		}
	}
}
