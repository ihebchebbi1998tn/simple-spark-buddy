namespace Main.Data.NHibernateProvider
{
	using Crm.Library.AutoFac;

	public interface INHibernateAdditionalMappingProvider : ISingletonDependency
	{
		string AddMappings(string xml);
	}
}
