namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191111164700)]
	public class AddDefaultValueForServiceOrderTimeStatus : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimes] ADD CONSTRAINT DF_ServiceOrderTimes_Status DEFAULT 'Created' FOR [Status]");
		}
	}
}