namespace Main.Model.Mappings
{
	using Crm.Library.Model.Site;

	using LMobile.Unicore;
	using LMobile.Unicore.NHibernate;

	using NHibernate.Mapping.ByCode.Conformist;

	public class GDPRDomainExtensionMapping : ComponentMapping<GDPRDomainExtension>, INHibernateMappingExtension<Domain, GDPRDomainExtension>
	{
		public GDPRDomainExtensionMapping()
		{
			Property(x => x.CompanyName);
			Property(x => x.DataProtectionOfficer);
			Property(x => x.ResponsibleAddress);
		}
	}
}
