namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131031163215)]
	public class AddColumnDiagnosisToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("Diagnosis", DbType.String, 4000, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}