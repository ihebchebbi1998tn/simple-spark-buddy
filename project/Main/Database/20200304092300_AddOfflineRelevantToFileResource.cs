namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20200304092300)]
	public class AddOfflineRelevantToFileResource : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.FileResource", new Column("OfflineRelevant", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
