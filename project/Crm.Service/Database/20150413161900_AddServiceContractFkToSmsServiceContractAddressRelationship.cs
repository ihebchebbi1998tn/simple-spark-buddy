namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413161900)]
	public class AddServiceContractFkToSmsServiceContractAddressRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContractAddressRelationship_ServiceContract'") == 0)
			{
				Database.ExecuteNonQuery("DELETE sar FROM [SMS].[ServiceContractAddressRelationship] sar LEFT OUTER JOIN [CRM].[Contact] c ON sar.[ServiceContractKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceContractAddressRelationship_ServiceContract", "[SMS].[ServiceContractAddressRelationship]", "ServiceContractKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}