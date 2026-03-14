namespace Main.BusinessRules.SiteRules
{
	using Crm.Library.Model.Site;
	using Crm.Library.Validation.BaseRules;

	public class DataProtectionOfficerRequired : RequiredRule<GDPRDomainExtension>
	{
		public DataProtectionOfficerRequired()
		{
			Init(x => x.DataProtectionOfficer);
		}
	}
}
