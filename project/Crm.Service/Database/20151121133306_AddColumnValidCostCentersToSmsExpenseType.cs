namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20151121133306)]
	public class AddColumnValidCostCentersToSmsExpenseType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ExpenseType", new Column("ValidCostCenters", DbType.String, Int32.MaxValue, ColumnProperty.Null));
		}
		public override void Down()
		{
		}
	}
}