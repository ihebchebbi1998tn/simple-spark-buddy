namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130617183725)]
	public class AlterColumnsInvoiceInformationInSmsServiceContractTypeToNvarcharMax : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE SMS.ServiceContract ALTER COLUMN InvoiceSpecialConditions NVARCHAR(max) NULL");
			Database.ExecuteNonQuery("ALTER TABLE SMS.ServiceContract ALTER COLUMN InternalInvoiceInformation NVARCHAR(max) NULL");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}