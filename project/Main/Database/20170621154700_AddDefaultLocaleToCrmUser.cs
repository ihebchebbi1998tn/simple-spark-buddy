namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20170621154700)]
	public class AddDefaultLocaleToCrmUser : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[User]", new Column("DefaultLocale", DbType.String, 20, ColumnProperty.Null));
		}
	}
}