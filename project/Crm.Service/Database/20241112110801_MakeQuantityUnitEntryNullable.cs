namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20241112110801)]
	public class MakeQuantityUnitEntryNullable : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE SMS.[ServiceOrderMaterial] ALTER COLUMN [QuantityUnitEntryKey] UNIQUEIDENTIFIER NULL");
			Database.ExecuteNonQuery("ALTER TABLE SMS.[ReplenishmentOrderItem] ALTER COLUMN [QuantityUnitEntryKey] UNIQUEIDENTIFIER NULL");
		}
	}
}
