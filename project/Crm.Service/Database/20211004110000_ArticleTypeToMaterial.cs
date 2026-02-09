namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211004110000)]
	public class ArticleTypeToMaterial : Migration
	{
		public override void Up()
		{
			Database.AddColumn("SMS.ServiceOrderMaterial", new Column("ArticleTypeKey")
			{
				Type = System.Data.DbType.String,
				Size = 20,
				ColumnProperty = ColumnProperty.Null
			});
			Database.ExecuteNonQuery(@"
				UPDATE material
				SET material.ArticleTypeKey = article.ArticleType
				FROM SMS.ServiceOrderMaterial material
				JOIN CRM.Contact contact ON material.ArticleId = contact.ContactId
				JOIN CRM.Article article ON contact.ContactId = article.ArticleId
				WHERE material.IsActive = 1 AND contact.IsActive = 1");
		}
	}
}
