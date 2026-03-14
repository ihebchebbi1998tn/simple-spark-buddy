namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180716142000)]
	public class AlterEntityTypeOfUserTask : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE EntityType SET[Name] = 'Crm.Model.Task' WHERE[Name] = 'Crm.Model.UserTask'");
		}
	}
}
