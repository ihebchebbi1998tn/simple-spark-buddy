namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Library.Model;

	[Migration(20180606135500)]
	public class ChangeCrmUsergroupToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceNotifications_Usergroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceNotifications' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [SMS].[ServiceNotifications] DROP CONSTRAINT [FK_ServiceNotifications_Usergroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_PreferredTechnicianUsergroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='PreferredTechnicianUsergroup' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_PreferredTechnicianUsergroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderHead_UserGroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [SMS].[ServiceOrderHead] DROP CONSTRAINT [FK_ServiceOrderHead_UserGroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ContactUserGroup_Usergroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ContactUserGroup' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [CRM].[ContactUserGroup] DROP CONSTRAINT [FK_ContactUserGroup_Usergroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_OrderUsergroup_Usergroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='OrderUsergroup' AND COLUMN_NAME='UsergroupKey' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [CRM].[OrderUsergroup] DROP CONSTRAINT [FK_OrderUsergroup_Usergroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_UserUserGroup_UserGroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='UserUserGroup' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [CRM].[UserUserGroup] DROP CONSTRAINT [FK_UserUserGroup_UserGroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ContactUserGroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ContactUserGroup' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [CRM].[ContactUserGroup] DROP CONSTRAINT [PK_ContactUserGroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_UserUserGroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='UserUserGroup' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [CRM].[UserUserGroup] DROP CONSTRAINT [PK_UserUserGroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_OrderUsergroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='OrderUsergroup' AND COLUMN_NAME='UsergroupKey' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [CRM].[OrderUsergroup] DROP CONSTRAINT [PK_OrderUsergroup]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_UserGroup') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Usergroup' AND COLUMN_NAME='UserGroupId' AND DATA_TYPE = 'int')
					BEGIN
						ALTER TABLE [CRM].[Usergroup] DROP CONSTRAINT [PK_UserGroup]
					END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Usergroup' AND COLUMN_NAME='UserGroupId' AND DATA_TYPE = 'int')
				BEGIN
					ALTER TABLE [CRM].[Usergroup] ADD [UserGroupIdOld] int NULL
					EXEC('UPDATE [CRM].[Usergroup] SET [UserGroupIdOld] = [UserGroupId]')
					ALTER TABLE [CRM].[Usergroup] DROP COLUMN [UserGroupId]
					ALTER TABLE [CRM].[Usergroup] ADD [UsergroupId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [CRM].[Usergroup] ADD CONSTRAINT [PK_Usergroup] PRIMARY KEY CLUSTERED ([UsergroupId] ASC)
				END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceNotifications' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceNotifications].[UserGroupKey]', 'UserGroupKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceNotifications] ADD [UsergroupKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[UsergroupKey] = b.[UsergroupId] FROM [SMS].[ServiceNotifications] a LEFT OUTER JOIN [CRM].[Usergroup] b ON a.[UserGroupKeyOld] = b.[UserGroupIdOld]')
					ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_Usergroup] FOREIGN KEY([UsergroupKey]) REFERENCES [CRM].[Usergroup]([UsergroupId]) ON UPDATE CASCADE ON DELETE SET NULL
				END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='PreferredTechnicianUsergroup' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[PreferredTechnicianUsergroup]', 'PreferredTechnicianUsergroupOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [PreferredTechnicianUsergroup] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[PreferredTechnicianUsergroup] = b.[UsergroupId] FROM [SMS].[ServiceOrderHead] a LEFT OUTER JOIN [CRM].[Usergroup] b ON a.[PreferredTechnicianUsergroupOld] = b.[UserGroupIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_PreferredTechnicianUsergroup] FOREIGN KEY([PreferredTechnicianUsergroup]) REFERENCES [CRM].[Usergroup]([UsergroupId])
				END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderHead].[UserGroupKey]', 'UserGroupKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [UsergroupKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[UsergroupKey] = b.[UsergroupId] FROM [SMS].[ServiceOrderHead] a LEFT OUTER JOIN [CRM].[Usergroup] b ON a.[UserGroupKeyOld] = b.[UserGroupIdOld]')
					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_UserGroup] FOREIGN KEY([UsergroupKey]) REFERENCES [CRM].[Usergroup]([UsergroupId])
				END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ContactUserGroup' AND COLUMN_NAME='UserGroupKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[ContactUserGroup].[UserGroupKey]', 'UserGroupKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[ContactUserGroup] ADD [UsergroupKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[UsergroupKey] = b.[UsergroupId] FROM [CRM].[ContactUserGroup] a LEFT OUTER JOIN [CRM].[Usergroup] b ON a.[UserGroupKeyOld] = b.[UserGroupIdOld]')
					DELETE FROM [CRM].[ContactUserGroup] WHERE [UsergroupKey] IS NULL
					ALTER TABLE [CRM].[ContactUserGroup] ALTER COLUMN [UsergroupKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[ContactUserGroup] ALTER COLUMN [UserGroupKeyOld] int NULL
					ALTER TABLE [CRM].[ContactUserGroup] ADD CONSTRAINT [FK_ContactUserGroup_Usergroup] FOREIGN KEY([UsergroupKey]) REFERENCES [CRM].[Usergroup]([UsergroupId]) ON UPDATE CASCADE ON DELETE CASCADE
					ALTER TABLE [CRM].[ContactUserGroup] ADD CONSTRAINT [PK_ContactUserGroup] PRIMARY KEY CLUSTERED ([ContactKey] ASC, [UsergroupKey] ASC)
				END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='OrderUserGroup' AND COLUMN_NAME='UsergroupKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[OrderUserGroup].[UsergroupKey]', 'UsergroupKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[OrderUserGroup] ADD [UsergroupKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[UsergroupKey] = b.[UsergroupId] FROM [CRM].[OrderUserGroup] a LEFT OUTER JOIN [CRM].[Usergroup] b ON a.[UsergroupKeyOld] = b.[UserGroupIdOld]')
					DELETE FROM [CRM].[OrderUserGroup] WHERE [UsergroupKey] IS NULL
					ALTER TABLE [CRM].[OrderUserGroup] ALTER COLUMN [UsergroupKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[OrderUserGroup] ALTER COLUMN [UsergroupKeyOld] int NULL
					ALTER TABLE [CRM].[OrderUserGroup] ADD CONSTRAINT [FK_OrderUsergroup_Usergroup] FOREIGN KEY([UsergroupKey]) REFERENCES [CRM].[Usergroup]([UsergroupId]) ON UPDATE CASCADE ON DELETE CASCADE
					ALTER TABLE [CRM].[OrderUsergroup] ADD CONSTRAINT [PK_OrderUsergroup] PRIMARY KEY CLUSTERED ([OrderKey] ASC, [UsergroupKey] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='UserUserGroup' AND COLUMN_NAME='UsergroupKey' AND DATA_TYPE = 'int')
				BEGIN
					EXEC sp_rename '[CRM].[UserUserGroup].[UserGroupKey]', 'UserGroupKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[UserUserGroup] ADD [UsergroupKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[UsergroupKey] = b.[UsergroupId] FROM [CRM].[UserUserGroup] a LEFT OUTER JOIN [CRM].[Usergroup] b ON a.[UserGroupKeyOld] = b.[UserGroupIdOld]')
					DELETE FROM [CRM].[UserUserGroup] WHERE [UsergroupKey] IS NULL
					ALTER TABLE [CRM].[UserUserGroup] ALTER COLUMN [UsergroupKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[UserUserGroup] ALTER COLUMN [UserGroupKeyOld] int NULL
					ALTER TABLE [CRM].[UserUserGroup] ADD CONSTRAINT [FK_UserUserGroup_UserGroup] FOREIGN KEY([UsergroupKey]) REFERENCES [CRM].[Usergroup]([UsergroupId]) ON UPDATE CASCADE ON DELETE CASCADE
					ALTER TABLE [CRM].[UserUserGroup] ADD CONSTRAINT [PK_UserUserGroup] PRIMARY KEY CLUSTERED ([Username] ASC, [UsergroupKey] ASC)
				END");
			
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Usergroup>("CRM", "Usergroup");
		}
	}
}
