namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130910101011)]
	public class AddPlannedTimeAndFixedPlannedDateToServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("PlannedTime", DbType.DateTime, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("PlannedDateFix", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}