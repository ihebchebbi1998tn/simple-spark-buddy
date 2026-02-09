namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Model;

	public class InstallationConfiguration : EntityConfiguration<Installation>
	{
		public override void Initialize()
		{
			NestedProperty(x => x.LocationAddress.ZipCode, c => c.Filterable());
			NestedProperty(x => x.LocationAddress.City, c => c.Filterable());
			NestedProperty(x => x.LocationAddress.Street, c => c.Filterable());

			Property(x => x.InstallationNo, c =>
				{
					c.Filterable();
					c.Sortable();
				});
			Property(x => x.Description, c => c.Filterable());
			Property(x => x.LegacyInstallationId, c => c.Filterable());
			Property(x => x.ExternalReference, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.InstallationType, c => c.Filterable());
			Property(x => x.StatisticsKeyProductType, c => c.Filterable());
			Property(x => x.Status, c => { c.Filterable(); });
			Property(x => x.StatusKey, c => { c.Sortable(); });
			Property(x => x.WarrantyFrom, c => { c.Sortable(); });
			Property(x => x.WarrantyUntil, c => { c.Sortable(); });
			Property(x => x.ResponsibleUser, c => c.Filterable(f => f.Definition(new UserFilterDefinition { WithGroups = true })));
			Property(x => x.LocationContactId, m =>
			{
				m.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<Company>("CompanyAutocomplete",
					new { Plugin = "Crm" },
					"Crm_Company",
					"Helper.Company.getDisplayName",
					x => x.Id,
					x => x.LegacyId,
					x => x.Name)));
				m.Sortable();
			});
			Property(x => x.FolderId, c => c.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<ServiceObject>("ContactAutocomplete", new { Plugin = "Crm", contactType = "ServiceObject" }, "CrmService_ServiceObject", "Helper.ServiceObject.getDisplayName", x => x.Id, x => x.ObjectNo, x => x.Name) { Caption = "ServiceObject" })));
			NestedProperty(x => x.LocationAddress.Latitude, c => c.Filterable(f => f.Definition(new GeoCoordinateFilterDefinition())));
			NestedProperty(x => x.LocationAddress.Longitude, c => c.Filterable(f => f.Definition(new GeoCoordinateFilterDefinition())));
			NestedProperty(x => x.Article.ItemNo, c => c.Filterable());
		}
		public InstallationConfiguration(IEntityConfigurationHolder<Installation> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
