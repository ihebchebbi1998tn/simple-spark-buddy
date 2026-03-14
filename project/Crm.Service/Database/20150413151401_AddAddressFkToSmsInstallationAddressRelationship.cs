namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413151401)]
	public class AddAddressFkToSmsInstallationAddressRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_InstallationAddressRelationship_Address'") == 0)
			{
				Database.ExecuteNonQuery("DELETE iar FROM [SMS].[InstallationAddressRelationship] iar LEFT OUTER JOIN [CRM].[Address] a ON iar.[AddressKey] = a.[AddressId] WHERE a.[AddressId] IS NULL");
				Database.AddForeignKey("FK_InstallationAddressRelationship_Address", "[SMS].[InstallationAddressRelationship]", "AddressKey", "[CRM].[Address]", "AddressId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}