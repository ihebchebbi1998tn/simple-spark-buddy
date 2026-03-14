namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190705123000)]
	public class UpdateDispatchTimeToContainDate : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
					UPDATE [SMS].[ServiceOrderDispatch] 
						SET [Time] = CAST(CONVERT(date, Date) as datetime) + CAST(CONVERT(time, Time) as datetime),
								[ModifyDate] = GETUTCDATE(),
								[ModifyUser] = 'Migration_20190705123000'
						WHERE Status in ('Scheduled','Released','Read','InProgress','SignedByCustomer')");
		}
	}
}
