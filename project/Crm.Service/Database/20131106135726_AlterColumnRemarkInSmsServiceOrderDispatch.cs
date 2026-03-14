namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131106135726)]
	public class AlterColumnRemarkInSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE SMS.ServiceOrderDispatch ALTER COLUMN Remark nvarchar(max)");
		}

		public override void Down()
		{
		}
	}
}