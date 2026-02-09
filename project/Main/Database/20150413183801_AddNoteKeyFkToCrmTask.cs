namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413183801)]
	public class AddNoteKeyFkToCrmTask : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Task_Note'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE t SET t.[NoteKey] = NULL FROM [CRM].[Task] t LEFT OUTER JOIN [CRM].[Note] n ON t.[NoteKey] = n.[NoteId] WHERE n.[NoteId] IS NULL");
				Database.AddForeignKey("FK_Task_Note", "[CRM].[Task]", "NoteKey", "[CRM].[Note]", "NoteId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}