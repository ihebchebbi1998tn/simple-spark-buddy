namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191230132800)]
	public class AddDateIndexToLog : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"CREATE NONCLUSTERED INDEX [IX_Log_Date] ON [CRM].[Log]
(
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
			Database.ExecuteNonQuery(@"CREATE NONCLUSTERED INDEX [IX_Log_Level] ON [CRM].[Log]
(
	[Level] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]");
		}
	}
}
