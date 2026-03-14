namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240208103600)]
	public class AddServiceCaseTemplateAssetTable : Migration {
		public override void Up() {
			if (!Database.TableExists("[SMS].[ServiceCaseTemplateAsset]"))
			{
				Database.AddTable("[SMS].[ServiceCaseTemplateAsset]",
					new Column("ServiceCaseTemplateId", DbType.Guid, ColumnProperty.NotNull),
					new Column("AssetKey", DbType.String, 20, ColumnProperty.NotNull));
				
				Database.AddForeignKey("FK_ServiceCaseTemplateAsset_ServiceCaseTemplate", "[SMS].[ServiceCaseTemplateAsset]", "ServiceCaseTemplateId", "[SMS].[ServiceCaseTemplate]", "ServiceCaseTemplateId", ForeignKeyConstraint.Cascade);

			}
		}
	}
}
