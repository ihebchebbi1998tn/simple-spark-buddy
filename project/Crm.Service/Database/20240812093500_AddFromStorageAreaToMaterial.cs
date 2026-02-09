namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240812093500)]
	public class AddFromStorageAreaToMaterial : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial",
				new Column("FromStorageArea",
					DbType.String,
					ColumnProperty.Null));
		}
	}
}
