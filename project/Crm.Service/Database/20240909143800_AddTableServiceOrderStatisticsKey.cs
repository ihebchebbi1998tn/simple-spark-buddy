namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240909143800)]
	public class AddTableServiceOrderStatisticsKey : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderStatisticsKey]"))
			{
				return;
			}

			Database.AddTable(
				"[SMS].[ServiceOrderStatisticsKey]",
				new Column("ServiceOrderStatisticsKeyId",
					DbType.Guid,
					ColumnProperty.PrimaryKey,
					"NEWSEQUENTIALID()"),
				new Column("CreateUser",
					DbType.String,
					ColumnProperty.NotNull,
					"'Setup'"),
				new Column("ModifyUser",
					DbType.String,
					ColumnProperty.NotNull,
					"'Setup'"),
				new Column("CreateDate",
					DbType.DateTime,
					ColumnProperty.NotNull,
					"GETUTCDATE()"),
				new Column("ModifyDate",
					DbType.DateTime,
					ColumnProperty.NotNull,
					"GETUTCDATE()"),
				new Column("IsActive",
					DbType.Boolean,
					ColumnProperty.NotNull,
					true),
				new Column("DispatchId",
					DbType.Guid,
					ColumnProperty.Null),
				new Column("ServiceOrderId",
					DbType.Guid,
					ColumnProperty.NotNull),
				new Column("ProductTypeKey",
					DbType.String,
					ColumnProperty.Null),
				new Column("MainAssemblyKey",
					DbType.String,
					ColumnProperty.Null),
				new Column("SubAssemblyKey",
					DbType.String,
					ColumnProperty.Null),
				new Column("AssemblyGroupKey",
					DbType.String,
					ColumnProperty.Null),
				new Column("FaultImageKey",
					DbType.String,
					ColumnProperty.Null),
				new Column("RemedyKey",
					DbType.String,
					ColumnProperty.Null),
				new Column("CauseKey",
					DbType.String,
					ColumnProperty.Null),
				new Column("WeightingKey",
					DbType.String,
					ColumnProperty.Null),
				new Column("CauserKey",
					DbType.String,
					ColumnProperty.Null)
			);

			Database.AddForeignKey("FK_ServiceOrderStatisticsKey_ServiceOrder",
				"SMS.ServiceOrderStatisticsKey",
				"ServiceOrderId",
				"CRM.Contact",
				"ContactId",
				ForeignKeyConstraint.Cascade);
			Database.AddForeignKey("FK_ServiceOrderStatisticsKey_Dispatch",
				"SMS.ServiceOrderStatisticsKey",
				"DispatchId",
				"SMS.ServiceOrderDispatch",
				"DispatchId",
				ForeignKeyConstraint.Cascade);

			Database.ExecuteNonQuery(@"INSERT INTO SMS.ServiceOrderStatisticsKey (ServiceOrderId, CreateUser, ModifyUser, CreateDate, ModifyDate, ProductTypeKey, MainAssemblyKey, SubAssemblyKey, AssemblyGroupKey, FaultImageKey, RemedyKey, CauseKey, WeightingKey, CauserKey) SELECT ContactId, ModifyUser, ModifyUser, ModifyDate, ModifyDate, StatisticsKeyProductTypeKey, StatisticsKeyMainAssemblyKey, StatisticsKeySubAssemblyKey, StatisticsKeyAssemblyGroupKey, StatisticsKeyFaultImageKey, StatisticsKeyRemedyKey, StatisticsKeyCauseKey, StatisticsKeyWeightingKey, StatisticsKeyCauserKey FROM SMS.ServiceOrderHead JOIN CRM.Contact ON ContactId = ContactKey WHERE (StatisticsKeyProductTypeKey IS NOT NULL OR StatisticsKeyMainAssemblyKey IS NOT NULL OR StatisticsKeySubAssemblyKey IS NOT NULL OR StatisticsKeyAssemblyGroupKey IS NOT NULL OR StatisticsKeyFaultImageKey IS NOT NULL OR StatisticsKeyRemedyKey IS NOT NULL OR StatisticsKeyCauseKey IS NOT NULL OR StatisticsKeyWeightingKey IS NOT NULL OR StatisticsKeyCauserKey IS NOT NULL) AND IsActive = 1");

			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeyProductType");
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeyMainAssembly");
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeySubAssembly");
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeyAssemblyGroup");
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeyFaultImage");
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeyRemedy");
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeyCause");
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeyWeighting");
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "StatisticsKeyCauser");
		}
	}
}
