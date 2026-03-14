namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160204100000)]
    public class AlterServiceOrderHeadTableForeignKey : Migration
    {
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[SMS].[FK_ServiceOrderHead_Payer]') AND parent_object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]'))
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT FK_ServiceOrderHead_Payer
			");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] WITH CHECK ADD  CONSTRAINT [FK_ServiceOrderHead_Payer] FOREIGN KEY([PayerId]) REFERENCES [CRM].[Company] ([ContactKey])");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] CHECK CONSTRAINT [FK_ServiceOrderHead_Payer]");

			Database.ExecuteNonQuery(@"
				IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[SMS].[FK_ServiceOrderHead_InvoiceRecipient]') AND parent_object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]'))
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT FK_ServiceOrderHead_InvoiceRecipient
			");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] WITH CHECK ADD  CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipient] FOREIGN KEY([InvoiceRecipientId]) REFERENCES [CRM].[Company] ([ContactKey])");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] CHECK CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipient]");

			Database.ExecuteNonQuery(@"
				IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[SMS].[FK_ServiceOrderHead_Initiator]') AND parent_object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]'))
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT FK_ServiceOrderHead_Initiator
			");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] WITH CHECK ADD CONSTRAINT [FK_ServiceOrderHead_Initiator] FOREIGN KEY([InitiatorId]) REFERENCES [CRM].[Company] ([ContactKey])");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] CHECK CONSTRAINT [FK_ServiceOrderHead_Initiator]");
		}
	}
}
