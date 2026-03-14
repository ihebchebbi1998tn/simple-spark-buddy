namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20201222144600)]
	public class AddReportFooterExtensions : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("ReportFooterCol1", DbType.String, 4000, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("ReportFooterCol2", DbType.String, 4000, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("ReportFooterCol3", DbType.String, 4000, ColumnProperty.Null));
			Database.ExecuteNonQuery("UPDATE dbo.Domain SET ReportFooterCol2 = ReportFooter");
			Database.RemoveColumn("dbo.Domain", "ReportFooter");
		}
	}
}
