namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170822153901)]
	public class UpdateSmsServiceOrderTimeHasToolToNonNullable : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
UPDATE [SMS].[ServiceOrderTimes] SET [HasTool] = 0 WHERE [HasTool] IS NULL OR [HasTool] <> 1
ALTER TABLE [SMS].[ServiceOrderTimes] ALTER COLUMN [HasTool] BIT NOT NULL
ALTER TABLE [SMS].[ServiceOrderTimes] ADD DEFAULT 0 FOR [HasTool]
");
		}
	}
}