namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210921173000)]
	public class AddIsExportedReplenishmentOrder : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("SMS.ReplenishmentOrder", "IsExported"))
			{
				return;
			}
			Database.AddColumn("SMS.ReplenishmentOrder", new Column("IsExported", DbType.Boolean, ColumnProperty.NotNull, false));
			Database.ExecuteNonQuery("UPDATE [SMS].[ReplenishmentOrder] SET IsExported = 1, ModifyDate = GETUTCDATE(), ModifyUser = 'migration_20210921173000' WHERE IsClosed = 1 AND IsActive = 1");
		}
	}
}
