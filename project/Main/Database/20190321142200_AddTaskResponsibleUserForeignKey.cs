namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190321142200)]
	public class AddTaskResponsibleUserForeignKey : Migration
	{
		public override void Up()
		{
			var responsibleUserLength = GetColumnLength("CRM", "Task", "ResponsibleUser");
			var usernameLength = GetColumnLength("CRM", "User", "Username");
			if (usernameLength > responsibleUserLength)
			{
				Database.ChangeColumn("[CRM].[Task]", new Column("ResponsibleUser", DbType.String, usernameLength));
			}
			Database.ExecuteNonQuery(@"
				UPDATE [CRM].[Task] 
					SET ResponsibleUser=NULL
					WHERE ResponsibleUser IN (SELECT t.ResponsibleUser FROM [CRM].[Task] AS t LEFT JOIN [CRM].[User] AS u ON t.ResponsibleUser = u.Username WHERE t.ResponsibleUser IS NOT NULL AND u.Username IS NULL);
			");
			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE [parent_object_id] = OBJECT_ID('[CRM].[Task]') AND [referenced_object_id] = OBJECT_ID('[CRM].[User]')) BEGIN
					ALTER TABLE [CRM].[Task]
					ADD CONSTRAINT FK_Task_ResponsibleUser FOREIGN KEY (ResponsibleUser) REFERENCES [CRM].[User](Username)
				END;
			");
		}
		private int GetColumnLength(string schema, string table, string column)
		{
			return (int)Database.ExecuteScalar($"select CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns WHERE table_schema = '{schema}' AND table_name = '{table}' AND COLUMN_NAME='{column}'");
		}
	}
}
