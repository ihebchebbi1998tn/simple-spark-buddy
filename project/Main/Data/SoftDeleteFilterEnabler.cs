namespace Main.Data
{
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Helper;

	using LMobile.Unicore.NHibernate;

	using NHibernate;

	public class SoftDeleteFilterEnabler : ISessionFilterEnabler
	{
		private readonly bool isActive;
		public SoftDeleteFilterEnabler(IAppSettingsProvider appSettingsProvider)
		{
			isActive = appSettingsProvider.GetValue(MainPlugin.Settings.System.SoftDelete);
		}
		public virtual void EnableFilter(ISession session)
		{
			if (!isActive)
			{
				return;
			}
			session.EnableFilter(SoftDeleteFilter.Name).SetParameter(SoftDeleteFilter.ParameterName, false);
		}
	}
}
