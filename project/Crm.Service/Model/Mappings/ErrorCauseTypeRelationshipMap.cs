namespace Crm.Service.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ErrorCauseTypeRelationshipMap : EntityClassMapping<ErrorCauseTypeRelationship>
	{
		public ErrorCauseTypeRelationshipMap()
		{
			Schema("SMS");
			Table("ErrorCauseTypeRelationship");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("ErrorCauseTypeRelationshipId");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});
			Property(x => x.StatisticsKeyCauseKey);
			Property(x => x.ErrorTypeKey);
			ManyToOne(x => x.StatisticsKeyCause, map =>
			{
				map.Column("StatisticsKeyCauseKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
		}
	}
}
