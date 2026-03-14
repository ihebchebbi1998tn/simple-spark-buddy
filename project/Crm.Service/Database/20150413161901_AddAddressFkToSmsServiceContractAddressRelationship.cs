namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413161901)]
	public class AddAddressFkToSmsServiceContractAddressRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContractAddressRelationship_Address'") == 0)
			{
				Database.ExecuteNonQuery("DELETE sar FROM [SMS].[ServiceContractAddressRelationship] sar LEFT OUTER JOIN [CRM].[Address] a ON sar.[AddressKey] = a.[AddressId] WHERE a.[AddressId] IS NULL");
				Database.AddForeignKey("FK_ServiceContractAddressRelationship_Address", "[SMS].[ServiceContractAddressRelationship]", "AddressKey", "[CRM].[Address]", "AddressId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}