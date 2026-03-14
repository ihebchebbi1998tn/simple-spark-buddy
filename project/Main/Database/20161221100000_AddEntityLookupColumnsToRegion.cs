namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161221100000)]
	public class AddEntityLookupColumnsToRegion : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[Region]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[Region] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuRegion_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[Region] DROP CONSTRAINT DF_LuRegion_CreateDate
END
IF COL_LENGTH('[LU].[Region]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[Region] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuRegion_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[Region] DROP CONSTRAINT DF_LuRegion_ModifyDate
END
IF COL_LENGTH('[LU].[Region]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[Region] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuRegion_CreateUser DEFAULT 'Migration_20161221100000'
	ALTER TABLE [LU].[Region] DROP CONSTRAINT DF_LuRegion_CreateUser
END
IF COL_LENGTH('[LU].[Region]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[Region] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuRegion_ModifyUser DEFAULT 'Migration_20161221100000'
	ALTER TABLE [LU].[Region] DROP CONSTRAINT DF_LuRegion_ModifyUser
END
IF COL_LENGTH('[LU].[Region]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[Region] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuRegion_IsActive DEFAULT 1
	ALTER TABLE [LU].[Region] DROP CONSTRAINT DF_LuRegion_IsActive
END
			");
		}
	}
}
