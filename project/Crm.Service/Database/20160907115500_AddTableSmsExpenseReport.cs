namespace Crm.Service.Database
{
	 using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160907115500)]
	public class AddTableSmsExpenseReport : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ExpenseReport]"))
			{
				Database.AddTable("[SMS].[ExpenseReport]", new Column("Id", DbType.Guid, ColumnProperty.PrimaryKey)
																								, new Column("Username", DbType.String, ColumnProperty.Null)
																								, new Column("Status", DbType.String, ColumnProperty.Null)
																								, new Column("Type", DbType.String, ColumnProperty.Null)
																								, new Column("[From]", DbType.DateTime, ColumnProperty.Null)
																								, new Column("[To]", DbType.DateTime, ColumnProperty.Null)
																								, new Column("[Date]", DbType.DateTime, ColumnProperty.Null)
																								, new Column("CreateUser", DbType.String, ColumnProperty.NotNull)
																								, new Column("ModifyUser", DbType.String, ColumnProperty.NotNull)
																								, new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull)
																								, new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull)
																								, new Column("TenantKey", DbType.Int32, ColumnProperty.Null)
																								, new Column("IsActive", DbType.Boolean, ColumnProperty.Null, 1));
			}
		}
	}
}