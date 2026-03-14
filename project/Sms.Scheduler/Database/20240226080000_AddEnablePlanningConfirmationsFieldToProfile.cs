using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Sms.Scheduler.Database
{
	[Migration(20240226080000)]
	public class AddEnablePlanningConfirmationsFieldToProfile : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("RPL.Profile", new Column("EnablePlanningConfirmations", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}
