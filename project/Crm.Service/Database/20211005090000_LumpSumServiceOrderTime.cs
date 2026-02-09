namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211005090000)]
	public class LumpSumServiceOrderTime : Migration
	{
		public override void Up()
		{
			Database.AddColumn("SMS.ServiceOrderTimes", new Column("IsCostLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
			Database.AddColumn("SMS.ServiceOrderTimes", new Column("IsMaterialLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
			Database.AddColumn("SMS.ServiceOrderTimes", new Column("IsTimeLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
			Database.AddColumn("SMS.ServiceOrderTimes", new Column("InvoicingTypeKey")
			{
				Type = System.Data.DbType.String,
				Size = 20
			});
		}
	}
}
