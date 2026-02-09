namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140205131123)]
	public class AddInvoiceRecipientToSmsServiceContract : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("InvoiceRecipientId", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}