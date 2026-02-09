namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140218091100)]
	public class AddTableLuInvoicingType : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("LU.InvoicingType"))
			{
				Database.AddTable("LU.InvoicingType",
					new Column("InvoicingTypeId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null));
			}
		}
	}
}