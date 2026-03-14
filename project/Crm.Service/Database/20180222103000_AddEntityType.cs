namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model;
	using Crm.Service.Model.Notes;
	using Crm.Service.Model.Relationships;

	[Migration(20180222103000)]
	public class AddEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<InstallationAddressRelationship>("SMS", "InstallationAddressRelationship");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ServiceContractAddressRelationship>("SMS", "ServiceContractAddressRelationship");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ServiceContractInstallationRelationship>("SMS", "ServiceContractInstallationRelationship");

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<OrderStatusChangedNote>("CRM", "Note");
			helper.AddOrGetEntityTypeId<ServiceCaseStatusChangedNote>();
			helper.AddOrGetEntityTypeId<ServiceContractStatusChangedNote>();
			helper.AddOrGetEntityTypeId<ServiceOrderHeadCreatedNote>();

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ServiceOrderDispatch>("SMS", "ServiceOrderDispatch");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Installation>("CRM", "Contact");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<InstallationPos>("SMS", "InstallationPos");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<InstallationPosSerial>("SMS", "InstallationPosSerials");
			helper.AddOrGetEntityTypeId<MaintenancePlan>();
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<RdsPpStructure>("SMS", "RdsPpStructure");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ReplenishmentOrder>("SMS", "ReplenishmentOrder");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ReplenishmentOrderItem>("SMS", "ReplenishmentOrderItem");
			helper.AddOrGetEntityTypeId<ServiceCase>();
			helper.AddOrGetEntityTypeId<ServiceContract>();
			helper.AddOrGetEntityTypeId<ServiceObject>();
			helper.AddOrGetEntityTypeId<ServiceOrderHead>();
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ServiceOrderMaterial>("SMS", "ServiceOrderMaterial");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ServiceOrderTime>("SMS", "ServiceOrderTimes");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ServiceOrderTimePosting>("SMS", "ServiceOrderTimePostings");
		}
	}
}
