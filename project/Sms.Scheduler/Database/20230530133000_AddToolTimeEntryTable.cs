using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;

namespace Sms.Scheduler.Database
{
	[Migration(20230530133000)]
	public class AddToolTimeEntryTable : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ToolTimeEntry]"))
			{
				Database.AddTable("[SMS].[ToolTimeEntry]",
					new Column("TimeEntryId", DbType.Guid, ColumnProperty.PrimaryKey),
					new Column("Description", DbType.String, ColumnProperty.None),
					new Column("Date", DbType.DateTime, ColumnProperty.None),
					new Column("DurationInMinutes", DbType.Int32, ColumnProperty.None),
					new Column("[From]", DbType.DateTime, ColumnProperty.None),
					new Column("[To]", DbType.DateTime, ColumnProperty.None),
					new Column("CostCenterKey", DbType.String, ColumnProperty.None),
					new Column("TimeEntryType", DbType.String, ColumnProperty.None),
					new Column("ResourceKey", DbType.String, ColumnProperty.None)
				);
			}
		}
	}
}
