namespace Main.Controllers.OData
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using AutoMapper.Extensions.ExpressionMapping;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Extensions;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Licensing;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Signalr;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	using Main.Extensions;
	using Main.Rest.Model;

	using Medallion.Threading;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Http.Extensions;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Formatter;
	using Microsoft.AspNetCore.OData.Query;

	using User = Crm.Library.Model.User;

	[ControllerName(EntitySetName)]
	public class UserAdminODataController : ODataControllerEx, IEntityApiController
	{
		public const string EntitySetName = "Main_User";
		public const string ParameterUser = "User";
		public const string ParameterAdName = "AdName";
		public const string ParameterLogLevel = "LogLevel";
		public const string ParameterRemark = "Remark";
		public const string ParameterUsergroupIds = "UsergroupIds";
		public const string ParameterUsergroupNames = "UsergroupNames";
		public const string ParameterPassword = "Password";
		public const string ParameterRoleIds = "RoleIds";
		public const string ParameterRoleNames = "RoleNames";
		private readonly IRuleValidationService ruleValidationService;
		private readonly IResourceManager resourceManager;
		private readonly IDistributedLockProvider lockProvider;
		private readonly IAuthenticationService authenticationService;
		private readonly IUserService userService;
		private readonly IUsergroupService usergroupService;
		private readonly IAccessControlManager accessControlManager;
		private readonly IEnumerable<IPluginUserSaveExtension> pluginUserSaveExtensions;
		private readonly IEnumerable<IODataWriteFunction<User, UserRest, string>> writeFunctions;
		private readonly IODataMapper mapper;
		private readonly Lazy<IGrantedRoleStore> grantedRoleStore;
		private readonly ISignalrProfiler signalrProfiler;
		private readonly ILicensingService licensingService;
		public Type EntityType => typeof(User);
		public UserAdminODataController(
			IODataMapper mapper,
			IRuleValidationService ruleValidationService,
			IResourceManager resourceManager,
			IDistributedLockProvider lockProvider,
			IAuthenticationService authenticationService,
			IUserService userService,
			IUsergroupService usergroupService,
			IAccessControlManager accessControlManager,
			IEnumerable<IPluginUserSaveExtension> pluginUserSaveExtensions,
			Lazy<IGrantedRoleStore> grantedRoleStore,
			IEnumerable<IODataWriteFunction<User, UserRest, string>> writeFunctions,
			ISignalrProfiler signalrProfiler,
			ILicensingService licensingService)
		{
			this.ruleValidationService = ruleValidationService;
			this.resourceManager = resourceManager;
			this.lockProvider = lockProvider;
			this.authenticationService = authenticationService;
			this.userService = userService;
			this.usergroupService = usergroupService;
			this.accessControlManager = accessControlManager;
			this.pluginUserSaveExtensions = pluginUserSaveExtensions;
			this.grantedRoleStore = grantedRoleStore;
			this.writeFunctions = writeFunctions;
			this.signalrProfiler = signalrProfiler;
			this.mapper = mapper;
			this.licensingService = licensingService;
		}
		[RequiredPermission(MainPlugin.PermissionName.CreateUser, Group = PermissionGroup.UserAdmin)]
		[HttpPost]
		public virtual IActionResult AddUser(ODataActionParameters parameters)
		{
			if (authenticationService.PasswordResetSupported && parameters?.ContainsKey(ParameterPassword) != true)
			{
				ModelState.AddModelError(nameof(parameters), $"{ParameterPassword} is required for the currently active authentication service");
			}

			if (ModelState.ErrorCount > 0)
			{
				return ValidationProblem(ModelState);
			}

			var rest = parameters.GetValue<UserRest>(ParameterUser);
			var user = mapper.Map<User>(rest);
			if ((userService.GetUser(user.Id) ?? userService.GetUser(user.UserId)) != null)
			{
				return Conflict("entity already exists");
			}

			if (authenticationService.PasswordResetSupported)
			{
				user.Password = parameters.GetValue<string>(ParameterPassword);
			}

			if (GetODataError(ruleValidationService.GetRuleViolations(user), resourceManager) is { } error)
			{
				return BadRequest(error);
			}

			user.UserId = Guid.Empty; //ensure IUserService does all its magic
			user.Password = null; //ensure this is not saved in clear text
			if (parameters.GetOptionalValue<string>(ParameterAdName, out var adName))
			{
				user.AdName = adName;
			}

			if (parameters.GetOptionalValue<string>(ParameterRemark, out var remark))
			{
				user.Remark = remark;
			}

			{
				// validate roles are part of the DefaultPermissionSchema (and not the template)
				parameters.GetOptionalValue<IEnumerable<Guid>>(ParameterRoleIds, out var roleIds);
				parameters.GetOptionalValue<IEnumerable<string>>(ParameterRoleNames, out var roleNames);
				if (roleIds?.Any() == true || roleNames?.Any() == true)
				{
					var roles = accessControlManager.ListPermissionSchemaRoles(UnicoreDefaults.DefaultPermissionSchema);
					if (roleIds?.Intersect(roles.Select(x => x.UId)).Count() != roleIds?.Count())
					{
						return BadRequest($"{ParameterRoleIds} must all be part of {UnicoreDefaults.DefaultPermissionSchema}: {string.Join(",", roleIds)}");
					}

					if (roleNames?.Intersect(roles.Select(x => x.Name)).Count() != roleNames?.Count())
					{
						return BadRequest($"{ParameterRoleNames} must all be part of {UnicoreDefaults.DefaultPermissionSchema}: {string.Join(",", roleNames)}");
					}
				}
			}
			if (parameters.GetOptionalValue<IEnumerable<Guid>>(ParameterUsergroupIds, out var usergroupIds))
			{
				user.AddUsergroups(usergroupIds.Select(x => usergroupService.GetUsergroup(x)));
			}
			else if (parameters.GetOptionalValue<IEnumerable<string>>(ParameterUsergroupNames, out var usergroupNames))
			{
				user.AddUsergroups(usergroupNames.Select(x => usergroupService.GetUsergroup(x)));
			}

			using (lockProvider.GetLock(nameof(UserAdminODataController)))
			{
				if (authenticationService.PasswordResetSupported)
				{
					authenticationService.ResetPassword(user, parameters.GetValue<string>(ParameterPassword));
				}
				else
				{
					userService.SaveUser(user);
				}

				foreach (var pluginUserSaveExtension in pluginUserSaveExtensions)
				{
					pluginUserSaveExtension.Save(user, parameters);
				}

				if (parameters.GetOptionalValue<IEnumerable<Guid>>(ParameterRoleIds, out var roleIds))
				{
					foreach (var roleId in roleIds)
					{
						if (!licensingService.AssignPermissionSchemaRole(userService,
								accessControlManager,
								user,
								roleId))
						{
							var roleName = accessControlManager.FindPermissionSchemaRole(roleId).Name;
							ModelState.AddModelError(nameof(parameters), resourceManager.GetTranslation("CouldNotAssignRoleInsufficientLicense", userService?.CurrentUser?.DefaultLanguageKey).WithArgs(roleName));
							return ValidationProblem(ModelState);
						}
					}
				}
				else if (parameters.GetOptionalValue<IEnumerable<string>>(ParameterRoleNames, out var roleNames))
				{
					foreach (var roleName in roleNames)
					{
						var roleId = accessControlManager.FindPermissionSchemaRole(UnicoreDefaults.DefaultPermissionSchema, roleName).UId;
						if (!licensingService.AssignPermissionSchemaRole(userService,
								accessControlManager,
								user,
								roleId))
						{
							ModelState.AddModelError(nameof(parameters), resourceManager.GetTranslation("CouldNotAssignRoleInsufficientLicense", userService?.CurrentUser?.DefaultLanguageKey).WithArgs(roleName));
							return ValidationProblem(ModelState);
						}
					}
				}
			}

			foreach (var writeFunction in writeFunctions)
			{
				writeFunction.Apply(Request,
					rest,
					user);
			}

			var url = Request.GetDisplayUrl();
			url = url[..url.IndexOf(EntitySetName, StringComparison.Ordinal)];
			url += $"{EntitySetName}('{user.Id}')";
			Response.GetTypedHeaders().Location = new(url);
			Response.Headers["OData-EntityId"] = user.Id;
			if (PrefersMinimal)
			{
				return NoContent();
			}

			var savedRestEntity = mapper.Map<User, UserRest>(user);
			return StatusCode((int)HttpStatusCode.Created, savedRestEntity);
		}

		[HttpGet]
		[RequiredPermission(MainPlugin.PermissionName.SignalR, Group = PermissionGroup.UserAdmin)]
		public virtual IActionResult GetLocalDatabaseFromLog(string username, string logDate)
		{
			var user = userService.GetUser(username);
			return new ObjectResult(signalrProfiler.GetLocalDatabaseFromLog(user, long.Parse(logDate)));
		}

		[HttpGet]
		[RequiredPermission(MainPlugin.PermissionName.SignalR, Group = PermissionGroup.UserAdmin)]
		public virtual IActionResult GetLocalStorageFromLog(string username, string logDate)
		{
			var user = userService.GetUser(username);
			return new ObjectResult(signalrProfiler.GetLocalStorageFromLog(user, long.Parse(logDate)));
		}

		[HttpGet]
		[RequiredPermission(MainPlugin.PermissionName.SignalR, Group = PermissionGroup.UserAdmin)]
		public virtual IActionResult GetSignalRInformation([FromQuery] ODataQueryOptions<UserRest> options)
		{
			var settings = new ODataQuerySettings
			{
				EnableConstantParameterization = false,
				HandleNullPropagation = HandleNullPropagationOption.False
			};
			var query = userService.GetUsersQuery();
			var projectedQuery = query.UseAsDataSource(mapper).For<UserRest>().AsQueryable();
			var appliedProjectedQuery = (IQueryable<UserRest>)options.ApplyTo(projectedQuery,
				settings,
				AllowedQueryOptions.Apply | AllowedQueryOptions.Expand | AllowedQueryOptions.Select);
			var result = (IEnumerable)appliedProjectedQuery.Provider.Execute(appliedProjectedQuery.Expression);
			result = mapper.Map<IEnumerable<UserSignalRInformation>>(result);
			return new ObjectResult(result);
		}

		[RequiredPermission(MainPlugin.PermissionName.SetLogLevel, Group = PermissionGroup.UserAdmin)]
		[HttpPost, HttpPut]
		public virtual IActionResult SetLogLevel(ODataActionParameters parameters)
		{
			var rest = parameters.GetValue<UserRest>(ParameterUser);
			var user = userService.GetUser(rest.Id);
			var logLevel = parameters.GetValue<JavaScriptLogLevel>(ParameterLogLevel);
			signalrProfiler.StartProfiling(user, logLevel);
			var savedRestEntity = mapper.Map<User, UserRest>(user);
			return StatusCode((int)HttpStatusCode.OK, savedRestEntity);
		}

		[RequiredPermission(MainPlugin.PermissionName.EditUser, Group = PermissionGroup.UserAdmin)]
		[HttpPost, HttpPut]
		public virtual IActionResult SetRoles(ODataActionParameters parameters)
		{
			var rest = parameters.GetValue<UserRest>(ParameterUser);
			var user = userService.GetUser(rest.Id);

			var roleIds = new List<Guid>();
			if (parameters.GetOptionalValue<IEnumerable<Guid>>(ParameterRoleIds, out var parameterRoleIds))
			{
				roleIds = parameterRoleIds.ToList();
			}
			else if (parameters.GetOptionalValue<IEnumerable<string>>(ParameterRoleNames, out var roleNames))
			{
				var permissionSchema = accessControlManager.FindPermissionSchema(UnicoreDefaults.DefaultPermissionSchema);
				roleIds = accessControlManager.QueryPermissionSchemaRoles().Where(x => x.PermissionSchemaId == permissionSchema.UId && roleNames.ToArray().Contains(x.Name)).Select(x => x.UId).ToList();
			}

			var userRoles = grantedRoleStore.Value.ListByUser(user.UserId).Select(x => x.RoleId).ToList();
			var unassignedRoleIds = userRoles.Where(x => !roleIds.Contains(x));

			foreach (var roleId in unassignedRoleIds)
			{
				licensingService.UnassignPermissionSchemaRole(userService,
					accessControlManager,
					user,
					roleId);
			}

			foreach (var roleId in roleIds.Where(x => !userRoles.Contains(x)))
			{
				if (!licensingService.AssignPermissionSchemaRole(userService,
						accessControlManager,
						user,
						roleId))
				{
					var roleName = accessControlManager.FindPermissionSchemaRole(roleId).Name;
					ModelState.AddModelError(nameof(parameters), resourceManager.GetTranslation("CouldNotAssignRoleInsufficientLicense", userService?.CurrentUser?.DefaultLanguageKey).WithArgs(roleName));
				}
			}

			if (ModelState.ErrorCount > 0)
			{
				return ValidationProblem(ModelState);
			}

			var savedRestEntity = mapper.Map<User, UserRest>(user);
			return StatusCode((int)HttpStatusCode.OK, savedRestEntity);
		}

		[RequiredPermission(MainPlugin.PermissionName.EditUser, Group = PermissionGroup.UserAdmin)]
		[HttpPost, HttpPut]
		public virtual IActionResult SetUsergroups(ODataActionParameters parameters)
		{
			var rest = parameters.GetValue<UserRest>(ParameterUser);
			var user = userService.GetUser(rest.Id);

			if (parameters.GetOptionalValue<IEnumerable<Guid>>(ParameterUsergroupIds, out var usergroupIds))
			{
				user.RemoveUsergroups(user.Usergroups);
				user.AddUsergroups(usergroupIds.Select(x => usergroupService.GetUsergroup(x)));
			}
			else if (parameters.GetOptionalValue<IEnumerable<string>>(ParameterUsergroupNames, out var usergroupNames))
			{
				user.RemoveUsergroups(user.Usergroups);
				user.AddUsergroups(usergroupNames.Select(x => usergroupService.GetUsergroup(x)));
			}

			var savedRestEntity = mapper.Map<User, UserRest>(user);
			return StatusCode((int)HttpStatusCode.OK, savedRestEntity);
		}
	}
}
