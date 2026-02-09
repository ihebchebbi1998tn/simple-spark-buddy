namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151110164200)]
	public class ChangeTasksToGuidIds : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'CRM' AND TABLE_NAME = 'Task' AND COLUMN_NAME = 'TaskId' AND DATA_TYPE = 'uniqueidentifier'") == 0)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Task_ContactKey_ResponsibleUser_IsActive] ON [CRM].[Task]");
				Database.ExecuteNonQuery("DROP INDEX [IX_Task_ResponsibleUser_IsActive] ON [CRM].[Task]");
				Database.ExecuteNonQuery("DROP INDEX [IX_Task_ResponsibleUser_IsActive_DueDate] ON [CRM].[Task]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Task] DROP CONSTRAINT [PK_Task]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Task] ADD [TaskIdOld] INT NULL");
				Database.ExecuteNonQuery("UPDATE [CRM].[Task] SET [TaskIdOld] = [TaskId]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Task] DROP COLUMN [TaskId]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Task] ADD [TaskId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Task] ADD CONSTRAINT [PK_Task] PRIMARY KEY([TaskId])");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Task_ContactKey_ResponsibleUser_IsActive] ON [CRM].[Task] ([ContactKey] ASC, [ResponsibleUser] ASC, [IsActive] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Task_ResponsibleUser_IsActive] ON [CRM].[Task] ([ResponsibleUser] ASC, [IsActive] ASC)");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Task_ResponsibleUser_IsActive_DueDate] ON [CRM].[Task] ([ResponsibleUser] ASC, [IsActive] ASC, [DueDate] ASC)");
			}
		}
	}
}
