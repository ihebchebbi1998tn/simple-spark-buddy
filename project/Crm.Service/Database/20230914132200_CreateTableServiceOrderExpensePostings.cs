namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230914132200)]
	public class CreateTableServiceOrderExpensePostings : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceOrderExpensePostings]"))
			{
				Database.AddTable("[SMS].[ServiceOrderExpensePostings]",
					new Column("ServiceOrderExpensePostingId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Date", DbType.DateTime, ColumnProperty.NotNull),
					new Column("Amount", DbType.Decimal, 5, ColumnProperty.NotNull),
					new Column("Description", DbType.String, 500, ColumnProperty.Null),
					new Column("CurrencyKey", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Closed", DbType.Boolean, 1, ColumnProperty.NotNull),
					new Column("FileResourceKey", DbType.Guid, ColumnProperty.Null),
					new Column("OrderId", DbType.Guid, ColumnProperty.NotNull),
					new Column("PerDiemReportId", DbType.Guid, ColumnProperty.Null),
					new Column("CostCenter", DbType.String, 50, ColumnProperty.Null),
					new Column("ResponsibleUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ExpenseType", DbType.String, 20, ColumnProperty.Null),
					new Column("IsExported", DbType.Boolean, 1, ColumnProperty.NotNull, false),
					new Column("ArticleId", DbType.Guid, 16, ColumnProperty.Null),
					new Column("ItemNo", DbType.String, 50, ColumnProperty.Null),
					new Column("DispatchId", DbType.Guid, 16, ColumnProperty.Null),
					new Column("ServiceOrderTimeId", DbType.Guid, 16, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));

				Database.AddForeignKey("FK_ServiceOrderExpensePostings_FileResource", "[SMS].[ServiceOrderExpensePostings]", "FileResourceKey", "[CRM].[FileResource]", "Id");
				Database.AddForeignKey("FK_ServiceOrderExpensePostings_Order", "[SMS].[ServiceOrderExpensePostings]", "OrderId", "[SMS].[ServiceOrderHead]", "ContactKey");
				Database.AddForeignKey("FK_ServiceOrderExpensePostings_Article", "[SMS].[ServiceOrderExpensePostings]", "ArticleId", "[CRM].[Article]", "ArticleId");
				Database.AddForeignKey("FK_ServiceOrderExpensePostings_Dispatch", "[SMS].[ServiceOrderExpensePostings]", "DispatchId", "[SMS].[ServiceOrderDispatch]", "DispatchId");
				Database.AddForeignKey("FK_ServiceOrderExpensePostings_OrderTime", "[SMS].[ServiceOrderExpensePostings]", "ServiceOrderTimeId", "[SMS].[ServiceOrderTimes]", "Id");

			}
		}
	}
}
