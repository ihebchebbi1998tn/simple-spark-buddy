namespace Main.Data.NHibernateProvider
{
	using Crm.Library.AutoFac;

	using NHibernate.Cfg;

	public interface INHibernateInitializer : ISingletonDependency
	{
		Configuration Configuration { get; }
	}
}
