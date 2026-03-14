namespace Main.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Dynamic.Core;
	using System.Net;

	using AutoMapper.Extensions.ExpressionMapping;
	using AutoMapper.QueryableExtensions;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Extensions;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Licensing;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Unicore;

	using LMobile.Unicore;
	using LMobile.Unicore.NHibernate;

	using Main.Model.Extension;
	using Main.Rest.Model;
	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Extensions;
	using Microsoft.AspNetCore.OData.Formatter;
	using Microsoft.AspNetCore.OData.Query;

	[ControllerName("Main_PermissionSchemaRole")]
	public class RoleController : ODataControllerEx
	{
		public const string ParameterSourcePermissionSchemaRoleName = "SourcePermissionSchemaRoleName";
		private readonly IUserService userService;
		private readonly ISyncService<PermissionSchemaRole> roleSyncService;
		private readonly IODataMapper mapper;
		private readonly IAccessControlManager accessControlManager;
		private readonly IResourceManager resourceManager;
		private readonly IPermissionManager permissionManager;
		private readonly ILicensingService licensingService;
		public RoleController(IUserService userService, ISyncService<PermissionSchemaRole> roleSyncService, IODataMapper mapper, IAccessControlManager accessControlManager, IResourceManager resourceManager, IPermissionManager permissionManager, ILicensingService licensingService)
		{
			this.userService = userService;
			this.roleSyncService = roleSyncService;
			this.mapper = mapper;
			this.accessControlManager = accessControlManager;
			this.resourceManager = resourceManager;
			this.permissionManager = permissionManager;
			this.licensingService = licensingService;
		}

		[RequiredPermission(MainPlugin.PermissionName.CreateRole, Group = PermissionGroup.UserAdmin)]
		[HttpPost]
		public virtual IActionResult AddRole(ODataActionParameters parameters)
		{
			if (ModelState.ErrorCount > 0)
			{
				return ValidationProblem(ModelState);
			}

			var rest = parameters.GetValue<PermissionSchemaRoleRest>("Role");
			var role = mapper.Map<PermissionSchemaRole>(rest);

			var permissionSchemaId = accessControlManager.FindPermissionSchema(UnicoreDefaults.DefaultPermissionSchema).UId;
			parameters.GetOptionalValue<string>(ParameterSourcePermissionSchemaRoleName, out var sourcePermissionSchemaRoleName);
			if (sourcePermissionSchemaRoleName != null)
			{
				var sourcePermissionSchema = accessControlManager.FindPermissionSchema(UnicoreDefaults.TemplatePermissionSchema);
				role.SourcePermissionSchemaRoleId = accessControlManager.FindPermissionSchemaRole(sourcePermissionSchema.UId, sourcePermissionSchemaRoleName).UId;
			}
			if (role.SourcePermissionSchemaRoleId.HasValue && role.SourcePermissionSchemaRoleId != default(Guid))
			{
				var sourcePermissionSchema = accessControlManager.FindPermissionSchema(UnicoreDefaults.TemplatePermissionSchema);
				var sourcePermissionSchemaRole = accessControlManager.FindPermissionSchemaRole(role.SourcePermissionSchemaRoleId.Value);
				rest.ExtensionValues.TryGetValue("Description", out var description);

				role = accessControlManager.ClonePermissionSchemaRole(sourcePermissionSchema.UId, sourcePermissionSchemaRole.Name, permissionSchemaId, role.Name);
				role.GetExtension<PermissionSchemaRoleExtension>().Description = (string)description;
			}
			else
			{
				role = accessControlManager.CreatePermissionSchemaRole(permissionSchemaId, UnicoreDefaults.CommonDomainId, role.Name, role.Group, role.IgnoreCircles, x => x.Extensions = role.Extensions);
			}

			accessControlManager.UpdatePermissionSchemaRole(role);

			var savedRestEntity = mapper.Map<PermissionSchemaRole, PermissionSchemaRoleRest>(role);
			return StatusCode((int)HttpStatusCode.Created, savedRestEntity);
		}
		[RequiredPermission(MainPlugin.PermissionName.EditRole, Group = PermissionGroup.UserAdmin)]
		[HttpPost, HttpPut]
		public virtual IActionResult AssignPermissions(ODataActionParameters parameters)
		{
			var roleKey = parameters.GetValue<Guid>("RoleKey");
			var role = accessControlManager.FindPermissionSchemaRole(roleKey);
			var currentPermissionNames = accessControlManager.ListPermissionSchemaRolePermissions(roleKey).Select(x => x.Name).ToList();
			if (parameters.GetOptionalValue<IEnumerable<string>>("UnassignedPermissions", out var unassignedPermissions))
			{
				unassignedPermissions = unassignedPermissions.Where(x => currentPermissionNames.Contains(x)).ToArray();
				if (unassignedPermissions.Any())
				{
					accessControlManager.RemovePermissionSchemaRolePermission(roleKey, unassignedPermissions.ToArray());
				}
			}

			if (parameters.GetOptionalValue<IEnumerable<string>>("AssignedPermissions", out var assignedPermissions))
			{
				assignedPermissions = assignedPermissions.Where(x => !currentPermissionNames.Contains(x)).ToArray();
				if (assignedPermissions.Any())
				{
					accessControlManager.AddPermissionSchemaRolePermission(roleKey, assignedPermissions.ToArray());
				}
			}

			return StatusCode((int)HttpStatusCode.OK, mapper.Map<PermissionSchemaRole, PermissionSchemaRoleRest>(role));
		}
		[RequiredPermission(MainPlugin.PermissionName.AssignRole, Group = PermissionGroup.UserAdmin)]
		[HttpPost, HttpPut]
		public virtual IActionResult AssignUsers(ODataActionParameters parameters)
		{
			var roleKey = parameters.GetValue<Guid>("RoleKey");
			var role = accessControlManager.FindPermissionSchemaRole(roleKey);
			if (parameters.GetOptionalValue<IEnumerable<string>>("UnassignedUsernames", out var unassignedUsernames))
			{
				foreach (var username in unassignedUsernames)
				{
					licensingService.UnassignPermissionSchemaRole(userService, accessControlManager, userService.GetUser(username), roleKey);
				}
			}

			if (parameters.GetOptionalValue<IEnumerable<string>>("AssignedUsernames", out var assignedUsernames))
			{
				foreach (var username in assignedUsernames)
				{
					if (!licensingService.AssignPermissionSchemaRole(userService, accessControlManager, userService.GetUser(username), roleKey))
					{
						var roleName = accessControlManager.FindPermissionSchemaRole(roleKey).Name;
						ModelState.AddModelError(nameof(parameters), resourceManager.GetTranslation("CouldNotAssignRoleInsufficientLicense", userService?.CurrentUser?.DefaultLanguageKey).WithArgs(roleName));
						return ValidationProblem(ModelState);
					}
				}
			}

			return StatusCode((int)HttpStatusCode.OK, mapper.Map<PermissionSchemaRole, PermissionSchemaRoleRest>(role));
		}
		[HttpDelete]
		public virtual IActionResult Delete(Guid key)
		{
			var role = accessControlManager.FindPermissionSchemaRole(key);
			roleSyncService.Remove(role);
			return new NoContentResult();
		}
		[HttpGet]
		public virtual IActionResult Get(ODataQueryOptions<PermissionSchemaRoleRest> options) => Get(options, null);
		[HttpGet]
		public virtual IActionResult Get(ODataQueryOptions<PermissionSchemaRoleRest> options, Guid? key)
		{
			var settings = new ODataQuerySettings { EnableConstantParameterization = false, HandleNullPropagation = HandleNullPropagationOption.False };

			var query = roleSyncService.GetAll(userService.CurrentUser);
			var projectedQuery = query
				.UseAsDataSource(mapper)
				.For<PermissionSchemaRoleRest>();
			var destinationQuery = options.ApplyTo(projectedQuery, settings, AllowedQueryOptions.All & ~(AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top));
			var sourceQuery = (IQueryable<PermissionSchemaRole>)destinationQuery.Provider.Execute(destinationQuery.Expression);

			if (options.Count != null)
			{
				var properties = options.Request.ODataFeature();
				var destinationQueryWithoutExpands = query.UseAsDataSource(mapper).For<PermissionSchemaRoleRest>();
				var countQuery = options.ApplyTo(destinationQueryWithoutExpands, settings, AllowedQueryOptions.Count | AllowedQueryOptions.Expand | AllowedQueryOptions.Select | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top);
				if (Request.IsCountRequest())
				{
					return new ObjectResult(countQuery.Count());
				}

				properties.TotalCount = countQuery.Count();
			}

			if (key.HasValue)
			{
				var role = sourceQuery.SingleOrDefault(x => x.UId == key);
				if (role != null)
				{
					return Ok(mapper.Map<PermissionSchemaRoleRest>(role));
				}

				return NotFound();
			}

			return Ok(mapper.Map<IEnumerable<PermissionSchemaRoleRest>>(sourceQuery));
		}
		[HttpGet]
		[ProducesResponseType(typeof(RolePermissions), (int)HttpStatusCode.OK)]
		public virtual IActionResult GetRolePermissions(Guid RoleKey)
		{
			var role = accessControlManager.FindPermissionSchemaRole(RoleKey);
			var model = new RoleEditViewModel(accessControlManager, resourceManager, permissionManager, role) { Item = role };
			var result = new RolePermissions { PermissionGroups = model.PermissionGroups.Select(x => new RolePermissionGroup { Name = x.Key, Permissions = x.Value.Select(y => new PermissionGroupPermission { PermissionName = y.Name, ImportedBy = model.PermissionImports.Keys.Where(z => model.PermissionImports[z].Contains(y.Name)).ToArray(), ImportedPermissions = model.PermissionImports.ContainsKey(y.Name) ? model.PermissionImports[y.Name].ToArray() : Array.Empty<string>() }).ToArray() }).ToArray(), Permissions = model.Item.PersistenceExtensionBag<PermissionSchemaRolePersistenceExtension, Permission>(p => p.Permissions).Cast<Permission>().Select(x => x.Name).ToArray(), SourcePermissions = model.Item.PersistenceExtension<PermissionSchemaRolePersistenceExtension, PermissionSchemaRole>(p => p.SourcePermissionSchemaRole)?.PersistenceExtensionBag<PermissionSchemaRolePersistenceExtension, Permission>(p => p.Permissions).Cast<Permission>().Select(x => x.Name).ToArray() };
			return Ok(result);
		}
		[HttpGet]
		public virtual IActionResult GetTemplates(ODataQueryOptions<PermissionSchemaRoleRest> options)
		{
			var settings = new ODataQuerySettings { EnableConstantParameterization = false, HandleNullPropagation = HandleNullPropagationOption.False };
			var query = Enumerable.Empty<PermissionSchemaRoleRest>().AsQueryable();
			query = (IQueryable<PermissionSchemaRoleRest>)options.ApplyTo(query, settings, AllowedQueryOptions.All & ~(AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top));
			var result = query.Map(accessControlManager.QueryPermissionSchemaRoles(), mapper.ConfigurationProvider);
			var templatePermissionSchema = accessControlManager.FindPermissionSchema(UnicoreDefaults.TemplatePermissionSchema);
			result = result.Where(x => x.PermissionSchemaId == templatePermissionSchema.UId);
			return Ok(mapper.Map<IEnumerable<PermissionSchemaRoleRest>>(result));
		}
		[HttpPut]
		public virtual IActionResult Put(Guid key, [FromBody] PermissionSchemaRoleRest restEntity)
		{
			if (ModelState.ErrorCount > 0)
			{
				return ValidationProblem(ModelState);
			}

			if (restEntity == null)
			{
				return BadRequest("rest entity is null or could not be deserialized");
			}

			var originalEntity = accessControlManager.FindPermissionSchemaRole(key);
			if (originalEntity == null)
			{
				return NotFound();
			}

			var entity = mapper.Map(restEntity, originalEntity);
			var savedEntity = roleSyncService.Save(entity);
			var savedRestEntity = mapper.Map<PermissionSchemaRole, PermissionSchemaRoleRest>(savedEntity);
			return StatusCode((int)HttpStatusCode.OK, savedRestEntity);
		}
		[HttpPost, HttpPut]
		public virtual IActionResult ResetRole(ODataActionParameters parameters)
		{
			var roleKey = parameters.GetValue<Guid>("RoleKey");
			accessControlManager.ResetPermissionSchemaRolePermissions(roleKey);
			var role = accessControlManager.FindPermissionSchemaRole(roleKey);
			return StatusCode((int)HttpStatusCode.OK, mapper.Map<PermissionSchemaRole, PermissionSchemaRoleRest>(role));
		}
	}
}
