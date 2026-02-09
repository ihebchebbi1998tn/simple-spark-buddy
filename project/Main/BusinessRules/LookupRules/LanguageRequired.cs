namespace Main.BusinessRules.LookupRules
{
	using System.Reflection;

	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Validation.BaseRules;

	public class LanguageRequired : RequiredRule<ILookup>
	{
		public LanguageRequired()
		{
			Init(x => x.Language);
		}
		protected override bool IsIgnoredFor(ILookup entity)
		{
			var attribute = entity.GetType().GetCustomAttribute<LookupAttribute>();
			return attribute == null || attribute.IgnoreValidation || base.IsIgnoredFor(entity);
		}
	}
}
