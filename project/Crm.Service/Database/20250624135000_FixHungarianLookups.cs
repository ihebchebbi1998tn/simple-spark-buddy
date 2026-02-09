namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20250627135000)]
	public class FixHungarianLookups : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[SparePartsBudgetTimeSpanUnit]"))
			{
				AddOrUpdate("[LU].[SparePartsBudgetTimeSpanUnit]",
					"PerYear",
					"évente");
				AddOrUpdate("[LU].[SparePartsBudgetTimeSpanUnit]",
					"PerQuarter",
					"negyedévente");
				AddOrUpdate("[LU].[SparePartsBudgetTimeSpanUnit]",
					"PerMonth",
					"havonta");
				AddOrUpdate("[LU].[SparePartsBudgetTimeSpanUnit]",
					"PerServiceOrder",
					"szervíz rendelésenként");
			}

			if (Database.TableExists("[LU].[SparePartsBudgetInvoiceType]"))
			{
				AddOrUpdate("[LU].[SparePartsBudgetInvoiceType]",
					"InvoiceDifference",
					"Számlakülönbözet");
				AddOrUpdate("[LU].[SparePartsBudgetInvoiceType]",
					"CompleteInvoice",
					"Teljes számla");
			}

			if (Database.TableExists("[SMS].[InstallationType]"))
			{
				AddOrUpdate("[SMS].[InstallationType]",
					"0",
					"Egyéb");
			}

			if (Database.TableExists("[LU].[InstallationAddressRelationshipType]"))
			{
				AddOrUpdate("[LU].[InstallationAddressRelationshipType]",
					"InvoiceRecipient",
					"Számla címzettje");
			}

			if (Database.TableExists("[LU].[NoCausingItemSerialNoReason]"))
			{
				AddOrUpdate("[LU].[NoCausingItemSerialNoReason]",
					"NotAvailable",
					"Nem elérhető");
				AddOrUpdate("[LU].[NoCausingItemSerialNoReason]",
					"NotReplaced",
					"Nem cserélték ki");
			}

			if (Database.TableExists("[LU].[NoCausingItemPreviousSerialNoReason]"))
			{
				AddOrUpdate("[LU].[NoCausingItemPreviousSerialNoReason]",
					"NotAvailable",
					"Nem elérhető");
				AddOrUpdate("[LU].[NoCausingItemPreviousSerialNoReason]",
					"Nonreadable",
					"Nem olvasható");
			}

			if (Database.TableExists("[LU].[NoPreviousSerialNoReason]"))
			{
				AddOrUpdate("[LU].[NoPreviousSerialNoReason]",
					"NotAvailable",
					"Nem elérhető");
				AddOrUpdate("[LU].[NoPreviousSerialNoReason]",
					"Nonreadable",
					"Nem olvasható");
			}

			if (Database.TableExists("[SMS].[ServiceOrderDispatchRejectReason]"))
			{
				AddOrUpdate("[SMS].[ServiceOrderDispatchRejectReason]",
					"FalseAlarm",
					"Téves riasztás");
				AddOrUpdate("[SMS].[ServiceOrderDispatchRejectReason]",
					"ConflictingDates",
					"Nem összeegyeztethető időpontok");
				AddOrUpdate("[SMS].[ServiceOrderDispatchRejectReason]",
					"CustomerNotAccessible",
					"Ügyfél nem elérhető");
				AddOrUpdate("[SMS].[ServiceOrderDispatchRejectReason]",
					"InstallationNotAccessible",
					"Berendezés nem elérhető");
			}

			if (Database.TableExists("[SMS].[ServicePriority]"))
			{
				AddOrUpdate("[SMS].[ServicePriority]",
					"0",
					"Alacsony");
				AddOrUpdate("[SMS].[ServicePriority]",
					"1",
					"Közepes");
				AddOrUpdate("[SMS].[ServicePriority]",
					"2",
					"Magas");
			}

			if (Database.TableExists("[SMS].[ServiceOrderNoInvoiceReason]"))
			{
				AddOrUpdate("[SMS].[ServiceOrderNoInvoiceReason]",
					"Goodwill",
					"Jóakarat");
				AddOrUpdate("[SMS].[ServiceOrderNoInvoiceReason]",
					"Rectification",
					"Helyreigazítás");
				AddOrUpdate("[SMS].[ServiceOrderNoInvoiceReason]",
					"Warranty",
					"Garancia");
			}

			if (Database.TableExists("[SMS].[ServiceNotificationCategory]"))
			{
				AddOrUpdate("[SMS].[ServiceNotificationCategory]",
					"0",
					"Hiba");
				AddOrUpdate("[SMS].[ServiceNotificationCategory]",
					"1",
					"Kérdés");
				AddOrUpdate("[SMS].[ServiceNotificationCategory]",
					"2",
					"Panasz");
				AddOrUpdate("[SMS].[ServiceNotificationCategory]",
					"3",
					"Javaslat");
			}

			if (Database.TableExists("[SMS].[NotificationStandardAction]"))
			{
				AddOrUpdate("[SMS].[NotificationStandardAction]",
					"dismiss",
					"Azonnal elbocsátani");
				AddOrUpdate("[SMS].[NotificationStandardAction]",
					"resolved",
					"Megoldottra állítva");
				AddOrUpdate("[SMS].[NotificationStandardAction]",
					"convert",
					"Átalakítani rendelésre");
				AddOrUpdate("[SMS].[NotificationStandardAction]",
					"info",
					"Információ");
				AddOrUpdate("[SMS].[NotificationStandardAction]",
					"progress",
					"Folyamatban");
			}

			if (Database.TableExists("[SMS].[ServiceNotificationStatus]"))
			{
				AddOrUpdate("[SMS].[ServiceNotificationStatus]",
					"0",
					"Új");
				AddOrUpdate("[SMS].[ServiceNotificationStatus]",
					"2",
					"Elhalasztva");
				AddOrUpdate("[SMS].[ServiceNotificationStatus]",
					"4",
					"Folyamatban");
				AddOrUpdate("[SMS].[ServiceNotificationStatus]",
					"5",
					"Újranyitva");
				AddOrUpdate("[SMS].[ServiceNotificationStatus]",
					"6",
					"Lezárva");
			}

			if (Database.TableExists("[LU].[ServiceObjectCategory]"))
			{
				AddOrUpdate("[LU].[ServiceObjectCategory]",
					"Transport",
					"Közlekedés");
				AddOrUpdate("[LU].[ServiceObjectCategory]",
					"PublicFacilities",
					"Közintézmények");
				AddOrUpdate("[LU].[ServiceObjectCategory]",
					"Sport",
					"Sport");
				AddOrUpdate("[LU].[ServiceObjectCategory]",
					"Industry",
					"Ipar");
				AddOrUpdate("[LU].[ServiceObjectCategory]",
					"Shopping",
					"Bevásárlás");
			}

			if (Database.TableExists("[LU].[ServiceContractAddressRelationshipType]"))
			{
				AddOrUpdate("[LU].[ServiceContractAddressRelationshipType]",
					"InvoiceRecipient",
					"Számla címzettje");
			}

			if (Database.TableExists("[SMS].[ServiceContractLimitType]"))
			{
				AddOrUpdate("[SMS].[ServiceContractLimitType]",
					"1",
					"Nincs anyagi számla");
				AddOrUpdate("[SMS].[ServiceContractLimitType]",
					"2",
					"Teljes számla határértékétől");
				AddOrUpdate("[SMS].[ServiceContractLimitType]",
					"3",
					"Különbözeti számla határértékétől");
			}

			if (Database.TableExists("[SMS].[ServiceOrderTimeStatus]"))
			{
				AddOrUpdate("[SMS].[ServiceOrderTimeStatus]",
					"Started",
					"Megkezdve");
				AddOrUpdate("[SMS].[ServiceOrderTimeStatus]",
					"Created",
					"Létrehozva");
				AddOrUpdate("[SMS].[ServiceOrderTimeStatus]",
					"Interrupted",
					"Megszakítva");
				AddOrUpdate("[SMS].[ServiceOrderTimeStatus]",
					"Finished",
					"Befejezve");
			}
		}

		public void AddOrUpdate(string table, string lookupValue, string lookupName)
		{
			if ((int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {table} WHERE [Value] = N'{lookupValue}'") == 0)
			{
				return;
			}

			if ((int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {table} WHERE [Language] = 'hu' AND [Value] = N'{lookupValue}'") == 0)
			{
				var favorite = (bool)Database.ExecuteScalar($"SELECT [Favorite] FROM {table} WHERE [Language] = 'en' AND [Value] = N'{lookupValue}'") ? "1" : "0";
				var sortOrder = Database.ExecuteScalar($"SELECT [SortOrder] FROM {table} WHERE [Language] = 'en' AND [Value] = N'{lookupValue}'").ToString();
				string color = null;
				if (Database.ColumnExists(table, "Color"))
				{
					color = Database.ExecuteScalar($"SELECT Color FROM {table} WHERE [Language] = 'en' AND [Value] = N'{lookupValue}'").ToString();
				}

				if (color == null)
				{
					Database.ExecuteNonQuery($"INSERT INTO {table} ([Language], [Value], [Name], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES ('hu', N'{lookupValue}', N'{lookupName}', '{favorite}', '{sortOrder}', GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
				}
				else
				{
					Database.ExecuteNonQuery($"INSERT INTO {table} ([Language], [Value], [Name], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES ('hu', N'{lookupValue}', N'{lookupName}', '{favorite}', '{sortOrder}', '{color}', GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
				}
			}
			else
			{
				Database.ExecuteNonQuery($"UPDATE {table} SET [Name] = N'{lookupName}', [ModifyDate] = GETUTCDATE() WHERE [Language] = 'hu' AND [Value] = N'{lookupValue}'");
			}
		}
	}
}
