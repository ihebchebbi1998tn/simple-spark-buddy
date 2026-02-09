namespace Main.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Extensions;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Services.Interfaces;

	using Main.Rest.Model;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Formatter;

	[ControllerName(EntitySetName)]
	public class UsergroupsODataController : ODataControllerEx, IEntityApiController
	{
		public const string EntitySetName = "Main_Usergroup";
		public const string ParameterUserGroup = "UserGroup";
		public const string ParameterUserIds = "UserIds";
		private readonly IUserService userService;
		private readonly IUsergroupService usergroupService;
		private readonly IODataMapper mapper;
		public Type EntityType => typeof(Usergroup);
		public UsergroupsODataController(
			IODataMapper mapper,
			IUserService userService,
			IUsergroupService usergroupService)
		{
			this.userService = userService;
			this.usergroupService = usergroupService;
			this.mapper = mapper;
		}

		[RequiredPermission(MainPlugin.PermissionName.EditUser, Group = PermissionGroup.UserAdmin)]
		[HttpPost, HttpPut]
		public virtual IActionResult SetUsers(ODataActionParameters parameters)
		{
			var userGroupId = parameters.GetValue<Guid>(ParameterUserGroup);
			var userGroup = usergroupService.GetUsergroup(userGroupId);
			var selectedUserIds = parameters.GetValue<IEnumerable<string>>(ParameterUserIds);
			var selectedUsers =  selectedUserIds.Select(s => userService.GetUser(s)).ToArray();

			userService.GetUsers().ForEach(
				user =>
				{
					if (selectedUsers.Contains(user))
					{
						if (!user.Usergroups.Contains(userGroup))
						{
							user.AddUsergroup(userGroup);
							userService.SaveUser(user);
						}
					}
					else
					{
						if (user.Usergroups.Contains(userGroup))
						{
							user.RemoveUsergroup(userGroup);
							userService.SaveUser(user);
						}
					}
				});

			var savedRestEntity = mapper.Map<Usergroup, UsergroupRest>(userGroup);
			return StatusCode((int)HttpStatusCode.OK, savedRestEntity);
		}
	}
}
