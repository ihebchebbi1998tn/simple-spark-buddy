namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160824210000)]
	public class RemoveProfilingTables : Migration
	{
		public override void Up()
		{
			Database.RemoveForeignKey("[dbo].[MiniProfilers]", "FK_MiniProfilers_MiniProfilers");
			Database.RemoveForeignKey("[dbo].[MiniProfilers]", "FK_MiniProfilers_MiniProfilers1");
			Database.RemoveForeignKey("[dbo].[MiniProfilers]", "FK_MiniProfilers_MiniProfilers2");
			Database.RemoveForeignKey("[dbo].[MiniProfilerSqlTimingParameters]", "FK_MiniProfilerSqlTimingParameters_MiniProfilers");
			Database.RemoveForeignKey("[dbo].[MiniProfilerSqlTimingParameters]", "FK_MiniProfilerSqlTimingParameters_MiniProfilerSqlTimingParameters");
			Database.RemoveForeignKey("[dbo].[MiniProfilerSqlTimings]", "FK_MiniProfilerSqlTimings_MiniProfilers");
			Database.RemoveForeignKey("[dbo].[MiniProfilerSqlTimings]", "FK_MiniProfilerSqlTimings_MiniProfilerSqlTimings");

			Database.RemoveForeignKey("[dbo].[MiniProfilerTimings]", "FK_MiniProfilerTimings_MiniProfilerTimings");
			Database.RemoveForeignKey("[dbo].[MiniProfilerTimings]", "FK_MiniProfilerTimings_MiniProfilerTimings1");
			Database.RemoveForeignKey("[dbo].[MiniProfilerTimings]", "FK_MiniProfilerTimings_MiniProfilers");

			Database.RemoveTableIfExisting("[dbo].[MiniProfilerClientTimings]");
			Database.RemoveTableIfExisting("[dbo].[MiniProfilers]");
			Database.RemoveTableIfExisting("[dbo].[MiniProfilerSqlTimingParameters]");
			Database.RemoveTableIfExisting("[dbo].[MiniProfilerSqlTimings]");
			Database.RemoveTableIfExisting("[dbo].[MiniProfilerTimings]");
			Database.RemoveColumnIfExisting("[dbo].[Site]", "IsProfilingActive");
		}
	}
}