namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160414163001)]
	public class AddLegacyVersionToCrmTask : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Task]", new Column("LegacyVersion", DbType.Int64, ColumnProperty.Null));
		}
	}
}