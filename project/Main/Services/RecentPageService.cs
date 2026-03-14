namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;

	using log4net;

	public class RecentPageService : IRecentPageService
	{
		private readonly IRepositoryWithTypedId<RecentPage, Guid> recentPageRepository;
		private readonly Func<RecentPage> recentPageFactory;

		public RecentPageService(IRepositoryWithTypedId<RecentPage, Guid> recentPageRepository, Func<RecentPage> recentPageFactory)
		{
			this.recentPageRepository = recentPageRepository;
			this.recentPageFactory = recentPageFactory;
		}
		public virtual void AddRecentPage(string username, string title, string url)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(url))
			{
				return;
			}
			var recentPage = recentPageRepository.GetAll().Where(x => x.Url.ToLower() == url.ToLower() && x.Username == username).FirstOrDefault();
			if (recentPage != null)
			{
				recentPage.Count += 1;
			}
			else
			{
				recentPage = recentPageFactory();
				recentPage.Title = title;
				recentPage.Url = url;
				recentPage.Username = username;
				recentPage.Count = 1;
				recentPage.Id = Guid.NewGuid();
			}
			recentPageRepository.SaveOrUpdate(recentPage);
		}
		public virtual void RemoveRecentPages(List<string> urls)
		{
			var recentPages = recentPageRepository.GetAll().Where(x => urls.Select(y => y.ToLower()).Contains(x.Url.ToLower()));
			foreach (RecentPage recentPage in recentPages)
			{
				recentPageRepository.Delete(recentPage);
			}
		}
		public virtual void RemoveRecentPages(List<string> urls, string username)
		{
			var recentPages = recentPageRepository.GetAll().Where(x => x.Username == username && urls.Select(y => y.ToLower()).ToArray().Contains(x.Url.ToLower()));
			foreach (RecentPage recentPage in recentPages)
			{
				recentPageRepository.Delete(recentPage);
			}
		}
	}
}
