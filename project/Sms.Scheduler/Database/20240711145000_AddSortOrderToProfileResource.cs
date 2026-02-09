namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240711145000)]
	public class AddSortOrderToProfileResource : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[RPL].[ProfileResource]", new Column("SortOrder", DbType.Int32, ColumnProperty.Null));
		}
	}
}
