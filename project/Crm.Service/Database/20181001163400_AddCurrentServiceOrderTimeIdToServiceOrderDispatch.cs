namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20181001163400)]
	public class AddCurrentServiceOrderTimeIdToServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderDispatch", "CurrentServiceOrderTimeId"))
			{
				Database.AddColumn("SMS.ServiceOrderDispatch", new Column("CurrentServiceOrderTimeId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceOrderDispatch_ServiceOrderTime", "SMS.ServiceOrderDispatch", "CurrentServiceOrderTimeId", "SMS.ServiceOrderTimes", "Id");
			}
		}
	}
}
