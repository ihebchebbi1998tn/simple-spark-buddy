namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170309180000)]
	public class ChangeBusinessRelationshipIdToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='BusinessRelationship' AND COLUMN_NAME='BusinessRelationshipId' AND DATA_TYPE = 'int')
BEGIN
	DECLARE @pkName AS NVARCHAR(MAX)
	SET @pkName = (SELECT name FROM sys.objects WHERE type = 'PK' AND parent_object_id = object_id('CRM.BusinessRelationship'))
	EXEC('ALTER TABLE [CRM].[BusinessRelationship] DROP CONSTRAINT ' + @pkName)
	EXEC sp_rename 'CRM.BusinessRelationship.BusinessRelationshipId', 'BusinessRelationshipIdOld', 'COLUMN'; 
	
	ALTER TABLE [CRM].[BusinessRelationship] ADD [BusinessRelationshipId] uniqueidentifier NOT NULL DEFAULT(newid())
	ALTER TABLE [CRM].[BusinessRelationship] ADD CONSTRAINT PK_BusinessRelationship PRIMARY KEY (BusinessRelationshipId)
	UPDATE [CRM].[BusinessRelationship] SET ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20170309180000'
END
		");
		}
	}
}