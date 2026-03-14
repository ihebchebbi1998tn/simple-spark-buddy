namespace Main.Database
{
	using System.Collections.Generic;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714111100)]
	public class UpdateOldPermissionNames : Migration
	{
		internal class Permission
		{
			public Permission(string uniqueOldName, string name, string pluginName, string group)
			{
				UniqueOldName = uniqueOldName;
				Name = name;
				PluginName = pluginName;
				Group = group;
			}
			public string UniqueOldName { get; private set; }
			public string Name { get; private set; }
			public string PluginName { get; private set; }
			public string Group { get; private set; }
		}
		
		public override void Up()
		{
			var query = new StringBuilder();

			var permissions = new List<Permission>
			{
				new Permission("ReplenishmentUserSelectable", "ReplenishmentsFromOtherUsersSelectable", "Crm.Service", "ReplenishmentOrder"), 
				new Permission("ViewCompanySidebarBravo", "ViewCompanySideBar", "Main", "Bravo"),
				new Permission("ViewPersonSidebarBravo", "ViewPersonSideBar", "Main", "Bravo"),
				new Permission("EditVisitPossibleAims", "EditPossibleAims", "Crm.VisitReport", "VisitReport"),
				new Permission("EditVisitReportPossibleAnswers", "EditPossibleAnswers", " Crm.VisitReport", "VisitReport"),
				new Permission("DetachDocument", "Detach", "Crm.Service", "Document"),
				new Permission("ViewPersonSidebarContactInfo", "ViewPersonSidebarContactInfo", "Main", "Company"),

				new Permission("CreateProject", "Create", "Crm.Project", "Project"),
				new Permission("EditProject", "Edit", "Crm.Project", "Project"),
				new Permission("DeleteProject", "Delete", "Crm.Project", "Project"), 

				new Permission("CreateCompany", "Create", "Main", "Company"), 
				new Permission("EditCompany", "Edit", "Main", "Company"), 
				new Permission("DeleteCompany", "Delete", "Main", "Company"), 

				new Permission("CreateCampaign", "Create", "Main", "Campaign"), 
				new Permission("EditCampaign", "Edit", "Main", "Campaign"), 
				new Permission("DeleteCampaign", "Delete", "Main", "Campaign"), 

				new Permission("CreateCompany", "Create", "Main", "Company"),
				new Permission("EditCompany", "Edit", "Main", "Company"),
				new Permission("DeleteCompany", "Delete", "Main", "Company"),

				new Permission("CreatePerson", "Create", "Main", "Person"),
				new Permission("EditPerson", "Edit", "Main", "Person"),
				new Permission("DeletePerson", "Delete", "Main", "Person"),

				new Permission("CreateService", "Create", "Crm.Article", "Service"),
				new Permission("EditService", "Edit", "Crm.Article", "Service"),
				new Permission("DeleteService", "Delete", "Crm.Article", "Service"),

				new Permission("CreateMaterial", "Create", "Crm.Article", "Material"),
				new Permission("EditMaterial", "Edit", "Crm.Article", "Material"),
				new Permission("DeleteMaterial", "Delete", "Crm.Article", "Material"),

				new Permission("CreateTool", "Create", "Crm.Article", "Tool"),
				new Permission("EditTool", "Edit", "Crm.Article", "Tool"),
				new Permission("DeleteTool", "Delete", "Crm.Article", "Tool"),

				new Permission("CreateArticle", "Create", "Crm.Article", "Article"),
				new Permission("EditArticle", "Edit", "Crm.Article", "Article"),
				new Permission("DeleteArticle", "Delete", "Crm.Article", "Article"),

				new Permission("CreateInstallation", "Create", "Crm.Service", "Installation"),
				new Permission("EditInstallation", "Edit", "Crm.Service", "Installation"),
				new Permission("DeleteInstallation", "Delete", "Crm.Service", "Installation"),

				new Permission("CreateServiceOrder", "Create", "Crm.Service", "ServiceOrder"),
				new Permission("EditServiceOrder", "Edit", "Crm.Service", "ServiceOrder"),
				new Permission("DeleteServiceOrder", "Delete", "Crm.Service", "ServiceOrder"),

				new Permission("CreateServiceCase", "Create", "Crm.Service", "ServiceCase"),
				new Permission("EditServiceCase", "Edit", "Crm.Service", "ServiceCase"),
				new Permission("DeleteServiceCase", "Delete", "Crm.Service", "ServiceCase"),

				new Permission("CreateDispatch", "Create", "Crm.Service", "Dispatch"),
				new Permission("EditDispatch", "Edit", "Crm.Service", "Dispatch"),
				new Permission("DeleteDispatch", "Delete", "Crm.Service", "Dispatch"),

				new Permission("CreateNote", "Create", "Main", "Note"),
				new Permission("EditNote", "Edit", "Main", "Note"),
				new Permission("DeleteNote", "Delete", "Main", "Note"),
				new Permission("DeleteNotes", "Delete", "Main", "Note"),

				new Permission("CreateBravo", "Create", "Main", "Bravo"),
				new Permission("EditBravo", "Edit", "Main", "Bravo"),
				new Permission("DeleteBravo", "Delete", "Main", "Bravo"),
				new Permission("ActivateBravo", "Activate", "Main", "Bravo"),
				
				new Permission("CreateServiceContract", "Create", "Crm.Service", "ServiceContract"),
				new Permission("EditServiceContract", "Edit", "Crm.Service", "ServiceContract"),
				new Permission("DeleteServiceContract", "Delete", "Crm.Service", "ServiceContract"),

				new Permission("CreateTask", "Create", "Crm.Service", "Task"),
				new Permission("EditTask", "Edit", "Crm.Service", "Task"),
				new Permission("DeleteTask", "Delete", "Crm.Service", "Task"),

				new Permission("MaterialNew", "CreateMaterial", "Crm.Service", "ServiceOrder"),
				new Permission("MaterialEdit", "EditMaterial", "Crm.Service", "ServiceOrder"),
				new Permission("MaterialSave", "SaveMaterial", "Crm.Service", "ServiceOrder"),
				new Permission("MaterialDelete", "DeleteMaterial", "Crm.Service", "ServiceOrder"),

				new Permission("CreateServiceCase", "Create", "Crm.Service", "ServiceCase"),
				new Permission("SkillAssign", "AssignSkill", "Main", "UserAdmin")
			};
			
			foreach (var permission in permissions)
			{
				query.AppendLine(string.Format("IF NOT EXISTS (SELECT * FROM CRM.Permission WHERE Name = '{1}' AND PGroup = '{3}') UPDATE CRM.Permission SET Name = '{1}', PluginName = '{2}', [PGroup] = '{3}' WHERE Name = '{0}'", permission.UniqueOldName, permission.Name, permission.PluginName, permission.Group));
			}

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}