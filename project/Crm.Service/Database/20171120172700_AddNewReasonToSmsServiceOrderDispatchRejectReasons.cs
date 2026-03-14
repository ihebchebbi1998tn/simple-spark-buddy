namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20171120172700)]
	public class AddNewReasonToSmsServiceOrderDispatchRejectReasons : Migration
	{
		public override void Up()
		{
		    Database.ExecuteNonQuery(@"INSERT INTO [SMS].[ServiceOrderDispatchRejectReason]
           ([Value],[Name],[Language],[Favorite],[SortOrder],[CreateDate],[CreateUser],[ModifyDate],[ModifyUser],[IsActive],[ServiceOrderStatus],[ShowInMobileClient])
            VALUES ('RejectedBySystem','Rejected by system','en',0,4,GETUTCDATE(),'Migration_20171120172700',GETUTCDATE(),'Migration_20171120172700',1,'ReadyForScheduling',0),
		           ('RejectedBySystem','Vom System abgelehnt','de',0,4,GETUTCDATE(),'Migration_20171120172700',GETUTCDATE(),'Migration_20171120172700',1,'ReadyForScheduling',0)");
		}

		public override void Down()
		{
		}
	}
}