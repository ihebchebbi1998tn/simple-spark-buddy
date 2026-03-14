namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131105112123)]
	public class AddPayerAndInvoiceRecipientToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("PayerId", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("PayerAddressId", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("InvoiceRecipientId", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("InvoiceRecipientAddressId", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}