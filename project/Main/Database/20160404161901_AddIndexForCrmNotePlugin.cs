namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161901)]
	public class AddIndexForCrmNotePlugin : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Note]') AND name = N'IX_Note_Plugin'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Note_Plugin] ON [CRM].[Note]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Note_Plugin] ON [CRM].[Note] ([Plugin] ASC) INCLUDE ([ElementKey])");
		}
	}
}