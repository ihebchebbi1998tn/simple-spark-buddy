namespace Main.Services.Interfaces
{
	using System.Collections.Generic;

	using Crm.Library.AutoFac;
	using Crm.Library.Model;
	using Crm.Library.Validation;

	public interface ILoginValidator : IDependency
	{
		IEnumerable<RuleViolation> GetRuleViolations(User user);
	}
}
