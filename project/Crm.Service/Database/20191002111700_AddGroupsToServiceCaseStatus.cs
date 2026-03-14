namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20191002111700)]
	public class AddGroupsToServiceCaseStatus : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceNotificationStatus", new Column("Groups", DbType.String, ColumnProperty.Null));
		}
	}
}