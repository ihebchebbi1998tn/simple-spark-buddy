namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151112154700)]
	public class ChangeAddressesToGuidId : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress')
					BEGIN
						DROP INDEX [IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress] ON [CRM].[Address]
					END

					IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Address_IsActive_IsCompanyStandardAddress')
					BEGIN
						DROP INDEX [IX_Address_IsActive_IsCompanyStandardAddress] ON [CRM].[Address]
					END

					IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_IsActive_IsCompanyStandardAddress')
					BEGIN
						DROP INDEX [IX_IsActive_IsCompanyStandardAddress] ON [CRM].[Address]
					END		

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'V_CTI_ReverseSearch')
					BEGIN
						DROP VIEW [dbo].[V_CTI_ReverseSearch]
					END			

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Address_Contact')
					BEGIN
						ALTER TABLE [CRM].[Address] DROP CONSTRAINT [FK_Address_Contact]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Communication_Address')
					BEGIN
						ALTER TABLE [CRM].[Communication] DROP CONSTRAINT [FK_Communication_Address]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Person_Address')
					BEGIN
						ALTER TABLE [CRM].[Person] DROP CONSTRAINT [FK_Person_Address]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_InstallationAddressRelationship_Address')
					BEGIN
						ALTER TABLE [SMS].[InstallationAddressRelationship] DROP CONSTRAINT [FK_InstallationAddressRelationship_Address]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_InstallationHead_LocationAddress')
					BEGIN
						ALTER TABLE [SMS].[InstallationHead] DROP CONSTRAINT [FK_InstallationHead_LocationAddress]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContract_InvoiceAddress')
					BEGIN
						ALTER TABLE [SMS].[ServiceContract] DROP CONSTRAINT [FK_ServiceContract_InvoiceAddress]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContract_PayerAddress')
					BEGIN
						ALTER TABLE [SMS].[ServiceContract] DROP CONSTRAINT [FK_ServiceContract_PayerAddress]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContractAddressRelationship_Address')
					BEGIN
						ALTER TABLE [SMS].[ServiceContractAddressRelationship] DROP CONSTRAINT [FK_ServiceContractAddressRelationship_Address]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_InvoiceRecipientAddress')
					BEGIN
						ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipientAddress]
					END

					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_PayerAddress')
					BEGIN
						ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_PayerAddress]
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Address' AND COLUMN_NAME='AddressId' AND DATA_TYPE = 'int')
					BEGIN
						IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Address')
						BEGIN
							ALTER TABLE [CRM].[Address] DROP CONSTRAINT [PK_Address]
						END
						ALTER TABLE [CRM].[Address] ADD [AddressIdOld] INT NULL
						EXEC('UPDATE [CRM].[Address] SET [AddressIdOld] = [AddressId]')
						ALTER TABLE [CRM].[Address] DROP COLUMN [AddressId]
						ALTER TABLE [CRM].[Address] ADD [AddressId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
						ALTER TABLE [CRM].[Address] ALTER COLUMN [AddressIdOld] int NULL
						ALTER TABLE [CRM].[Address] ADD CONSTRAINT [PK_Address] PRIMARY KEY([AddressId])
					END

					IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress')
					BEGIN
						CREATE NONCLUSTERED INDEX [IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress] ON [CRM].[Address]
						(
							[AddressId] ASC,
							[CompanyKey] ASC,
							[IsCompanyStandardAddress] ASC
						)
						INCLUDE ([Name1],
							[Name2],
							[Name3],
							[City],
							[CountryKey],
							[ZipCode],
							[Street],
							[Latitude],
							[Longitude]) ON [PRIMARY]
					END

					IF NOT EXISTS(SELECT * FROM sys.indexes WHERE NAME = 'IX_Address_IsActive_IsCompanyStandardAddress')
						BEGIN
							CREATE NONCLUSTERED INDEX[IX_Address_IsActive_IsCompanyStandardAddress] ON[CRM].[Address]
							(
								[IsActive] ASC,
								[IsCompanyStandardAddress] ASC
							)
							INCLUDE([AddressId],
								[CompanyKey])
						END

					IF NOT EXISTS(SELECT* FROM sys.indexes WHERE NAME = 'IX_IsActive_IsCompanyStandardAddress')
						BEGIN
							CREATE NONCLUSTERED INDEX[IX_IsActive_IsCompanyStandardAddress] ON[CRM].[Address]
						(
							[IsActive] ASC,
							[IsCompanyStandardAddress] ASC
						)
						INCLUDE([AddressId],
							[CompanyKey])
						END

					IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Address_Contact')
					BEGIN
						ALTER TABLE [CRM].[Address] ADD CONSTRAINT [FK_Address_Contact] FOREIGN KEY([CompanyKey]) REFERENCES [CRM].[Contact] ([ContactId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Communication' AND COLUMN_NAME='AddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[CRM].[Communication].[AddressKey]', 'AddressKeyOld', 'COLUMN'
						ALTER TABLE [CRM].[Communication] ADD [AddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[AddressKey] = b.[AddressId] FROM [Crm].[Communication] a LEFT OUTER JOIN [CRM].[Address] b ON a.[AddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [CRM].[Communication] ADD CONSTRAINT [FK_Communication_Address] FOREIGN KEY([AddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Order' AND COLUMN_NAME='BusinessPartnerAddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[CRM].[Order].[BusinessPartnerAddressKey]', 'BusinessPartnerAddressKeyOld', 'COLUMN'
						ALTER TABLE [CRM].[Order] ADD [BusinessPartnerAddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[BusinessPartnerAddressKey] = b.[AddressId] FROM [Crm].[Order] a LEFT OUTER JOIN [CRM].[Address] b ON a.[BusinessPartnerAddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [CRM].[Order] ALTER COLUMN [BusinessPartnerAddressKey] uniqueidentifier NOT NULL
						ALTER TABLE [CRM].[Order] ALTER COLUMN [BusinessPartnerAddressKeyOld] int NULL
						ALTER TABLE [CRM].[Order] ADD CONSTRAINT [FK_Order_BusinessPartnerAddress] FOREIGN KEY([BusinessPartnerAddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Order' AND COLUMN_NAME='BillAddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[CRM].[Order].[BillAddressKey]', 'BillAddressKeyOld', 'COLUMN'
						ALTER TABLE [CRM].[Order] ADD [BillAddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[BillAddressKey] = b.[AddressId] FROM [Crm].[Order] a LEFT OUTER JOIN [CRM].[Address] b ON a.[BillAddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [CRM].[Order] ALTER COLUMN [BillAddressKeyOld] int NULL
						ALTER TABLE [CRM].[Order] ADD CONSTRAINT [FK_Order_BillAddress] FOREIGN KEY([BillAddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Order' AND COLUMN_NAME='DeliveryAddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[CRM].[Order].[DeliveryAddressKey]', 'DeliveryAddressKeyOld', 'COLUMN'
						ALTER TABLE [CRM].[Order] ADD [DeliveryAddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[DeliveryAddressKey] = b.[AddressId] FROM [Crm].[Order] a LEFT OUTER JOIN [CRM].[Address] b ON a.[DeliveryAddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [CRM].[Order] ALTER COLUMN [DeliveryAddressKeyOld] int NULL
						ALTER TABLE [CRM].[Order] ADD CONSTRAINT [FK_Order_DeliveryAddress] FOREIGN KEY([DeliveryAddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Person' AND COLUMN_NAME='AddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[CRM].[Person].[AddressKey]', 'AddressKeyOld', 'COLUMN'
						ALTER TABLE [CRM].[Person] ADD [AddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[AddressKey] = b.[AddressId] FROM [Crm].[Person] a LEFT OUTER JOIN [CRM].[Address] b ON a.[AddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [CRM].[Person] ALTER COLUMN [AddressKeyOld] int NULL
						ALTER TABLE [CRM].[Person] ADD CONSTRAINT [FK_Person_Address] FOREIGN KEY([AddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Visit' AND COLUMN_NAME='AddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[CRM].[Visit].[AddressKey]', 'AddressKeyOld', 'COLUMN'
						ALTER TABLE [CRM].[Visit] ADD [AddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[AddressKey] = b.[AddressId] FROM [Crm].[Visit] a LEFT OUTER JOIN [CRM].[Address] b ON a.[AddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [CRM].[Visit] ALTER COLUMN [AddressKey] uniqueidentifier NOT NULL
						ALTER TABLE [CRM].[Visit] ALTER COLUMN [AddressKeyOld] int NULL
						ALTER TABLE [CRM].[Visit] ADD CONSTRAINT [FK_Visit_Address] FOREIGN KEY([AddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='VisitReport' AND COLUMN_NAME='AddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[CRM].[VisitReport].[AddressKey]', 'AddressKeyOld', 'COLUMN'
						ALTER TABLE [CRM].[VisitReport] ADD [AddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[AddressKey] = b.[AddressId] FROM [Crm].[VisitReport] a LEFT OUTER JOIN [CRM].[Address] b ON a.[AddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [CRM].[VisitReport] ALTER COLUMN [AddressKey] uniqueidentifier NOT NULL
						ALTER TABLE [CRM].[VisitReport] ALTER COLUMN [AddressKeyOld] int NULL
						ALTER TABLE [CRM].[VisitReport] ADD CONSTRAINT [FK_VisitReport_Address] FOREIGN KEY([AddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationAddressRelationship' AND COLUMN_NAME='AddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[SMS].[InstallationAddressRelationship].[AddressKey]', 'AddressKeyOld', 'COLUMN'
						ALTER TABLE [SMS].[InstallationAddressRelationship] ADD [AddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[AddressKey] = b.[AddressId] FROM [SMS].[InstallationAddressRelationship] a LEFT OUTER JOIN [CRM].[Address] b ON a.[AddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [SMS].[InstallationAddressRelationship] ALTER COLUMN [AddressKey] uniqueidentifier NOT NULL
						ALTER TABLE [SMS].[InstallationAddressRelationship] ALTER COLUMN [AddressKeyOld] int NULL
						ALTER TABLE [SMS].[InstallationAddressRelationship] ADD CONSTRAINT [FK_InstallationAddressRelationship_Address] FOREIGN KEY([AddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationHead' AND COLUMN_NAME='LocationAddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[SMS].[InstallationHead].[LocationAddressKey]', 'LocationAddressKeyOld', 'COLUMN'
						ALTER TABLE [SMS].[InstallationHead] ADD [LocationAddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[LocationAddressKey] = b.[AddressId] FROM [SMS].[InstallationHead] a LEFT OUTER JOIN [CRM].[Address] b ON a.[LocationAddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_LocationAddress] FOREIGN KEY([LocationAddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContract' AND COLUMN_NAME='InvoiceAddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[SMS].[ServiceContract].[InvoiceAddressKey]', 'InvoiceAddressKeyOld', 'COLUMN'
						ALTER TABLE [SMS].[ServiceContract] ADD [InvoiceAddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[InvoiceAddressKey] = b.[AddressId] FROM [SMS].[ServiceContract] a LEFT OUTER JOIN [CRM].[Address] b ON a.[InvoiceAddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_InvoiceAddress] FOREIGN KEY([InvoiceAddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContract' AND COLUMN_NAME='PayerAddressId' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[SMS].[ServiceContract].[PayerAddressId]', 'PayerAddressIdOld', 'COLUMN'
						ALTER TABLE [SMS].[ServiceContract] ADD [PayerAddressId] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[PayerAddressId] = b.[AddressId] FROM [SMS].[ServiceContract] a LEFT OUTER JOIN [CRM].[Address] b ON a.[PayerAddressIdOld] = b.[AddressIdOld]')
						ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_PayerAddress] FOREIGN KEY([PayerAddressId]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContractAddressRelationship' AND COLUMN_NAME='AddressKey' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[SMS].[ServiceContractAddressRelationship].[AddressKey]', 'AddressKeyOld', 'COLUMN'
						ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD [AddressKey] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[AddressKey] = b.[AddressId] FROM [SMS].[ServiceContractAddressRelationship] a LEFT OUTER JOIN [CRM].[Address] b ON a.[AddressKeyOld] = b.[AddressIdOld]')
						ALTER TABLE [SMS].[ServiceContractAddressRelationship] ALTER COLUMN [AddressKey] uniqueidentifier NOT NULL
						ALTER TABLE [SMS].[ServiceContractAddressRelationship] ALTER COLUMN [AddressKeyOld] int NULL
						ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD CONSTRAINT [FK_ServiceContractAddressRelationship_Address] FOREIGN KEY([AddressKey]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='InvoiceRecipientAddressId' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[SMS].[ServiceOrderHead].[InvoiceRecipientAddressId]', 'InvoiceRecipientAddressIdOld', 'COLUMN'
						ALTER TABLE [SMS].[ServiceOrderHead] ADD [InvoiceRecipientAddressId] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[InvoiceRecipientAddressId] = b.[AddressId] FROM [SMS].[ServiceOrderHead] a LEFT OUTER JOIN [CRM].[Address] b ON a.[InvoiceRecipientAddressIdOld] = b.[AddressIdOld]')
						ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipientAddress] FOREIGN KEY([InvoiceRecipientAddressId]) REFERENCES [CRM].[Address] ([AddressId])
					END

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='PayerAddressId' AND DATA_TYPE = 'int')
					BEGIN
						EXEC sp_rename '[SMS].[ServiceOrderHead].[PayerAddressId]', 'PayerAddressIdOld', 'COLUMN'
						ALTER TABLE [SMS].[ServiceOrderHead] ADD [PayerAddressId] uniqueidentifier NULL
						EXEC('UPDATE a SET a.[PayerAddressId] = (SELECT TOP 1 b.[AddressId] FROM [CRM].[Address] b WHERE a.[PayerAddressIdOld] = b.[AddressIdOld]) FROM [SMS].[ServiceOrderHead] a')
						EXEC('UPDATE a SET a.[PayerAddressId] = b.[AddressId] FROM [SMS].[ServiceOrderHead] a LEFT OUTER JOIN [CRM].[Address] b ON a.[PayerAddressIdOld] = b.[AddressIdOld]')
						ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_PayerAddress] FOREIGN KEY([PayerAddressId]) REFERENCES [CRM].[Address] ([AddressId])
					END
			");
		}
	}
}