using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;


namespace Sms.Scheduler.Database
{
	[Migration(20230519100000)]
	public class AddResourceTooltipToProfile : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("RPL.Profile", new Column("ResourceTooltip", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}
