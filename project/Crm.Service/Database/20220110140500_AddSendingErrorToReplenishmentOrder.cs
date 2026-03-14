namespace Crm.Service.Database
{
	using System.Data;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220110140500)]
	public class AddSendingErrorToReplenishmentOrder : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ReplenishmentOrder", new Column("SendingError", DbType.String, int.MaxValue, ColumnProperty.Null));
		}
	}
}