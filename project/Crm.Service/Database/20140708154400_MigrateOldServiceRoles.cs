namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140708154400)]
	public class MigrateOldServiceRoles : Migration
	{
		public override void Up()
		{
			if (Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [CRM].[Role] WHERE [Name] = 'FieldService'")) == 0 && Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [CRM].[Role] WHERE [Name] = 'Techniker'")) == 1)
			{
				Database.ExecuteNonQuery("UPDATE [CRM].[Role] SET [Name] = 'FieldService' WHERE [Name] = 'Techniker'");
			}
			else
			{
				// update user role references which are not already set
				Database.ExecuteNonQuery("UPDATE ur SET ur.[RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'FieldService') FROM [CRM].[UserRole] AS ur WHERE ur.[RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'Techniker') AND NOT EXISTS (SELECT TOP 1 NULL FROM [CRM].[UserRole] WHERE [RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'FieldService') AND [Username] = ur.[Username]) AND EXISTS (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'FieldService')");
				// delete remaining user role references of old role
				Database.ExecuteNonQuery("DELETE FROM [CRM].[UserRole] WHERE [RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'Techniker')");
				// update role permission references which are not already set
				Database.ExecuteNonQuery("UPDATE rp SET rp.[RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'FieldService') FROM [CRM].[RolePermission] AS rp WHERE rp.[RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'Techniker') AND NOT EXISTS (SELECT TOP 1 NULL FROM [CRM].[RolePermission] WHERE [RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'FieldService') AND [PermissionKey] = rp.[PermissionKey]) AND EXISTS (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'FieldService')");
				// delete remaining role permission references of old role
				Database.ExecuteNonQuery("DELETE FROM [CRM].[RolePermission] WHERE [RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'Techniker')");
				// delete role
				Database.ExecuteNonQuery("DELETE FROM [CRM].[Role] WHERE [Name] = 'Techniker'");
			}
			if (Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [CRM].[Role] WHERE [Name] = 'ServicePlanner'")) == 0 && Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [CRM].[Role] WHERE [Name] = 'Einsatzplaner'")) == 1)
			{
				Database.ExecuteNonQuery("UPDATE [CRM].[Role] SET [Name] = 'ServicePlanner' WHERE [Name] = 'Einsatzplaner'");
			}
			else
			{
				// update user role references which are not already set
				Database.ExecuteNonQuery("UPDATE ur SET ur.[RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'ServicePlanner') FROM [CRM].[UserRole] AS ur WHERE ur.[RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'Einsatzplaner') AND NOT EXISTS (SELECT TOP 1 NULL FROM [CRM].[UserRole] WHERE [RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'ServicePlanner') AND [Username] = ur.[Username]) AND EXISTS (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'ServicePlanner')");
				// delete remaining user role references of old role
				Database.ExecuteNonQuery("DELETE FROM [CRM].[UserRole] WHERE [RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'Einsatzplaner')");
				// update role permission references which are not already set
				Database.ExecuteNonQuery("UPDATE rp SET rp.[RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'ServicePlanner') FROM [CRM].[RolePermission] AS rp WHERE rp.[RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'Einsatzplaner') AND NOT EXISTS (SELECT TOP 1 NULL FROM [CRM].[RolePermission] WHERE [RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'ServicePlanner') AND [PermissionKey] = rp.[PermissionKey]) AND EXISTS (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'ServicePlanner')");
				// delete remaining role permission references of old role
				Database.ExecuteNonQuery("DELETE FROM [CRM].[RolePermission] WHERE [RoleKey] = (SELECT TOP 1 [RoleId] FROM [CRM].[Role] WHERE [Name] = 'Einsatzplaner')");
				// delete role
				Database.ExecuteNonQuery("DELETE FROM [CRM].[Role] WHERE [Name] = 'Einsatzplaner'");
			}
		}
	}
}