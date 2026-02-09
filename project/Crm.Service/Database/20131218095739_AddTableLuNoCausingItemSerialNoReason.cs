namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131218095739)]
	public class AddTableLuNoCausingItemSerialNoReason : Migration
	{
		public override void Up()
		{
			Database.AddLookupTable("NoCausingItemSerialNoReason");
			Database.InsertLookupValue("NoCausingItemSerialNoReason", "NotAvailable", "Not available", "en");
			Database.InsertLookupValue("NoCausingItemSerialNoReason", "NotAvailable", "Nicht vorhanden", "de");
			Database.InsertLookupValue("NoCausingItemSerialNoReason", "NotReplaced", "Not replaced", "en");
			Database.InsertLookupValue("NoCausingItemSerialNoReason", "NotReplaced", "nicht getauscht", "de");
		}
	}
}