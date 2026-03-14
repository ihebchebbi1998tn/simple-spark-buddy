namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140909134900)]
	public class AddTableDboMiniProfilerClientTimings : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[dbo].[MiniProfilerClientTimings]"))
			{
				var stringBuilder = new StringBuilder();

				stringBuilder.AppendLine("create table MiniProfilerClientTimings");
				stringBuilder.AppendLine("(");
				stringBuilder.AppendLine(" RowId                               integer not null identity constraint PK_MiniProfilerClientTimings primary key clustered,");
				stringBuilder.AppendLine(" Id                                  uniqueidentifier not null,");
				stringBuilder.AppendLine(" MiniProfilerId                      uniqueidentifier not null,");
				stringBuilder.AppendLine(" Name                                nvarchar(200) not null,");
				stringBuilder.AppendLine(" Start                               decimal(9, 3) not null,");
				stringBuilder.AppendLine(" Duration                            decimal(9, 3) not null");
				stringBuilder.AppendLine(");");

				stringBuilder.AppendLine("create unique nonclustered index IX_MiniProfilerClientTimings_Id on MiniProfilerClientTimings (Id);");
				stringBuilder.AppendLine("create nonclustered index IX_MiniProfilerClientTimings_MiniProfilerId on MiniProfilerClientTimings (MiniProfilerId);");

				Database.ExecuteNonQuery(stringBuilder.ToString());
			}
		}
	}
}