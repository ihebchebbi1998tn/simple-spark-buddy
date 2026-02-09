namespace Main.BusinessRules.DomainRules
{
	using Crm.Library.Validation.BaseRules;

	using LMobile.Unicore;

	public class NameRequired : RequiredRule<Domain>
	{
		public NameRequired()
		{
			Init(x => x.Name);
		}
	}
}
