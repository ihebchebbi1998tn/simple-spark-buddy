namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161121125900)]
	public class AddClosedByToReplenishmentOrder : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[SMS].[ReplenishmentOrder]", "ClosedBy"))
			{
				Database.AddColumn("[SMS].[ReplenishmentOrder]", "ClosedBy", DbType.String, 255, ColumnProperty.Null);
			}
		}
	}
}
