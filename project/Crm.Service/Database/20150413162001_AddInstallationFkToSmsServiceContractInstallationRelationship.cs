namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413162001)]
	public class AddInstallationFkToSmsServiceContractInstallationRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContractInstallationRelationship_Installation'") == 0)
			{
				Database.ExecuteNonQuery("DELETE sir FROM [SMS].[ServiceContractInstallationRelationship] sir LEFT OUTER JOIN [CRM].[Contact] c ON sir.[InstallationKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceContractInstallationRelationship_Installation", "[SMS].[ServiceContractInstallationRelationship]", "InstallationKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}