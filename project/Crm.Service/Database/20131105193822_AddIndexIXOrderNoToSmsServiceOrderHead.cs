namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131105193822)]
	public class AddIndexIXOrderNoToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_OrderNo') " +
																		 "BEGIN " +
																		 "CREATE NONCLUSTERED INDEX IX_OrderNo ON [SMS].[ServiceOrderHead] ([OrderNo]) INCLUDE ([ContactKey]) " +
																		 "END");
		}

		public override void Down()
		{
		}
	}
}