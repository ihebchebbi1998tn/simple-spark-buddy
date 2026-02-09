namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131111162211)]
	public class AddColumnPurchaseDateToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("PurchaseDate", DbType.DateTime, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}