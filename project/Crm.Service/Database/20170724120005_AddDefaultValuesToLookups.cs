namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Extensions;

	[Migration(20170724120005)]
	public class AddDefaultValuesToLookups : Migration
	{
		public override void Up()
		{
			new[]
			{
				new { Schema = "LU", Table = "CauseOfFailure" },
				new { Schema = "SMS", Table = "CommissioningStatus" },
				new { Schema = "SMS", Table = "Components" },
				new { Schema = "LU", Table = "CostCenter" },
				new { Schema = "SMS", Table = "ErrorCode" },
				new { Schema = "LU", Table = "ExpenseReportStatus" },
				new { Schema = "LU", Table = "ExpenseReportType" },
				new { Schema = "SMS", Table = "ExpenseType" },
				new { Schema = "LU", Table = "InstallationAddressRelationshipType" },
				new { Schema = "SMS", Table = "InstallationHeadStatus" },
				new { Schema = "SMS", Table = "InstallationType" },
				new { Schema = "LU", Table = "Manufacturer" },
				new { Schema = "SMS", Table = "MonitoringDataType" },
				new { Schema = "LU", Table = "NoCausingItemPreviousSerialNoReason" },
				new { Schema = "LU", Table = "NoCausingItemSerialNoReason" },
				new { Schema = "LU", Table = "NoPreviousSerialNoReason" },
				new { Schema = "SMS", Table = "NotificationStandardAction" },
				new { Schema = "SMS", Table = "PostingCode" },
				new { Schema = "SMS", Table = "PostingOrigin" },
				new { Schema = "SMS", Table = "ServiceNotificationCategory" },
				new { Schema = "SMS", Table = "ServiceNotificationStatus" },
				new { Schema = "LU", Table = "ServiceContractAddressRelationshipType" },
				new { Schema = "SMS", Table = "ServiceContractLimitType" },
				new { Schema = "SMS", Table = "ServiceContractType" },
				new { Schema = "LU", Table = "ServiceObjectCategory" },
				new { Schema = "SMS", Table = "ServiceOrderDispatchRejectReason" },
				new { Schema = "SMS", Table = "ServiceOrderDispatchStatus" },
				new { Schema = "SMS", Table = "ServiceOrderInvoiceReason" },
				new { Schema = "SMS", Table = "ServiceOrderNoInvoiceReason" },
				new { Schema = "SMS", Table = "ServiceOrderStatus" },
				new { Schema = "SMS", Table = "ServiceOrderTimeCategory" },
				new { Schema = "SMS", Table = "ServiceOrderTimeLocation" },
				new { Schema = "SMS", Table = "ServiceOrderTimePriority" },
				new { Schema = "SMS", Table = "ServiceOrderTimeScannedValueTypes" },
				new { Schema = "SMS", Table = "ServiceOrderTimeStatus" },
				new { Schema = "SMS", Table = "ServiceOrderType" },
				new { Schema = "SMS", Table = "ServicePriority" },
				new { Schema = "LU", Table = "SparePartsBudgetInvoiceType" },
				new { Schema = "LU", Table = "SparePartsBudgetTimeSpanUnit" },
				new { Schema = "LU", Table = "TimeEntryReportStatus" },
				new { Schema = "LU", Table = "TimeEntryReportType" },
				new { Schema = "SMS", Table = "TimeEntryType" },
				new { Schema = "SMS", Table = "TimeScale" }
			}
			.ForEach(x => Database.AddEntityBaseDefaultContraints(x.Schema, x.Table));
		}
	}
}