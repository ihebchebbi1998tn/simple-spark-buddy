namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131218101515)]
	public class AddCausingItemColumnsToSmsServiceOrderTimes : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimes]", new Column("CausingItemNo", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimes]", new Column("CausingItemSerialNo", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimes]", new Column("CausingItemPreviousSerialNo", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimes]", new Column("NoCausingItemSerialNoReason", DbType.String, 20, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimes]", new Column("NoCausingItemPreviousSerialNoReason", DbType.String, 20, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}