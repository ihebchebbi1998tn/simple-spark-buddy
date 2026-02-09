namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220812113000)]
	public class AddNonInternalServiceCaseCreateUserAndCreateDate : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("Crm.Task", "TaskCreateUser"))
			{
				Database.AddColumn("CRM.Task", new Column("TaskCreateUser", DbType.String, 60, ColumnProperty.NotNull, "'System'"));
				Database.ExecuteNonQuery(@"UPDATE CRM.Task SET TaskCreateUser = CreateUser");
			}
		}
	}
}
