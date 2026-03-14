namespace Main.Model.Configuration;

using Crm.Library.EntityConfiguration;
using Crm.Library.Model;

public class UserGroupConfiguration : EntityConfiguration<Usergroup>
{
	public UserGroupConfiguration(IEntityConfigurationHolder<Usergroup> entityConfigurationHolder)
		: base(entityConfigurationHolder)
	{
	}
	public override void Initialize()
	{
		Property(x => x.Name, c =>
		{
			c.Filterable();
			c.Sortable();
		});
	}
}
