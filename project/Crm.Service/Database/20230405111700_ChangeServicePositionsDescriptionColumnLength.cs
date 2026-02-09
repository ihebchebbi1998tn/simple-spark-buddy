namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230405111700)]
	public class ChangeServicePositionsDescriptionColumnLength : Migration
	{
		public override void Up()
		{
			const string schema = "SMS";
			string[] tableNames = { "ServiceOrderMaterial", "ServiceOrderTimes" };
			const string column = "Description";
			const int newValue = 500;

			foreach (var table in tableNames)
			{
				if (Database.ColumnExists($"[{schema}].[{table}]", column))
				{
					var materialDescriptionLength = (int)Database.ExecuteScalar(
						@$"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = '{schema}' AND
								TABLE_NAME = '{table}' AND
								COLUMN_NAME = '{column}'");

					if (materialDescriptionLength < newValue)
						Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [{column}] NVARCHAR({newValue}) {(table == "ServiceOrderTimes" ? "" : "NOT")} NULL");
				}
			}
		}
	}
}