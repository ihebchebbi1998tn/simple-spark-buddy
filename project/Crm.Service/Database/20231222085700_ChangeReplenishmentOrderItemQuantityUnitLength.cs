namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20231222085700)]
	public class ChangeReplenishmentOrderItemQuantityUnitLength : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[SMS].[ReplenishmentOrderItem]",
				new Column("QuantityUnitKey",
					DbType.String,
					20,
					ColumnProperty.Null));
		}
	}
}
