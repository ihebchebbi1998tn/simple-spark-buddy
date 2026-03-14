namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140128111932)]
	public class AddInvoiceRemarkToServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("InvoiceRemark", DbType.String, Int32.MaxValue, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}