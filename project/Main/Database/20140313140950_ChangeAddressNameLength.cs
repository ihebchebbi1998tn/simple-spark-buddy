namespace Main.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140313140950)]
	public class ChangeAddressNameLength : Migration
	{
		public override void Up()
		{
			var sb = new StringBuilder();
			
			sb.AppendLine("begin");
			sb.AppendLine("ALTER TABLE CRM.Address ALTER COLUMN Name1 VARCHAR(180);");
			sb.AppendLine("ALTER TABLE CRM.Address ALTER COLUMN Name2 VARCHAR(180);");
			sb.AppendLine("ALTER TABLE CRM.Address ALTER COLUMN Name3 VARCHAR(180);");
			sb.AppendLine("end");

			Database.ExecuteNonQuery(sb.ToString());
		}
	}
}