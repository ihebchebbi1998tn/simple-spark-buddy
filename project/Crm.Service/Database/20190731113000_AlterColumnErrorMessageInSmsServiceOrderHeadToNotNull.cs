namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190731113000)]
	public class AlterColumnErrorMessageInSmsServiceOrderHeadToNotNull : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderHead] SET ErrorMessage = '' WHERE ErrorMessage IS NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] ALTER COLUMN [ErrorMessage] NVARCHAR(MAX) NOT NULL");
		}
	}
}