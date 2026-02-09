namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	using System.Data;
	[Migration(20240226152000)]
	public class _AddSupplierToSmsInstallationPos : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.InstallationPos", new Column("Supplier", DbType.String, 256, ColumnProperty.Null));
		}
	}
}
