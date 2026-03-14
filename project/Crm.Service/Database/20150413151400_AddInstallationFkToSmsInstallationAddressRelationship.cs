namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413151400)]
	public class AddInstallationFkToSmsInstallationAddressRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_InstallationAddressRelationship_Installation'") == 0)
			{
				Database.ExecuteNonQuery("DELETE iar FROM [SMS].[InstallationAddressRelationship] iar LEFT OUTER JOIN [CRM].[Contact] c ON iar.[InstallationKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_InstallationAddressRelationship_Installation", "[SMS].[InstallationAddressRelationship]", "InstallationKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}