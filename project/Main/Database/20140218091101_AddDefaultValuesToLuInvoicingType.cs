namespace Main.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140218091101)]
	public class AddDefaultValuesToLuInvoicingType : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[InvoicingType]") && Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[InvoicingType]")) == 0)
			{
				InsertLookupValue("LumpSum", "Lump sum", "en", "0", "0");
				InsertLookupValue("LumpSum", "Pauschal", "de", "0", "0");
				InsertLookupValue("TMbasis", "T&M basis", "en", "0", "1");
				InsertLookupValue("TMbasis", "nach Aufwand", "de", "0", "1");
				InsertLookupValue("ByStatus", "By status", "en", "0", "2");
				InsertLookupValue("ByStatus", "Festlegung nach Sachstand", "de", "0", "2");
				InsertLookupValue("Goodwill", "Goodwill", "en", "0", "2");
				InsertLookupValue("Goodwill", "Kulanz", "de", "0", "2");
			}
		}
		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.Insert("[LU].[InvoicingType]",
				new[] { "Value", "Name", "Language", "Favorite", "SortOrder" },
				new[] { value, name, language, favorite, sortOrder });
		}
	}
}