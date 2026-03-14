namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140715165800)]
	public class AlterSmsServiceOrderTimesSetDescriptionToNullable : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[SMS].[ServiceOrderTimes]", new Column("Description", DbType.String, 100, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}