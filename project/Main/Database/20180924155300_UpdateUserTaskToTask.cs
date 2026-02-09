namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180924155300)]
	public class UpdateUserTaskToTask : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE CRM.[Task] SET TaskType = 'Task' WHERE TaskType = 'UserTask'");
		}
	}
}
