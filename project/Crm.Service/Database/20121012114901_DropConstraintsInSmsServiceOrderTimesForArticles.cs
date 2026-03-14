namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20121012114901)]
	public class DropConstraintsInSmsServiceOrderTimesForArticles : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF EXISTS (SELECT 1 from sys.objects where name = 'FK_ServiceOrderTimes_Articles') ALTER TABLE [SMS].[ServiceOrderTimes] DROP CONSTRAINT FK_ServiceOrderTimes_Articles");
		}
		public override void Down()
		{
			if (!Database.ConstraintExists("[SMS].[ServiceOrderTimes]", "FK_ServiceOrderTimes_Articles"))
			{
				var sb = new StringBuilder();
				sb.AppendLine("ALTER TABLE [SMS].[ServiceOrderTimes] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderTimes_Articles] FOREIGN KEY([ItemNo])");
				sb.AppendLine("REFERENCES [SMS].[Old_Articles] ([ItemNo])");
				sb.AppendLine("GO");

				sb.AppendLine("ALTER TABLE [SMS].[ServiceOrderTimes] CHECK CONSTRAINT [FK_ServiceOrderTimes_Articles]");
				sb.AppendLine("GO");

				Database.ExecuteNonQuery(sb.ToString());
			}
		}
	}
}