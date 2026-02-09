namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161221102500)]
	public class AddEntityLookupColumnsToEmailType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[EmailType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[EmailType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuEmailType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[EmailType] DROP CONSTRAINT DF_LuEmailType_CreateDate
END
IF COL_LENGTH('[LU].[EmailType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[EmailType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuEmailType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[EmailType] DROP CONSTRAINT DF_LuEmailType_ModifyDate
END
IF COL_LENGTH('[LU].[EmailType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[EmailType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuEmailType_CreateUser DEFAULT 'Migration_20161221102500'
	ALTER TABLE [LU].[EmailType] DROP CONSTRAINT DF_LuEmailType_CreateUser
END
IF COL_LENGTH('[LU].[EmailType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[EmailType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuEmailType_ModifyUser DEFAULT 'Migration_20161221102500'
	ALTER TABLE [LU].[EmailType] DROP CONSTRAINT DF_LuEmailType_ModifyUser
END
IF COL_LENGTH('[LU].[EmailType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[EmailType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuEmailType_IsActive DEFAULT 1
	ALTER TABLE [LU].[EmailType] DROP CONSTRAINT DF_LuEmailType_IsActive
END
			");
		}
	}
}
