namespace Main.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Services.Interfaces;

	using LMobile.Unicore;

	using Main.Rest.Model;

	using Microsoft.AspNetCore.Mvc;

	[ControllerName("Main_User")]
	public class AuthorizationODataController : ODataControllerEx, IEntityApiController
	{
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<AdvancedGrantedPermission, Guid> grantedPermissionRepository;
		private readonly IODataMapper mapper;
		public Type EntityType => typeof(Crm.Library.Model.User);
		public AuthorizationODataController(IUserService userService, IRepositoryWithTypedId<AdvancedGrantedPermission, Guid> grantedPermissionRepository, IODataMapper mapper)
		{
			this.userService = userService;
			this.grantedPermissionRepository = grantedPermissionRepository;
			this.mapper = mapper;
		}
		[HttpGet]
		public virtual IActionResult GetPermissions()
		{
			var permissions = grantedPermissionRepository.GetAll()
				.Where(x => x.UserId == userService.CurrentUser.UserId)
				.Select(x => x.Permission)
				.OrderBy(x => x.Name);
			var result = mapper.Map<IEnumerable<PermissionRest>>(permissions);
			return Ok(result);
		}
		[HttpGet]
		public virtual IActionResult GetRoles()
		{
			return Ok(userService.CurrentUser.Roles.Select(x => x.Name));
		}
		[HttpGet]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
		public virtual IActionResult GetToken()
		{
			return Ok(userService.CurrentUser.GeneralToken);
		}
		[HttpGet]
		public virtual IActionResult GetUser()
		{
			return Ok(mapper.Map<UserRest>(userService.CurrentUser));
		}
	}
}
