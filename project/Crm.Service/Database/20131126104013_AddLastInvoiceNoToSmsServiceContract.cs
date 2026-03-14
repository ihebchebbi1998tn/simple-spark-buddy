namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131126104013)]
	public class AddLastInvoiceNoToSmsServiceContract : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceContract", "LastInvoiceNo"))
			{
				Database.ExecuteNonQuery("ALTER TABLE SMS.ServiceContract ADD LastInvoiceNo nvarchar(120)");
			}
		}

		public override void Down()
		{
		}
	}
}