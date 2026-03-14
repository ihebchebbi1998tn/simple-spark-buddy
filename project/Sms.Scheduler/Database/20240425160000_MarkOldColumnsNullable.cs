using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;

namespace Sms.Scheduler.Database
{
	[Migration(20240425160000)]
	public class MarkOldColumnsNullable : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[RPL].[Profile]", "PipelineRowHeight"))
			{
				Database.ChangeColumn("[RPL].[Profile]", new Column("PipelineRowHeight", DbType.Int16, ColumnProperty.Null));
			}
			if (Database.ColumnExists("[RPL].[Profile]", "ResourceRowHeight"))
			{
				Database.ChangeColumn("[RPL].[Profile]", new Column("ResourceRowHeight", DbType.Int16, ColumnProperty.Null));
			}
			if (Database.ColumnExists("[RPL].[Profile]", "EnablePlanningConfirmations"))
			{
				Database.ChangeColumn("[RPL].[Profile]", new Column("EnablePlanningConfirmations", DbType.Boolean, ColumnProperty.Null));
			}
			if (Database.ColumnExists("[RPL].[Profile]", "LowerBound"))
			{
				Database.ChangeColumn("[RPL].[Profile]", new Column("LowerBound", DbType.Decimal, ColumnProperty.Null));
			}
			if (Database.ColumnExists("[RPL].[Profile]", "UpperBound"))
			{
				Database.ChangeColumn("[RPL].[Profile]", new Column("UpperBound", DbType.Decimal, ColumnProperty.Null));
			}
		}
	}
}
