namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20181205120400)]
	public class AddDescriptionToRole : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("dbo.PermissionSchemaRole", new Column("Description", DbType.String, ColumnProperty.Null));
		}
	}
}
