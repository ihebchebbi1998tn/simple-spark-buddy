namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191014133700)]
	public class AddOriginatingServiceOrderTimeIdToServiceCase : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceNotifications", "OriginatingServiceOrderTimeId"))
			{
				Database.AddColumn("SMS.ServiceNotifications", new Column("OriginatingServiceOrderTimeId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceNotifications_OriginatingServiceOrderTime", "SMS.ServiceNotifications", "OriginatingServiceOrderTimeId", "SMS.ServiceOrderTimes", "id");
			}
		}
	}
}