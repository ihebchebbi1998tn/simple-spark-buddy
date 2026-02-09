namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130905150706)]
	public class AddColumnValidCostCentersToSmsTimeEntryType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.TimeEntryType", new Column("ValidCostCenters", DbType.String, Int32.MaxValue, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}