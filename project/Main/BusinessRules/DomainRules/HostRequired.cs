namespace Main.BusinessRules.DomainRules
{
	using Crm.Library.Model.Site;
	using Crm.Library.Validation.BaseRules;

	public class HostRequired : RequiredRule<DomainExtension>
	{
		public HostRequired()
		{
			Init(x => x.Host);
		}
	}
}
