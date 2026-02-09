namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140903101700)]
	public class AddColumnInitiatorPersonIdToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("InitiatorPersonId", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}