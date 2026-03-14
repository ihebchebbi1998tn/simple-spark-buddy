namespace Crm.Service.Team.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230426101600)]
	public class AddTableCrmArticleUserGroupRelationship : Migration
		{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[ArticleUserGroupRelationship]"))
			{
				Database.AddTable(
					"CRM.ArticleUserGroupRelationship",
					new Column("ArticleUserGroupRelationshipId", DbType.Guid, ColumnProperty.PrimaryKey),
					new Column("ArticleKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("UserGroupKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("[From]", DbType.DateTime, ColumnProperty.Null),
					new Column("[To]", DbType.DateTime, ColumnProperty.Null),
					new Column("AuthDataId", DbType.Guid, ColumnProperty.Null),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));

				Database.ExecuteNonQuery("ALTER TABLE CRM.ArticleUserGroupRelationship ADD FOREIGN KEY (ArticleKey) REFERENCES CRM.Article(ArticleId)");
				Database.ExecuteNonQuery("ALTER TABLE CRM.ArticleUserGroupRelationship ADD FOREIGN KEY (UserGroupKey) REFERENCES CRM.[UserGroup](UsergroupId)");
				Database.ExecuteNonQuery("ALTER TABLE CRM.ArticleUserGroupRelationship ADD FOREIGN KEY (AuthDataId) REFERENCES dbo.EntityAuthData(UId)");

			}
		}
	}
}
