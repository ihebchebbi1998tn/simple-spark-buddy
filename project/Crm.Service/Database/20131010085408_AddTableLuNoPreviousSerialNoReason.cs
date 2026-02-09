namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131010085408)]
	public class AddTableLuNoPreviousSerialNoReason : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[NoPreviousSerialNoReason]"))
			{
				Database.AddTable("[LU].[NoPreviousSerialNoReason]",
					new Column("NoPreviousSerialNoReasonId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null));

				InsertLookupValue("NotAvailable", "Not available", "en", "0", "0");
				InsertLookupValue("NotAvailable", "Nicht vorhanden", "de", "0", "0");
				InsertLookupValue("Nonreadable", "Nonreadable", "en", "0", "1");
				InsertLookupValue("Nonreadable", "Nicht lesbar", "de", "0", "1");
			}
		}
		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.Insert("[LU].[NoPreviousSerialNoReason]",
				new[] { "Value", "Name", "Language", "Favorite", "SortOrder" },
				new [] { value, name, language, favorite, sortOrder });
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}