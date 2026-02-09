namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class UserSkillMap : EntityClassMapping<UserSkill>
	{
		public UserSkillMap()
		{
			Schema("CRM");
			Table("UserSkill");

			Id(a => a.Id, m =>
			{
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.Username);
			ManyToOne(x => x.User, map =>
			{
				map.Column("Username");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
			});
			Property(x => x.SkillKey);
			Property(x => x.ValidFrom);
			Property(x => x.ValidTo);
			Property(x => x.DaysToNotifyBeforeExpiration);
		}
	}
}
