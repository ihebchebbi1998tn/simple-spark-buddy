namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240412150000)]
	public class UpdateColorsForDispatchStatuses : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderDispatchStatus] SET [Color] = '#FFDAC2' WHERE [Value] = 'Scheduled'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderDispatchStatus] SET [Color] = '#FBEDB1' WHERE [Value] = 'Released'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderDispatchStatus] SET [Color] = '#C2D6FF' WHERE [Value] = 'InProgress'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderDispatchStatus] SET [Color] = '#C2EFFF' WHERE [Value] = 'SignedByCustomer'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderDispatchStatus] SET [Color] = '#CAC2FF' WHERE [Value] = 'ClosedNotComplete'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderDispatchStatus] SET [Color] = '#F9D2DA' WHERE [Value] = 'ClosedComplete'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderDispatchStatus] SET [Color] = '#D4F7E9' WHERE [Value] = 'Read'");
		}
	}
}
