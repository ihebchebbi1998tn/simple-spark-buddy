namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20210218152300)]
	public class AddCompleteDateAndCompleteUserToServiceOrderTime : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimes", new Column("CompleteDate", DbType.DateTime, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimes", new Column("CompleteUser", DbType.String, ColumnProperty.Null));
		}
	}
}