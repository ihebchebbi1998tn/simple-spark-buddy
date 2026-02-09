using System;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20210428163200)]
	public class AddSpanishLookups : Migration
	{
		public override void Up()
		{
			string tableName = "[LU].[InstallationAddressRelationshipType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Destinatario de la factura', 'es', 'InvoiceRecipient', 0, 0");
			}
			tableName = "[LU].[ServiceContractAddressRelationshipType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'InvoiceRecipient', 'Destinatario de la factura', 'es', 0, 0");
			}
			tableName = "[LU].[ServiceObjectCategory]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "Favorite", "SortOrder", "Color" };
				InsertLookupValue(tableName, columns, "'Transport', 'Transporte', 'es', 0, 0, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'PublicFacilities', 'Instalaciones públicas', 'es', 0, 1, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Sport', 'Deporte', 'es', 0, 2, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Shopping', 'Compras', 'es', 0, 3, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Industry', 'Industria', 'es', 0, 4, '#9E9E9E'");
			}
			tableName = "[LU].[SparePartsBudgetInvoiceType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'InvoiceDifference', 'Diferencia de la factura', 'es', 0, 0");
				InsertLookupValue(tableName, columns, "'CompleteInvoice', 'Factura completa', 'es', 0, 1");
			}
			tableName = "[LU].[SparePartsBudgetTimeSpanUnit]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'PerYear', 'por año', 'es', 0, 0");
				InsertLookupValue(tableName, columns, "'PerQuarter', 'por trimestre', 'es', 0, 1");
				InsertLookupValue(tableName, columns, "'PerMonth', 'por mes', 'es', 0, 2");
				InsertLookupValue(tableName, columns, "'PerServiceOrder', 'por pedido de servicio', 'es', 1, 3");
			}
			tableName = "[SMS].[ServiceNotificationStatus]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "SortOrder", "Groups", "SettableStatuses" };
				InsertLookupValue(tableName, columns, "'Nuevo', 'es', 0, 0, 'Preparation', '2,4,6'");
				InsertLookupValue(tableName, columns, "'En espera', 'es', 2, 2, 'Preparation', '4,5,6'");
				InsertLookupValue(tableName, columns, "'En curso', 'es', 4, 4, 'InProgress', '2,6'");
				InsertLookupValue(tableName, columns, "'Reabierto', 'es', 5, 5, 'InProgress', '2,4,6'");
				InsertLookupValue(tableName, columns, "'Cerrado', 'es', 6, 6, 'Closed', '5'");
			}
			tableName = "[SMS].[ServicePriority]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder", "Color" };
				InsertLookupValue(tableName, columns, "'Baja', 'es', 0, 0, 2, '#03A9F4'");
				InsertLookupValue(tableName, columns, "'Media', 'es', 1, 0, 1, '#FF9800'");
				InsertLookupValue(tableName, columns, "'Alta', 'es', 2, 0, 0, '#F44336'");
			}
			tableName = "[SMS].[ServiceNotificationCategory]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Error', 'es', 0, 0, 0");
				InsertLookupValue(tableName, columns, "'Pregunta', 'es', 1, 0, 0");
				InsertLookupValue(tableName, columns, "'Queja', 'es', 2, 0, 0");
				InsertLookupValue(tableName, columns, "'Sugerencia', 'es', 3, 0, 0");
			}
			tableName = "[SMS].[ServiceOrderType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "NumberingSequence", "MaintenanceOrder", "ShowInMobileClient", "Color" };
				InsertLookupValue(tableName, columns, "'Orden de mantenimiento', 'es', 0, 0, 'SMS.ServiceOrderHead.MaintenanceOrder', 1, 1, '#009688'");
				InsertLookupValue(tableName, columns, "'Orden de servicio correctivo', 'es', 1, 1, 'SMS.ServiceOrderHead.ServiceOrder', 0, 1, '#FF9800'");
				InsertLookupValue(tableName, columns, "'Orden de plantilla', 'es', 2, 0, 'SMS.ServiceOrderHead.ServiceOrder', 0, 1, '#607D8B'");
			}
			tableName = "[SMS].[ServiceOrderStatus]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "SortOrder", "Groups", "SettableStatuses" };
				InsertLookupValue(tableName, columns, "'Nuevo', 'es', 'New', 1, 'Preparation', 'ReadyForScheduling'");
				InsertLookupValue(tableName, columns, "'Listo para programación', 'es', 'ReadyForScheduling', 2, 'Scheduling', 'New'");
				InsertLookupValue(tableName, columns, "'Programado', 'es', 'Scheduled', 3, 'Scheduling', NULL");
				InsertLookupValue(tableName, columns, "'Parcialmente publicado', 'es', 'PartiallyReleased', 4, 'Scheduling', NULL");
				InsertLookupValue(tableName, columns, "'Publicado', 'es', 'Released', 5, 'Scheduling', NULL");
				InsertLookupValue(tableName, columns, "'En curso', 'es', 'InProgress', 6, 'InProgress', NULL");
				InsertLookupValue(tableName, columns, "'Parcialmente completado', 'es', 'PartiallyCompleted', 7, 'InProgress', 'PostProcessing'");
				InsertLookupValue(tableName, columns, "'Técnicamente completado', 'es', 'Completed', 8, 'InProgress', NULL");
				InsertLookupValue(tableName, columns, "'Postprocesamiento', 'es', 'PostProcessing', 9, 'PostProcessing', 'ReadyForInvoice,Completed'");
				InsertLookupValue(tableName, columns, "'Listo para la factura', 'es', 'ReadyForInvoice', 10, 'PostProcessing', 'PostProcessing,Invoiced'");
				InsertLookupValue(tableName, columns, "'Facturado', 'es', 'Invoiced', 11, 'PostProcessing', 'Closed'");
				InsertLookupValue(tableName, columns, "'Cerrado', 'es', 'Closed', 12, 'Closed', NULL");
			}
			tableName = "[SMS].[ServiceOrderDispatchStatus]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Programado', 'es', 'Scheduled', 1");
				InsertLookupValue(tableName, columns, "'Publicado', 'es', 'Released', 2");
				InsertLookupValue(tableName, columns, "'En curso', 'es', 'InProgress', 4");
				InsertLookupValue(tableName, columns, "'Firmado por el cliente', 'es', 'SignedByCustomer', 5");
				InsertLookupValue(tableName, columns, "'Cerrado (se requiere una orden de seguimiento)', 'es', 'ClosedNotComplete', 6");
				InsertLookupValue(tableName, columns, "'Cerrado', 'es', 'ClosedComplete', 7");
				InsertLookupValue(tableName, columns, "'Leído', 'es', 'Read', 3");
				InsertLookupValue(tableName, columns, "'Rechazado', 'es', 'Rejected', 8");
			}
			tableName = "[SMS].[InstallationType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder", "GroupKey" };
				InsertLookupValue(tableName, columns, "'Otro', 'es', 0, 0, 0, NULL");
			}
			tableName = "[SMS].[InstallationHeadStatus]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Color" };
				InsertLookupValue(tableName, columns, "'Planificado', 'es', 0, '#607d8b'");
				InsertLookupValue(tableName, columns, "'En instalación', 'es', 1, '#2196F3'");
				InsertLookupValue(tableName, columns, "'Funcionando', 'es', 2, '#4CAF50'");
				InsertLookupValue(tableName, columns, "'Funcionamiento limitado', 'es', 3, '#ff9800'");
				InsertLookupValue(tableName, columns, "'Averiado', 'es', 4, '#F44336'");
			}
			tableName = "[SMS].[ServiceContractType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Color", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Desconocido', 'es', 0, '#000000', 1");
				InsertLookupValue(tableName, columns, "'Platinum', 'es', 1, '#ededef', 2");
				InsertLookupValue(tableName, columns, "'Gold', 'es', 2, '#ffd700', 3");
				InsertLookupValue(tableName, columns, "'Silver', 'es', 3, '#c0c0c0', 4");
			}
		}
		private void InsertLookupValue(string tableName, string[] columns, string values, bool hasIsActiveColumn = true)
		{
			int keyColumnIndex = Array.IndexOf(columns, "Value");
			string keyValue = values.Split(',')[keyColumnIndex].Trim(new char[] { ' ', '\'' });
			if ((int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {tableName} WHERE {(hasIsActiveColumn ? "[IsActive]" : 1)} = 1 AND [Value] = '{keyValue}'") > 0)
			{
				Database.ExecuteNonQuery($"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({values})");
			}
		}
	}
}