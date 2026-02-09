namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131106092012)]
	public class AddColumnInvoicedUntilToSmsServiceContract : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("InvoicedUntil", DbType.DateTime, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}