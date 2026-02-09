namespace Main.BusinessRules.DomainRules
{
	using Crm.Library.Model.Site;
	using Crm.Library.Validation.BaseRules;

	public class HostFormat : WebsiteRule<DomainExtension>
	{
		public HostFormat()
		{
			Init(x => x.Host);
		}
	}
}
