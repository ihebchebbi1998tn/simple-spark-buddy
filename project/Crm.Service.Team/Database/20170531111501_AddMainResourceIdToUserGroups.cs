namespace Crm.Service.Team.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20170531111501)]
	public class AddMainResourceIdToUserGroups : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Usergroup]", new Column("MainResourceId", DbType.String, 256, ColumnProperty.Null));
		}
	}
}
