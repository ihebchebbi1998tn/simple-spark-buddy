namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191002154700)]
	public class AddServiceOrderTimeIdToServiceCase : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceNotifications", "ServiceOrderTimeId"))
			{
				Database.AddColumn("SMS.ServiceNotifications", new Column("ServiceOrderTimeId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceNotifications_ServiceOrderTime", "SMS.ServiceNotifications", "ServiceOrderTimeId", "SMS.ServiceOrderTimes", "Id");
			}
		}
	}
}