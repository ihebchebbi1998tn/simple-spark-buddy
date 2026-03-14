namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714111106)]
	public class UpdateRoleType : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();
			query.AppendLine(string.Format("UPDATE CRM.[Role] SET [Type] = 10 WHERE Name = 'FieldService' OR Name = 'ServicePlanner' OR Name = 'SalesBackOffice' OR Name = 'InternalSales' OR Name = 'InternalService' OR Name = 'HeadOfService' OR Name = 'FieldSales' OR Name = 'HeadOfSales' OR Name = 'ServiceBackOffice'"));

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}