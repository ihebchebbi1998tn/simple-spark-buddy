namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Model;

	public class UserRecentPagesMap : EntityClassMapping<RecentPage>
	{
		public UserRecentPagesMap()
		{
			Schema("CRM");
			Table("UserRecentPages");

			Id(x => x.Id, map =>
			{
				map.Column("Id");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});
			Property(x => x.Url, m =>
			{
				m.Update(false);
			});
			Property(x => x.Username, m =>
			{
				m.Update(false);
			});
			Property(x => x.Title);
			Property(x => x.Count);
			Property(x => x.Category);
		}
	}
}
