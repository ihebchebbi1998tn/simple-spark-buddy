namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170331151000)]
	public class InsertValuesLuNoteType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
INSERT INTO [LU].[NoteType]
           ([Name]
           ,[Language]
           ,[Value]
		   ,[Color]
		   ,[Icon]
           ,[Favorite]
           ,[SortOrder]
           ,[TenantKey]
           ,[CreateDate]
           ,[ModifyDate]
           ,[CreateUser]
           ,[ModifyUser]
           ,[IsActive])
     VALUES
 ('Servicefall-Statusänderung',		'de', 'ServiceCaseStatusChangedNote',	'#9164a6', '\f12d', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331151000', 'Migration_20170331151000', 1)
,('Service Case Status changed',	'en', 'ServiceCaseStatusChangedNote',	'#9164a6', '\f12d', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331151000', 'Migration_20170331151000', 1)
,('Angelegter Serviceauftrag',		'de', 'ServiceOrderHeadCreatedNote',	'#4caf50', '\f156', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331151000', 'Migration_20170331151000', 1)
,('Service Order created',			'en', 'ServiceOrderHeadCreatedNote',	'#4caf50', '\f156', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331151000', 'Migration_20170331151000', 1)
			");
		}
	}
}