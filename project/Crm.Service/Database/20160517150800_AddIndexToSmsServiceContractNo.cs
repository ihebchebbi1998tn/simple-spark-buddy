namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160517150800)]
	public class AddIndexToSmsServiceContractNo : Migration
	{
		public override void Up()
		{
			const string query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_ServiceContract_ServiceContractNo')" +
			                     "CREATE NONCLUSTERED INDEX [IX_ServiceContract_ServiceContractNo] ON [SMS].[ServiceOrderHead] ([ServiceContractNo])";

			Database.ExecuteNonQuery(query);
		}
	}
}