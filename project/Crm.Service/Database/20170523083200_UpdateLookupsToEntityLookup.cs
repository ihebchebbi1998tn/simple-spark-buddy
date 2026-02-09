namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170523083200)]
	public class UpdateLookupsToEntityLookup : Migration
	{
		public override void Up()
		{
			const string createUser = "Migration_20170523083200";
			AddEntityLookupColumns("Sms", "Components", createUser);
			AddEntityLookupColumns("Sms", "InstallationType", createUser);
			AddEntityLookupColumns("Sms", "MonitoringDataType", createUser);
			AddEntityLookupColumns("Lu", "NoCausingItemPreviousSerialNoReason", createUser);
			AddEntityLookupColumns("Lu", "NoCausingItemSerialNoReason", createUser);
			AddEntityLookupColumns("Lu", "NoPreviousSerialNoReason", createUser);
			AddEntityLookupColumns("Sms", "NotificationStandardAction", createUser);
			AddEntityLookupColumns("Sms", "ServiceNotificationCategory", createUser);
			AddEntityLookupColumns("Sms", "ServiceNotificationStatus", createUser);
			AddEntityLookupColumns("Sms", "ServiceContractLimitType", createUser);
			AddEntityLookupColumns("Sms", "ServiceContractType", createUser);
			AddEntityLookupColumns("Lu", "ServiceObjectCategory", createUser);
			AddEntityLookupColumns("Sms", "ServiceOrderInvoiceReason", createUser);
			AddEntityLookupColumns("Sms", "ServiceOrderNoInvoiceReason", createUser);
			AddEntityLookupColumns("Sms", "ServiceOrderStatus", createUser);
			AddEntityLookupColumns("Sms", "ServiceOrderTimeCategory", createUser);
			AddEntityLookupColumns("Sms", "ServiceOrderTimeLocation", createUser);
			AddEntityLookupColumns("Sms", "ServiceOrderTimePriority", createUser);
			AddEntityLookupColumns("Sms", "ServiceOrderTimeStatus", createUser);
			AddEntityLookupColumns("Sms", "ServiceOrderType", createUser);
			AddEntityLookupColumns("Sms", "ServicePriority", createUser);
		}

		private void AddEntityLookupColumns(string schema, string table, string createUser)
		{
			Database.ExecuteNonQuery($@"
				IF COL_LENGTH('[{schema}].[{table}]','CreateDate') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_{schema}{table}_CreateDate DEFAULT GETUTCDATE()
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_CreateDate
				END
				IF COL_LENGTH('[{schema}].[{table}]','ModifyDate') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_{schema}{table}_ModifyDate DEFAULT GETUTCDATE()
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_ModifyDate
				END
				IF COL_LENGTH('[{schema}].[{table}]','CreateUser') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_{schema}{table}_CreateUser DEFAULT '{createUser}'
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_CreateUser
				END
				IF COL_LENGTH('[{schema}].[{table}]','ModifyUser') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_{schema}{table}_ModifyUser DEFAULT '{createUser}'
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_ModifyUser
				END
				IF COL_LENGTH('[{schema}].[{table}]','IsActive') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [IsActive] bit NOT NULL CONSTRAINT DF_{schema}{table}_IsActive DEFAULT 1
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_IsActive
				END
			");
		}
	}
}