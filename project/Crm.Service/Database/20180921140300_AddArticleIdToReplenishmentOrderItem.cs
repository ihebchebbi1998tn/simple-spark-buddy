namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180921140300)]
	public class AddArticleIdToReplenishmentOrderItem : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ReplenishmentOrderItem", "ArticleId"))
			{
				Database.AddColumn("SMS.ReplenishmentOrderItem", new Column("ArticleId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ReplenishmentOrderItem SET SMS.ReplenishmentOrderItem.ArticleId = CRM.Article.ArticleId FROM SMS.ReplenishmentOrderItem JOIN CRM.Article ON CRM.Article.ItemNo = SMS.ReplenishmentOrderItem.ArticleNo");
				Database.AddForeignKey("FK_ReplenishmentOrderItem_Article", "SMS.ReplenishmentOrderItem", "ArticleId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ReplenishmentOrderItem_ArticleId] ON [SMS].[ReplenishmentOrderItem] ([ArticleId] ASC)");
			}
		}
	}
}
