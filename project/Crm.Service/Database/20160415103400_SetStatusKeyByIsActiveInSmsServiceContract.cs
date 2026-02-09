namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160415103400)]
	public class SetStatusKeyByIsActiveInSmsServiceContract : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"UPDATE sc
																SET sc.StatusKey = CASE c.IsActive WHEN 0 THEN 'Inactive' WHEN 1 THEN 'Active' END
																FROM [CRM].[Contact] c JOIN [SMS].[ServiceContract] sc ON c.ContactId = sc.ContactKey");
			Database.ExecuteNonQuery(@"UPDATE c
																SET ModifyUser = 'Migration_20160415103400', ModifyDate = GETUTCDATE()
																FROM [CRM].[Contact] c JOIN [SMS].[ServiceContract] sc ON c.ContactId = sc.ContactKey");

			Database.ExecuteNonQuery(@"UPDATE sc 
																SET sc.StatusKey = 'Expired'
																FROM [CRM].[Contact] c JOIN [SMS].[ServiceContract] sc ON c.ContactId = sc.ContactKey
																WHERE c.IsActive = 1
																AND sc.ValidTo < GETUTCDATE()");
			Database.ExecuteNonQuery(@"UPDATE c
																SET ModifyUser = 'Migration_20160415103400', ModifyDate = GETUTCDATE()
																FROM [CRM].[Contact] c JOIN [SMS].[ServiceContract] sc ON c.ContactId = sc.ContactKey
																WHERE c.IsActive = 1
																AND sc.ValidTo < GETUTCDATE()");

			Database.ExecuteNonQuery(@"UPDATE sc 
																SET sc.StatusKey = 'Pending'
																FROM [CRM].[Contact] c JOIN [SMS].[ServiceContract] sc ON c.ContactId = sc.ContactKey
																WHERE c.IsActive = 1
																AND GETUTCDATE() < sc.ValidFrom");
			Database.ExecuteNonQuery(@"UPDATE c 
																SET ModifyUser = 'Migration_20160415103400', ModifyDate = GETUTCDATE()
																FROM [CRM].[Contact] c JOIN [SMS].[ServiceContract] sc ON c.ContactId = sc.ContactKey
																WHERE c.IsActive = 1
																AND GETUTCDATE() < sc.ValidFrom");
		}
	}
}