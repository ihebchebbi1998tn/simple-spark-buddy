namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201007103200)]
	public class AddServiceOrderDispatchCompletedNoteType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				@"INSERT INTO LU.NoteType ([Name], [Language], [Value], [Color], [Icon], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES 
		('Dispatch completion', 'en', 'ServiceOrderDispatchCompletedNote', '#8BC34A', '\f1ed', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20201007103200', 'Migration_20201007103200', 1),
		('Achèvement de l''intervention', 'fr', 'ServiceOrderDispatchCompletedNote', '#8BC34A', '\f1ed', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20201007103200', 'Migration_20201007103200', 1),
		('Kiszállás teljesülés', 'hu', 'ServiceOrderDispatchCompletedNote', '#8BC34A', '\f1ed', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20201007103200', 'Migration_20201007103200', 1),
		('Einsatzabschluss', 'de', 'ServiceOrderDispatchCompletedNote', '#8BC34A', '\f1ed', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20201007103200', 'Migration_20201007103200', 1)");
		}
	}
}