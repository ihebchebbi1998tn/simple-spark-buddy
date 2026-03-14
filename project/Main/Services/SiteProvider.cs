namespace Main.Services
{
	using System;

	using Crm.Library.AutoFac;
	using Crm.Library.Model.Site;

	public class SiteHolder : ISiteHolder, ISingletonDependency
	{
		public virtual Site Site { get; protected set; }
		
		public virtual void UpdateSite(Site newSite)
		{
			if (newSite == null)
			{
				throw new ArgumentNullException(nameof(newSite));
			}
			if (Equals(newSite, Site))
			{
				return;
			}

			Site = newSite;
		}
	}

	public interface ISiteHolder
	{
		Site Site { get; }
		void UpdateSite(Site newSite);
	}
}
