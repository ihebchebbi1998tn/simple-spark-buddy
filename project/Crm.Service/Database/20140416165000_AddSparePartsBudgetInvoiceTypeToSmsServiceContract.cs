namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140416165000)]
	public class AddSparePartsBudgetInvoiceTypeToSmsServiceContract : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("SparePartsBudgetInvoiceType", DbType.String, 50, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}