namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161221102000)]
	public class AddEntityLookupColumnsToFaxType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[FaxType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[FaxType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuFaxType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[FaxType] DROP CONSTRAINT DF_LuFaxType_CreateDate
END
IF COL_LENGTH('[LU].[FaxType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[FaxType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuFaxType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[FaxType] DROP CONSTRAINT DF_LuFaxType_ModifyDate
END
IF COL_LENGTH('[LU].[FaxType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[FaxType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuFaxType_CreateUser DEFAULT 'Migration_20161221102000'
	ALTER TABLE [LU].[FaxType] DROP CONSTRAINT DF_LuFaxType_CreateUser
END
IF COL_LENGTH('[LU].[FaxType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[FaxType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuFaxType_ModifyUser DEFAULT 'Migration_20161221102000'
	ALTER TABLE [LU].[FaxType] DROP CONSTRAINT DF_LuFaxType_ModifyUser
END
IF COL_LENGTH('[LU].[FaxType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[FaxType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuFaxType_IsActive DEFAULT 1
	ALTER TABLE [LU].[FaxType] DROP CONSTRAINT DF_LuFaxType_IsActive
END
			");
		}
	}
}
