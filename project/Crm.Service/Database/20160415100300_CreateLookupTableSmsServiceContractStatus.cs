namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160415100300)]
	public class CreateLookupTableSmsServiceContractStatus : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceContractStatus]"))
			{
				Database.ExecuteNonQuery(@"CREATE TABLE [SMS].[ServiceContractStatus] (
																		ServiceContractStatusId int IDENTITY(1,1),
																		Name nvarchar(250),
																		Language nvarchar(20),
																		Value nvarchar(20),
																		Favorite bit,
																		SortOrder int,
																		SettableStatuses nvarchar(500),
																		CreateDate datetime,
																		ModifyDate datetime,
																		CreateUser nvarchar(100),
																		ModifyUser nvarchar(100),
																		IsActive bit,
																		TenantKey int)");
			}
			else
			{
				Database.ExecuteNonQuery(@"TRUNCATE TABLE [SMS].[ServiceContractStatus]");
			}
			Database.ExecuteNonQuery(@"INSERT INTO [SMS].[ServiceContractStatus] ([Name],[Language],[Value],[Favorite],[SortOrder],[SettableStatuses],[CreateDate],[ModifyDate],[CreateUser],[ModifyUser],[IsActive],[TenantKey]) 
                 VALUES ('Inaktiv','de','Inactive',0,2,'Pending,Active',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
                        ('Aktiv','de','Active',1,3,'Inactive',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
												('Abgelaufen','de','Expired',0,4,NULL,GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
												('Ausstehend','de','Pending',0,1,'Inactive',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
                        
                        ('Inactive','en','Inactive',0,2,'Pending,Active',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
                        ('Active','en','Active',1,3,'Inactive',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
												('Expired','en','Expired',0,4,NULL,GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
												('Pending','en','Pending',0,1,'Inactive',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
												
												('Inactif', 'fr', 'Inactive', 0, 2, 'Pending,Active', GETDATE(), GETDATE(), 'Setup', 'Setup', 1, NULL),
												('Active', 'fr', 'Active', 1, 3, 'Inactive', GETDATE(), GETDATE(), 'Setup', 'Setup', 1, NULL),
												('Expiré','fr','Expired',0,4,NULL,GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
												('En attente','fr','Pending',0,1,'Inactive',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),

												('Inaktív','hu','Inactive',0,2,'Pending,Active',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
                        ('Aktív','hu','Active',1,3,'Inactive',GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
												('Lejárt','hu','Expired',0,4,NULL,GETDATE(),GETDATE(),'Setup','Setup',1,NULL),
												('Függőben','hu','Pending',0,1,'Inactive',GETDATE(),GETDATE(),'Setup','Setup',1,NULL)");
		}
	}
}