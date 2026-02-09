namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131101110525)]
	public class AddColumnInitiatorIdToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("InitiatorId", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}