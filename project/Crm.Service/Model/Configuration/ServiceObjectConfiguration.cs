namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class ServiceObjectConfiguration : EntityConfiguration<ServiceObject>
	{
		public override void Initialize()
		{
			Property(x => x.Name, c => c.Filterable());
			NestedProperty(x => x.StandardAddress.Latitude, c => c.Filterable(f => f.Definition(new GeoCoordinateFilterDefinition())));
			NestedProperty(x => x.StandardAddress.Longitude, c => c.Filterable(f => f.Definition(new GeoCoordinateFilterDefinition())));
			NestedProperty(x => x.StandardAddress.ZipCode, c => c.Filterable());
			NestedProperty(x => x.StandardAddress.City, c => c.Filterable());
			NestedProperty(x => x.StandardAddress.Street, c => c.Filterable());
			NestedProperty(x => x.StandardAddress.Country, c => c.Filterable());
			Property(x => x.Category, c => c.Filterable());
			Property(x => x.ObjectNo, c => c.Filterable());
			//Property(x => x.Installations, c => c.Filterable(f => f.Definition(new CollectionFilterDefinition<Installation>(x => x.InstallationNo) { Caption = "InstallationNo".GetTranslation() })));
		}
		public ServiceObjectConfiguration(IEntityConfigurationHolder<ServiceObject> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
