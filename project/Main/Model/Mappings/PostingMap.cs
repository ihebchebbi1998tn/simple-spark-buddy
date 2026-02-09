namespace Main.Model.Mappings
{
	using Crm.Library.Model;

	using NHibernate.Mapping.ByCode.Conformist;

	public class PostingMap : SubclassMapping<Posting>
	{
		public PostingMap()
		{
			DiscriminatorValue(PostingCategory.Posting);
		}
	}
}
