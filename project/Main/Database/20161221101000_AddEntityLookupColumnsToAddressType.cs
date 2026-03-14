namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161221101000)]
	public class AddEntityLookupColumnsToAddressType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[AddressType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[AddressType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuAddressType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[AddressType] DROP CONSTRAINT DF_LuAddressType_CreateDate
END
IF COL_LENGTH('[LU].[AddressType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[AddressType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuAddressType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[AddressType] DROP CONSTRAINT DF_LuAddressType_ModifyDate
END
IF COL_LENGTH('[LU].[AddressType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[AddressType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuAddressType_CreateUser DEFAULT 'Migration_20161221101000'
	ALTER TABLE [LU].[AddressType] DROP CONSTRAINT DF_LuAddressType_CreateUser
END
IF COL_LENGTH('[LU].[AddressType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[AddressType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuAddressType_ModifyUser DEFAULT 'Migration_20161221101000'
	ALTER TABLE [LU].[AddressType] DROP CONSTRAINT DF_LuAddressType_ModifyUser
END
IF COL_LENGTH('[LU].[AddressType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[AddressType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuAddressType_IsActive DEFAULT 1
	ALTER TABLE [LU].[AddressType] DROP CONSTRAINT DF_LuAddressType_IsActive
END
			");
		}
	}
}
