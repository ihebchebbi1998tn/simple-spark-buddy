namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200618171700)]
	public class AddServiceCaseCreatedNoteType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				@"INSERT INTO LU.NoteType ([Name]
      ,[Language]
      ,[Value]
      ,[Color]
      ,[Icon]
      ,[Favorite]
      ,[SortOrder]
      ,[CreateDate]
      ,[ModifyDate]
      ,[CreateUser]
      ,[ModifyUser]
      ,[IsActive]) VALUES 
		('Service case created', 'en', 'ServiceCaseCreatedNote', '#4caf50', '\f1ed', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20200618171700', 'Migration_20200618171700', 1),
		('Cas de service créé', 'fr', 'ServiceCaseCreatedNote', '#4caf50', '\f1ed', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20200618171700', 'Migration_20200618171700', 1),
		('Szolgáltatási eset létrehozva', 'hu', 'ServiceCaseCreatedNote', '#4caf50', '\f1ed', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20200618171700', 'Migration_20200618171700', 1),
		('Servicefall erstellt', 'de', 'ServiceCaseCreatedNote', '#4caf50', '\f1ed', 0, 0, GETUTCDATE(), GETUTCDATE(), 'Migration_20200618171700', 'Migration_20200618171700', 1)");
		}
	}
}
