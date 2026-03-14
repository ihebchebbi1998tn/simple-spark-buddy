namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151112154900)]
	public class ChangeContactsToGuidIAlterReferences : Migration
	{
		public override void Up()
		{
			 Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_CreateDate_IsActive_ContactType_ContactId')
				BEGIN
					CREATE NONCLUSTERED INDEX [IX_CreateDate_IsActive_ContactType_ContactId] ON [CRM].[Contact]
					(
						[CreateDate] ASC,
						[IsActive] ASC,
						[ContactType] ASC,
						[ContactId] ASC
					) ON [PRIMARY]
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Address' AND COLUMN_NAME='CompanyKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Address].[CompanyKey]', 'CompanyKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Address] ADD [CompanyKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[CompanyKey] = b.[ContactId] FROM [CRM].[Address] a Join [CRM].[Contact] b ON a.[CompanyKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[Address] ADD CONSTRAINT [FK_Address_Company] FOREIGN KEY([CompanyKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Article' AND COLUMN_NAME='ArticleId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Article].[ArticleId]', 'ArticleIdOld', 'COLUMN'
					ALTER TABLE [CRM].[Article] ADD [ArticleId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ArticleId] = b.[ContactId] FROM [CRM].[Article] a Join [CRM].[Contact] b ON a.[ArticleIdOld] = b.[ContactIdOld]')							
					ALTER TABLE [CRM].[Article] ALTER COLUMN [ArticleId] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[Article] ALTER COLUMN [ArticleIdOld] int NULL
					ALTER TABLE [CRM].[Article] ADD CONSTRAINT [FK_Article_Contact] FOREIGN KEY([ArticleId]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[Article] ADD CONSTRAINT [PK_Article] PRIMARY KEY([ArticleId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ArticleRelationship' AND COLUMN_NAME='ParentArticleKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ArticleRelationship].[ParentArticleKey]', 'ParentArticleKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ArticleRelationship] ADD [ParentArticleKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ParentArticleKey] = b.[ContactId] FROM [CRM].[ArticleRelationship] a Join [CRM].[Contact] b ON a.[ParentArticleKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE [CRM].[ArticleRelationship] WHERE [ParentArticleKey] IS NULL')
					ALTER TABLE [CRM].[ArticleRelationship] ALTER COLUMN [ParentArticleKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[ArticleRelationship] ALTER COLUMN [ParentArticleKeyOld] int NULL
					ALTER TABLE [CRM].[ArticleRelationship] ADD CONSTRAINT [FK_ArticleRelationship_ParentArticle] FOREIGN KEY([ParentArticleKey]) REFERENCES [CRM].[Contact] ([ContactId])	
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ArticleRelationship' AND COLUMN_NAME='ChildArticleKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ArticleRelationship].[ChildArticleKey]', 'ChildArticleKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ArticleRelationship] ADD [ChildArticleKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ChildArticleKey] = b.[ContactId] FROM [CRM].[ArticleRelationship] a Join [CRM].[Contact] b ON a.[ChildArticleKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE [CRM].[ArticleRelationship] WHERE [ChildArticleKey] IS NULL')
					ALTER TABLE [CRM].[ArticleRelationship] ALTER COLUMN [ChildArticleKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[ArticleRelationship] ALTER COLUMN [ChildArticleKeyOld] int NULL
					ALTER TABLE [CRM].[ArticleRelationship] ADD CONSTRAINT [FK_ArticleRelationship_ChildArticle] FOREIGN KEY([ChildArticleKey]) REFERENCES [CRM].[Contact] ([ContactId])	
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Bravo' AND COLUMN_NAME='CompanyKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Bravo].[CompanyKey]', 'CompanyKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Bravo] ADD [CompanyKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[CompanyKey] = b.[ContactId] FROM [CRM].[Bravo] a Join [CRM].[Contact] b ON a.[CompanyKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE [CRM].[Bravo] WHERE [CompanyKey] IS NULL')
					ALTER TABLE [CRM].[Bravo] ALTER COLUMN [CompanyKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[Bravo] ALTER COLUMN [CompanyKeyOld] int NULL
					ALTER TABLE [CRM].[Bravo] ADD CONSTRAINT [FK_Bravo_Company] FOREIGN KEY([CompanyKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='BusinessRelationship' AND COLUMN_NAME='ParentCompanyKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[BusinessRelationship].[ParentCompanyKey]', 'ParentCompanyKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[BusinessRelationship] ADD [ParentCompanyKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ParentCompanyKey] = b.[ContactId] FROM [CRM].[BusinessRelationship] a Join [CRM].[Contact] b ON a.[ParentCompanyKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE FROM [CRM].[BusinessRelationship] WHERE [ParentCompanyKey] is null')
					ALTER TABLE [CRM].[BusinessRelationship] ALTER COLUMN [ParentCompanyKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[BusinessRelationship] ALTER COLUMN [ParentCompanyKeyOld] int NULL
					ALTER TABLE [CRM].[BusinessRelationship] ADD CONSTRAINT [FK_BusinessRelationship_ParentCompany] FOREIGN KEY([ParentCompanyKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='BusinessRelationship' AND COLUMN_NAME='ChildCompanyKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[BusinessRelationship].[ChildCompanyKey]', 'ChildCompanyKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[BusinessRelationship] ADD [ChildCompanyKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ChildCompanyKey] = b.[ContactId] FROM [CRM].[BusinessRelationship] a Join [CRM].[Contact] b ON a.[ChildCompanyKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE FROM [CRM].[BusinessRelationship] WHERE [ChildCompanyKey] is null')
					ALTER TABLE [CRM].[BusinessRelationship] ALTER COLUMN [ChildCompanyKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[BusinessRelationship] ALTER COLUMN [ChildCompanyKeyOld] int NULL
					ALTER TABLE [CRM].[BusinessRelationship] ADD CONSTRAINT [FK_BusinessRelationship_ChildCompany] FOREIGN KEY([ChildCompanyKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='CampaignContact' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[CampaignContact].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[CampaignContact] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[CampaignContact] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE FROM [CRM].[CampaignContact] WHERE [ContactKey] is null')
					ALTER TABLE [CRM].[CampaignContact] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[CampaignContact] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[CampaignContact] ADD CONSTRAINT [FK_CampaignContact_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])	
					ALTER TABLE [CRM].[CampaignContact] ADD CONSTRAINT [PK_CampaignContact] PRIMARY KEY ([CampaignKey], [ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Communication' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Communication].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Communication] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[Communication] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE FROM [CRM].[Communication] WHERE [ContactKey] is null')
					ALTER TABLE [CRM].[Communication] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[Communication] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[Communication] ADD CONSTRAINT [FK_Communication_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Company' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Company].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Company] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[Company] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE FROM [CRM].[Company] WHERE [ContactKey] is null')
					ALTER TABLE [CRM].[Company] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[Company] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[Company] ADD CONSTRAINT [FK_Company_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[Company] ADD CONSTRAINT [PK_Company] PRIMARY KEY ([ContactKey])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Contact' AND COLUMN_NAME='ParentKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Contact].[ParentKey]', 'ParentKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Contact] ADD [ParentKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ParentKey] = b.[ContactId] FROM [CRM].[Contact] a Join [CRM].[Contact] b ON a.[ParentKeyOld] = b.[ContactIdOld]')		
					ALTER TABLE [CRM].[Contact] ADD CONSTRAINT [FK_Contact_Parent] FOREIGN KEY([ParentKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ContactTags' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ContactTags].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ContactTags] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[ContactTags] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE FROM [CRM].[ContactTags] WHERE [ContactKey] IS NULL')
					ALTER TABLE [CRM].[ContactTags] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[ContactTags] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[ContactTags] ADD CONSTRAINT [FK_ContactTags_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[ContactTags] ADD CONSTRAINT [PK_ContactTags] PRIMARY KEY ([ContactKey], [TagName])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ContactUser' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ContactUser].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ContactUser] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[ContactUser] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')		
					EXEC('DELETE [CRM].[ContactUser] WHERE [ContactKey] IS NULL')
					ALTER TABLE [CRM].[ContactUser] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[ContactUser] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[ContactUser] ADD CONSTRAINT [FK_ContactUser_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[ContactUser] ADD CONSTRAINT [PK_ContactUser] PRIMARY KEY ([Username], [ContactKey])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ContactUserGroup' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ContactUserGroup].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ContactUserGroup] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[ContactUserGroup] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [CRM].[ContactUserGroup] where ContactKey IS NULL')
					ALTER TABLE [CRM].[ContactUserGroup] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[ContactUserGroup] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[ContactUserGroup] ADD CONSTRAINT [FK_ContactUserGroup_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[ContactUserGroup] ADD CONSTRAINT [PK_ContactUserGroup] PRIMARY KEY ([ContactKey], [UserGroupKey])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DocumentAttributes' AND COLUMN_NAME='ReferenceKey' AND DATA_TYPE = 'nvarchar')
				BEGIN
					EXEC sp_rename '[CRM].[DocumentAttributes].[ReferenceKey]', 'ReferenceKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[DocumentAttributes] ADD [ReferenceKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ReferenceKey] = b.[ContactId] FROM [CRM].[DocumentAttributes] a Join [CRM].[Contact] b ON a.[ReferenceKeyOld] = b.[Name]')		
					EXEC('DELETE FROM [CRM].[DocumentAttributes] where ReferenceKey IS NULL')
					ALTER TABLE [CRM].[DocumentAttributes] ALTER COLUMN [ReferenceKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[DocumentAttributes] ALTER COLUMN [ReferenceKeyOld] nvarchar(40) NULL
					ALTER TABLE [CRM].[DocumentAttributes] ADD CONSTRAINT [FK_DocumentAttribute_Contact] FOREIGN KEY([ReferenceKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormReference' AND COLUMN_NAME='ReferenceKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[DynamicFormReference].[ReferenceKey]', 'ReferenceKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[DynamicFormReference] ADD [ReferenceKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ReferenceKey] = b.[ContactId] FROM [CRM].[DynamicFormReference] a Join [CRM].[Contact] b ON a.[ReferenceKeyOld] = b.[ContactIdOld]')		
					ALTER TABLE [CRM].[DynamicFormReference] ALTER COLUMN [ReferenceKeyOld] int NULL
					ALTER TABLE [CRM].[DynamicFormReference] ADD CONSTRAINT [FK_DynamicFormReference_Contact] FOREIGN KEY([ReferenceKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			//Current timeout limit is 30 sec but in inotec db updating this table needs 46 sec so throws timeout error
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ERPDocument' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ERPDocument].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ERPDocument] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[ERPDocument] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')		
					ALTER TABLE [CRM].[ERPDocument] ADD CONSTRAINT [FK_ErpDocument_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Note' AND COLUMN_NAME='ElementKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Note].[ElementKey]', 'ElementKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Note] ADD [ElementKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ElementKey] = b.[ContactId] FROM [CRM].[Note] a Join [CRM].[Contact] b ON a.[ElementKeyOld] = b.[ContactIdOld]')	
					ALTER TABLE [CRM].[Note] ADD CONSTRAINT [FK_Note_Contact] FOREIGN KEY([ElementKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Order' AND COLUMN_NAME='BusinessPartnerContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Order].[BusinessPartnerContactKey]', 'BusinessPartnerContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Order] ADD [BusinessPartnerContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[BusinessPartnerContactKey] = b.[ContactId] FROM [CRM].[Order] a Join [CRM].[Contact] b ON a.[BusinessPartnerContactKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[Order] ALTER COLUMN [BusinessPartnerContactKey] uniqueidentifier NOT NULL
					EXEC('DELETE FROM [CRM].[Order] WHERE [BusinessPartnerContactKey] is null')
					ALTER TABLE [CRM].[Order] ALTER COLUMN [BusinessPartnerContactKeyOld] int NULL
					ALTER TABLE [CRM].[Order] ADD CONSTRAINT [FK_Order_BusinessPartnerContact] FOREIGN KEY([BusinessPartnerContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Order' AND COLUMN_NAME='ContactPerson' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Order].[ContactPerson]', 'ContactPersonOld', 'COLUMN'
					ALTER TABLE [CRM].[Order] ADD [ContactPerson] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactPerson] = b.[ContactId] FROM [CRM].[Order] a Join [CRM].[Contact] b ON a.[ContactPersonOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[Order] ADD CONSTRAINT [FK_Order_ContactPerson] FOREIGN KEY([ContactPerson]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='OrderItem' AND COLUMN_NAME='ArticleKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[OrderItem].[ArticleKey]', 'ArticleKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[OrderItem] ADD [ArticleKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ArticleKey] = b.[ContactId] FROM [CRM].[OrderItem] a Join [CRM].[Contact] b ON a.[ArticleKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[OrderItem] ADD CONSTRAINT [FK_OrderItem_Article] FOREIGN KEY([ArticleKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Person' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Person].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Person] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[Person] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[Person] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					EXEC('DELETE FROM [CRM].[Person] WHERE [ContactKey] is null')
					ALTER TABLE [CRM].[Person] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[Person] ADD CONSTRAINT [FK_Person_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[Person] ADD CONSTRAINT [PK_Person] PRIMARY KEY ([ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='PersonPosition' AND COLUMN_NAME='PersonKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[PersonPosition].[PersonKey]', 'PersonKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[PersonPosition] ADD [PersonKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[PersonKey] = b.[ContactId] FROM [CRM].[PersonPosition] a Join [CRM].[Contact] b ON a.[PersonKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [CRM].[PersonPosition] WHERE [PersonKey] is null')
					ALTER TABLE [CRM].[PersonPosition] ALTER COLUMN [PersonKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[PersonPosition] ALTER COLUMN [PersonKeyOld] int NULL
					ALTER TABLE [CRM].[PersonPosition] ADD CONSTRAINT [FK_PersonPosition_Person] FOREIGN KEY([PersonKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Project' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Project].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Project] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[Project] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [CRM].[Project] WHERE [ContactKey] is null')
					ALTER TABLE [CRM].[Project] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[Project] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[Project] ADD CONSTRAINT [FK_Project_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[Project] ADD CONSTRAINT [PK_Project] PRIMARY KEY ([ContactKey])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Project' AND COLUMN_NAME='CompetitorId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Project].[CompetitorId]', 'CompetitorIdOld', 'COLUMN'
					ALTER TABLE [CRM].[Project] ADD [CompetitorId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[CompetitorId] = b.[ContactId] FROM [CRM].[Project] a Join [CRM].[Contact] b ON a.[CompetitorIdOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[Project] ADD CONSTRAINT [FK_Project_Competitor] FOREIGN KEY([CompetitorId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Project' AND COLUMN_NAME='FolderKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Project].[FolderKey]', 'FolderKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Project] ADD [FolderKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[FolderKey] = b.[ContactId] FROM [CRM].[Project] a Join [CRM].[Contact] b ON a.[FolderKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[Project] ADD CONSTRAINT [FK_Project_Folder] FOREIGN KEY([FolderKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ProjectContactRelationship' AND COLUMN_NAME='ProjectKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ProjectContactRelationship].[ProjectKey]', 'ProjectKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ProjectContactRelationship] ADD [ProjectKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ProjectKey] = b.[ContactId] FROM [CRM].[ProjectContactRelationship] a Join [CRM].[Contact] b ON a.[ProjectKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [CRM].[ProjectContactRelationship] WHERE [ProjectKey] is null')
					ALTER TABLE [CRM].[ProjectContactRelationship] ALTER COLUMN [ProjectKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[ProjectContactRelationship] ALTER COLUMN [ProjectKeyOld] int NULL
					ALTER TABLE [CRM].[ProjectContactRelationship] ADD CONSTRAINT [FK_ProjectContactRelationship_Project] FOREIGN KEY([ProjectKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ProjectContactRelationship' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ProjectContactRelationship].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ProjectContactRelationship] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[ProjectContactRelationship] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [CRM].[ProjectContactRelationship] WHERE [ContactKey] is null')
					ALTER TABLE [CRM].[ProjectContactRelationship] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[ProjectContactRelationship] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[ProjectContactRelationship] ADD CONSTRAINT [FK_ProjectContactRelationship_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Task' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Task].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Task] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[Task] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[Task] ADD CONSTRAINT [FK_Task_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Turnover' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Turnover].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Turnover] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[Turnover] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[Turnover] ADD CONSTRAINT [FK_Turnover_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='UserRecentPages' AND COLUMN_NAME='ContactId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[UserRecentPages].[ContactId]', 'ContactIdOld', 'COLUMN'
					ALTER TABLE [CRM].[UserRecentPages] ADD [ContactId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactId] = b.[ContactId] FROM [CRM].[UserRecentPages] a Join [CRM].[Contact] b ON a.[ContactIdOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[UserRecentPages] ADD CONSTRAINT [FK_UserRecentPage_Contact] FOREIGN KEY([ContactId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Visit' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[Visit].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Visit] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[Visit] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [CRM].[Visit] where ContactKey IS NULL')
					ALTER TABLE [CRM].[Visit] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[Visit] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[Visit] ADD CONSTRAINT [FK_Visit_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[Visit] ADD CONSTRAINT [PK_Visit] PRIMARY KEY ([ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='VisitReport' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[VisitReport].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[VisitReport] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [CRM].[VisitReport] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [CRM].[VisitReport] WHERE [ContactKey] IS NULL')
					ALTER TABLE [CRM].[VisitReport] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[VisitReport] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [CRM].[VisitReport] ADD CONSTRAINT [FK_VisitReport_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [CRM].[VisitReport] ADD CONSTRAINT [PK_VisitReport] PRIMARY KEY ([ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='VisitReport' AND COLUMN_NAME='VisitId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[VisitReport].[VisitId]', 'VisitIdOld', 'COLUMN'
					ALTER TABLE [CRM].[VisitReport] ADD [VisitId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[VisitId] = b.[ContactId] FROM [CRM].[VisitReport] a Join [CRM].[Contact] b ON a.[VisitIdOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[VisitReport] ADD CONSTRAINT [FK_VisitReport_Visit] FOREIGN KEY([VisitId]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='VisitReportAnswer' AND COLUMN_NAME='VisitReportKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[VisitReportAnswer].[VisitReportKey]', 'VisitReportKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[VisitReportAnswer] ADD [VisitReportKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[VisitReportKey] = b.[ContactId] FROM [CRM].[VisitReportAnswer] a Join [CRM].[Contact] b ON a.[VisitReportKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [CRM].[VisitReportAnswer] ADD CONSTRAINT [FK_VisitReportAnswer_VisitReport] FOREIGN KEY([VisitReportKey]) REFERENCES [CRM].[Contact] ([ContactId])	
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='VisitTopic' AND COLUMN_NAME='VisitKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[VisitTopic].[VisitKey]', 'VisitKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[VisitTopic] ADD [VisitKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[VisitKey] = b.[ContactId] FROM [CRM].[VisitTopic] a Join [CRM].[Contact] b ON a.[VisitKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [CRM].[VisitTopic] WHERE [VisitKey] IS NULL')
					ALTER TABLE [CRM].[VisitTopic] ALTER COLUMN [VisitKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[VisitTopic] ALTER COLUMN [VisitKeyOld] int NULL
					ALTER TABLE [CRM].[VisitTopic] ADD CONSTRAINT [FK_VisitTopic_Visit] FOREIGN KEY([VisitKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='RPL' AND TABLE_NAME='Dispatch' AND COLUMN_NAME='DispatchOrderKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[RPL].[Dispatch].[DispatchOrderKey]', 'DispatchOrderKeyOld', 'COLUMN'
					ALTER TABLE [RPL].[Dispatch] ADD [DispatchOrderKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DispatchOrderKey] = b.[ContactId] FROM [RPL].[Dispatch] a Join [CRM].[Contact] b ON a.[DispatchOrderKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [RPL].[Dispatch] ADD CONSTRAINT [FK_Dispatch_DispatchOrder] FOREIGN KEY([DispatchOrderKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationAdditionalContacts' AND COLUMN_NAME='InstallationId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[InstallationAdditionalContacts].[InstallationId]', 'InstallationIdOld', 'COLUMN'
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ADD [InstallationId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InstallationId] = b.[ContactId] FROM [SMS].[InstallationAdditionalContacts] a Join [CRM].[Contact] b ON a.[InstallationIdOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[InstallationAdditionalContacts] WHERE [InstallationId] IS NULL')
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ALTER COLUMN [InstallationId] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ALTER COLUMN [InstallationIdOld] int NULL
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ADD CONSTRAINT [FK_InstallationAdditionalContact_Installation] FOREIGN KEY([InstallationId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationAdditionalContacts' AND COLUMN_NAME='ContactId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[InstallationAdditionalContacts].[ContactId]', 'ContactIdOld', 'COLUMN'
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ADD [ContactId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactId] = b.[ContactId] FROM [SMS].[InstallationAdditionalContacts] a Join [CRM].[Contact] b ON a.[ContactIdOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[InstallationAdditionalContacts] WHERE [ContactId] IS NULL')
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ALTER COLUMN [ContactId] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ALTER COLUMN [ContactIdOld] int NULL
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ADD CONSTRAINT [FK_InstallationAdditionalContact_Contact] FOREIGN KEY([ContactId]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [SMS].[InstallationAdditionalContacts] ADD CONSTRAINT [PK_InstallationAdditionalContacts] PRIMARY KEY ([InstallationId], [ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationAddressRelationship' AND COLUMN_NAME='InstallationKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[InstallationAddressRelationship].[InstallationKey]', 'InstallationKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[InstallationAddressRelationship] ADD [InstallationKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InstallationKey] = b.[ContactId] FROM [SMS].[InstallationAddressRelationship] a Join [CRM].[Contact] b ON a.[InstallationKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[InstallationAddressRelationship] WHERE [InstallationKey] IS NULL')
					ALTER TABLE [SMS].[InstallationAddressRelationship] ALTER COLUMN [InstallationKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[InstallationAddressRelationship] ALTER COLUMN [InstallationKeyOld] int NULL
					ALTER TABLE [SMS].[InstallationAddressRelationship] ADD CONSTRAINT [FK_InstallationAddressRelationship_Installation] FOREIGN KEY([InstallationKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationHead' AND COLUMN_NAME='LocationContactId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[InstallationHead].[LocationContactId]', 'LocationContactIdOld', 'COLUMN'
					ALTER TABLE [SMS].[InstallationHead] ADD [LocationContactId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[LocationContactId] = b.[ContactId] FROM [SMS].[InstallationHead] a Join [CRM].[Contact] b ON a.[LocationContactIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_LocationContact] FOREIGN KEY([LocationContactId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationHead' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[InstallationHead].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[InstallationHead] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [SMS].[InstallationHead] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[InstallationHead] WHERE [ContactKey] IS NULL')
					ALTER TABLE [SMS].[InstallationHead] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[InstallationHead] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationHead' AND COLUMN_NAME='LocationPersonId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[InstallationHead].[LocationPersonId]', 'LocationPersonIdOld', 'COLUMN'
					ALTER TABLE [SMS].[InstallationHead] ADD [LocationPersonId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[LocationPersonId] = b.[ContactId] FROM [SMS].[InstallationHead] a Join [CRM].[Contact] b ON a.[LocationPersonIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_LocationPerson] FOREIGN KEY([LocationPersonId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='InstallationHead' AND COLUMN_NAME='FolderKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[InstallationHead].[FolderKey]', 'FolderKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[InstallationHead] ADD [FolderKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[FolderKey] = b.[ContactId] FROM [SMS].[InstallationHead] a Join [CRM].[Contact] b ON a.[FolderKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_Folder] FOREIGN KEY([FolderKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='MaintenancePlan' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[MaintenancePlan].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[MaintenancePlan] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [SMS].[MaintenancePlan] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[MaintenancePlan] WHERE [ContactKey] IS NULL')
					ALTER TABLE [SMS].[MaintenancePlan] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[MaintenancePlan] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [SMS].[MaintenancePlan] ADD CONSTRAINT [FK_MaintenancePlan_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [SMS].[MaintenancePlan] ADD CONSTRAINT [PK_MaintenancePlan] PRIMARY KEY ([ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='MaintenancePlan' AND COLUMN_NAME='ServiceContractKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[MaintenancePlan].[ServiceContractKey]', 'ServiceContractKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[MaintenancePlan] ADD [ServiceContractKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ServiceContractKey] = b.[ContactId] FROM [SMS].[MaintenancePlan] a Join [CRM].[Contact] b ON a.[ServiceContractKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[MaintenancePlan] WHERE [ServiceContractKey] IS NULL')
					ALTER TABLE [SMS].[MaintenancePlan] ALTER COLUMN [ServiceContractKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[MaintenancePlan] ALTER COLUMN [ServiceContractKeyOld] int NULL
					ALTER TABLE [SMS].[MaintenancePlan] ADD CONSTRAINT [FK_MaintenancePlan_ServiceContract] FOREIGN KEY([ServiceContractKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContract' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceContract].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceContract] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [SMS].[ServiceContract] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceContract] WHERE [ContactKey] IS NULL')
					ALTER TABLE [SMS].[ServiceContract] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceContract] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [PK_ServiceContract] PRIMARY KEY ([ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContract' AND COLUMN_NAME='InvoiceRecipientId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceContract].[InvoiceRecipientId]', 'InvoiceRecipientIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceContract] ADD [InvoiceRecipientId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InvoiceRecipientId] = b.[ContactId] FROM [SMS].[ServiceContract] a Join [CRM].[Contact] b ON a.[InvoiceRecipientIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_InvoiceRecipient] FOREIGN KEY([InvoiceRecipientId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContract' AND COLUMN_NAME='PayerId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceContract].[PayerId]', 'PayerIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceContract] ADD [PayerId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[PayerId] = b.[ContactId] FROM [SMS].[ServiceContract] a Join [CRM].[Contact] b ON a.[PayerIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_Payer] FOREIGN KEY([PayerId]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContract' AND COLUMN_NAME='ServiceObjectId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceContract].[ServiceObjectId]', 'ServiceObjectIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceContract] ADD [ServiceObjectId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ServiceObjectId] = b.[ContactId] FROM [SMS].[ServiceContract] a Join [CRM].[Contact] b ON a.[ServiceObjectIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceObject_ServiceObject] FOREIGN KEY([ServiceObjectId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContractAddressRelationship' AND COLUMN_NAME='ServiceContractKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceContractAddressRelationship].[ServiceContractKey]', 'ServiceContractKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD [ServiceContractKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ServiceContractKey] = b.[ContactId] FROM [SMS].[ServiceContractAddressRelationship] a Join [CRM].[Contact] b ON a.[ServiceContractKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceContractAddressRelationship] WHERE [ServiceContractKey] IS NULL')
					ALTER TABLE [SMS].[ServiceContractAddressRelationship] ALTER COLUMN [ServiceContractKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceContractAddressRelationship] ALTER COLUMN [ServiceContractKeyOld] int NULL
					ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD CONSTRAINT [FK_ServiceContractAddressRelationship_ServiceContract] FOREIGN KEY([ServiceContractKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContractInstallationRelationship' AND COLUMN_NAME='ServiceContractKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceContractInstallationRelationship].[ServiceContractKey]', 'ServiceContractKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD [ServiceContractKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ServiceContractKey] = b.[ContactId] FROM [SMS].[ServiceContractInstallationRelationship] a Join [CRM].[Contact] b ON a.[ServiceContractKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceContractInstallationRelationship] WHERE [ServiceContractKey] IS NULL')
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ALTER COLUMN [ServiceContractKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ALTER COLUMN [ServiceContractKeyOld] int NULL
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD CONSTRAINT [FK_ServiceContractInstallationRelationship_ServiceContract] FOREIGN KEY([ServiceContractKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceContractInstallationRelationship' AND COLUMN_NAME='InstallationKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceContractInstallationRelationship].[InstallationKey]', 'InstallationKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD [InstallationKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InstallationKey] = b.[ContactId] FROM [SMS].[ServiceContractInstallationRelationship] a Join [CRM].[Contact] b ON a.[InstallationKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceContractInstallationRelationship] WHERE [InstallationKey] IS NULL')
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ALTER COLUMN [InstallationKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ALTER COLUMN [InstallationKeyOld] int NULL
					ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD CONSTRAINT [FK_ServiceContractInstallationRelationship_Installation] FOREIGN KEY([InstallationKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceNotifications' AND COLUMN_NAME='Id')
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP COLUMN [Id]
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceNotifications' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceNotifications].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceNotifications] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [SMS].[ServiceNotifications] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceNotifications] WHERE [ContactKey] IS NULL')
					ALTER TABLE [SMS].[ServiceNotifications] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceNotifications] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotification_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceNotifications' AND COLUMN_NAME='ContactPersonKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceNotifications].[ContactPersonKey]', 'ContactPersonKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceNotifications] ADD [ContactPersonKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactPersonKey] = b.[ContactId] FROM [SMS].[ServiceNotifications] a Join [CRM].[Contact] b ON a.[ContactPersonKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotification_ContactPerson] FOREIGN KEY([ContactPersonKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceNotifications' AND COLUMN_NAME='AffectedCompanyKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceNotifications].[AffectedCompanyKey]', 'AffectedCompanyKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceNotifications] ADD [AffectedCompanyKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[AffectedCompanyKey] = b.[ContactId] FROM [SMS].[ServiceNotifications] a Join [CRM].[Contact] b ON a.[AffectedCompanyKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotification_AffectedCompany] FOREIGN KEY([AffectedCompanyKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceNotifications' AND COLUMN_NAME='AffectedInstallationKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceNotifications].[AffectedInstallationKey]', 'AffectedInstallationKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceNotifications] ADD [AffectedInstallationKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[AffectedInstallationKey] = b.[ContactId] FROM [SMS].[ServiceNotifications] a Join [CRM].[Contact] b ON a.[AffectedInstallationKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotification_AffectedInstallation] FOREIGN KEY([AffectedInstallationKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceObject' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceObject].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceObject] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [SMS].[ServiceObject] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceObject] WHERE [ContactKey] IS NULL')
					ALTER TABLE [SMS].[ServiceObject] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceObject] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [SMS].[ServiceObject] ADD CONSTRAINT [FK_ServiceObject_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderDispatch' AND COLUMN_NAME='OrderId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderDispatch].[OrderId]', 'OrderIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderDispatch] ADD [OrderId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[OrderId] = b.[ContactId] FROM [SMS].[ServiceOrderDispatch] a Join [CRM].[Contact] b ON a.[OrderIdOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceOrderDispatch] WHERE [OrderId] IS NULL')
					ALTER TABLE [SMS].[ServiceOrderDispatch] ALTER COLUMN [OrderId] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceOrderDispatch] ALTER COLUMN [OrderIdOld] int NULL
					ALTER TABLE [SMS].[ServiceOrderDispatch] ADD CONSTRAINT [FK_ServiceOrderDispatch_OrderId] FOREIGN KEY([OrderId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='CustomerContactId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[CustomerContactId]', 'CustomerContactIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [CustomerContactId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[CustomerContactId] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[CustomerContactIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_CustomerContact] FOREIGN KEY([CustomerContactId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='MaintenancePlanId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[MaintenancePlanId]', 'MaintenancePlanIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [MaintenancePlanId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[MaintenancePlanId] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[MaintenancePlanIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_MaintenancePlan] FOREIGN KEY([MaintenancePlanId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='PredecessorNotificationId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[PredecessorNotificationId]', 'PredecessorNotificationIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [PredecessorNotificationId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[PredecessorNotificationId] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[PredecessorNotificationIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_PredecessorNotification] FOREIGN KEY([PredecessorNotificationId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='ContactKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[ContactKey]', 'ContactKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [ContactKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ContactKey] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[ContactKeyOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceOrderHead] WHERE [ContactKey] IS NULL')
					ALTER TABLE [SMS].[ServiceOrderHead] ALTER COLUMN [ContactKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceOrderHead] ALTER COLUMN [ContactKeyOld] int NULL
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_Contact] FOREIGN KEY([ContactKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='ServiceCaseKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[ServiceCaseKey]', 'ServiceCaseKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [ServiceCaseKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ServiceCaseKey] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[ServiceCaseKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_ServiceCase] FOREIGN KEY([ServiceCaseKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='InitiatorId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[InitiatorId]', 'InitiatorIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [InitiatorId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InitiatorId] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[InitiatorIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_Initiator] FOREIGN KEY([InitiatorId]) REFERENCES [CRM].[Company] ([ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='PayerId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[PayerId]', 'PayerIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [PayerId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[PayerId] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[PayerIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_Payer] FOREIGN KEY([PayerId]) REFERENCES [CRM].[Company] ([ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='InvoiceRecipientId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[InvoiceRecipientId]', 'InvoiceRecipientIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [InvoiceRecipientId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InvoiceRecipientId] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[InvoiceRecipientIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipient] FOREIGN KEY([InvoiceRecipientId]) REFERENCES [CRM].[Company] ([ContactKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='ServiceObjectId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[ServiceObjectId]', 'ServiceObjectIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [ServiceObjectId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ServiceObjectId] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[ServiceObjectIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_ServiceObject] FOREIGN KEY([ServiceObjectId]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='InitiatorPersonId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[InitiatorPersonId]', 'InitiatorPersonIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [InitiatorPersonId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InitiatorPersonId] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[InitiatorPersonIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_InitiatorPerson] FOREIGN KEY([InitiatorPersonId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='AffectedInstallationKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[AffectedInstallationKey]', 'AffectedInstallationKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [AffectedInstallationKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[AffectedInstallationKey] = b.[ContactId] FROM [SMS].[ServiceOrderHead] a Join [CRM].[Contact] b ON a.[AffectedInstallationKeyOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_AffectedInstallation] FOREIGN KEY([AffectedInstallationKey]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderSkill' AND COLUMN_NAME='ServiceOrderId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderSkill].[ServiceOrderId]', 'ServiceOrderIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderSkill] ADD [ServiceOrderId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ServiceOrderId] = b.[ContactId] FROM [SMS].[ServiceOrderSkill] a Join [CRM].[Contact] b ON a.[ServiceOrderIdOld] = b.[ContactIdOld]')
					EXEC('DELETE FROM [SMS].[ServiceOrderSkill] WHERE [ServiceOrderId] IS NULL')
					ALTER TABLE [SMS].[ServiceOrderSkill] ALTER COLUMN [ServiceOrderId] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ServiceOrderSkill] ALTER COLUMN [ServiceOrderIdOld] int NULL
					ALTER TABLE [SMS].[ServiceOrderSkill] ADD CONSTRAINT [FK_ServiceOrderSkill_ServiceOrder] FOREIGN KEY([ServiceOrderId]) REFERENCES [CRM].[Contact] ([ContactId])
					ALTER TABLE [SMS].[ServiceOrderSkill] ADD CONSTRAINT [PK_ServiceOrderSkill] PRIMARY KEY ([ServiceOrderId], [SkillKey])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderTimes' AND COLUMN_NAME='InstallationId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderTimes].[InstallationId]', 'InstallationIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderTimes] ADD [InstallationId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InstallationId] = b.[ContactId] FROM [SMS].[ServiceOrderTimes] a Join [CRM].[Contact] b ON a.[InstallationIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[ServiceOrderTimes] ADD CONSTRAINT [FK_ServiceOrderTimes_Installation] FOREIGN KEY([InstallationId]) REFERENCES [CRM].[Contact] ([ContactId])
				END

				IF EXISTS (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='TimeManagementEvent' AND COLUMN_NAME='InstallationId' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[TimeManagementEvent].[InstallationId]', 'InstallationIdOld', 'COLUMN'
					ALTER TABLE [SMS].[TimeManagementEvent] ADD [InstallationId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[InstallationId] = b.[ContactId] FROM [SMS].[TimeManagementEvent] a Join [CRM].[Contact] b ON a.[InstallationIdOld] = b.[ContactIdOld]')
					ALTER TABLE [SMS].[TimeManagementEvent] ADD CONSTRAINT [FK_TimeManagementEvent_Installation] FOREIGN KEY([InstallationId]) REFERENCES [CRM].[Contact] ([ContactId])
				END");

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