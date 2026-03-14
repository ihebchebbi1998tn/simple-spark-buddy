namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131105193821)]
	public class AddIndexIXContractNoToSmsServiceContract : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceContract]') AND name = N'IX_ContractNo') " +
															 "BEGIN " +
															 "CREATE NONCLUSTERED INDEX IX_ContractNo ON [SMS].[ServiceContract] ([ContractNo]) " +
															 "END");
		}

		public override void Down()
		{
		}
	}
}