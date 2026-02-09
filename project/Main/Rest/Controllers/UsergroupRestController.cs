namespace Main.Rest.Controllers
{
	using System;
	using System.Linq;

	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class UsergroupRestController : RestController<Usergroup>
	{
		private readonly IUsergroupService usergroupService;
		private readonly IUserService userService;
		private readonly IRuleValidationService ruleValidationService;
		private readonly IResourceManager resourceManager;

		// Methods
		[RequiredRole(RoleName.Administrator)]
		public virtual ActionResult Create(Usergroup usergroup)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(usergroup);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			usergroupService.SaveUsergroup(usergroup);
			return Rest(usergroup.Id.ToString());
		}
		
		[RequiredRole(RoleName.Administrator)]
		public virtual ActionResult Update(Usergroup usergroup)
		{
			usergroupService.SaveUsergroup(usergroup);
			return new EmptyResult();
		}

		[RequiredRole(RoleName.Administrator)]
		public virtual ActionResult List()
		{
			var usergroups = usergroupService.GetUsergroups().OrderBy(x => x.Name).ToList();
			return Rest(usergroups, "Usergroups");
		}

		public virtual ActionResult GetUsers(Guid id, bool? withEverybodyUser, bool? withEmptyUser)
		{
			var users = userService.GetUsersByUsergroupId(id).OrderBy(x => x.DisplayName).ToList();
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

		// Constructor
		public UsergroupRestController(IUsergroupService usergroupService, IUserService userService, IRuleValidationService ruleValidationService, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.usergroupService = usergroupService;
			this.userService = userService;
			this.ruleValidationService = ruleValidationService;
			this.resourceManager = resourceManager;
		}
	}
}
