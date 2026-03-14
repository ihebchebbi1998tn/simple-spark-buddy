namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191115110000)]
	public class AddSPDeleteEntityByDate : Migration
	{
		public override void Up()
		{
			var procedureName = "DeleteEntityByDate";
			if ((int)Database.ExecuteScalar("select count(*) from sys.procedures where type_desc like 'SQL_STORED_PROCEDURE' and name like 'DeleteEntityByDate'") == 0)
			{
				Database.ExecuteNonQuery(
					$@"
				CREATE PROCEDURE [dbo].[{procedureName}] @TableName nvarchar(255), @IdColumnName nvarchar(255), @DateColumnName nvarchar(255), @DeprecationDays nvarchar(255), @AdditionalConditions nvarchar(max) = NULL, @BatchSize nvarchar(32) = '1000'
				AS 
				DECLARE @select_sql NVARCHAR(MAX)
				DECLARE @delete_sql NVARCHAR(MAX)

				SET @select_sql = CONCAT(('INSERT INTO #Temp SELECT TOP ' + CONVERT(NVARCHAR, @BatchSize) + ' mt.' + @IdColumnName + ' FROM ' + @TableName + ' mt WHERE mt.' + @DateColumnName + ' < GETDATE() - ' + @DeprecationDays), @AdditionalConditions);
				SET @delete_sql = CONCAT(('DELETE mt FROM ' + @TableName + ' mt JOIN #Temp T ON T.Id = mt.' + @IdColumnName), 'WHERE 1 = 1' + @AdditionalConditions);
				CREATE TABLE #temp ( Id nvarchar(max) );

				deleteMore:
				EXECUTE sp_executesql @select_sql
				EXECUTE sp_executesql @delete_sql

				IF (SELECT COUNT(*) FROM #temp) > 0 BEGIN
				    TRUNCATE TABLE #Temp
				    goto deleteMore
				END ELSE BEGIN
				    DROP TABLE #Temp
				END
				");
			}
		}
	}
}
