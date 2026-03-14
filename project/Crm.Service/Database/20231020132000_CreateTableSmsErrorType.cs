namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model;

	[Migration(20231020132000)]
	public class CreateTableSmsErrorType : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceOrderErrorTypes]"))
			{
				Database.AddTable("[SMS].[ServiceOrderErrorTypes]",
					new Column("ServiceOrderErrorTypeId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("StatisticsKeyFaultImageKey", DbType.String, 20, ColumnProperty.NotNull),
					new Column("InternalRemark", DbType.String, 500, ColumnProperty.Null),
					new Column("Description", DbType.String, 500, ColumnProperty.Null),
					new Column("IsSuspected", DbType.Boolean, 1, ColumnProperty.NotNull, false),
					new Column("IsConfirmed", DbType.Boolean, 1, ColumnProperty.NotNull, false),
					new Column("IsMainErrorType", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("IsExported", DbType.Boolean, 1, ColumnProperty.NotNull, false),
					new Column("OrderId", DbType.Guid, ColumnProperty.Null),
					new Column("DispatchId", DbType.Guid, 16, ColumnProperty.Null),
					new Column("ServiceOrderTimeId", DbType.Guid, 16, ColumnProperty.Null),
					new Column("ServiceCaseId", DbType.Guid, 16, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));

				Database.AddForeignKey("FK_ServiceOrderErrorTypes_Order", "[SMS].[ServiceOrderErrorTypes]", "OrderId", "[SMS].[ServiceOrderHead]", "ContactKey");
				Database.AddForeignKey("FK_ServiceOrderErrorTypes_Dispatch", "[SMS].[ServiceOrderErrorTypes]", "DispatchId", "[SMS].[ServiceOrderDispatch]", "DispatchId");
				Database.AddForeignKey("FK_ServiceOrderErrorTypes_OrderTime", "[SMS].[ServiceOrderErrorTypes]", "ServiceOrderTimeId", "[SMS].[ServiceOrderTimes]", "Id");
				Database.AddForeignKey("FK_ServiceOrderErrorTypes_ServiceCase", "[SMS].[ServiceOrderErrorTypes]", "ServiceCaseId", "[SMS].[ServiceNotifications]", "ContactKey");


				var helper = new UnicoreMigrationHelper(Database);
				helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderErrorType>("SMS", "ServiceOrderErrorTypes", "ServiceOrderErrorTypeId");
			}
		}
	}
}
