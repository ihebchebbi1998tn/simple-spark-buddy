namespace Crm.Service.Team.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230511113001)]

	public class AddTeamIdToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("TeamId", DbType.Guid, ColumnProperty.Null));
		}
	}
}
