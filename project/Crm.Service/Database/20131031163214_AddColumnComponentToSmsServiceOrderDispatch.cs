namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131031163214)]
	public class AddColumnComponentToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("Component", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}