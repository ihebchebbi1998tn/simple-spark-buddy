namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140729160300)]
	public class AddColumnServiceObjectIdToSmsServiceContract : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceContract", new Column("ServiceObjectId", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}