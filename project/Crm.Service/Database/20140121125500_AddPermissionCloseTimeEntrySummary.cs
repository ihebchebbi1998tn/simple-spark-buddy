namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140121125500)]
	public class AddPermissionCloseTimeEntrySummary : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('CloseTimeEntrySummary', 'Crm.Service')");
		}
		public override void Down()
		{

		}
	}
}