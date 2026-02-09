namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160414163004)]
	public class AddLegacyVersionToCrmNote : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Note]", new Column("LegacyVersion", DbType.Int64, ColumnProperty.Null));
		}
	}
}