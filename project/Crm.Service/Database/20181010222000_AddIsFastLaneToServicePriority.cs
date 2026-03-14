namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20181010222000)]
	public class AddIsFastLaneToServicePriority : Migration
	{
		public override void Up()
		{
			Database.AddColumn("SMS.ServicePriority", new Column("IsFastLane", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
