namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20150310133901)]
	public class AddColumnIsActiveToSmsReplenishmentOrderItem : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ReplenishmentOrderItem", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, 1));
		}

		public override void Down()
		{
		}
	}
}