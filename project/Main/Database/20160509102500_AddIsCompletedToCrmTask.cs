namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160509102500)]
	public class AddIsCompletedToCrmTask : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Task]", "IsCompleted"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Task] ADD IsCompleted BIT NOT NULL DEFAULT(0)");
				Database.ExecuteNonQuery("UPDATE [CRM].[Task] SET IsCompleted = 1 WHERE IsActive = 0");
				Database.ExecuteNonQuery("UPDATE [CRM].[Task] SET IsActive = 1, ModifyDate = GETUTCDATE()");
			}
		}
	}
}