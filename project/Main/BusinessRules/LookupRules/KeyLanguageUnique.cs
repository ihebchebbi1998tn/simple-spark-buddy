namespace Main.BusinessRules.LookupRules
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Validation;

	public class KeyLanguageUnique : Rule<ILookup>
	{
		private readonly ILookupService lookupService;
		public KeyLanguageUnique(ILookupService lookupService) : base(RuleClass.Unique)
		{
			this.lookupService = lookupService;
		}
		protected override RuleViolation CreateRuleViolation(ILookup entity)
		{
			return new RuleViolation(entity, nameof(ILookup.Key), ruleClass: RuleClass.Unique);
		}
		public override bool IsSatisfiedBy(ILookup entity)
		{
			IEnumerable<ILookup> languages = lookupService
				.GetLookupQuery(entity.GetType())
				.Where(x => x.Id != entity.Id && x.Key == entity.Key && x.Language == entity.Language);
			if (entity is Lookup<string>)
			{
				languages = languages.AsEnumerable().Where(x => string.Equals((string)x.Key, (string)entity.Key, System.StringComparison.Ordinal));
			}
			return languages.Count() == 0;
		}
		protected override bool IsIgnoredFor(ILookup entity)
		{
			var attribute = entity.GetType().GetCustomAttribute<LookupAttribute>();
			return attribute == null || attribute.IgnoreValidation || base.IsIgnoredFor(entity);
		}
	}
}
