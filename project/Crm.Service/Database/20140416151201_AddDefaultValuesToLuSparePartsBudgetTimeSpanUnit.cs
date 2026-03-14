namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140416151201)]
	public class AddDefaultValuesToLuSparePartsBudgetTimeSpanUnit : Migration
	{
		public override void Up()
		{
			InsertLookupValue("PerYear", "per year", "en", "0", "0");
			InsertLookupValue("PerYear", "pro Jahr", "de", "0", "0");
			InsertLookupValue("PerQuarter", "per quarter", "en", "0", "1");
			InsertLookupValue("PerQuarter", "pro Quartal", "de", "0", "1");
			InsertLookupValue("PerMonth", "per month", "en", "0", "2");
			InsertLookupValue("PerMonth", "pro Monat", "de", "0", "2");
			InsertLookupValue("PerServiceOrder", "per service order", "en", "1", "3");
			InsertLookupValue("PerServiceOrder", "pro Serviceauftrag", "de", "1", "3");

			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceContract] SET [BudgetServiceProvisionPerTimeSpanUnitKey] = 'PerYear' WHERE [BudgetServiceProvisionPerTimeSpanUnitKey] = 'Year'");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceContract] SET [BudgetServiceProvisionPerTimeSpanUnitKey] = 'PerQuarter' WHERE [BudgetServiceProvisionPerTimeSpanUnitKey] = 'Quarter'");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceContract] SET [BudgetServiceProvisionPerTimeSpanUnitKey] = 'PerMonth' WHERE [BudgetServiceProvisionPerTimeSpanUnitKey] = 'Month'");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceContract] SET [BudgetServiceProvisionPerTimeSpanUnitKey] = 'PerServiceOrder' WHERE [BudgetServiceProvisionPerTimeSpanUnitKey] NOT IN ('PerYear', 'PerQuarter', 'PerMonth')");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceContract] SET [BudgetSparePartsPerTimeSpanUnitKey] = 'PerYear' WHERE [BudgetSparePartsPerTimeSpanUnitKey] = 'Year'");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceContract] SET [BudgetSparePartsPerTimeSpanUnitKey] = 'PerQuarter' WHERE [BudgetSparePartsPerTimeSpanUnitKey] = 'Quarter'");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceContract] SET [BudgetSparePartsPerTimeSpanUnitKey] = 'PerMonth' WHERE [BudgetSparePartsPerTimeSpanUnitKey] = 'Month'");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceContract] SET [BudgetSparePartsPerTimeSpanUnitKey] = 'PerServiceOrder' WHERE [BudgetSparePartsPerTimeSpanUnitKey] NOT IN ('PerYear', 'PerQuarter', 'PerMonth')");
		}
		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.ExecuteNonQuery(String.Format("INSERT INTO [LU].[SparePartsBudgetTimeSpanUnit] " +
			                                       "([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [CreateUser], [ModifyDate], [ModifyUser]) " +
			                                       "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', GETUTCDATE(), '{5}', GETUTCDATE(), '{6}')",
				value, name, language, favorite, sortOrder, "Setup", "Setup"));
		}
	}
}