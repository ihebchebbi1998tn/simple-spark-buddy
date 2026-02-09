namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131031190605)]
	public class AddColumnCauseOfFailureToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("CauseOfFailure", DbType.String, 20, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}