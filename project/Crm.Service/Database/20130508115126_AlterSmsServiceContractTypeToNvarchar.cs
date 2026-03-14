namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130508115126)]
	public class AlterSmsServiceContractTypeToNvarchar : Migration
	{
		public override void Up()
		{
			Database.DropDefault("SMS", "ServiceContractType", "Language");
			Database.DropDefault("SMS", "ServiceContractType", "Value");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceContractType] ALTER COLUMN [Value] nvarchar(20)");
      Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceContract] ALTER COLUMN [ContractTypeKey] nvarchar(20)");
      if (Database.ColumnExists("[SMS].[ServiceContractType]", "MaintenanceContractHeadTypeId") && !Database.ColumnExists("[SMS].[ServiceContractType]", "ServiceContractTypeId"))
	    {
		    Database.RenameColumn("[SMS].[ServiceContractType]", "MaintenanceContractHeadTypeId", "ServiceContractTypeId");
	    }
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}