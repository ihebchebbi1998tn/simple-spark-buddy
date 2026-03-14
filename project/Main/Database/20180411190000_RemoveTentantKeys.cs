namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180411190000)]
	public class RemoveTentantKeys : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				DECLARE @cmd NVARCHAR(MAX) = '
					IF COL_LENGTH(''?'', ''TenantKey'') IS NOT NULL
					BEGIN
						DECLARE @tenantKey INT
						EXEC sp_executesql N''SET @tenantKey = (SELECT TOP 1 TenantKey FROM ? WHERE TenantKey IS NOT NULL)'', N''@tenantKey INT OUT'', @tenantKey = @tenantKey OUT
						IF @tenantKey IS NULL
							ALTER TABLE ? DROP COLUMN TenantKey
						ELSE
							EXEC sp_rename ''?.TenantKey'', ''TenantKeyOld'', ''COLUMN''
					END'
				EXEC sp_MSforeachtable @cmd");
		}
	}
}
