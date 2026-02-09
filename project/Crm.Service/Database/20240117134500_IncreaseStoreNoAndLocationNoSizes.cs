namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240117134500)]
	public class IncreaseStoreNoAndLocationNoSizes : Migration
	{
		public override void Up()
		{
			IncreaseStoreNoAndLocation("SMS",
				"ServiceOrderMaterial",
				"FromWarehouse");
			IncreaseStoreNoAndLocation("SMS",
				"ServiceOrderMaterial",
				"FromLocationNo");
			IncreaseStoreNoAndLocation("SMS",
				"ServiceOrderMaterial",
				"ToWarehouse");
			IncreaseStoreNoAndLocation("SMS",
				"ServiceOrderMaterial",
				"ToLocationNo");

			IncreaseStoreNoAndLocation("SMS",
				"ServiceOrderHead",
				"FromWarehouse");
			IncreaseStoreNoAndLocation("SMS",
				"ServiceOrderHead",
				"FromLocationNo");
			IncreaseStoreNoAndLocation("SMS",
				"ServiceOrderHead",
				"ToWarehouse");
			IncreaseStoreNoAndLocation("SMS",
				"ServiceOrderHead",
				"ToLocationNo");
		}

		private void IncreaseStoreNoAndLocation(string schema, string table, string col)
		{
			var tableName = $"[{schema}].[{table}]";
			if (Database.TableExists(tableName) && Database.ColumnExists(tableName, col))
			{
				var colLength = (int)Database.ExecuteScalar(
					$"SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{schema}' AND TABLE_NAME = '{table}' AND COLUMN_NAME = '{col}'");
				if (colLength < 50)
				{
					Database.ChangeColumn(tableName,
						new Column(col,
							DbType.String,
							50,
							ColumnProperty.Null));
				}
			}
		}
	}
}
