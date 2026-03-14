using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230510075400)]
	public class AddIndexToServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			if (Database.IndexExists("[SMS].[ServiceOrderTimePostings]", "IX_ServiceOrderTimePostings_IsActive_Included"))
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_ServiceOrderTimePostings_IsActive_Included] ON [SMS].[ServiceOrderTimePostings]");
			}

			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimePostings_IsActive_Included] ON [SMS].[ServiceOrderTimePostings] ([IsActive]) INCLUDE ([ModifyDate], [Date], [IsClosed], [OrderId], [OrderTimesId], [PerDiemReportId])");
		}
	}
}
