namespace Crm.Database.Modify
{
	using System.Text;
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220630092200)]
	public class AddColumnBatchNoToSmsInstallationPos : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[InstallationPos]") && !Database.ColumnExists("[SMS].[InstallationPos]", "BatchNo"))
			{
				StringBuilder query = new StringBuilder();
				query.Append("ALTER TABLE [SMS].[InstallationPos] ADD BatchNo nvarchar(250) ");
				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}
