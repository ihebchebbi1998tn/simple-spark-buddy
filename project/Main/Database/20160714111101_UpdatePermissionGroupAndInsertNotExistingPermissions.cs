namespace Main.Database
{
	using System.Collections.Generic;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714111101)]
	public class UpdatePermissionGroupAndInsertNotExistingPermissions : Migration
	{
		internal class Permission
		{
			public Permission(string name, string pluginName, string group)
			{
				Name = name;
				PluginName = pluginName;
				Group = group;
			}
			public string Name { get; private set; }
			public string PluginName { get; private set; }
			public string Group { get; private set; }
		}

		public override void Up()
		{
			StringBuilder query = new StringBuilder();

			var permissions = new List<Permission>
			{
				new Permission("SeeAllUsersVisitReports", "Crm.VisitReport", "VisitReport"), 
				new Permission("EditAllUsersVisitReports", "Crm.VisitReport", "VisitReport"),
				new Permission("SeeAllUsersTourPlanning", "Crm.VisitReport", "Visit"),
				new Permission("EditAllUsersTourPlanning", "Crm.VisitReport", "Visit"),
				new Permission("ViewAllReporting", "Crm.VisitReport", "VisitReport"),
				new Permission("CreatePosSerial", "Crm.Service", "Installation"),
				new Permission("SeeAllUsersOffers", "Crm.Order", "Offer"),
				new Permission("SeeAllUsersOrders", "Crm.Order", "Order"),
				new Permission("EditStandardAction", "Crm.Service", "Installation"),
				new Permission("CreateStandardAction", "Crm.Service", "Installation"),
				new Permission("CreateUpdate", "Crm.Article", "Article"),

				new Permission("Delete", "Main", "Note"),
				new Permission("Delete", "Main", "Person"),
				new Permission("Delete", "Main", "DocumentArchive"),
				new Permission("Delete", "Main", "Campaign"),
				new Permission("Delete", "Crm.Project", "Project"),

				new Permission("Create", "Crm.VisitReport", "VisitReport"),
				new Permission("Create", "Main", "Campaign"),
				new Permission("Create", "Crm.DynamicForms", "DynamicForms"),
				new Permission("Create", "Crm.Project", "Project"),
				new Permission("Create", "Crm.Service", "Installation"),
				new Permission("Create", "Crm.Service", "ServiceCase"),
				new Permission("Create", "Crm.Service", "ServiceContract"),
				new Permission("Create", "Main", "Task"),
				new Permission("Create", "Main", "Person"),
				new Permission("Create", "Main", "Company"),
				new Permission("Create", "Main", "Note"),

				new Permission("Edit", "Crm.Campaigns", "Campaign"),
				new Permission("Edit", "Crm.Service", "Installation"),
				new Permission("Edit", "Crm.Service", "ServiceCase"),
				new Permission("Edit", "Crm.Service", "ServiceContract"),
				new Permission("Edit", "Crm.Service", "ServiceOrder"),
				new Permission("Edit", "Crm.VisitReport", "Visit"),
				new Permission("Edit", "Main", "Company"),
				new Permission("Edit", "Main", "Person"),
				new Permission("Edit", "Main", "Note"),
				
				new Permission("ChangeFinalizedProjectStatus", "Crm.Project", "Project"),
				new Permission("AttachDocument", "Main", "DocumentArchive"),

				new Permission("CreateTag", "Main", "Person"),
				new Permission("AssociateTag", "Main", "Person"),
				new Permission("RenameTag", "Main", "Person"),
				new Permission("RemoveTag", "Main", "Person"),
				new Permission("DeleteTag", "Main", "Person"),

				new Permission("CreateTag", "Crm.Article", "Article"),
				new Permission("AssociateTag", "Crm.Article", "Article"),
				new Permission("RenameTag", "Crm.Article", "Article"),
				new Permission("RemoveTag", "Crm.Article", "Article"),
				new Permission("DeleteTag", "Crm.Article", "Article"),

				new Permission("CreateTag", "Main", "Company"),
				new Permission("AssociateTag", "Main", "Company"),
				new Permission("RenameTag", "Main", "Company"),
				new Permission("RemoveTag", "Main", "Company"),
				new Permission("DeleteTag", "Main", "Company"),

				new Permission("CreateTag", "Main", "Folder"),
				new Permission("AssociateTag", "Main", "Folder"),
				new Permission("RenameTag", "Main", "Folder"),
				new Permission("RemoveTag", "Main", "Folder"),
				new Permission("DeleteTag", "Main", "Folder"),

				new Permission("CreateTag", "Crm.Service", "Installation"),
				new Permission("ListTags", "Crm.Service", "Installation"),
				new Permission("AssociateTag", "Crm.Service", "Installation"),
				new Permission("RenameTag", "Crm.Service", "Installation"),
				new Permission("RemoveTag", "Crm.Service", "Installation"),
				new Permission("DeleteTag", "Crm.Service", "Installation"),

				new Permission("CreateTag", "Crm.Service", "ServiceCase"),
				new Permission("AssociateTag", "Crm.Service", "ServiceCase"),
				new Permission("RenameTag", "Crm.Service", "ServiceCase"),
				new Permission("RemoveTag", "Crm.Service", "ServiceCase"),
				new Permission("DeleteTag", "Crm.Service", "ServiceCase"),

				new Permission("CreateTag", "Crm.Service", "ServiceContract"),
				new Permission("AssociateTag", "Crm.Service", "ServiceContract"),
				new Permission("RenameTag", "Crm.Service", "ServiceContract"),
				new Permission("RemoveTag", "Crm.Service", "ServiceContract"),
				new Permission("DeleteTag", "Crm.Service", "ServiceContract"),

				new Permission("CreateTag", "Crm.Service", "ServiceOrderHead"),
				new Permission("AssociateTag", "Crm.Service", "ServiceOrderHead"),
				new Permission("RenameTag", "Crm.Service", "ServiceOrderHead"),
				new Permission("RemoveTag", "Crm.Service", "ServiceOrderHead"),
				new Permission("DeleteTag", "Crm.Service", "ServiceOrderHead"),

				new Permission("CreateTag", "Crm.Service", "ServiceObject"),
				new Permission("AssociateTag", "Crm.Service", "ServiceObject"),
				new Permission("RenameTag", "Crm.Service", "ServiceObject"),
				new Permission("RemoveTag", "Crm.Service", "ServiceObject"),
				new Permission("DeleteTag", "Crm.Service", "ServiceObject"),

				new Permission("DeleteUser", "Main", "UserAdmin")
			};

			foreach (var permission in permissions)
			{
				query.AppendLine(string.Format("IF EXISTS (SELECT * FROM CRM.Permission WHERE Name = '{0}' AND PluginName = '{1}' AND PGroup IS NULL) AND NOT EXISTS (SELECT * FROM CRM.Permission WHERE Name = '{0}' AND PluginName = '{1}' AND PGroup = '{2}')", permission.Name, permission.PluginName, permission.Group));
				query.AppendLine("BEGIN");
				query.AppendLine(string.Format("UPDATE CRM.Permission SET [PGroup] = '{0}' WHERE Name = '{1}' AND PluginName = '{2}'", permission.Group, permission.Name, permission.PluginName));
				query.AppendLine("END");

				query.AppendLine(string.Format("IF EXISTS (SELECT * FROM CRM.Permission WHERE Name = '{0}' AND PluginName IS NULL AND PGroup = '{2}')", permission.Name, permission.PluginName, permission.Group));
				query.AppendLine("BEGIN");
				query.AppendLine(string.Format("UPDATE CRM.Permission SET [PluginName] = '{2}' WHERE Name = '{1}' AND PGroup = '{0}' AND PluginName IS NULL", permission.Group, permission.Name, permission.PluginName));
				query.AppendLine("END");

				query.AppendLine(string.Format("IF NOT EXISTS (SELECT * FROM CRM.Permission WHERE Name = '{0}' AND PGroup = '{2}')", permission.Name, permission.PluginName, permission.Group));
				query.AppendLine("BEGIN");
				query.AppendLine(string.Format("INSERT INTO CRM.Permission (Name, PluginName, PGroup) VALUES ('{0}', '{1}', '{2}')", permission.Name, permission.PluginName, permission.Group));
				query.AppendLine("END");
			}

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}