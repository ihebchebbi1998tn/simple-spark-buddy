using System;

using Crm.Library.Validation;
using Crm.Library.Validation.BaseRules;
using Crm.Service.Model;

namespace Crm.Service.BusinessRules.ServiceOrderTimePostingRules
{
	public class ToLessThanTomorrow : Rule<ServiceOrderTimePosting>
	{
		public ToLessThanTomorrow() : base(RuleClass.LessOrEqual) { }

		protected override bool IsIgnoredFor(ServiceOrderTimePosting timePosting) => !timePosting.To.HasValue;

		protected override RuleViolation CreateRuleViolation(ServiceOrderTimePosting timePosting) => RuleViolation(timePosting, t => t.ToAsString, t => t.To);
		public override bool IsSatisfiedBy(ServiceOrderTimePosting entity) => entity.To.Value.ToLocalTime() <= DateTime.Today.AddDays(1);
	}
}
