namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170124144400)]
	public class AddEntityLookupColumnsToDepartmentType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[DepartmentType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[DepartmentType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuDepartmentType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[DepartmentType] DROP CONSTRAINT DF_LuDepartmentType_CreateDate
END
IF COL_LENGTH('[LU].[DepartmentType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[DepartmentType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuDepartmentType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[DepartmentType] DROP CONSTRAINT DF_LuDepartmentType_ModifyDate
END
IF COL_LENGTH('[LU].[DepartmentType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[DepartmentType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuDepartmentType_CreateUser DEFAULT 'Migration_20170124144400'
	ALTER TABLE [LU].[DepartmentType] DROP CONSTRAINT DF_LuDepartmentType_CreateUser
END
IF COL_LENGTH('[LU].[DepartmentType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[DepartmentType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuDepartmentType_ModifyUser DEFAULT 'Migration_20170124144400'
	ALTER TABLE [LU].[DepartmentType] DROP CONSTRAINT DF_LuDepartmentType_ModifyUser
END
IF COL_LENGTH('[LU].[DepartmentType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[DepartmentType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuDepartmentType_IsActive DEFAULT 1
	ALTER TABLE [LU].[DepartmentType] DROP CONSTRAINT DF_LuDepartmentType_IsActive
END
			");
		}
	}
}
