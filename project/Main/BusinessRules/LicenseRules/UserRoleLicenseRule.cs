namespace Main.BusinessRules.LicenseRules
{
	using Crm.Library.Licensing;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;

	using LMobile.Unicore;

	using User = Crm.Library.Model.User;

	public class UserRoleLicenseRule : Rule<User>
	{
		private readonly ILicensingService licensingService;
		private readonly IAccessControlManager accessControlManager;
		private readonly IUserService userService;
		
		public override bool IsSatisfiedBy(User user)
		{
			if (user.Discharged || user.LicensedAt == null)
			{
				return true;
			}
			
			return licensingService.CheckAndUpdateRoleLicenses(accessControlManager, userService, user);
		}


		protected override RuleViolation CreateRuleViolation(User user)
		{
			var options = new RuleViolationOptions
			{
				Entity = user,
				PropertyName = "Roles",
				ErrorMessageKey = "CouldNotActivateUserBecauseRoleInsufficientLicense",
				Rule = this,
				RuleClass = RuleClass
			};

			return RuleViolation(options);
		}

		public UserRoleLicenseRule(ILicensingService licensingService, IAccessControlManager accessControlManager, IUserService userService)
			: base(RuleClass.Undefined)
		{
			this.licensingService = licensingService;
			this.accessControlManager = accessControlManager;
			this.userService = userService;
		}
	}
}
