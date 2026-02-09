namespace Main.BusinessRules.SiteRules
{
	using Crm.Library.Model.Site;
	using Crm.Library.Validation.BaseRules;

	public class CompanyNameRequired : RequiredRule<GDPRDomainExtension>
	{
		public CompanyNameRequired()
		{
			Init(x => x.CompanyName, "DataProtectionCompanyName");
		}
	}
}
