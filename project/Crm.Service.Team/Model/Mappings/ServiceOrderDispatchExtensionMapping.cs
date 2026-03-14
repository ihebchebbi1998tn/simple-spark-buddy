namespace Crm.Service.Team.Model.Mappings
{
	using Crm.Service.Model;

	using LMobile.Unicore.NHibernate;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class ServiceOrderDispatchExtensionMapping : ComponentMapping<ServiceOrderDispatchExtension>, INHibernateMappingExtension<ServiceOrderDispatch, ServiceOrderDispatchExtension>
	{
		public ServiceOrderDispatchExtensionMapping()
		{
			Property(x => x.TeamId);
			ManyToOne(x => x.Team, m =>
			{
				m.Column("TeamId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
		}
	}
}
