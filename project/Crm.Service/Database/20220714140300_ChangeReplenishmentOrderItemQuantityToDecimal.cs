namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220714140300)]
	public class ChangeReplenishmentOrderItemQuantityToDecimal : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[SMS].[ReplenishmentOrderItem]", "Quantity"))
			{
				Database.ChangeColumn("[SMS].[ReplenishmentOrderItem]", new Column("Quantity", DbType.Decimal, ColumnProperty.NotNull));
			}
		}
	}
}
