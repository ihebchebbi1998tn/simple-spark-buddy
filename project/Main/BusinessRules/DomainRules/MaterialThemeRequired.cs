namespace Main.BusinessRules.DomainRules
{
	using Crm.Library.Model.Site;
	using Crm.Library.Validation.BaseRules;

	public class MaterialThemeRequired : RequiredRule<DomainExtension>
	{
		public MaterialThemeRequired()
		{
			Init(x => x.MaterialTheme);
		}
	}
}
