namespace Main.BackgroundServices
{
	using System.Collections.Specialized;

	using Autofac;
	using Autofac.Core.Lifetime;
	using Autofac.Extras.Quartz;

	using Crm.Library.BackgroundServices.QuartzRedisJobStore;
	using Crm.Library.Helper;
	using Crm.Library.Model.Site;

	using Quartz.Impl;

	public class DefaultQuartzAutofacFactoryModule : QuartzAutofacFactoryModule
	{
		public DefaultQuartzAutofacFactoryModule()
			: base((string)MatchingScopeLifetimeTags.RequestLifetimeScopeTag)
		{
			ConfigurationProvider = ctx =>
			{
				var instanceName = ctx.Resolve<Site>().Name;
				var appSettingsProvider = ctx.Resolve<IAppSettingsProvider>();
				var configuration = new NameValueCollection
				{
					[StdSchedulerFactory.PropertySchedulerInstanceId] = $"{StdSchedulerFactory.DefaultInstanceId}_{instanceName}",
					[StdSchedulerFactory.PropertySchedulerInstanceName] = $"QuartzScheduler_{instanceName}",
					[StdSchedulerFactory.PropertySchedulerThreadName] = $"QuartzSchedulerThread_{instanceName}"
				};
				var redisConfiguration = appSettingsProvider.GetValue(MainPlugin.Settings.System.RedisConfiguration);
				if (!string.IsNullOrWhiteSpace(redisConfiguration))
				{
					configuration[StdSchedulerFactory.PropertyJobStoreType] = typeof(RedisJobStore).AssemblyQualifiedName;
					configuration["quartz.serializer.type"] = "json";
					configuration["quartz.jobStore.useProperties"] = "true";
					configuration["quartz.jobStore.redisConfiguration"] = redisConfiguration;
					configuration["quartz.jobStore.keyPrefix"] = $"Quartz_{instanceName}_";
				}

				return configuration;
			};
		}
	}
}
