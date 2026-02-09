namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714111104)]
	public class DeleteOldPermissions : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();
			query.AppendLine("DELETE CRM.Permission WHERE PGroup is null");
			query.AppendLine("DELETE CRM.Permission WHERE [Name] = 'MaterialAdd'");
			query.AppendLine("DELETE CRM.Permission WHERE [Name] = 'ViewCompanySideBar'");
			query.AppendLine("DELETE CRM.Permission WHERE [Name] = 'ViewPersonSidebarContactInfo'");
			query.AppendLine("DELETE CRM.Permission WHERE [Name] = 'ViewPersonSideBar'");
			query.AppendLine("DELETE CRM.Permission WHERE [Name] = 'ListTags'");
			query.AppendLine("DELETE CRM.Permission WHERE [Name] = 'Service'");
			query.AppendLine("DELETE CRM.Permission WHERE [Name] = 'Document'");

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}