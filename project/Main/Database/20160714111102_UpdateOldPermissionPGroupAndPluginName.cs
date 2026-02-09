namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714111102)]
	public class UpdateOldPermissionPGroupAndPluginName : Migration
	{

		public override void Up()
		{
			StringBuilder query = new StringBuilder();

			query.AppendLine(
				@"UPDATE old
				SET 
					old.PluginName = new.PluginName
					,old.PGroup = new.PGroup
					FROM 
						[CRM].[Permission] old
					INNER JOIN 
						[CRM].[Permission] new
						ON old.Name = new.Name
					WHERE 
						old.PermissionId <> new.PermissionId
						AND (old.PGroup IS NULL AND new.PGroup IS NOT NULL)");

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}