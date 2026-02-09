namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413164505)]
	public class AddInvoiceRecipientAddressIdFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_InvoiceRecipient'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[InvoiceRecipientAddressId] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[Address] a ON soh.[InvoiceRecipientAddressId] = a.[AddressId] WHERE a.[AddressId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_InvoiceRecipientAddress", "[SMS].[ServiceOrderHead]", "InvoiceRecipientAddressId", "[CRM].[Address]", "AddressId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}