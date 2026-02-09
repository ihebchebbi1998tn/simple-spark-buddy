namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160125142149)]
	public class AddColumnNameToStore : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[Store]", new Column("Name", DbType.String, 200, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[Store]", new Column("LegacyId", DbType.String, 200, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[Store]", new Column("LegacyVersion", DbType.Int64, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}