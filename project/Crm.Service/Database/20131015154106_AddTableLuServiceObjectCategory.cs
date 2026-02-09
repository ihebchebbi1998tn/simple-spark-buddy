namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131015154106)]
	public class AddTableLuServiceObjectCategory : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[ServiceObjectCategory]"))
			{
				Database.AddTable("[LU].[ServiceObjectCategory]",
					new Column("ServiceObjectCategoryId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null));

				InsertLookupValue("Transport", "Transport", "en", "0", "0");
				InsertLookupValue("Transport", "Transport", "de", "0", "0");
				InsertLookupValue("PublicFacilities", "Public facilities", "en", "0", "1");
				InsertLookupValue("PublicFacilities", "Öffentliche Einrichtungen", "de", "0", "1");
				InsertLookupValue("Sport", "Sport", "en", "0", "2");
				InsertLookupValue("Sport", "Sport / Freizeit", "de", "0", "2");
				InsertLookupValue("Shopping", "Shopping", "en", "0", "3");
				InsertLookupValue("Shopping", "Shopping", "de", "0", "3");
				InsertLookupValue("Industry", "Industry", "en", "0", "4");
				InsertLookupValue("Industry", "Industrie / Gewerbe", "de", "0", "4");
			}
		}
		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.Insert("[LU].[ServiceObjectCategory]",
				new[] { "Value", "Name", "Language", "Favorite", "SortOrder" },
				new [] { value, name, language, favorite, sortOrder });
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}