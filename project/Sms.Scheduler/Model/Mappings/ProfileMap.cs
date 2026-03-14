namespace Sms.Scheduler.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ProfileMap : EntityClassMapping<Profile>
	{
		public ProfileMap()
		{
			Schema("RPL");
			Table("`Profile`");

			Id(x => x.Id, map =>
			{
				map.Column("Id");
				map.Generator(Generators.Identity);
			});
			Property(x => x.InternalId);
			Property(x => x.Username);
			Property(x => x.Name);
			Property(x => x.DefaultProfile);
			Property(x => x.TemplateKey);
			Property(x => x.ClientConfig);

			ManyToOne(x => x.User, map =>
			{
				map.Column("Username");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});

			Set(x => x.ResourceKeys, map =>
			{
				map.Schema("RPL");
				map.Table("ProfileResource");
				map.Key(rk => rk.Column("ProfileKey"));
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Persist);
				map.BatchSize(100);
				map.OrderBy("SortOrder");
			}, r => r.Element(m => m.Column("ResourceKey")));
		}
	}
}
