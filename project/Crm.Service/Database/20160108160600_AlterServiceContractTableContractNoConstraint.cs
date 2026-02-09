namespace Crm.Service.Database {
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160108160600)]
	public class AlterServiceContractTableContractNoConstraint : Migration {
		public override void Up()
		{
			Database.ExecuteNonQuery("IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceContract]') AND name = N'IX_ContractNo') " +
												 "BEGIN " +
												 "DROP INDEX [IX_ContractNo] ON [SMS].[ServiceContract] " +
												 "END");
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceContract]') AND name = N'UC_ServiceContract_ContractNo') " +
												 "BEGIN " +
												 "ALTER TABLE SMS.ServiceContract ADD Constraint UC_ServiceContract_ContractNo UNIQUE (ContractNo) " +
												 "END");
		}
	}
}
