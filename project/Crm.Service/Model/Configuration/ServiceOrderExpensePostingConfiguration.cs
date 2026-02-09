namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Helper;
	using Crm.PerDiem.Model.Lookups;


	public class ServiceOrderExpensePostingConfiguration : EntityConfiguration<ServiceOrderExpensePosting>
	{
		private readonly IAppSettingsProvider appSettingsProvider;

		public override void Initialize()
		{
			bool articlesAreUsed = articlesAreUsed = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceOrderExpensePosting.UseArticleAsExpenseType);
			if (articlesAreUsed)
			{
				Property(x => x.ItemNo, m => m.Filterable());
			}
			else
			{
				Property(x => x.ExpenseTypeKey, m => m.Filterable(f => f.Definition(new TypeFilterDefinition(typeof(ExpenseType)))));

			}
			Property(x => x.Description, m => m.Filterable());
			Property(x => x.ResponsibleUser, c => c.Filterable(f => f.Definition(new UserFilterDefinition { CustomFilterExpression = true, WithGroups = false, FilterForGroup = true })));
		}
		public ServiceOrderExpensePostingConfiguration(IEntityConfigurationHolder<ServiceOrderExpensePosting> entityConfigurationHolder, IAppSettingsProvider appSettingsProvider)
			: base(entityConfigurationHolder)
		{
			this.appSettingsProvider = appSettingsProvider;
		}
	}
}
