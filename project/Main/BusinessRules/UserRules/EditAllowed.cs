namespace Main.BusinessRules.UserRules;

using System;
using System.Linq;

using Crm.Library.Globalization.Resource;
using Crm.Library.Model;
using Crm.Library.Model.Authorization;
using Crm.Library.Model.Authorization.Interfaces;
using Crm.Library.Services.Interfaces;
using Crm.Library.Validation;

public class EditAllowed : Rule<User>
{
	private readonly IUserService userService;
	private readonly IAuthorizationManager authorizationManager;

	public override bool IsSatisfiedBy(User user)
	{
		var isUserCreation = user.UserId == Guid.Empty || userService.GetUsersQuery().FirstOrDefault(x => x.UserId == user.UserId) == null;
		if (isUserCreation)
		{
			return true;
		}

		var isCurrentUser = userService.CurrentUser.Id == user.Id;
		var isAuthorizedToEdit = authorizationManager.IsAuthorizedForAction(userService.CurrentUser, PermissionGroup.UserAdmin, MainPlugin.PermissionName.EditUser);

		return isCurrentUser || isAuthorizedToEdit;
	}

	public override string GetTranslatedErrorMessage(IResourceManager resourceManager) => resourceManager.GetTranslation(nameof(EditAllowed));

	protected override RuleViolation CreateRuleViolation(User user)
	{
		return new RuleViolation(nameof(EditAllowed));
	}

	public EditAllowed(IUserService userService, IAuthorizationManager authorizationManager)
		: base(RuleClass.Undefined)
	{
		this.userService = userService;
		this.authorizationManager = authorizationManager;
	}
}
