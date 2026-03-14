namespace Main.Data
{
	using Crm.Library.Data.Domain;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Helper;

	using NHibernate;

	public class IsActiveFilterEnabler : ISessionFilterEnabler
	{
		private readonly bool isActive;
		public IsActiveFilterEnabler(IAppSettingsProvider appSettingsProvider)
		{
			isActive = appSettingsProvider.GetValue(MainPlugin.Settings.System.SoftDelete);
		}
		public virtual void EnableFilter(ISession session)
		{
			if (!isActive)
			{
				return;
			}
			session.EnableFilter(IsActiveFilter.Name).SetParameter(IsActiveFilter.ParameterName, true);
		}
	}
}
