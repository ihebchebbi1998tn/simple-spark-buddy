using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Crm.Service.Database
{
	[Migration(20230904174000)]
	public class RemoveServiceOrderDispatchTime : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("SMS.ServiceOrderDispatch", "Time"))
			{
				Database.ExecuteNonQuery("update SMS.ServiceOrderDispatch set [Date] = [Time]");
				Database.RemoveColumn("SMS.ServiceOrderDispatch", "Time");
			}
		}
	}
}
