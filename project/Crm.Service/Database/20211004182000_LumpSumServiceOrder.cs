namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211004182000)]
	public class LumpSumServiceOrder : Migration
	{
		public override void Up()
		{
			Database.AddColumn("SMS.ServiceOrderHead", new Column("IsCostLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
			Database.AddColumn("SMS.ServiceOrderHead", new Column("IsMaterialLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
			Database.AddColumn("SMS.ServiceOrderHead", new Column("IsTimeLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
		}
	}
}
