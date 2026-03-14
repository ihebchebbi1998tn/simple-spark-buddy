namespace Crm.Service.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	using LMobile.Unicore;

	using Main.Rest.Model;

	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.AspNetCore.Mvc;

	[ControllerName("Main_User")]
	public class TechnicianODataController : ODataControllerEx, IEntityApiController
	{
		private readonly IUserService userService;
		private readonly IAccessControlManager accessControlManager;
		private readonly IRepositoryWithTypedId<AdvancedGrantedPermission, Guid> grantedPermissionRepository;
		private readonly IODataMapper mapper;
		public Type EntityType => typeof(Library.Model.User);
		public TechnicianODataController(
			IUserService userService,
			IAccessControlManager accessControlManager,
			IRepositoryWithTypedId<AdvancedGrantedPermission, Guid> grantedPermissionRepository,
			IODataMapper mapper)
		{
			this.userService = userService;
			this.accessControlManager = accessControlManager;
			this.grantedPermissionRepository = grantedPermissionRepository;
			this.mapper = mapper;
		}
		[HttpGet]
		public virtual IActionResult GetTechnicians(ODataQueryOptions<UserRest> options) => GetTechnicians(options, false);
		[HttpGet]
		public virtual IActionResult GetTechnicians(ODataQueryOptions<UserRest> options, bool filterByRole)
		{
			var userIds = grantedPermissionRepository.GetAll()
				.Where(x => x.Permission.Name == $"{PermissionGroup.WebApiWrite}::{nameof(ServiceOrderDispatch)}")
				.Select(x => x.UserId);
			var technicians = userService.GetActiveUsersQuery().Where(x => userIds.Contains(x.UserId));
			if (filterByRole)
			{
				var template = accessControlManager.QueryPermissionSchemaRoles().Single(x => x.Name == ServicePlugin.Roles.FieldService && x.CompilationName == "DefaultCompilation");
				technicians = technicians.Where(technician => technician.Roles.Any(role => role.SourcePermissionSchemaRoleId == template.UId));
			}
			var settings = new ODataQuerySettings { EnableConstantParameterization = false, HandleNullPropagation = HandleNullPropagationOption.False };
			var result = mapper.Map<IEnumerable<UserRest>>(technicians);
			result = (IEnumerable<UserRest>)options.ApplyTo(result.AsQueryable(), settings);
			return Ok(result);
		}
	}
}
