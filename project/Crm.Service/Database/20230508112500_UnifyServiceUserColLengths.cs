namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230508112500)]
	public class UnifyServiceUserColLengths : Migration
	{
		public override void Up()
		{
			(string, string)[] tables = { ("LU", "CauseOfFailure"), ("LU", "ServiceObjectCategory"), ("SMS", "Components"), ("SMS", "ErrorCode"), ("SMS", "CommissioningStatus"), ("SMS", "InstallationPos"), ("SMS", "InstallationPosSerials"), ("SMS", "InstallationType"), ("SMS", "Location"), ("SMS", "MonitoringDataType"), ("SMS", "NotificationStandardAction"), ("SMS", "RdsPpStructure"), ("SMS", "ServiceContractLimitType"), ("SMS", "ServiceContractStatus"), ("SMS", "ServiceContractType"), ("SMS", "ServiceNotificationCategory"), ("SMS", "ServiceNotificationStatus"), ("SMS", "ServiceOrderInvoiceReason"), ("SMS", "ServiceOrderMaterialSerials"), ("SMS", "ServiceOrderNoInvoiceReason"), ("SMS", "ServiceOrderStatus"), ("SMS", "ServiceOrderTimeCategory"), ("SMS", "ServiceOrderTimeLocation"), ("SMS", "ServiceOrderTimePriority"), ("SMS", "ServiceOrderTimeStatus"), ("SMS", "ServiceOrderType"), ("SMS", "ServicePriority"), ("SMS", "Store") };
			foreach (var table in tables)
			{
				var tableName = $"[{table.Item1}].[{table.Item2}]";
				if (Database.TableExists($"{tableName}"))
				{
					if (Database.ColumnExists($"{tableName}", "ModifyUser"))
					{
						var modifyUserLength = (int)Database.ExecuteScalar(
							$"SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{table.Item1}' AND TABLE_NAME = '{table.Item2}' AND COLUMN_NAME = 'ModifyUser'");
						if (modifyUserLength < 255)
							Database.ExecuteNonQuery($"ALTER TABLE {tableName} ALTER COLUMN ModifyUser NVARCHAR(255) NOT NULL");
					}

					if (Database.ColumnExists($"{tableName}", "CreateUser"))
					{
						var createUserLength = (int)Database.ExecuteScalar(
							$"SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{table.Item1}' AND TABLE_NAME = '{table.Item2}' AND COLUMN_NAME = 'CreateUser'");
						if (createUserLength < 255)
							Database.ExecuteNonQuery($"ALTER TABLE {tableName} ALTER COLUMN CreateUser NVARCHAR(255) NOT NULL");
					}
				}
			}
		}
	}
}
