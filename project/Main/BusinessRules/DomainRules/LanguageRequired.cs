namespace Main.BusinessRules.DomainRules
{
	using Crm.Library.Model.Site;
	using Crm.Library.Validation.BaseRules;

	public class LanguageRequired : RequiredRule<DomainExtension>
	{
		public LanguageRequired()
		{
			Init(x => x.DefaultLanguageKey);
		}
	}
}
