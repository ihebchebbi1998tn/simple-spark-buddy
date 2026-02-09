namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190920200001)]
	public class ExtendContactIndex : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Contact]') AND name = N'IX_Contact_IsActive')
			BEGIN
				DROP INDEX [IX_Contact_IsActive] ON [CRM].[Contact]
			END");
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Contact]') AND name = N'IX_Contact_IsActive_ContactType')
			BEGIN
				DROP INDEX [IX_Contact_IsActive_ContactType] ON [CRM].[Contact]
			END");
			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Contact]') AND name = N'IX_Contact_IsActive_ContactType')
			BEGIN
				CREATE NONCLUSTERED INDEX [IX_Contact_IsActive_ContactType] ON [CRM].[Contact]
				(
					[IsActive] ASC,
					[ContactType] ASC
					-- please include AuthDataId (from multitenant plugin) when changing this
				)
				INCLUDE ([ContactId],[CreateUser],[ParentKey],[Visibility])
			END");
		}
	}
}
