namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240208103500)]
	public class AddServiceNotificationsAssetTable : Migration {
		public override void Up() {
			if (!Database.TableExists("[SMS].[ServiceNotificationsAsset]"))
			{
				Database.AddTable("[SMS].[ServiceNotificationsAsset]",
					new Column("ContactKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("AssetKey", DbType.String, 20, ColumnProperty.NotNull));
				
				Database.AddForeignKey("FK_ServiceNotificationsAsset_ServiceNotifications", "[SMS].[ServiceNotificationsAsset]", "ContactKey", "[SMS].[ServiceNotifications]", "ContactKey", ForeignKeyConstraint.Cascade);

			}
		}
	}
}
