namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160414163002)]
	public class AddIsExportedToCrmTask : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Task]", new Column("IsExported", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}