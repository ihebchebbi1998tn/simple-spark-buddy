namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180718094000)]
	public class ChangeServiceOrderServiceContractNoToServiceContractId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderHead", "ServiceContractId") && Database.ColumnExists("SMS.ServiceOrderHead", "ServiceContractNo"))
			{
				Database.AddColumn("SMS.ServiceOrderHead", new Column("ServiceContractId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderHead SET SMS.ServiceOrderHead.ServiceContractId = SMS.ServiceContract.ContactKey FROM SMS.ServiceOrderHead JOIN SMS.ServiceContract ON SMS.ServiceContract.ContractNo = SMS.ServiceOrderHead.ServiceContractNo");
				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_ServiceContract_ServiceContractNo'") == 1)
				{
					Database.ExecuteNonQuery("DROP INDEX [IX_ServiceContract_ServiceContractNo] ON [SMS].[ServiceOrderHead]");
				}
				
				Database.RemoveColumn("SMS.ServiceOrderHead", "ServiceContractNo");
				Database.AddForeignKey("FK_ServiceOrderHead_ServiceContract", "SMS.ServiceOrderHead", "ServiceContractId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderHead_ServiceContractId] ON [SMS].[ServiceOrderHead] ([ServiceContractId] ASC)");
			}
		}
	}
}