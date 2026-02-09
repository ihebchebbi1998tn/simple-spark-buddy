namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20210308132200)]
	public class AddDefaultLocaleToLanguage : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[LU].[Language]", new Column("DefaultLocale", DbType.String, ColumnProperty.Null, false));
		}
	}
}
