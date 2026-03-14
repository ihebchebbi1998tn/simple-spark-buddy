namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161221103000)]
	public class AddEntityLookupColumnsToPhoneType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[PhoneType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[PhoneType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuPhoneType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[PhoneType] DROP CONSTRAINT DF_LuPhoneType_CreateDate
END
IF COL_LENGTH('[LU].[PhoneType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[PhoneType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuPhoneType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[PhoneType] DROP CONSTRAINT DF_LuPhoneType_ModifyDate
END
IF COL_LENGTH('[LU].[PhoneType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[PhoneType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuPhoneType_CreateUser DEFAULT 'Migration_20161221103000'
	ALTER TABLE [LU].[PhoneType] DROP CONSTRAINT DF_LuPhoneType_CreateUser
END
IF COL_LENGTH('[LU].[PhoneType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[PhoneType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuPhoneType_ModifyUser DEFAULT 'Migration_20161221103000'
	ALTER TABLE [LU].[PhoneType] DROP CONSTRAINT DF_LuPhoneType_ModifyUser
END
IF COL_LENGTH('[LU].[PhoneType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[PhoneType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuPhoneType_IsActive DEFAULT 1
	ALTER TABLE [LU].[PhoneType] DROP CONSTRAINT DF_LuPhoneType_IsActive
END
			");
		}
	}
}
