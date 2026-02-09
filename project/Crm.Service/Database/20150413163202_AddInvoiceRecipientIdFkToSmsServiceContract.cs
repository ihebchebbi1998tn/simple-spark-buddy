namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413163202)]
	public class AddInvoiceRecipientIdFkToSmsServiceContract : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContract_InvoiceRecipient'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE sc SET sc.[InvoiceRecipientId] = NULL FROM [SMS].[ServiceContract] sc LEFT OUTER JOIN [CRM].[Contact] c on sc.[InvoiceRecipientId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceContract_InvoiceRecipient", "[SMS].[ServiceContract]", "InvoiceRecipientId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}