namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230405114000)]
	public class ChangeServiceOrderHeadOrderNoColumnLength : Migration
	{
		public override void Up()
		{
			const string schema = "SMS";
			const string table = "ServiceOrderHead";
			const string column = "OrderNo";
			const int newValue = 120;

			if (Database.ColumnExists($"[{schema}].[{table}]", $"{column}"))
			{
				var orderNoLength = (int)Database.ExecuteScalar(
					@$"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = '{schema}' AND
								TABLE_NAME = '{table}' AND
								COLUMN_NAME = '{column}'");

				if (orderNoLength < newValue)
				{
					if (Database.IndexExists($"[{schema}].[{table}]", $"IX_{column}"))
					{
						Database.ExecuteNonQuery($"DROP INDEX [IX_{column}] ON [{schema}].[{table}]");
					}

					Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [{column}] NVARCHAR({newValue}) NOT NULL");
					Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [Predecessor{column}] NVARCHAR({newValue}) NULL");

					Database.ExecuteNonQuery(@$"CREATE NONCLUSTERED INDEX [IX_{column}] ON [{schema}].[{table}] ([{column}] ASC)
					INCLUDE([ContactKey]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
				}
			}
		}
	}
}
