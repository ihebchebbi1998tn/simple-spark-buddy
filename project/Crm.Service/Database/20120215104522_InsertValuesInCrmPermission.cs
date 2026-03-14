namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120215104522)]
	public class InsertValuesInCrmPermission : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('CreateArticle', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('EditArticle', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('DeleteArticle', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('CreateInstallation', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('EditInstallation', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('DeleteInstallation', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('CreateServiceOrder', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('EditServiceOrder', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('DeleteServiceOrder', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('CreateServiceCase', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('EditServiceCase', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('DeleteServiceCase', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('CreateDispatch', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('EditDispatch', 'Crm.Service')");
			query.AppendLine("INSERT INTO CRM.Permission (Name, PluginName) VALUES ('DeleteDispatch', 'Crm.Service')");
			Database.ExecuteNonQuery(query.ToString());
		}
		public override void Down()
		{
			var query = new StringBuilder();
			query.AppendLine("DELETE up FROM CRM.UserPermission up JOIN CRM.Permission p ON p.PermissionId = up.PermissionKey WHERE p.PluginName = 'Crm.Service'");
			query.AppendLine("DELETE rp FROM CRM.RolePermission rp JOIN CRM.Permission p ON p.PermissionId = rp.PermissionKey WHERE p.PluginName = 'Crm.Service'");
			query.AppendLine("DELETE FROM CRM.Permission WHERE PluginName = 'Crm.Service'");
			Database.ExecuteNonQuery(query.ToString());
		}
	}
}