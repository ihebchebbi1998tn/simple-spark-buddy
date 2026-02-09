namespace Crm.Service.Model.Mappings
{
	using Crm.Model.Notes;
	using Crm.Service.Model.Extensions;

	using LMobile.Unicore.NHibernate;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class NoteExtensionMapping : ComponentMapping<NoteExtensions>, INHibernateMappingExtension<Note, NoteExtensions>
	{
		public NoteExtensionMapping()
		{
			Property(x => x.DispatchId);
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
