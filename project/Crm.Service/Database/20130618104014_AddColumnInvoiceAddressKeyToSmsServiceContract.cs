namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130618104014)]
	public class AddColumnInvoiceAddressKeyToSmsServiceContract: Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceContract", new Column("InvoiceAddressKey", DbType.Int32, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}