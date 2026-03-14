using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Crm.Service.Database
{
	[Migration(20230809115400)]
	public class RemoveDateFromServiceOrderTimePostingsIndex : Migration
	{
		public override void Up()
		{
			if (Database.IndexExists("[SMS].[ServiceOrderTimePostings]", "IX_ServiceOrderTimePostings_IsActive_Included"))
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_ServiceOrderTimePostings_IsActive_Included] ON [SMS].[ServiceOrderTimePostings]");
			}

			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimePostings_IsActive_Included] ON [SMS].[ServiceOrderTimePostings] ([IsActive]) INCLUDE ([ModifyDate], [From], [To], [IsClosed], [OrderId], [OrderTimesId], [PerDiemReportId])");
		}
	}
}
