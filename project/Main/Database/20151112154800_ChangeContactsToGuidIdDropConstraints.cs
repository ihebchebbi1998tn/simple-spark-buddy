namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151112154800)]
	public class ChangeContactsToGuidIdDropConstraints : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_CreateDate_IsActive_ContactType_ContactId')
				BEGIN
					DROP INDEX [IX_CreateDate_IsActive_ContactType_ContactId] ON [CRM].[Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Address_Contact')
				BEGIN
					ALTER TABLE [CRM].[Address] DROP CONSTRAINT [FK_Address_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ArticleRelationship_ChildArticleKey_Article')
				BEGIN
					ALTER TABLE [CRM].[ArticleRelationship] DROP CONSTRAINT [FK_ArticleRelationship_ChildArticleKey_Article]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ArticleRelationship_ParentArticleKey_Article')
				BEGIN
					ALTER TABLE [CRM].[ArticleRelationship] DROP CONSTRAINT [FK_ArticleRelationship_ParentArticleKey_Article]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_OrderItem_Article')
				BEGIN
					ALTER TABLE [CRM].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Article]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Article')
				BEGIN
					ALTER TABLE [CRM].[Article] DROP CONSTRAINT [PK_Article]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_BusinessRelationship_Child')
				BEGIN
					ALTER TABLE [CRM].[BusinessRelationship] DROP CONSTRAINT [FK_BusinessRelationship_Child]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_BusinessRelationship_Parent')
				BEGIN
					ALTER TABLE [CRM].[BusinessRelationship] DROP CONSTRAINT [FK_BusinessRelationship_Parent]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_CampaignContact_Contact')
				BEGIN
					ALTER TABLE [CRM].[CampaignContact] DROP CONSTRAINT [FK_CampaignContact_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_CampaignContact')
				BEGIN
					ALTER TABLE [CRM].[CampaignContact] DROP CONSTRAINT [PK_CampaignContact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Communication_Contact')
				BEGIN
					ALTER TABLE [CRM].[Communication] DROP CONSTRAINT [FK_Communication_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Company_Contact')
				BEGIN
					ALTER TABLE [CRM].[Company] DROP CONSTRAINT [FK_Company_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Initiator')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_Initiator]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_InvoiceRecipient')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipient]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Payer')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_Payer]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Company')
				BEGIN
					ALTER TABLE [CRM].[Company] DROP CONSTRAINT [PK_Company]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Contact_Parent')
				BEGIN
					ALTER TABLE [CRM].[Contact] DROP CONSTRAINT [FK_Contact_Parent]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Contact_ParentKey')
				BEGIN
					ALTER TABLE [CRM].[Contact] DROP CONSTRAINT [FK_Contact_ParentKey]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ContactTags_Contact')
				BEGIN
					ALTER TABLE [CRM].[ContactTags] DROP CONSTRAINT [FK_ContactTags_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ContactTags')
				BEGIN
					ALTER TABLE [CRM].[ContactTags] DROP CONSTRAINT [PK_ContactTags]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ContactUser')
				BEGIN
					ALTER TABLE [CRM].[ContactUser] DROP CONSTRAINT [PK_ContactUser]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ContactUserGroup')
				BEGIN
					ALTER TABLE [CRM].[ContactUserGroup] DROP CONSTRAINT [PK_ContactUserGroup]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ErpDocument_Contact')
				BEGIN
					ALTER TABLE [CRM].[ERPDocument] DROP CONSTRAINT [FK_ErpDocument_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Note_Contact')
				BEGIN
					ALTER TABLE [CRM].[Note] DROP CONSTRAINT [FK_Note_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Order_Contact')
				BEGIN
					ALTER TABLE [CRM].[Order] DROP CONSTRAINT [FK_Order_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Order_Person')
				BEGIN
					ALTER TABLE [CRM].[Order] DROP CONSTRAINT [FK_Order_Person]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_OrderItem_Article')
				BEGIN
					ALTER TABLE [CRM].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Article]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Person_Contact')
				BEGIN
					ALTER TABLE [CRM].[Person] DROP CONSTRAINT [FK_Person_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Person')
				BEGIN
					ALTER TABLE [CRM].[Person] DROP CONSTRAINT [PK_Person]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Project_Contact')
				BEGIN
					ALTER TABLE [CRM].[Project] DROP CONSTRAINT [FK_Project_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Project_Folder')
				BEGIN
					ALTER TABLE [CRM].[Project] DROP CONSTRAINT [FK_Project_Folder]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ProjectContactRelationship_Contact')
				BEGIN
					ALTER TABLE [CRM].[ProjectContactRelationship] DROP CONSTRAINT [FK_ProjectContactRelationship_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ProjectContactRelationship_Project')
				BEGIN
					ALTER TABLE [CRM].[ProjectContactRelationship] DROP CONSTRAINT [FK_ProjectContactRelationship_Project]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Project')
				BEGIN
					ALTER TABLE [CRM].[Project] DROP CONSTRAINT [PK_Project]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Task_Contact')
				BEGIN
					ALTER TABLE [CRM].[Task] DROP CONSTRAINT [FK_Task_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_UserRecentPages_Contact')
				BEGIN
					ALTER TABLE [CRM].[UserRecentPages] DROP CONSTRAINT [FK_UserRecentPages_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Visit_Contact')
				BEGIN
					ALTER TABLE [CRM].[Visit] DROP CONSTRAINT [FK_Visit_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_VisitReport_Contact')
				BEGIN
					ALTER TABLE [CRM].[VisitReport] DROP CONSTRAINT [FK_VisitReport_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_VisitReport')
				BEGIN
					ALTER TABLE [CRM].[VisitReport] DROP CONSTRAINT [PK_VisitReport]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_VisitTopic_Visit')
				BEGIN
					ALTER TABLE [CRM].[VisitTopic] DROP CONSTRAINT [FK_VisitTopic_Visit]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Visit')
				BEGIN
					ALTER TABLE [CRM].[Visit] DROP CONSTRAINT [PK_Visit]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_InstallationAdditionalContacts')
				BEGIN
					ALTER TABLE [SMS].[InstallationAdditionalContacts] DROP CONSTRAINT [PK_InstallationAdditionalContacts]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_InstallationAddressRelationship_Installation')
				BEGIN
					ALTER TABLE [SMS].[InstallationAddressRelationship] DROP CONSTRAINT [FK_InstallationAddressRelationship_Installation]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_InstallationHead_Contact')
				BEGIN
					ALTER TABLE [SMS].[InstallationHead] DROP CONSTRAINT [FK_InstallationHead_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_InstallationHead_Folder')
				BEGIN
					ALTER TABLE [SMS].[InstallationHead] DROP CONSTRAINT [FK_InstallationHead_Folder]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_InstallationHead_LocationContact')
				BEGIN
					ALTER TABLE [SMS].[InstallationHead] DROP CONSTRAINT [FK_InstallationHead_LocationContact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_InstallationHead_LocationPerson')
				BEGIN
					ALTER TABLE [SMS].[InstallationHead] DROP CONSTRAINT [FK_InstallationHead_LocationPerson]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_MaintenancePlan_Contact')
				BEGIN
					ALTER TABLE [SMS].[MaintenancePlan] DROP CONSTRAINT [FK_MaintenancePlan_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_MaintenancePlan_ServiceContract')
				BEGIN
					ALTER TABLE [SMS].[MaintenancePlan] DROP CONSTRAINT [FK_MaintenancePlan_ServiceContract]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_SmsMaintenancePlan')
				BEGIN
					ALTER TABLE [SMS].[MaintenancePlan] DROP CONSTRAINT [PK_SmsMaintenancePlan]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContract_Contact')
				BEGIN
					ALTER TABLE [SMS].[ServiceContract] DROP CONSTRAINT [FK_ServiceContract_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContract_InvoiceRecipient')
				BEGIN
					ALTER TABLE [SMS].[ServiceContract] DROP CONSTRAINT [FK_ServiceContract_InvoiceRecipient]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContract_Payer')
				BEGIN
					ALTER TABLE [SMS].[ServiceContract] DROP CONSTRAINT [FK_ServiceContract_Payer]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContract_ServiceObject')
				BEGIN
					ALTER TABLE [SMS].[ServiceContract] DROP CONSTRAINT [FK_ServiceContract_ServiceObject]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ServiceContract')
				BEGIN
					ALTER TABLE [SMS].[ServiceContract] DROP CONSTRAINT [PK_ServiceContract]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContractAddressRelationship_ServiceContract')
				BEGIN
					ALTER TABLE [SMS].[ServiceContractAddressRelationship] DROP CONSTRAINT [FK_ServiceContractAddressRelationship_ServiceContract]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContractInstallationRelationship_Installation')
				BEGIN
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] DROP CONSTRAINT [FK_ServiceContractInstallationRelationship_Installation]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceContractInstallationRelationship_ServiceContract')
				BEGIN
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] DROP CONSTRAINT [FK_ServiceContractInstallationRelationship_ServiceContract]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ServiceNotifications')
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP CONSTRAINT [PK_ServiceNotifications]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceNotifications_Contact')
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP CONSTRAINT [FK_ServiceNotifications_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceNotifications_Contact1')
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP CONSTRAINT [FK_ServiceNotifications_Contact1]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceNotifications_Contact2')
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP CONSTRAINT [FK_ServiceNotifications_Contact2]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceNotifications_Installation')
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP CONSTRAINT [FK_ServiceNotifications_Installation]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceObject_Contact')
				BEGIN
					ALTER TABLE [SMS].[ServiceObject] DROP CONSTRAINT [FK_ServiceObject_Contact]
				END

				IF EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND OBJECT_NAME(parent_object_id) = N'ServiceObject')
				BEGIN
					DECLARE @ServiceObjectPkName AS NVARCHAR(50)
					SET @ServiceObjectPkName = (SELECT name FROM sys.key_constraints WHERE type = 'PK' AND OBJECT_NAME(parent_object_id) = N'ServiceObject')
					EXEC('ALTER TABLE [SMS].[ServiceObject] DROP CONSTRAINT ' + @ServiceObjectPkName)
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderDispatch_Contact')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderDispatch] DROP CONSTRAINT [FK_ServiceOrderDispatch_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Contact')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_Contact]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Contact1')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_Contact1]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Contact2')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_Contact2]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Initiator')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_Initiator]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_InitiatorPerson')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_InitiatorPerson]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_InvoiceRecipient')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipient]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_Payer')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_Payer]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_ServiceNotifications')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_ServiceNotifications]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'UC_ContactKey')
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP CONSTRAINT [UC_ContactKey]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_ServiceNotifications1')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_ServiceNotifications1]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_ServiceObject')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_ServiceObject]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ServiceOrderSkill')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderSkill] DROP CONSTRAINT [PK_ServiceOrderSkill]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderTimes_Installation')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderTimes] DROP CONSTRAINT [FK_ServiceOrderTimes_Installation]
				END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_InstallationAdditionalContacts_Contact')
				BEGIN
					ALTER TABLE [SMS].[InstallationAdditionalContacts] DROP CONSTRAINT [FK_InstallationAdditionalContacts_Contact]
				END

				IF EXISTS (SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'[CRM].[Contact]') AND name = N'FK_Person_Company')
				BEGIN
					ALTER TABLE [CRM].[Contact] DROP CONSTRAINT [FK_Person_Company]
				END

				IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Contact_ContactType')
				BEGIN
					DROP INDEX [IX_Contact_ContactType] ON [CRM].[Contact]
				END

				IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Contact_ContactType_ContactId_LegacyId')
				BEGIN
					DROP INDEX [IX_Contact_ContactType_ContactId_LegacyId] ON [CRM].[Contact]
				END

				IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Contact_ContactId_ContactType_IsExported_LegacyId')
				BEGIN
					DROP INDEX [IX_Contact_ContactId_ContactType_IsExported_LegacyId] ON [CRM].[Contact]
				END

				IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Contact]') AND name = N'IX_ContactId')
				BEGIN
					DROP INDEX [IX_ContactId] ON [CRM].[Contact]
				END

				TRUNCATE TABLE [CRM].[UserRecentPages]");
		}
	}
}