using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;


namespace Sms.Scheduler.Database
{
	[Migration(20230512100000)]
	public class AddServiceOrderDispatchTooltipToProfile : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("RPL.Profile", new Column("ServiceOrderDispatchTooltip", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}
