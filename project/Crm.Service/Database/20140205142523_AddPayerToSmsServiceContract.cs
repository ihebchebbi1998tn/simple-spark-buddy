namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140205142523)]
	public class AddPayerToSmsServiceContract : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("PayerId", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("PayerAddressId", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}