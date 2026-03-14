namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131106143851)]
	public class AddIndexIXClosedDateToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_CloseDate') " +
			                         "BEGIN " +
			                         "CREATE NONCLUSTERED INDEX [IX_CloseDate] ON [SMS].[ServiceOrderHead] " +
			                         "([CloseDate] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
			                         "END");
		}

		public override void Down()
		{
		}
	}
}