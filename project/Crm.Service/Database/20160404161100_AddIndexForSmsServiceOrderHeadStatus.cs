namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161100)]
	public class AddIndexForSmsServiceOrderHeadStatus : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_Status'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Status] ON [SMS].[ServiceOrderHead]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Status] ON [SMS].[ServiceOrderHead] ([Status]) INCLUDE ([ContactKey])");
		}
	}
}