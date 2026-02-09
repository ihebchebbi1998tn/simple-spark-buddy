namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240607115500)]
	public class AddProfileIdToUser : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[User]", new Column("ActiveProfileId", DbType.Int32, ColumnProperty.Null));
		}
	}
}
