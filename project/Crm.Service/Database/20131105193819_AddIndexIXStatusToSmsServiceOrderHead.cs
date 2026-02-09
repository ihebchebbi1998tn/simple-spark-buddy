namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131105193819)]
	public class AddIndexIXStatusToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_Status') " +
			                         "BEGIN " +
			                         "CREATE NONCLUSTERED INDEX IX_Status ON [SMS].[ServiceOrderHead] ([Status]) INCLUDE ([ContactKey]) " +
			                         "END");
			
		}

		public override void Down()
		{
		}
	}
}