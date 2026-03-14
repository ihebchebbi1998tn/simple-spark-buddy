namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413155302)]
	public class AddLocationAddressKeyFkToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_InstallationHead_LocationAddress'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE ih SET ih.[LocationAddressKey] = NULL FROM [SMS].[InstallationHead] ih LEFT OUTER JOIN [CRM].[Address] a on ih.[LocationAddressKey] = a.[AddressId] WHERE a.[AddressId] IS NULL");
				Database.AddForeignKey("FK_InstallationHead_LocationAddress", "[SMS].[InstallationHead]", "LocationAddressKey", "[CRM].[Address]", "AddressId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}