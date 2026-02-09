namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	using System.Data;

	[Migration(20230913122800)]
	public class AddUserGroupKeyToServiceCaseTemplate : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceCaseTemplate", new Column("UserGroupKey", DbType.Guid, ColumnProperty.Null));
		}
	}
}
