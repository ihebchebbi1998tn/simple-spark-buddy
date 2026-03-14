namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190924190000)]
	public class ExtendContactKeyIndex : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_ContactKey')
			BEGIN
				DROP INDEX [IX_ContactKey] ON [SMS].[ServiceOrderHead]
			END");
			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_ContactKey')
			BEGIN
				CREATE NONCLUSTERED INDEX [IX_ContactKey] ON [SMS].[ServiceOrderHead]
				(
					[ContactKey] ASC
				) INCLUDE([Status]) 
			END");
		}
	}
}