using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20170309200000)]
	public class ChangeInstallationAddressRelationshipIdToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationAddressRelationship' AND COLUMN_NAME='InstallationAddressRelationshipId' AND DATA_TYPE = 'int')
BEGIN
	DECLARE @pkName AS NVARCHAR(MAX)
	SET @pkName = (SELECT name FROM sys.objects WHERE type = 'PK' AND parent_object_id = object_id('SMS.InstallationAddressRelationship'))
	EXEC('ALTER TABLE [SMS].[InstallationAddressRelationship] DROP CONSTRAINT ' + @pkName)
	EXEC sp_rename 'SMS.InstallationAddressRelationship.InstallationAddressRelationshipId', 'InstallationAddressRelationshipIdOld', 'COLUMN'; 
	
	ALTER TABLE [SMS].[InstallationAddressRelationship] ADD [InstallationAddressRelationshipId] uniqueidentifier NOT NULL DEFAULT(newid())
	ALTER TABLE [SMS].[InstallationAddressRelationship] ADD CONSTRAINT PK_InstallationAddressRelationship PRIMARY KEY (InstallationAddressRelationshipId)
	UPDATE [SMS].[InstallationAddressRelationship] SET ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20170309200000'
END
		");
		}
	}
}