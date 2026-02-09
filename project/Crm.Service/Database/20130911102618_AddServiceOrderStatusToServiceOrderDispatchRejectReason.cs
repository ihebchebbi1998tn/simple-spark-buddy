namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130911102618)]
	public class AddServiceOrderStatusToServiceOrderDispatchRejectReason : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatchRejectReason", new Column("ServiceOrderStatus", DbType.String, 50, ColumnProperty.Null));
		}
	}
}