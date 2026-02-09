namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180820144700)]
	public class ChangeNewIdToNewSequentialId : Migration
	{
		public override void Up()
		{
			Database.DropDefault("SMS", "InstallationAddressRelationship", "InstallationAddressRelationshipId");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[InstallationAddressRelationship] ADD CONSTRAINT [DF_InstallationAddressRelationship_InstallationAddressRelationshipId] DEFAULT (newsequentialid()) FOR [InstallationAddressRelationshipId]");
			Database.DropDefault("SMS", "ServiceContractAddressRelationship", "ServiceContractAddressRelationshipId");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD CONSTRAINT [DF_ServiceContractAddressRelationship_ServiceContractAddressRelationshipId] DEFAULT (newsequentialid()) FOR [ServiceContractAddressRelationshipId]");
			Database.DropDefault("SMS", "ServiceContractInstallationRelationship", "ServiceContractInstallationRelationshipId");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD CONSTRAINT [DF_ServiceContractInstallationRelationship_ServiceContractInstallationRelationshipId] DEFAULT (newsequentialid()) FOR [ServiceContractInstallationRelationshipId]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ExpenseReport] ADD CONSTRAINT [DF_ExpenseReport_Id] DEFAULT (newsequentialid()) FOR [Id]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[InstallationPos] ADD CONSTRAINT [DF_InstallationPos_Id] DEFAULT (newsequentialid()) FOR [id]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[InstallationPosSerials] ADD CONSTRAINT [DF_InstallationPosSerials_Id] DEFAULT (newsequentialid()) FOR [id]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[RdsPpStructure] ADD CONSTRAINT [DF_RdsPpStructure_RdsPpStructureId] DEFAULT (newsequentialid()) FOR [RdsPpStructureId]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CONSTRAINT [DF_ServiceOrderMaterial_Id] DEFAULT (newsequentialid()) FOR [Id]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderMaterialSerials] ADD CONSTRAINT [DF_ServiceOrderMaterialSerials_Id] DEFAULT (newsequentialid()) FOR [Id]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimePostings] ADD CONSTRAINT [DF_ServiceOrderTimePostings_Id] DEFAULT (newsequentialid()) FOR [id]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimes] ADD CONSTRAINT [DF_ServiceOrderTimes_Id] DEFAULT (newsequentialid()) FOR [id]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntryReport] ADD CONSTRAINT [DF_TimeEntryReport_Id] DEFAULT (newsequentialid()) FOR [Id]");
		}
	}
}