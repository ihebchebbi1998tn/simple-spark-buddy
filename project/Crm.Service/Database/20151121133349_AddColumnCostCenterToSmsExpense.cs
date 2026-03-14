namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20151121133349)]
	public class AddColumnCostCenterToSmsExpense : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[Expense]", new Column("CostCenter", DbType.String, 50, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}