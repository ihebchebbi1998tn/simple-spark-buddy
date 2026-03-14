namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131106171112)]
	public class AddIndexIXOrderIdToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderDispatch]') AND name = N'IX_OrderId') " +
			                         "BEGIN " +
			                         "CREATE NONCLUSTERED INDEX [IX_OrderId] ON [SMS].[ServiceOrderDispatch] " +
			                         "([OrderId] ASC) " +
			                         "END");
		}

		public override void Down()
		{
		}
	}
}