namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130205142000)]
	public class AlterSmsServiceNotificationsTextToNvarchar : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();

			query.AppendLine("ALTER TABLE [SMS].[ServiceNotifications]");
			query.AppendLine("ALTER COLUMN [Remark] NVARCHAR(MAX) NULL");

			Database.ExecuteNonQuery(query.ToString());
		}
		public override void Down()
		{
			var query = new StringBuilder();

			query.AppendLine("ALTER TABLE [SMS].[ServiceNotifications]");
			query.AppendLine("ALTER COLUMN [Remark] TEXT NULL");

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}