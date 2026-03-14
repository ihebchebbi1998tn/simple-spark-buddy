namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161221101500)]
	public class AddEntityLookupColumnsToWebsiteType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[WebsiteType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[WebsiteType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuWebsiteType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[WebsiteType] DROP CONSTRAINT DF_LuWebsiteType_CreateDate
END
IF COL_LENGTH('[LU].[WebsiteType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[WebsiteType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuWebsiteType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[WebsiteType] DROP CONSTRAINT DF_LuWebsiteType_ModifyDate
END
IF COL_LENGTH('[LU].[WebsiteType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[WebsiteType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuWebsiteType_CreateUser DEFAULT 'Migration_20161221101500'
	ALTER TABLE [LU].[WebsiteType] DROP CONSTRAINT DF_LuWebsiteType_CreateUser
END
IF COL_LENGTH('[LU].[WebsiteType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[WebsiteType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuWebsiteType_ModifyUser DEFAULT 'Migration_20161221101500'
	ALTER TABLE [LU].[WebsiteType] DROP CONSTRAINT DF_LuWebsiteType_ModifyUser
END
IF COL_LENGTH('[LU].[WebsiteType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[WebsiteType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuWebsiteType_IsActive DEFAULT 1
	ALTER TABLE [LU].[WebsiteType] DROP CONSTRAINT DF_LuWebsiteType_IsActive
END
			");
		}
	}
}
