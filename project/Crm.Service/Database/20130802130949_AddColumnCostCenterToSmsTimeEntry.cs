namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130802130949)]
	public class AddColumnCostCenterToSmsTimeEntry : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[TimeEntry]", new Column("CostCenter", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("CostCenter", DbType.String, 50, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}