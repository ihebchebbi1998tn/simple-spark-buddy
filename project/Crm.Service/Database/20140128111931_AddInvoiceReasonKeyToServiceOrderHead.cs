namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140128111931)]
	public class AddInvoiceReasonKeyToServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("InvoiceReason", DbType.String, 50, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}