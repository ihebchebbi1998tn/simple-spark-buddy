namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	using GuidCombGeneratorDef = LMobile.Unicore.NHibernate.GuidCombGeneratorDef;

	public class ServiceOrderDispatchReportRecipientMap : EntityClassMapping<ServiceOrderDispatchReportRecipient>
	{
		public ServiceOrderDispatchReportRecipientMap()
		{
			Schema("SMS");
			Table("ServiceOrderDispatchReportRecipient");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("ServiceOrderDispatchReportRecipientId");
					map.Generator(GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.Email);
			Property(x => x.Language);
			Property(x => x.Locale);
			Property(x => x.DispatchId);
			ManyToOne(
				x => x.Dispatch,
				m =>
				{
					m.Column("DispatchId");
					m.Insert(false);
					m.Update(false);
					m.Fetch(FetchKind.Select);
					m.Lazy(LazyRelation.Proxy);
				});
		}
	}
}