namespace Main.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class MessageConfiguration : EntityConfiguration<Message>
	{
		public MessageConfiguration(IEntityConfigurationHolder<Message> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{
			Property(x => x.CreateDate, m =>
			{
				m.Filterable(f => f.Definition(new DateFilterDefinition { AllowPastDates = true, AllowFutureDates = false }));
				m.Sortable();
			});
			Property(x => x.ErrorMessage, m => m.Filterable());
			Property(x => x.Subject, m => m.Filterable());
		}
	}
}
