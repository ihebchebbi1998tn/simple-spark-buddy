namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140612171000)]
	public class AlterSmsServiceOrderMaterialSetFromWarehouseToNullable : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[SMS].[ServiceOrderMaterial]", new Column("FromWarehouse", DbType.String, 20, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}