namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160414163000)]
	public class AddLegacyIdToCrmTask : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Task]", new Column("LegacyId", DbType.String, 50, ColumnProperty.Null));
		}
	}
}