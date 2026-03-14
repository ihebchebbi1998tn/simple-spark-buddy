namespace Sms.Scheduler.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class ProfileResourceMap : EntityClassMapping<ProfileResource>
	{
		public ProfileResourceMap()
		{
			Schema("RPL");
			Table("ProfileResource");

			Id(x => x.Id, map =>
			{
				map.Column("Id");
				map.Generator(Generators.Identity);
			});
			Property(x => x.ProfileKey);
			Property(x => x.ResourceKey);
			Property(x => x.SortOrder);
		}
	}
}
