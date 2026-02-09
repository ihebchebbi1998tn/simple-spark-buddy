namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140226100200)]
	public class AddColumnReplenishmentOrderItemIdToSmsServiceOrderMaterial : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("ReplenishmentOrderItemId", DbType.Int64, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}