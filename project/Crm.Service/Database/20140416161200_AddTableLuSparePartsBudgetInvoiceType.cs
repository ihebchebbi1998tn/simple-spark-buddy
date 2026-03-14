namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140416161200)]
	public class AddTableLuSparePartsBudgetInvoiceType : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[SparePartsBudgetInvoiceType]"))
			{
				Database.AddTable("[LU].[SparePartsBudgetInvoiceType]",
					new Column("SparePartsBudgetInvoiceTypeId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
			}
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}