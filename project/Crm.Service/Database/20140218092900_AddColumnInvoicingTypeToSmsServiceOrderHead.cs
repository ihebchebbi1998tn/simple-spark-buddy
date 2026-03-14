namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140218092900)]
	public class AddColumnInvoicingTypeToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("InvoicingType", DbType.String, 20, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}