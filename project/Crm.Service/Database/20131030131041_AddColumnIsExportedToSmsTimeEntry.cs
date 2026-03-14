namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131030131041)]
	public class AddColumnIsExportedToSmsTimeEntry : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[TimeEntry]", new Column("IsExported", DbType.Boolean, ColumnProperty.NotNull, false));
		}

		public override void Down()
		{
			Database.RemoveColumnIfExisting("[SMS].[TimeEntry]", "IsExported");
		}
	}
}