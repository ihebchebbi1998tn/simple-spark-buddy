namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180718130900)]
	public class ChangeServiceOrderTimePostingsItemNoToArticleId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderTimePostings", "ArticleId") && Database.ColumnExists("SMS.ServiceOrderTimePostings", "ItemNo"))
			{
				Database.AddColumn("SMS.ServiceOrderTimePostings", new Column("ArticleId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderTimePostings SET SMS.ServiceOrderTimePostings.ArticleId = CRM.Article.ArticleId FROM SMS.ServiceOrderTimePostings JOIN CRM.Article ON CRM.Article.ItemNo = SMS.ServiceOrderTimePostings.ItemNo");
				Database.AddForeignKey("FK_ServiceOrderTimePostings_Article", "SMS.ServiceOrderTimePostings", "ArticleId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimePostings_ArticleId] ON [SMS].[ServiceOrderTimePostings] ([ArticleId] ASC)");
			}
		}
	}
}