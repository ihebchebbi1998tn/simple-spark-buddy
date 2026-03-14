namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20241029084600)]
	public class DefaultInvoicingType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderType", new Column("DefaultInvoicingTypeKey", DbType.String, 20, ColumnProperty.Null));
		}
	}
}
