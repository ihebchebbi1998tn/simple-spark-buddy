using Crm.Library.Data.MigratorDotNet.Framework;
using System;

namespace Crm.Service.Database
{
	[Migration(20211022120900)]
	public class AddMoreSpanishLookups : Migration
	{
		public override void Up()
		{
			string tableName = "[SMS].[ServiceContractStatus]";
			if (Database.TableExists(tableName) && (int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {tableName} WHERE [Language] = 'es'") == 0)
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder", "SettableStatuses", "IsActive", "CreateDate", "ModifyDate", "CreateUser", "ModifyUser" };
				InsertLookupValue(tableName, columns, "'Inactivo','es','Inactive',0,2,'Pending,Active',1,GETDATE(),GETDATE(),'Setup','Setup'");
				InsertLookupValue(tableName, columns, "'Activo','es','Active',1,3,'Inactive',1,GETDATE(),GETDATE(),'Setup','Setup'");
				InsertLookupValue(tableName, columns, "'Caducado','es','Expired',0,4,NULL,1,GETDATE(),GETDATE(),'Setup','Setup'");
				InsertLookupValue(tableName, columns, "'Pendiente','es','Pending',0,1,'Inactive',1,GETDATE(),GETDATE(),'Setup','Setup'");
			}
			tableName = "[SMS].[ServiceOrderNoInvoiceReason]";
			if (Database.TableExists(tableName) && (int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {tableName} WHERE [Language] = 'es'") == 0)
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder", "IsActive", "CreateDate", "ModifyDate", "CreateUser", "ModifyUser" };
				InsertLookupValue(tableName, columns, "'Buena voluntad','es','Goodwill',0,0,1,GETDATE(),GETDATE(),'Setup','Setup'");
				InsertLookupValue(tableName, columns, "'Rectificación','es','Rectification',0,1,1,GETDATE(),GETDATE(),'Setup','Setup'");
				InsertLookupValue(tableName, columns, "'Garantía','es','Warranty',0,2,1,GETDATE(),GETDATE(),'Setup','Setup'");
			}
			tableName = "[SMS].[ServiceOrderTimeStatus]";
			if (Database.TableExists(tableName) && (int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {tableName} WHERE [Language] = 'es'") == 0)
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder", "IsActive", "Color", "CreateDate", "ModifyDate", "CreateUser", "ModifyUser" };
				InsertLookupValue(tableName, columns, "'Creado','es','Created',0,0,1,'#9e9e9e',GETDATE(),GETDATE(),'Setup','Setup'");
				InsertLookupValue(tableName, columns, "'Empezado','es','Started',0,1,1,'#2196f3',GETDATE(),GETDATE(),'Setup','Setup'");
				InsertLookupValue(tableName, columns, "'Interrumpido','es','Interrupted',0,2,1,'#ff9800',GETDATE(),GETDATE(),'Setup','Setup'");
				InsertLookupValue(tableName, columns, "'Finalizado','es','Finished',0,3,1,'#4caf50',GETDATE(),GETDATE(),'Setup','Setup'");
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