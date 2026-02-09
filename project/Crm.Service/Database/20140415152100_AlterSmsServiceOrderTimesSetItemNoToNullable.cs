namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140415152100)]
	public class AlterSmsServiceOrderTimesSetItemNoToNullable : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[SMS].[ServiceOrderTimes]", new Column("ItemNo", DbType.String, 50, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}