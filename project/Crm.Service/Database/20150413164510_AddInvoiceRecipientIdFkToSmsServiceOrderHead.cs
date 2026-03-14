namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413164510)]
	public class AddInvoiceRecipientIdFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_InvoiceRecipient'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[InvoiceRecipientId] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[Contact] c ON soh.[InvoiceRecipientId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_InvoiceRecipient", "[SMS].[ServiceOrderHead]", "InvoiceRecipientId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}