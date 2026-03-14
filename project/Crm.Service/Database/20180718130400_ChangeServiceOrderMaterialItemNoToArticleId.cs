namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180718130400)]
	public class ChangeServiceOrderMaterialItemNoToArticleId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderMaterial", "ArticleId") && Database.ColumnExists("SMS.ServiceOrderMaterial", "ItemNo"))
			{
				Database.AddColumn("SMS.ServiceOrderMaterial", new Column("ArticleId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderMaterial SET SMS.ServiceOrderMaterial.ArticleId = CRM.Article.ArticleId FROM SMS.ServiceOrderMaterial JOIN CRM.Article ON CRM.Article.ItemNo = SMS.ServiceOrderMaterial.ItemNo");
				Database.AddForeignKey("FK_ServiceOrderMaterial_Article", "SMS.ServiceOrderMaterial", "ArticleId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderMaterial_ArticleId] ON [SMS].[ServiceOrderMaterial] ([ArticleId] ASC)");
			}
		}
	}
}