namespace Main.Rest.Controllers
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using LMobile.Unicore;

	using Microsoft.AspNetCore.Mvc;

	public class AuthorizationController : RestController
	{
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<AdvancedGrantedPermission, Guid> grantedPermissionRepository;

		public AuthorizationController(IUserService userService, RestTypeProvider restTypeProvider, IRepositoryWithTypedId<AdvancedGrantedPermission, Guid> grantedPermissionRepository)
			: base(restTypeProvider)
		{
			this.userService = userService;
			this.grantedPermissionRepository = grantedPermissionRepository;
		}

		public virtual ActionResult Get(string id)
		{
			var user = id != null ? userService.GetUser(id) : userService.CurrentUser;
			var permissions = grantedPermissionRepository.GetAll().Where(x => x.UserId == user.UserId).Select(x => x.Permission).OrderBy(x => x.Name).ToArray();
			return Rest(permissions);
		}
	}
}
