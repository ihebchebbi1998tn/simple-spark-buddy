namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170309163000)]
	public class AddEntityLookupColumnsToBusinessRelationshipType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF COL_LENGTH('[LU].[BusinessRelationshipType]','CreateDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[BusinessRelationshipType] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_LuBusinessRelationshipType_CreateDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[BusinessRelationshipType] DROP CONSTRAINT DF_LuBusinessRelationshipType_CreateDate
END
IF COL_LENGTH('[LU].[BusinessRelationshipType]','ModifyDate') IS NULL 
BEGIN
	ALTER TABLE [LU].[BusinessRelationshipType] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_LuBusinessRelationshipType_ModifyDate DEFAULT GETUTCDATE()
	ALTER TABLE [LU].[BusinessRelationshipType] DROP CONSTRAINT DF_LuBusinessRelationshipType_ModifyDate
END
IF COL_LENGTH('[LU].[BusinessRelationshipType]','CreateUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[BusinessRelationshipType] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuBusinessRelationshipType_CreateUser DEFAULT 'Migration_20170309163000'
	ALTER TABLE [LU].[BusinessRelationshipType] DROP CONSTRAINT DF_LuBusinessRelationshipType_CreateUser
END
IF COL_LENGTH('[LU].[BusinessRelationshipType]','ModifyUser') IS NULL 
BEGIN
	ALTER TABLE [LU].[BusinessRelationshipType] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_LuBusinessRelationshipType_ModifyUser DEFAULT 'Migration_20170309163000'
	ALTER TABLE [LU].[BusinessRelationshipType] DROP CONSTRAINT DF_LuBusinessRelationshipType_ModifyUser
END
IF COL_LENGTH('[LU].[BusinessRelationshipType]','IsActive') IS NULL 
BEGIN
	ALTER TABLE [LU].[BusinessRelationshipType] ADD [IsActive] bit NOT NULL CONSTRAINT DF_LuBusinessRelationshipType_IsActive DEFAULT 1
	ALTER TABLE [LU].[BusinessRelationshipType] DROP CONSTRAINT DF_LuBusinessRelationshipType_IsActive
END
			");
		}
	}
}
