namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131218095740)]
	public class AddTableLuNoCausingItemPreviousSerialNoReason : Migration
	{
		public override void Up()
		{
			Database.AddLookupTable("NoCausingItemPreviousSerialNoReason");
			Database.InsertLookupValue("NoCausingItemPreviousSerialNoReason", "NotAvailable", "Not available", "en");
			Database.InsertLookupValue("NoCausingItemPreviousSerialNoReason", "NotAvailable", "Nicht vorhanden", "de");
			Database.InsertLookupValue("NoCausingItemPreviousSerialNoReason", "Nonreadable", "Nonreadable", "en");
			Database.InsertLookupValue("NoCausingItemPreviousSerialNoReason", "Nonreadable", "Nicht lesbar", "de");
		}
	}
}