namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140416161201)]
	public class AddDefaultValuesToLuSparePartsBudgetInvoiceType : Migration
	{
		public override void Up()
		{
			InsertLookupValue("InvoiceDifference", "Invoice difference", "en", "0", "0");
			InsertLookupValue("InvoiceDifference", "Differenzberechnung", "de", "0", "0");
			InsertLookupValue("CompleteInvoice", "Complete invoice", "en", "0", "1");
			InsertLookupValue("CompleteInvoice", "Vollberechnung", "de", "0", "1");;
		}
		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.ExecuteNonQuery(String.Format("INSERT INTO [LU].[SparePartsBudgetInvoiceType] " +
			                                       "([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [CreateUser], [ModifyDate], [ModifyUser]) " +
			                                       "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', GETUTCDATE(), '{5}', GETUTCDATE(), '{6}')",
				value, name, language, favorite, sortOrder, "Setup", "Setup"));
		}
	}
}