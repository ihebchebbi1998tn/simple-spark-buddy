namespace Main.Model.Configuration
{
	using System.Collections.Generic;

	using Crm.Library.EntityConfiguration;

	public class LogConfiguration : EntityConfiguration<Log>
	{
		public LogConfiguration(IEntityConfigurationHolder<Log> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{
			Property(x => x.Message, f => f.Filterable(c => c.Caption("Content")));
			Property(x => x.Exception, f => f.Filterable(c => c.Caption("Exception")));
			Property(x => x.CreateDate, f =>
			{
				f.Filterable(d => d.Definition(new DateFilterDefinition { AllowPastDates = true, AllowFutureDates = false, PickDateTime = true, KeepTimeZone = true }));
				f.Sortable();
			});
			var dropdownFilter = new DropDownFilterDefinition(new Dictionary<string, string> { { "DEBUG", "Debug" }, { "INFO", "Info" }, { "WARN", "Warning" }, { "ERROR", "Error" } });
			Property(x => x.Level, f => f.Filterable(d => d.Definition(dropdownFilter)));
			Property(x => x.Logger, f => f.Filterable());
		}
	}
}
