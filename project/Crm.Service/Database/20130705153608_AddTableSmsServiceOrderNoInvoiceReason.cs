namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130705153608)]
	public class AddTableSmsServiceOrderNoInvoiceReason : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceOrderNoInvoiceReason]"))
			{
				Database.AddTable("[SMS].[ServiceOrderNoInvoiceReason]",
					new Column("ServiceOrderNoInvoiceReasonId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull));

				InsertLookupValue("Goodwill", "Goodwill", "en", "0", "0");
				InsertLookupValue("Goodwill", "Kulanz", "de", "0", "0");
				InsertLookupValue("Rectification", "Rectification", "en", "0", "1");
				InsertLookupValue("Rectification", "Nachbesserung", "de", "0", "1");
				InsertLookupValue("Warranty", "Warranty", "en", "0", "2");
				InsertLookupValue("Warranty", "Gewährleistung", "de", "0", "2");
			}
		}
		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.Insert("[SMS].[ServiceOrderNoInvoiceReason]",
				new[] { "Value", "Name", "Language", "Favorite", "SortOrder" },
				new [] { value, name, language, favorite, sortOrder });
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}