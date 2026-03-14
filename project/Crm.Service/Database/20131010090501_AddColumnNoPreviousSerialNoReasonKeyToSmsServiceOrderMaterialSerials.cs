namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131010090501)]
	public class AddColumnNoPreviousSerialNoReasonKeyToSmsServiceOrderMaterialSerials : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderMaterialSerials]", new Column("NoPreviousSerialNoReason", DbType.String, 20, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}