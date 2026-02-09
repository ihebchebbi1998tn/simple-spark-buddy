namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161221100500)]
	public class AddEntityLookupColumnsToCompanyType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[CompanyType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuCompanyType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyType] DROP CONSTRAINT DF_LuCompanyType_CreateDate
END
IF COL_LENGTH('[LU].[CompanyType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuCompanyType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyType] DROP CONSTRAINT DF_LuCompanyType_ModifyDate
END
IF COL_LENGTH('[LU].[CompanyType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuCompanyType_CreateUser DEFAULT 'Migration_20161221100500'
	ALTER TABLE [LU].[CompanyType] DROP CONSTRAINT DF_LuCompanyType_CreateUser
END
IF COL_LENGTH('[LU].[CompanyType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuCompanyType_ModifyUser DEFAULT 'Migration_20161221100500'
	ALTER TABLE [LU].[CompanyType] DROP CONSTRAINT DF_LuCompanyType_ModifyUser
END
IF COL_LENGTH('[LU].[CompanyType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuCompanyType_IsActive DEFAULT 1
	ALTER TABLE [LU].[CompanyType] DROP CONSTRAINT DF_LuCompanyType_IsActive
END
			");
		}
	}
}
