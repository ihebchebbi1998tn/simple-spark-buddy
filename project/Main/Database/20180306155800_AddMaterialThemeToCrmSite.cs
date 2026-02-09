namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180306155800)]
	public class AddMaterialThemeToCrmSite : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Site]", new Column("MaterialTheme", DbType.String, 30, ColumnProperty.NotNull, "'bluegray'"));
		}
	}
}
