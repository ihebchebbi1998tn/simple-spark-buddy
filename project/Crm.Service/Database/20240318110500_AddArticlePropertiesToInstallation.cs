namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240318110500)]
	public class AddArticlePropertiesToInstallation : Migration
	{
		public override void Up()
		{
			if(!Database.ColumnExists("[SMS].[InstallationHead]", "ArticleKey")) {
				Database.AddColumn("[SMS].[InstallationHead]",
					new Column("ArticleKey",
						DbType.Guid,
						ColumnProperty.Null));
			}
			if(!Database.ColumnExists("[SMS].[InstallationHead]", "CustomItemNo")) {
				Database.AddColumn("[SMS].[InstallationHead]",
					new Column("CustomItemNo",
						DbType.String,
						ColumnProperty.Null));
			}
			if(!Database.ColumnExists("[SMS].[InstallationHead]", "CustomItemDesc")) {
				Database.AddColumn("[SMS].[InstallationHead]",
					new Column("CustomItemDesc",
						DbType.String,
						ColumnProperty.Null));
			}
		}
	}
}
