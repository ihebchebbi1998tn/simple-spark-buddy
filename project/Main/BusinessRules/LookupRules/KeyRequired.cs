namespace Main.BusinessRules.LookupRules
{
	using System.Reflection;

	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Validation.BaseRules;

	public class KeyRequired : RequiredRule<ILookup>
	{
		public KeyRequired()
		{
			Init(x => x.Key);
		}
		protected override bool IsIgnoredFor(ILookup entity)
		{
			if (entity.Key.GetType().IsValueType && entity.Key.EqualsDefault())
				return true;
			var attribute = entity.GetType().GetCustomAttribute<LookupAttribute>();
			return attribute == null || attribute.IgnoreValidation || base.IsIgnoredFor(entity);
		}
	}
}
