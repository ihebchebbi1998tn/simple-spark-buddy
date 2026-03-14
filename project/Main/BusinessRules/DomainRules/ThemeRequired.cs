namespace Main.BusinessRules.DomainRules
{
	using Crm.Library.Model.Site;
	using Crm.Library.Validation.BaseRules;

	public class ThemeRequired : RequiredRule<DomainExtension>
	{
		public ThemeRequired()
		{
			Init(x => x.Theme);
		}
	}
}
