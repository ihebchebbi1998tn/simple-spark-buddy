namespace Crm.Service.Team.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Team.Model;

	[Migration(20231010140400)]
	public class AddArticleUserGroupRelationshipEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<ArticleUserGroupRelationship>("CRM", "ArticleUserGroupRelationship");
		}
	}
}
