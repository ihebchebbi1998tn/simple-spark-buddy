namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180220110001)]
	public class StationToGuid_Readd_FK_ServiceOrderHead_Station : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"sp_rename 'SMS.ServiceOrderHead.StationKey', 'StationKeyOld', 'COLUMN'");
			Database.ExecuteNonQuery(@"
				ALTER TABLE SMS.ServiceOrderHead
				ADD StationKey UNIQUEIDENTIFIER NULL
				CONSTRAINT FK_ServiceOrderHead_Station FOREIGN KEY (StationKey) REFERENCES CRM.Station(StationId)");
			Database.ExecuteNonQuery(@"
				UPDATE SMS.ServiceOrderHead
				SET StationKey = StationId
				FROM SMS.ServiceOrderHead
				JOIN CRM.Station ON StationIdOld = StationKeyOld");
			Database.ExecuteNonQuery(@"
				UPDATE CRM.Contact
				SET ModifyDate = GETUTCDATE()
					,ModifyUser = 'Migration_20180220110000'
				FROM CRM.Contact contact
				JOIN SMS.ServiceOrderHead serviceOrder ON ContactKey = ContactId
				WHERE StationKey IS NOT NULL");
			Database.RemoveColumn("SMS.ServiceOrderHead", "StationKeyOld");
		}
	}
}