namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240415091400)]
	public class AddColumnIconClassToMenuEntry : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("Crm.MenuEntry",
				new Column("IconClass",
					DbType.String,
					ColumnProperty.Null));
		}
	}
}
