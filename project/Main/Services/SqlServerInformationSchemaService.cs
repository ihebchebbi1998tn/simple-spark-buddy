namespace Main.Services
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Model;

	using Main.Services.Interfaces;

	using NHibernate;

	public class SqlServerInformationSchemaCache : IInformationSchemaCache
	{
		private readonly ISessionFactory sessionFactory;
		private bool initialized;
		private readonly object initLock = new object();
		private List<InformationSchema> cache;
		public SqlServerInformationSchemaCache(ISessionFactory sessionFactory)
		{
			this.sessionFactory = sessionFactory;
		}
		public virtual IEnumerable<InformationSchema> GetAll()
		{
			if (initialized)
			{
				return cache;
			}
			lock (initLock)
			{
				if (initialized)
				{
					return cache;
				}
				using (var session = sessionFactory.OpenStatelessSession())
				{
					cache = session.Query<InformationSchema>().ToList();
				}
				initialized = true;
			}
			return cache;
		}
	}
}
