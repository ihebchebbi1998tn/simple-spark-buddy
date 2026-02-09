namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150414155100)]
	public class AddNewServicePermissions : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				"IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreatePosSerial' AND PGroup = 'Installation')" +
				"BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName,Status,PGroup) VALUES ('CreatePosSerial', 'Crm.Service', 1, 'Installation') END " +

				"IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreatePos' AND PGroup = 'Installation')" +
				"BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName,Status,PGroup) VALUES ('CreatePos', 'Crm.Service', 1, 'Installation') END "
				);
		}
	}
}
