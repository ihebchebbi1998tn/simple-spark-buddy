namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200609105000)]
	public class UpdateServiceCaseNameToServiceCaseNo : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
					UPDATE [CRM].[Contact] 
						SET [Name] = (SELECT ServiceCaseNo FROM [SMS].[ServiceNotifications] sc WHERE sc.ContactKey = ContactId),
								[ModifyDate] = GETUTCDATE(),
								[ModifyUser] = 'Migration_20200609105000'
						WHERE [ContactType] = 'ServiceCase'");
		}
	}
}