namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model;

	[Migration(20231020142000)]
	public class CreateTableSmsErrorCause : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceOrderErrorCauses]"))
			{
				Database.AddTable("[SMS].[ServiceOrderErrorCauses]",
					new Column("ServiceOrderErrorCauseId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("StatisticsKeyCauseKey", DbType.String, 20, ColumnProperty.NotNull),
					new Column("InternalRemark", DbType.String, 500, ColumnProperty.Null),
					new Column("Description", DbType.String, 500, ColumnProperty.Null),
					new Column("IsSuspected", DbType.Boolean, 1, ColumnProperty.NotNull, false),
					new Column("IsConfirmed", DbType.Boolean, 1, ColumnProperty.NotNull, false),
					new Column("IsExported", DbType.Boolean, 1, ColumnProperty.NotNull, false),
					new Column("ServiceOrderErrorTypeId", DbType.Guid, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));

				Database.AddForeignKey("FK_ServiceOrderErrorCauses_ServiceOrderErrorType", "[SMS].[ServiceOrderErrorCauses]", "ServiceOrderErrorTypeId", "[SMS].[ServiceOrderErrorTypes]", "ServiceOrderErrorTypeId");

				var helper = new UnicoreMigrationHelper(Database);
				helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderErrorCause>("SMS", "ServiceOrderErrorCauses", "ServiceOrderErrorCauseId");

			}
		}
	}
}
