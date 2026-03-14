namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160414163005)]
	public class AddIsExportedToCrmNote : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Note]", new Column("IsExported", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}