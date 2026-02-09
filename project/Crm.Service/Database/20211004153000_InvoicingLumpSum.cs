namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211004153000)]
	public class InvoicingLumpSum : Migration
	{
		public override void Up()
		{
			Database.AddColumn("LU.InvoicingType", new Column("IsCostLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
			Database.AddColumn("LU.InvoicingType", new Column("IsMaterialLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
			Database.AddColumn("LU.InvoicingType", new Column("IsTimeLumpSum")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});
		}
	}
}
