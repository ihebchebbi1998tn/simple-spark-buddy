namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140128111930)]
	public class AddTableSmsServiceOrderInvoiceReason : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceOrderInvoiceReason]"))
			{
				Database.AddTable("[SMS].[ServiceOrderInvoiceReason]",
					new Column("ServiceOrderInvoiceReasonId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull));
			}
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}