namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170112150000)]
	public class AddEntityLookupColumnsToTaskType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[TaskType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[TaskType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuTaskType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[TaskType] DROP CONSTRAINT DF_LuTaskType_CreateDate
END
IF COL_LENGTH('[LU].[TaskType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[TaskType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuTaskType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[TaskType] DROP CONSTRAINT DF_LuTaskType_ModifyDate
END
IF COL_LENGTH('[LU].[TaskType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[TaskType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuTaskType_CreateUser DEFAULT 'Migration_20170112150000'
	ALTER TABLE [LU].[TaskType] DROP CONSTRAINT DF_LuTaskType_CreateUser
END
IF COL_LENGTH('[LU].[TaskType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[TaskType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuTaskType_ModifyUser DEFAULT 'Migration_20170112150000'
	ALTER TABLE [LU].[TaskType] DROP CONSTRAINT DF_LuTaskType_ModifyUser
END
IF COL_LENGTH('[LU].[TaskType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[TaskType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuTaskType_IsActive DEFAULT 1
	ALTER TABLE [LU].[TaskType] DROP CONSTRAINT DF_LuTaskType_IsActive
END
			");
		}
	}
}
