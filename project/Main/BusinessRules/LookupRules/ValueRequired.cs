namespace Main.BusinessRules.LookupRules
{
	using System.Reflection;

	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Validation.BaseRules;

	public class ValueRequired : RequiredRule<ILookup>
	{
		public ValueRequired()
		{
			Init(x => x.Value);
		}
		protected override bool IsIgnoredFor(ILookup entity)
		{
			var attribute = entity.GetType().GetCustomAttribute<LookupAttribute>();
			return attribute == null || attribute.IgnoreValidation || base.IsIgnoredFor(entity);
		}
	}
}
