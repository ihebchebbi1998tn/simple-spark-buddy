namespace Main.BusinessRules.DomainRules
{
	using System.Linq;

	using Crm.Library.Validation;

	using LMobile.Unicore;

	public class NameUnique : Rule<Domain>
	{
		private readonly IDomainManager domainManager;

		public override bool IsSatisfiedBy(Domain domain)
		{
			return !domainManager.Query().Any(x => x.Name == domain.Name && x.UId != domain.UId);
		}

		protected override RuleViolation CreateRuleViolation(Domain domain)
		{
			return RuleViolation(domain, x => x.Name);
		}

		public NameUnique(IDomainManager domainManager)
			: base(RuleClass.Unique)
		{
			this.domainManager = domainManager;
		}
	}
}
