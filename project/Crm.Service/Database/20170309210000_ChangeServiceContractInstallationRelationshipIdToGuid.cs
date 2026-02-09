using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20170309210000)]
	public class ChangeServiceContractInstallationRelationshipIdToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContractInstallationRelationship' AND COLUMN_NAME='ServiceContractInstallationRelationshipId' AND DATA_TYPE = 'int')
BEGIN
	DECLARE @pkName AS NVARCHAR(MAX)
	SET @pkName = (SELECT name FROM sys.objects WHERE type = 'PK' AND parent_object_id = object_id('SMS.ServiceContractInstallationRelationship'))
	EXEC('ALTER TABLE [SMS].[ServiceContractInstallationRelationship] DROP CONSTRAINT ' + @pkName)
	EXEC sp_rename 'SMS.ServiceContractInstallationRelationship.ServiceContractInstallationRelationshipId', 'ServiceContractInstallationRelationshipIdOld', 'COLUMN'; 
	
	ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD [ServiceContractInstallationRelationshipId] uniqueidentifier NOT NULL DEFAULT(newid())
	ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD CONSTRAINT PK_ServiceContractInstallationRelationship PRIMARY KEY (ServiceContractInstallationRelationshipId)
	UPDATE [SMS].[ServiceContractInstallationRelationship] SET ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20170309210000'
END
		");
		}
	}
}