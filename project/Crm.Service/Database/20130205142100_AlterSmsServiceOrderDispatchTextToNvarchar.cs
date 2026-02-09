namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130205142100)]
	public class AlterSmsServiceOrderDispatchTextToNvarchar : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();

			query.AppendLine("ALTER TABLE [SMS].[ServiceOrderDispatch]");
			query.AppendLine("ALTER COLUMN [Signature] NVARCHAR(MAX) NULL");

			Database.ExecuteNonQuery(query.ToString());
		}
		public override void Down()
		{
			var query = new StringBuilder();

			query.AppendLine("ALTER TABLE [SMS].[ServiceOrderDispatch]");
			query.AppendLine("ALTER COLUMN [Signature] TEXT NULL");

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}