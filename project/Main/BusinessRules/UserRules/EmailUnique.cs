namespace Main.BusinessRules.UserRules
{
	using System.Linq;

	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;

	public class EmailUnique : Rule<User>
	{
		private readonly IUserService userService;

		// Methods
		public override bool IsSatisfiedBy(User user)
		{
			if (user.Email.IsNullOrEmpty())
			{
				return true;
			}

			return !userService.GetUsersQuery().Any(x => x.Email == user.Email && x.Id != user.Id);
		}

		protected override RuleViolation CreateRuleViolation(User user)
		{
			return RuleViolation(user, u => u.Email);
		}

		// Constructor
		public EmailUnique(IUserService userService)
			: base(RuleClass.Unique)
		{
			this.userService = userService;
		}
	}
}
