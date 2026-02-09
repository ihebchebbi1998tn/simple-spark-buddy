namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240208103400)]
	public class AddServiceOrderAssetTable : Migration {
		public override void Up() {
			if (!Database.TableExists("[SMS].[ServiceOrderAsset]"))
			{
				Database.AddTable("[SMS].[ServiceOrderAsset]",
					new Column("ServiceOrderId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AssetKey", DbType.String, 20, ColumnProperty.NotNull));
				
				Database.AddForeignKey("FK_ServiceOrderAsset_ServiceOrder", "[SMS].[ServiceOrderAsset]", "ServiceOrderId", "[SMS].[ServiceOrderHead]", "ContactKey", ForeignKeyConstraint.Cascade);

			}
		}
	}
}
