namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413163203)]
	public class AddInvoiceAddressKeyFkToSmsServiceContract : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContract_InvoiceAddress'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE sc SET sc.[InvoiceAddressKey] = NULL FROM [SMS].[ServiceContract] sc LEFT OUTER JOIN [CRM].[Address] a on sc.[InvoiceAddressKey] = a.[AddressId] WHERE a.[AddressId] IS NULL");
				Database.AddForeignKey("FK_ServiceContract_InvoiceAddress", "[SMS].[ServiceContract]", "InvoiceAddressKey", "[CRM].[Address]", "AddressId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}