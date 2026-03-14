namespace Main.BusinessRules.UsergroupRules
{
	using System.Linq;

	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;

	public class NameUnique : Rule<Usergroup>
	{
		private readonly IUsergroupService usergroupService;

		// Methods
		protected override bool IsIgnoredFor(Usergroup usergroup)
		{
			return usergroup.Name.IsNullOrEmpty();
		}

		public override bool IsSatisfiedBy(Usergroup usergroup)
		{
			return !usergroupService.GetUsergroupsQuery().Any(x => x.Name == usergroup.Name && x.Id != usergroup.Id);
		}

		protected override RuleViolation CreateRuleViolation(Usergroup usergroup)
		{
			return RuleViolation(usergroup, u => u.Name, "Usergroup");
		}

		// Constructor
		public NameUnique(IUsergroupService usergroupService)
			: base(RuleClass.Unique)
		{
			this.usergroupService = usergroupService;
		}
	}
}
