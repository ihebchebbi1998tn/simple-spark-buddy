namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140612171001)]
	public class AlterSmsServiceOrderMaterialSetFromLocationNoToNullable : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[SMS].[ServiceOrderMaterial]", new Column("FromLocationNo", DbType.String, 20, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}