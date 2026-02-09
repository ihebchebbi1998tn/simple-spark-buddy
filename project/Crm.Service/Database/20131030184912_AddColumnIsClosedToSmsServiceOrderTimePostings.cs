namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131030184912)]
	public class AddColumnIsClosedToSmsServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("IsClosed", DbType.Boolean, ColumnProperty.NotNull, false));
		}

		public override void Down()
		{
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderTimePostings]", "IsClosed");
		}
	}
}