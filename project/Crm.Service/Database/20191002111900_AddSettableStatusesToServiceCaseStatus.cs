namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20191002111900)]
	public class AddSettableStatusesToServiceCaseStatus : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceNotificationStatus", new Column("SettableStatuses", DbType.String, ColumnProperty.Null));
		}
	}
}