namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161221090000)]
	public class AddEntityLookupColumnsToCountry : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[Country]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[Country] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuCountry_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[Country] DROP CONSTRAINT DF_LuCountry_CreateDate
END
IF COL_LENGTH('[LU].[Country]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[Country] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuCountry_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[Country] DROP CONSTRAINT DF_LuCountry_ModifyDate
END
IF COL_LENGTH('[LU].[Country]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[Country] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuCountry_CreateUser DEFAULT 'Migration_20161221090000'
	ALTER TABLE [LU].[Country] DROP CONSTRAINT DF_LuCountry_CreateUser
END
IF COL_LENGTH('[LU].[Country]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[Country] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuCountry_ModifyUser DEFAULT 'Migration_20161221090000'
	ALTER TABLE [LU].[Country] DROP CONSTRAINT DF_LuCountry_ModifyUser
END
IF COL_LENGTH('[LU].[Country]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[Country] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuCountry_IsActive DEFAULT 1
	ALTER TABLE [LU].[Country] DROP CONSTRAINT DF_LuCountry_IsActive
END
			");
		}
	}
}
