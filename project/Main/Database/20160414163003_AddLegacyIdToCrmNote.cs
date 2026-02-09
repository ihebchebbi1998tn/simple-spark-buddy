namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160414163003)]
	public class AddLegacyIdToCrmNote : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Note]", new Column("LegacyId", DbType.String, 50, ColumnProperty.Null));
		}
	}
}