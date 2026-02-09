namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190910101400)]
	public class AddArticleIdToInstallationPos : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.InstallationPos", "ArticleId"))
			{
				Database.AddColumn("SMS.InstallationPos", new Column("ArticleId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_InstallationPos_Article", "[SMS].[InstallationPos]", "ArticleId", "[CRM].[Contact]", "ContactId");
				Database.ExecuteNonQuery("UPDATE [SMS].[InstallationPos] SET [ArticleId] = (SELECT TOP 1 [CRM].[Article].[ArticleId] FROM [CRM].[Article] JOIN [CRM].[Contact] ON [CRM].[Contact].[ContactId] = [CRM].[Article].[ArticleId] WHERE [CRM].[Article].[ItemNo] = [SMS].[InstallationPos].[ItemNo] ORDER BY [CRM].[Contact].[IsActive] DESC)");
			}
		}
	}
}