namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160126103700)]
	public class AddServiceClientPermissionToFieldService : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [CRM].[Permission] WHERE [PGroup] = 'Login' AND [Name] = 'ServiceClient'") == 0)
			{
				Database.ExecuteNonQuery("INSERT INTO [CRM].[Permission] (Name, PluginName, PGroup) VALUES ('ServiceClient', 'Crm.Service', 'Login')");
			}
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [CRM].[Role] WHERE [Name] = 'FieldService'") > 0 && (int)Database.ExecuteScalar("SELECT COUNT(*) FROM [CRM].[RolePermission] WHERE RoleKey = (SELECT TOP 1 RoleId FROM [CRM].[Role] WHERE Name = 'FieldService') AND PermissionKey = (SELECT TOP 1 PermissionId FROM [CRM].[Permission] WHERE PGroup = 'Login' AND Name = 'ServiceClient')") == 0)
			{
				Database.ExecuteNonQuery("INSERT INTO [CRM].[RolePermission] (RoleKey, PermissionKey) VALUES ((SELECT TOP 1 RoleId FROM [CRM].[Role] WHERE Name = 'FieldService'), (SELECT TOP 1 PermissionId FROM [CRM].[Permission] WHERE PGroup = 'Login' AND Name = 'ServiceClient'))");
			}
		}
	}
}