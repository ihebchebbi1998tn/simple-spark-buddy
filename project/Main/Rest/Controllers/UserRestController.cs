namespace Main.Rest.Controllers
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Licensing;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using LMobile.Unicore;

	using Microsoft.AspNetCore.Mvc;

	using User = Crm.Library.Model.User;

	public class UserRestController : RestController<User>
	{
		private readonly IUserService userService;
		private readonly IUsergroupService usergroupService;
		private readonly IResourceManager resourceManager;
		private readonly IAccessControlManager accessControlManager;
		private readonly Lazy<IGrantedRoleStore> grantedRoleStore;
		private readonly IRepositoryWithTypedId<AdvancedGrantedPermission, Guid> grantedPermissionRepository;
		private readonly ILicensingService licensingService;

		// Methods
		[RequiredRole("Administrator")]
		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Get(Guid id)
		{
			var user = userService.GetUser(id);
			return Rest(user);
		}
		
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Create(User user)
		{
			userService.SaveUser(user);
			return Rest(user.UserId.ToString());
		}
		
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Update(User user)
		{
			userService.SaveUser(user);
			return new EmptyResult();
		}

		[RequiredPermission(PermissionName.Delete, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Delete(string username)
		{
			userService.DeleteUser(username);
			return Content("User deleted.");
		}

		public virtual ActionResult CurrentUser()
		{
			return Rest(userService.CurrentUser.Self);
		}

		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult List(bool? withEverybodyUser, bool? withEmptyUser)
		{
			var users = userService.GetUsers().OrderBy(x => x.DisplayName).ToList();
			if (withEverybodyUser.GetValueOrDefault())
			{
				users.Insert(0, new User(resourceManager.GetTranslation("Everybody")));
			}
			if (withEmptyUser.GetValueOrDefault())
			{
				users.Insert(0, new User(null));
			}
			return Rest(users, "Users");
		}

		public virtual ActionResult SetDeviceToken(string deviceToken)
		{
			var callingUser = userService.GetUser(userService.CurrentUser.Id);
			callingUser.AppleDeviceToken = deviceToken;
			userService.SaveUser(callingUser);

			return new EmptyResult();
		}

		#region Permissions

		[RequiredPermission(MainPlugin.PermissionName.ListPermissions, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult ListPermissions(Guid id)
		{
			var permissions = grantedPermissionRepository.GetAll().Where(x => x.UserId == id).Select(x => x.Permission.Name).OrderBy(x => x).ToArray();
			return Rest(permissions, "Permissions");
		}

		#endregion Permissions

		#region Roles

		[RequiredPermission(MainPlugin.PermissionName.ListRoles, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult ListRoles(Guid id)
		{
			var roles = grantedRoleStore.Value.ListByUser(id);
			return Rest(roles, "Roles");
		}

		[RequiredPermission(MainPlugin.PermissionName.AddRole, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult AddRole(Guid id, Guid roleId)
		{
			licensingService.AssignPermissionSchemaRole(userService, accessControlManager, userService.GetUser(id), roleId);
			return new EmptyResult();
		}

		[RequiredPermission(MainPlugin.PermissionName.RemoveRole, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult RemoveRole(Guid id, Guid roleId)
		{
			licensingService.UnassignPermissionSchemaRole(userService, accessControlManager, userService.GetUser(id), roleId);
			return new EmptyResult();
		}

		#endregion Roles

		#region Usergroups

		[RequiredPermission(MainPlugin.PermissionName.ListUsergroups, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult ListUsergroups(Guid id)
		{
			var user = userService.GetUser(id);
			return Rest(user.Usergroups, "Usergroups");
		}

		[RequiredPermission(MainPlugin.PermissionName.AddUserGroup, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult AddUsergroup(Guid id, Guid usergroupId)
		{
			var user = userService.GetUser(id);
			var usergroupToAdd = usergroupService.GetUsergroup(usergroupId);
			user.AddUsergroup(usergroupToAdd);
			userService.SaveUser(user);
			return new EmptyResult();
		}

		[RequiredPermission(MainPlugin.PermissionName.RemoveUserGroup, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult RemoveUsergroup(Guid id, Guid usergroupId)
		{
			var user = userService.GetUser(id);
			var usergroupToRemove = usergroupService.GetUsergroup(usergroupId);
			user.RemoveUsergroup(usergroupToRemove);
			userService.SaveUser(user);
			return new EmptyResult();
		}

		#endregion Usergroups

		// Constructor
		public UserRestController(IUserService userService, IUsergroupService usergroupService, IResourceManager resourceManager, RestTypeProvider restTypeProvider, IAccessControlManager accessControlManager, Lazy<IGrantedRoleStore> grantedRoleStore, IRepositoryWithTypedId<AdvancedGrantedPermission, Guid> grantedPermissionRepository, ILicensingService licensingService)
			: base(restTypeProvider)
		{
			this.userService = userService;
			this.usergroupService = usergroupService;
			this.resourceManager = resourceManager;
			this.accessControlManager = accessControlManager;
			this.grantedRoleStore = grantedRoleStore;
			this.grantedPermissionRepository = grantedPermissionRepository;
			this.licensingService = licensingService;
		}
	}
}
