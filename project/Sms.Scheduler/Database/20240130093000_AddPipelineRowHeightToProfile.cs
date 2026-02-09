using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Sms.Scheduler.Database
{
	[Migration(20240130093000)]
	public class AddPipelineRowHeightToProfile : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("RPL.Profile", new Column("PipelineRowHeight", DbType.Int16, ColumnProperty.NotNull, 30));
		}
	}
}
