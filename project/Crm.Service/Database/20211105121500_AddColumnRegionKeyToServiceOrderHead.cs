namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20211105121500)]
	public class AddColumnRegionKeyToServiceOrderHead : Migration
	{
		public override void Up()
		{
			const string table = "[SMS].[ServiceOrderHead]";
			if (Database.TableExists(table))
			{
				Database.AddColumnIfNotExisting(table, new Column("RegionKey", DbType.String, 20, ColumnProperty.Null));
			}
		}
	}
}
