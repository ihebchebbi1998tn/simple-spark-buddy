namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20241021115300)]
	public class AddSchedulingGroupIdToServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]",
				new Column("SchedulingGroupId",
					DbType.Guid,
					ColumnProperty.Null));
		}
	}
}
