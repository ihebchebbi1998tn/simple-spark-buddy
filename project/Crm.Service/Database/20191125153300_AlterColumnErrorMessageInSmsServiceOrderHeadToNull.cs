namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191125153300)]
	public class AlterColumnErrorMessageInSmsServiceOrderHeadToNull : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] ALTER COLUMN [ErrorMessage] nvarchar(max) NULL");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderHead] SET ErrorMessage = NULL WHERE ErrorMessage = ''");
		}
	}
}