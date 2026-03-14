namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413162000)]
	public class AddServiceContractFkToSmsServiceContractInstallationRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContractInstallationRelationship_ServiceContract'") == 0)
			{
				Database.ExecuteNonQuery("DELETE sir FROM [SMS].[ServiceContractInstallationRelationship] sir LEFT OUTER JOIN [CRM].[Contact] c ON sir.[ServiceContractKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceContractInstallationRelationship_ServiceContract", "[SMS].[ServiceContractInstallationRelationship]", "ServiceContractKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}