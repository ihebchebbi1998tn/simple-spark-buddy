namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131205170100)]
	public class AddIsActiveToSmsTimeEntry : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.TimeEntry", "IsActive"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntry] ADD IsActive bit NULL");
				Database.ExecuteNonQuery("update [SMS].[TimeEntry] set IsActive = 1");
			}

		}

		public override void Down()
		{
		}
	}
}