namespace Main.BusinessRules.SiteRules
{
	using Crm.Library.Model.Site;
	using Crm.Library.Validation.BaseRules;

	public class ResponsibleAddressRequired : RequiredRule<GDPRDomainExtension>
	{
		public ResponsibleAddressRequired()
		{
			Init(x => x.ResponsibleAddress, "DataProtectionResponsibleAddress");
		}
	}
}
