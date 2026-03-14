namespace Crm.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240529120000)]
	public class AddConditionToUserTokenConstraint : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();
			if (Database.ColumnExists("[CRM].[User]", "GeneralToken"))
			{
				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'DF_USER_Token_Unique'") == 1)
				{
					query.AppendLine("ALTER TABLE [CRM].[User] DROP CONSTRAINT [DF_USER_Token_Unique]");
				}

				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.index_columns AS ic INNER JOIN sys.indexes AS i ON ic.[object_id] = i.[object_id] AND ic.index_id = i.index_id INNER JOIN sys.columns AS c ON ic.[object_id] = c.[object_id] AND ic.column_id = c.column_id WHERE ic.[object_id] = OBJECT_ID('[CRM].[User]') and c.name = 'GeneralToken' and i.is_unique = 1 and i.is_unique_constraint = 0") == 0)
				{
					query.AppendLine("CREATE UNIQUE INDEX [IX_UQ_GeneralToken] ON [CRM].[User] ([GeneralToken]) WHERE [Discharged] = 0");
				}
			}

			if (query.ToString() != string.Empty)
			{
				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}
