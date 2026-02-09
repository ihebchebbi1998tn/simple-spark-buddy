namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170125132100)]
	public class AddEntityLookupColumnsToCompanyGroupFlags : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[CompanyGroupFlag1]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag1] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag1_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag1] DROP CONSTRAINT DF_CompanyGroupFlag1_CreateDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag1]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag1] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag1_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag1] DROP CONSTRAINT DF_CompanyGroupFlag1_ModifyDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag1]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag1] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag1_CreateUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag1] DROP CONSTRAINT DF_CompanyGroupFlag1_CreateUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag1]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag1] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag1_ModifyUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag1] DROP CONSTRAINT DF_CompanyGroupFlag1_ModifyUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag1]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag1] ADD [IsActive] bit NOT NULL CONSTRAINT DF_CompanyGroupFlag1_IsActive DEFAULT 1
	ALTER TABLE [LU].[CompanyGroupFlag1] DROP CONSTRAINT DF_CompanyGroupFlag1_IsActive
END
			");
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[CompanyGroupFlag2]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag2] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag2_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag2] DROP CONSTRAINT DF_CompanyGroupFlag2_CreateDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag2]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag2] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag2_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag2] DROP CONSTRAINT DF_CompanyGroupFlag2_ModifyDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag2]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag2] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag2_CreateUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag2] DROP CONSTRAINT DF_CompanyGroupFlag2_CreateUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag2]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag2] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag2_ModifyUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag2] DROP CONSTRAINT DF_CompanyGroupFlag2_ModifyUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag2]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag2] ADD [IsActive] bit NOT NULL CONSTRAINT DF_CompanyGroupFlag2_IsActive DEFAULT 1
	ALTER TABLE [LU].[CompanyGroupFlag2] DROP CONSTRAINT DF_CompanyGroupFlag2_IsActive
END
			");
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[CompanyGroupFlag3]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag3] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag3_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag3] DROP CONSTRAINT DF_CompanyGroupFlag3_CreateDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag3]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag3] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag3_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag3] DROP CONSTRAINT DF_CompanyGroupFlag3_ModifyDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag3]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag3] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag3_CreateUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag3] DROP CONSTRAINT DF_CompanyGroupFlag3_CreateUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag3]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag3] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag3_ModifyUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag3] DROP CONSTRAINT DF_CompanyGroupFlag3_ModifyUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag3]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag3] ADD [IsActive] bit NOT NULL CONSTRAINT DF_CompanyGroupFlag3_IsActive DEFAULT 1
	ALTER TABLE [LU].[CompanyGroupFlag3] DROP CONSTRAINT DF_CompanyGroupFlag3_IsActive
END
			");
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[CompanyGroupFlag4]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag4] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag4_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag4] DROP CONSTRAINT DF_CompanyGroupFlag4_CreateDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag4]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag4] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag4_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag4] DROP CONSTRAINT DF_CompanyGroupFlag4_ModifyDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag4]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag4] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag4_CreateUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag4] DROP CONSTRAINT DF_CompanyGroupFlag4_CreateUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag4]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag4] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag4_ModifyUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag4] DROP CONSTRAINT DF_CompanyGroupFlag4_ModifyUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag4]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag4] ADD [IsActive] bit NOT NULL CONSTRAINT DF_CompanyGroupFlag4_IsActive DEFAULT 1
	ALTER TABLE [LU].[CompanyGroupFlag4] DROP CONSTRAINT DF_CompanyGroupFlag4_IsActive
END
			");
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[CompanyGroupFlag5]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag5] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag5_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag5] DROP CONSTRAINT DF_CompanyGroupFlag5_CreateDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag5]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag5] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_CompanyGroupFlag5_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[CompanyGroupFlag5] DROP CONSTRAINT DF_CompanyGroupFlag5_ModifyDate
END
IF COL_LENGTH('[LU].[CompanyGroupFlag5]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag5] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag5_CreateUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag5] DROP CONSTRAINT DF_CompanyGroupFlag5_CreateUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag5]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag5] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_CompanyGroupFlag5_ModifyUser DEFAULT 'Migration'
	ALTER TABLE [LU].[CompanyGroupFlag5] DROP CONSTRAINT DF_CompanyGroupFlag5_ModifyUser
END
IF COL_LENGTH('[LU].[CompanyGroupFlag5]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[CompanyGroupFlag5] ADD [IsActive] bit NOT NULL CONSTRAINT DF_CompanyGroupFlag5_IsActive DEFAULT 1
	ALTER TABLE [LU].[CompanyGroupFlag5] DROP CONSTRAINT DF_CompanyGroupFlag5_IsActive
END
			");
		}
	}
}
