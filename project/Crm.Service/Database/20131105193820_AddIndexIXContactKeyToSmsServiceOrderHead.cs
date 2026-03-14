namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131105193820)]
	public class AddIndexIXContactKeyToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_ContactKey') " +
																		 "BEGIN " +
																		 "CREATE NONCLUSTERED INDEX IX_ContactKey ON [SMS].[ServiceOrderHead] ([ContactKey]) " +
																		 "END");
		}

		public override void Down()
		{
		}
	}
}