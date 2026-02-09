using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20170309205000)]
	public class ChangeServiceContractAddressRelationshipIdToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContractAddressRelationship' AND COLUMN_NAME='ServiceContractAddressRelationshipId' AND DATA_TYPE = 'int')
BEGIN
	DECLARE @pkName AS NVARCHAR(MAX)
	SET @pkName = (SELECT name FROM sys.objects WHERE type = 'PK' AND parent_object_id = object_id('SMS.ServiceContractAddressRelationship'))
	EXEC('ALTER TABLE [SMS].[ServiceContractAddressRelationship] DROP CONSTRAINT ' + @pkName)
	EXEC sp_rename 'SMS.ServiceContractAddressRelationship.ServiceContractAddressRelationshipId', 'ServiceContractAddressRelationshipIdOld', 'COLUMN'; 
	
	ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD [ServiceContractAddressRelationshipId] uniqueidentifier NOT NULL DEFAULT(newid())
	ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD CONSTRAINT PK_ServiceContractAddressRelationship PRIMARY KEY (ServiceContractAddressRelationshipId)
	UPDATE [SMS].[ServiceContractAddressRelationship] SET ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20170309205000'
END
		");
		}
	}
}