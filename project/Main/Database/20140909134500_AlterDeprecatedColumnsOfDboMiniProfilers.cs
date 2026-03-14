namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140909134500)]
	public class AlterDeprecatedColumnsOfDboMiniProfilers : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[dbo].[MiniProfilers]", "Name"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilers ALTER COLUMN Name nvarchar(200) NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilers]", "HasSqlTimings"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilers ALTER COLUMN HasSqlTimings bit NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilers]", "HasDuplicateSqlTimings"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilers ALTER COLUMN HasDuplicateSqlTimings bit NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilers]", "HasTrivialTimings"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilers ALTER COLUMN HasTrivialTimings bit NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilers]", "HasAllTrivialTimings"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilers ALTER COLUMN HasAllTrivialTimings bit NULL");
			}

			if (Database.ColumnExists("[dbo].[MiniProfilerTimings]", "DurationWithoutChildrenMilliseconds"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilerTimings ALTER COLUMN DurationWithoutChildrenMilliseconds decimal(7,1) NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilerTimings]", "HasChildren"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilerTimings ALTER COLUMN HasChildren bit NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilerTimings]", "IsTrivial"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilerTimings ALTER COLUMN IsTrivial bit NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilerTimings]", "HasSqlTimings"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilerTimings ALTER COLUMN HasSqlTimings bit NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilerTimings]", "HasDuplicateSqlTimings"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilerTimings ALTER COLUMN HasDuplicateSqlTimings bit NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilerTimings]", "ExecutedReaders"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilerTimings ALTER COLUMN ExecutedReaders smallint NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilerTimings]", "ExecutedScalars"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilerTimings ALTER COLUMN ExecutedScalars smallint NULL");
			}
			if (Database.ColumnExists("[dbo].[MiniProfilerTimings]", "ExecutedNonQueries"))
			{
				Database.ExecuteNonQuery("ALTER TABLE dbo.MiniProfilerTimings ALTER COLUMN ExecutedNonQueries smallint NULL");
			}
		}
	}
}