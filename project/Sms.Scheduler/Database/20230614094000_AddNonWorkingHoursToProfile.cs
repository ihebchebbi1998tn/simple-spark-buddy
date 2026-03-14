using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;


namespace Sms.Scheduler.Database
{
	[Migration(20230614094000)]
	public class AddNonWorkingHoursToProfile : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("RPL.Profile", new Column("NonWorkingHours", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}
