namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151112155000)]
	public class ChangeContactsToGuidIAddConstraints : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Initiator')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead]  WITH CHECK ADD  CONSTRAINT [FK_ServiceOrderHead_Initiator] FOREIGN KEY([InitiatorId]) REFERENCES [CRM].[Company] ([ContactKey])
				END

				IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_InvoiceRecipient')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead]  WITH CHECK ADD  CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipient] FOREIGN KEY([InvoiceRecipientId]) REFERENCES [CRM].[Company] ([ContactKey])
				END

				IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Payer')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead]  WITH CHECK ADD  CONSTRAINT [FK_ServiceOrderHead_Payer] FOREIGN KEY([PayerId]) REFERENCES [CRM].[Company] ([ContactKey])
				END

				IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Contact_ContactType')
				BEGIN
					CREATE NONCLUSTERED INDEX [IX_Contact_ContactType] ON [CRM].[Contact]
					(
						[ContactType] ASC
					)
					INCLUDE ( 	[ContactId],
						[LegacyId])
				END

				IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Contact_ContactType_ContactId_LegacyId')
				BEGIN
					CREATE NONCLUSTERED INDEX [IX_Contact_ContactType_ContactId_LegacyId] ON [CRM].[Contact]
					(
						[ContactType] ASC,
						[ContactId] ASC,
						[LegacyId] ASC
					)
				END

				IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Contact_ContactType_ContactId_LegacyId')
				BEGIN
					CREATE NONCLUSTERED INDEX [IX_Contact_ContactId_ContactType_IsExported_LegacyId] ON [CRM].[Contact]
					(
						[ContactId] ASC,
						[ContactType] ASC,
						[IsExported] ASC,
						[LegacyId] ASC
					)
				END");
		}
	}
}