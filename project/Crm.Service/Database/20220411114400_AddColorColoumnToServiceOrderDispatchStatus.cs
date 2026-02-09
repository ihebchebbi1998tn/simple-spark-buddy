namespace Crm.Database.Modify
{
	using System.Text;
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220411114400)]
	public class AddColorColoumnToServiceOrderDispatchStatus : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderDispatchStatus]") && !Database.ColumnExists("[SMS].[ServiceOrderDispatchStatus]", "Color"))
			{
				StringBuilder query = new StringBuilder();
				query.Append("ALTER TABLE [SMS].[ServiceOrderDispatchStatus] ADD Color nvarchar(20) NOT NULL DEFAULT '#AAAAAA'");
				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}
