namespace Main.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class UserAssetConfiguration : EntityConfiguration<UserAsset>
	{
		public UserAssetConfiguration(IEntityConfigurationHolder<UserAsset> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{
			Property(x => x.Asset, c =>
			{
				c.Filterable();
			});
			Property(x => x.ValidTo, c =>
			{
				c.Sortable();
				c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = true, AllowPastDates = true }));
			});
		}
	}
}
