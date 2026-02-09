namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20210215105000)]
	public class AddReportFooterImage : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("ReportFooterImage", DbType.Binary, int.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("ReportFooterImageContentType", DbType.String, ColumnProperty.Null));
		}
	}
}
