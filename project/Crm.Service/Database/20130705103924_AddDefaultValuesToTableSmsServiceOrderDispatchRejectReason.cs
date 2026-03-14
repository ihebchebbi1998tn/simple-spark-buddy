namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130705103924)]
	public class AddDefaultValuesToTableSmsServiceOrderDispatchRejectReason : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderDispatchRejectReason]") && Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [SMS].[ServiceOrderDispatchRejectReason]")) == 0)
			{
				InsertLookupValue("FalseAlarm", "False alarm", "en", "0", "0");
				InsertLookupValue("FalseAlarm", "Fehlalarm", "de", "0", "0");
				InsertLookupValue("ConflictingDates", "Conflicting dates", "en", "0", "1");
				InsertLookupValue("ConflictingDates", "Terminkonflikt", "de", "0", "1");
				InsertLookupValue("CustomerNotAccessible", "Customer not accessible", "en", "0", "2");
				InsertLookupValue("CustomerNotAccessible", "Kunde nicht erreichbar", "de", "0", "2");
				InsertLookupValue("InstallationNotAccessible", "Installation not accessible", "en", "0", "3");
				InsertLookupValue("InstallationNotAccessible", "Anlage nicht erreichbar", "de", "0", "3");
			}
		}
		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.ExecuteNonQuery(String.Format("INSERT INTO [SMS].[ServiceOrderDispatchRejectReason] " +
			                                       "([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [CreateUser], [ModifyDate], [ModifyUser]) " +
			                                       "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', GETUTCDATE(), '{5}', GETUTCDATE(), '{6}')",
				value, name, language, favorite, sortOrder, "Setup", "Setup"));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}