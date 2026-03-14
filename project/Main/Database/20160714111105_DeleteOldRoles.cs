namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714111105)]
	public class DeleteOldRoles : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();
			query.AppendLine(string.Format("DELETE CRM.[Role] WHERE Name = 'RestAPI'"));

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}