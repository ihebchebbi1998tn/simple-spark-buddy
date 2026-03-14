namespace Crm.Service.Team.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Model;
	using Crm.Service.Model;

	public class ServiceOrderDispatchConfiguration : EntityExtensionConfiguration<ServiceOrderDispatchExtension, ServiceOrderDispatch>
	{
		public ServiceOrderDispatchConfiguration(IEntityConfigurationHolder<ServiceOrderDispatch> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{
			Property(x => x.TeamId,
				c => c.Filterable(f =>
				{
					f.Definition(new AutoCompleterFilterDefinition<Usergroup>(null,
						null,
						"Main_Usergroup",
						x => x.Name,
						x => x.Id,
						filterFunction: "Helper.User.filterUsergroupQueryForServiceTeams") { Caption = "Team" });
				}));
		}
	}
}
