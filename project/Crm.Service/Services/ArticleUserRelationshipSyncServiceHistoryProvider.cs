namespace Crm.Service.Services
{
	using Crm.Article.Services.Interfaces;
	using Crm.Library.Helper;

	public class ArticleUserRelationshipSyncServiceHistoryProvider : IArticleUserRelationshipSyncServiceHistoryProvider
	{
		private readonly IAppSettingsProvider appSettingsProvider;
		public ArticleUserRelationshipSyncServiceHistoryProvider(IAppSettingsProvider appSettingsProvider)
		{
			this.appSettingsProvider = appSettingsProvider;
		}

		public virtual int GetHistorySince()
		{
			return appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceOrderTimePosting.MaxDaysAgo);
		}
	}
}
