namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201102143000)]
	public class QuantityUnitKeyLength : Migration
	{
		public override void Up()
		{
			const string schema = "SMS";
			const string table = "ServiceOrderMaterial";
			const string column = "QuantityUnit";

			var length = (int)Database.ExecuteScalar(
				$@"	SELECT CHARACTER_MAXIMUM_LENGTH
						FROM INFORMATION_SCHEMA.COLUMNS
						WHERE TABLE_SCHEMA = '{schema}' AND
							TABLE_NAME = '{table}' AND
							COLUMN_NAME = '{column}'");
			if (length < 20)
			{
				Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [{column}] NVARCHAR(20) NOT NULL");
			}
		}
	}
}