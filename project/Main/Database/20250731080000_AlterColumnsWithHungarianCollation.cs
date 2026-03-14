namespace Main.Database
{
	using System.Collections.Generic;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20250731080000)]
	public class AlterColumnsWithHungarianCollation : Migration
	{
		public override void Up()
		{
			var alterStatements = new List<string>();
			using (var reader = Database.ExecuteQuery(@"
SELECT 
	'ALTER TABLE [' + s.name + '].[' + t.name + '] ' +
	'ALTER COLUMN [' + c.name + '] ' + 
	UPPER(tp.name) +
	CASE 
		WHEN tp.name IN ('varchar','char','nvarchar','nchar') 
			THEN '(' + 
				CASE 
					WHEN c.max_length = -1 THEN 'MAX'
					ELSE CAST(
						CASE WHEN tp.name LIKE 'n%' THEN c.max_length / 2 ELSE c.max_length END 
					AS VARCHAR(10))
				END + ')'
		ELSE ''
	END + 
	CASE WHEN c.is_nullable = 1 THEN ' NULL' ELSE ' NOT NULL' END
AS AlterStatement
FROM sys.columns c
JOIN sys.tables t ON c.object_id = t.object_id
JOIN sys.schemas s ON t.schema_id = s.schema_id
JOIN sys.types tp ON c.user_type_id = tp.user_type_id
WHERE c.collation_name = 'Hungarian_CI_AS'
AND tp.name IN ('varchar', 'char', 'nvarchar', 'nchar')
ORDER BY s.name, t.name, c.column_id;"))
			{
				while (reader.Read())
				{
					alterStatements.Add(reader.GetValue(0).ToString());
				}
			}

			foreach (var alterStatement in alterStatements)
			{
				Database.ExecuteNonQuery(alterStatement);
			}
		}
	}
}
