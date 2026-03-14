namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210215102400)]
	public class AddReportLogoContentType : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("dbo.Domain", "ReportLogoContentType"))
			{
				Database.AddColumn("dbo.Domain", new Column("ReportLogoContentType", DbType.String, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE dbo.Domain SET ReportLogoContentType = 'image/png' WHERE ReportLogo IS NOT NULL");
			}
		}
	}
}
