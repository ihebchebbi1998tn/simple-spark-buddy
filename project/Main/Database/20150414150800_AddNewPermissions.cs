namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150414150800)]
	public class AddNewPermissions : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				"IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateTag' AND PGroup = 'Company')" +
				"BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName,Status,PGroup) VALUES ('CreateTag', 'Main', 1, 'Company') END "
				);
		}
	}
}