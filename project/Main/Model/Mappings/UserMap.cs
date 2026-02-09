namespace Main.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Model;

	using NHibernate.Mapping.ByCode;

	public class UserMap : EntityClassMapping<User>
	{
		public UserMap()
		{
			Cache(x =>
			{
				x.Include(CacheInclude.All);
				x.Usage(CacheUsage.ReadWrite);
			});
			Schema("CRM");
			// Use db independent `instead of [], Nhibernate will convert it to the correct escaping
			// http://stackoverflow.com/questions/679279/nhibernate-force-escaping-on-table-names
			Table("`User`");

			// TODO: use ComposedId with NHibernate 3.3? (broken in 3.2: https://nhibernate.jira.com/browse/NH-2989)
			Id(x => x.Id, map =>
			{
				map.Column("Username");
				map.Generator(Generators.Assigned);
			});
			Property(x => x.UserId, map => map.Column("UserID"));
			Property(x => x.Avatar, map => map.Length(4194304));
			Property(x => x.DefaultLanguageKey, map => map.Column("DefaultLanguage"));
			Property(x => x.DefaultLocale);
			Property(x => x.GeneralToken);
			Property(x => x.AdName);
			Property(x => x.Discharged);
			Property(x => x.AppleDeviceToken);
			Property(x => x.FirstName);
			Property(x => x.LastName);
			Property(x => x.Email);
			Property(x => x.Remark);
			Property(x => x.Password);
			Property(x => x.PasswordChangeRequired);
			Property(x => x.PersonnelId);
			Property(x => x.StatusKey);
			Property(x => x.StatusMessage);
			Property(x => x.LastStatusUpdate);
			Property(x => x.LastLoginDate);
			Property(x => x.TimeZoneName);
			Property(x => x.Latitude);
			Property(x => x.Longitude);
			Property(x => x.MasterRecordNo);
			Property(x => x.IdentificationNo);
			Property(x => x.TravelTimeToBranchInMinutes);
			Property(x => x.DischargeDate);
			Property(x => x.LicensedAt);
			Property(x => x.OpenIdIdentifier);
			Property(x => x.TotpAuthorizationKey);

			Set(x => x.RecentPages, map =>
			{
				map.Schema("CRM");
				map.Table("UserRecentPages");
				map.Key(km => km.Column("Username"));
				map.Fetch(CollectionFetchMode.Select);
				map.BatchSize(10);
				map.Inverse(true);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.All.Include(Cascade.Persist));
			}, action => action.OneToMany());

			Set(x => x.Roles, map =>
			{
				map.Schema("dbo");
				map.Table("GrantedRole");
				map.Key(km =>
				{
					km.Column("UserId");
					km.PropertyRef(x => x.UserId);
				});
				map.Inverse(true);
				map.Fetch(CollectionFetchMode.Select);
				map.BatchSize(100);
				map.Cascade(Cascade.All.Include(Cascade.Persist));
			}, action => action.ManyToMany(map => map.Column("RoleId")));

			Set(x => x.Usergroups, map =>
			{
				map.Schema("CRM");
				map.Table("UserUserGroup");
				map.Key(km => km.Column("Username"));
				map.Fetch(CollectionFetchMode.Select);
				map.BatchSize(100);
				map.Cascade(Cascade.All.Include(Cascade.Persist));
			}, action => action.ManyToMany(map => map.Column("UserGroupKey")));
		}
	}
}
