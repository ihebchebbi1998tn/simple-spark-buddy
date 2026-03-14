namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131111162212)]
	public class AddColumnServiceObjectIdToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("ServiceObjectId", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}