namespace Main.Services
{
	using System.Collections.Generic;

	using Crm.Library.Model;
	using Crm.Library.Validation;

	using Main.Services.Interfaces;

	public class LicenseLoginValidator : ILoginValidator
	{
		public virtual IEnumerable<RuleViolation> GetRuleViolations(User user)
		{
			if (user != null)
			{
				if (user.LicensedAt == null)
				{
					yield return new RuleViolation("AccountNotLicensed");
				}
			}
		}
	}
}
