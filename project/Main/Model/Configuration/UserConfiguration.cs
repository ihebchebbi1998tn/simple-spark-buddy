namespace Main.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Model;

	using Main.Model.Lookups;

	using User = Crm.Library.Model.User;

	public class UserConfiguration : EntityConfiguration<User>
	{
		public override void Initialize()
		{
			Property(x => x.FirstName, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.LastName, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.Usergroups, c => c.Filterable(f => f.Definition(new CollectionAutoCompleterFilterDefinition<Usergroup, Usergroup>(x => x.Id, new AutoCompleterFilterDefinition<Usergroup>(null, null, "Main_Usergroup", x => x.Name, x => x.Id, "Helper.User.filterUsergroupQuery")) { Caption = "Usergroup",  })));
			Property(x => x.Email, c => c.Filterable());
			Property(x => x.PersonnelId, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.IdentificationNo, f => f.Filterable());
			Property(x => x.AdName, f => f.Filterable());
			Property(x => x.OpenIdIdentifier, f => f.Filterable());
			Property(x => x.DefaultLanguageKey, f => f.Filterable(c => c.Definition(new TypeFilterDefinition(typeof(Language)))));
			Property(x => x.LastLoginDate, c => c.Sortable(s => s.SortCaption("LastLoginLabel")));
		}
		public UserConfiguration(IEntityConfigurationHolder<User> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
