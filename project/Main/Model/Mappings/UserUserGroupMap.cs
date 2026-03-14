namespace Main.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Model;

	using NHibernate.Mapping.ByCode;
	public class UserUserGroupMap : EntityClassMapping<UserUserGroup>
	{
		public UserUserGroupMap()
		{
			Schema("CRM");
			Table("UserUserGroup");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("Id");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});
			Property(x => x.UserGroupKey);
			Property(x => x.Username);
			ManyToOne(x => x.User, m =>
			{
				m.Column("Username");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			ManyToOne(x => x.UserGroup, m =>
			{
				m.Column("UserGroupKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
		}
	}
}
