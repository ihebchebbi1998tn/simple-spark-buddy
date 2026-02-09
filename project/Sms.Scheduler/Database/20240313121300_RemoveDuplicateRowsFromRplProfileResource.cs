namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	
	[Migration(20240313121300)]
	public class RemoveDuplicateRowsFromRplProfileResource : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"DELETE FROM [RPL].[ProfileResource]
											WHERE [Id] NOT IN (
												SELECT MIN([Id])
												FROM [RPL].[ProfileResource]
												GROUP BY [ProfileKey], [ResourceKey])");
		}
	}
}
