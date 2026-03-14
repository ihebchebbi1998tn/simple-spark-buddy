namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131106135725)]
	public class AddColumnLegacyIdToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("LegacyId", DbType.String, 20, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}