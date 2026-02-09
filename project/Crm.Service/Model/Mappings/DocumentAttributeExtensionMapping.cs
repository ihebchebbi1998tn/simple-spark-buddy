namespace Crm.Service.Model.Mappings
{
	using Crm.Model;

	using LMobile.Unicore.NHibernate;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class DocumentAttributeExtensionMapping : ComponentMapping<DocumentAttributeExtension>, INHibernateMappingExtension<DocumentAttribute, DocumentAttributeExtension>
	{
		public DocumentAttributeExtensionMapping()
		{
			Property(x => x.DispatchId);
			Property(x => x.ServiceOrderMaterialId);
			Property(x => x.ServiceOrderTimeId);
			Property(x => x.ServiceOrderTimePosNo, m =>
			{
				m.Formula("(SELECT T.PosNo FROM SMS.ServiceOrderTimes T WHERE T.id = ServiceOrderTimeId)");
				m.Lazy(true);
			});
			ManyToOne(x => x.ServiceOrderTime, m =>
			{
				m.Column("ServiceOrderTimeId");
				m.Fetch(FetchKind.Select);
				m.Update(false);
				m.Insert(false);
				m.Cascade(Cascade.None);
				m.Lazy(LazyRelation.Proxy);
			});
			ManyToOne(x => x.ServiceOrderDispatch, m =>
			{
				m.Column("DispatchId");
				m.Fetch(FetchKind.Select);
				m.Update(false);
				m.Insert(false);
				m.Cascade(Cascade.None);
				m.Lazy(LazyRelation.Proxy);
			});
		}
	}
}
