namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131218101516)]
	public class AddColumnDiagnosisToSmsServiceOrderTimes : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimes]", new Column("Diagnosis", DbType.String, 4000, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}