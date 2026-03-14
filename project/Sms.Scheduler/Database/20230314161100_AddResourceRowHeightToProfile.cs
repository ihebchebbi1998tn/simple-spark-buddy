using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Sms.Scheduler.Database
{
	[Migration(20230314161100)]
	public class AddResourceRowHeightToProfile : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("RPL.Profile", new Column("ResourceRowHeight", DbType.Int16, ColumnProperty.NotNull, 70));
		}
	}
}
