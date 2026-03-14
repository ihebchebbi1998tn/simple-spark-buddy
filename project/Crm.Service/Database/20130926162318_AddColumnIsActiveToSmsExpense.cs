namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130926162318)]
	public class AddColumnIsActiveToSmsExpense : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[Expense]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}