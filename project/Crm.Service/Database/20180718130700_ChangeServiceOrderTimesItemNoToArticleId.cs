namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180718130700)]
	public class ChangeServiceOrderTimesItemNoToArticleId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderTimes", "ArticleId") && Database.ColumnExists("SMS.ServiceOrderTimes", "ItemNo"))
			{
				Database.AddColumn("SMS.ServiceOrderTimes", new Column("ArticleId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderTimes SET SMS.ServiceOrderTimes.ArticleId = CRM.Article.ArticleId FROM SMS.ServiceOrderTimes JOIN CRM.Article ON CRM.Article.ItemNo = SMS.ServiceOrderTimes.ItemNo");
				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderTimes]') AND name = N'IX_ServiceOrderTimes_ItemNo'") == 1)
				{
					Database.ExecuteNonQuery("DROP INDEX [IX_ServiceOrderTimes_ItemNo] ON [SMS].[ServiceOrderTimes]");
				}
				Database.AddForeignKey("FK_ServiceOrderTimes_Article", "SMS.ServiceOrderTimes", "ArticleId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimes_ArticleId] ON [SMS].[ServiceOrderTimes] ([ArticleId] ASC)");
			}
		}
	}
}