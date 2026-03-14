namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240531153000)]
	public class RemoveDischargedUsersFromProfile : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery($@" DELETE FROM [RPL].[ProfileResource] WHERE [ResourceKey] IN (SELECT [Username] FROM [CRM].[User] WHERE [Discharged] = 1)");
		}
	}
}
