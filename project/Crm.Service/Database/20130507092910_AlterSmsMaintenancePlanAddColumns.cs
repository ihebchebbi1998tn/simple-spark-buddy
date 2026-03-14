namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130507092910)]
	public class AlterSmsMaintenancePlanAddColumns : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[MaintenancePlan]", new Column("GenerateMaintenanceOrders", DbType.Boolean, ColumnProperty.NotNull, true));
			Database.AddColumnIfNotExisting("[SMS].[MaintenancePlan]", new Column("AllowPrematureMaintenance", DbType.Boolean, ColumnProperty.NotNull, false));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}