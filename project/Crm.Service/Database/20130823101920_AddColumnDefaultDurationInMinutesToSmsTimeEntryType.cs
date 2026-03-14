namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130823101920)]
	public class AddColumnDefaultDurationInMinutesToSmsTimeEntryType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.TimeEntryType", new Column("DefaultDurationInMinutes", DbType.Int32, ColumnProperty.Null));
		}
	}
}