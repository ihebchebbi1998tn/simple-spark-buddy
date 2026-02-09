namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240920152300)]
	public class AddServiceOrderDispatchReopenedNoteType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				@"INSERT INTO LU.NoteType ([Name], [Language], [Value], [Color], [Icon], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES 
		('Dispatch reopened', 'en', 'ServiceOrderDispatchReopenedNote', '#FF9800', 'wrench', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20240920152300', 'Migration_20240920152300', 1),
		('Einsatz Wiedereröffnet', 'de', 'ServiceOrderDispatchReopenedNote', '#FF9800', 'wrench', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20240920152300', 'Migration_20240920152300', 1)");
		}
	}
}
