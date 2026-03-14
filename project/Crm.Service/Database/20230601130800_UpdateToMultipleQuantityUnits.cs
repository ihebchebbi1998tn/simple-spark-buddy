namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230601130800)]
	public class UpdateToMultipleQuantityUnits : Migration
	{
		public override void Up()
		{
			UpdateTable("ReplenishmentOrderItem");
			UpdateTable("ServiceOrderMaterial");
			UpdateTable("InstallationPos");
		}

		private void UpdateTable(string table)
		{
			Database.AddColumnIfNotExisting($"SMS.{table}",
				new Column("QuantityUnitEntryKey",
					DbType.Guid,
					ColumnProperty.Null));
			Database.ExecuteNonQuery($@"
  ALTER TABLE SMS.{table}
ADD FOREIGN KEY (QuantityUnitEntryKey) REFERENCES CRM.QuantityUnitEntry(QuantityUnitEntryId);");
		}
	}
}
