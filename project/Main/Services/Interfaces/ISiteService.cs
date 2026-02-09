namespace Main.Services.Interfaces
{
	using System.Collections.Generic;

	using Crm.Library.AutoFac;
	using Crm.Library.Model.Site;

	public interface ISiteService : IDependency
	{
		Site CurrentSite { get; }
		void SaveSite(Site site, List<string> activePlugins = null);
	}
}
