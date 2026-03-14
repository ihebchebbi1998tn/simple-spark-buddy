namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131106171114)]
	public class AddIndexIXLegacyIdToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderDispatch]') AND name = N'IX_LegacyId') " +
			                         "BEGIN " +
															 "CREATE NONCLUSTERED INDEX [IX_LegacyId] ON [SMS].[ServiceOrderDispatch] " +
			                         "([LegacyId] ASC) " +
			                         "END");
		}

		public override void Down()
		{
		}
	}
}