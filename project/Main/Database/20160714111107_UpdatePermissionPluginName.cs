namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714111107)]
	public class UpdatePermissionPluginName : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();
			query.AppendLine("UPDATE CRM.Permission SET PluginName = 'Crm.Campaigns' WHERE PGroup = 'Campaign'");

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}