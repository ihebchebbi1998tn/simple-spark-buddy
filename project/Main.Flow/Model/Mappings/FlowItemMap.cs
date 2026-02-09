using Crm.Library.Model;
using NHibernate.Mapping.ByCode.Conformist;

namespace Main.Flow.Model.Mappings
{
	using NHibernate.Mapping.ByCode;

	public class FlowItemMap : SubclassMapping<FlowItem>
	{
		public FlowItemMap()
		{
			DiscriminatorValue(PostingCategory.FlowItem);
			Property(x => x.RuleKey);
			ManyToOne(x => x.Rule, map =>
			{
				map.Column("RuleKey");
				map.Fetch(FetchKind.Join);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
		}
	}
}
