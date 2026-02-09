namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131101164216)]
	public class AlterColumnErrorMessageInSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] ALTER COLUMN [ErrorMessage] NVARCHAR(MAX)");
		}

		public override void Down()
		{
		}
	}
}