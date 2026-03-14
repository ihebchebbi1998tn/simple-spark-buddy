using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;


namespace Sms.Scheduler.Database
{
	[Migration(20230509120000)]
	public class AddServiceOrderTooltipToProfile : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("RPL.Profile", new Column("ServiceOrderTooltip", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}
