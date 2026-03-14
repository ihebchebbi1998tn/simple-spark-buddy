namespace Main
{
	using System;
	using System.IO;
	using System.Reflection;

	using Autofac;

	using AutoMapper;
	using AutoMapper.Configuration;

	using CefToPdf;

	using Crm.Library.ActionFilterRegistry;
	using Crm.Library.AutoMapper;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;

	using log4net;

	using Main.Data.NHibernateProvider;
	using Main.Services;

	using Medallion.Threading;
	using Medallion.Threading.Redis;
	using Medallion.Threading.SqlServer;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Infrastructure;
	using Microsoft.Extensions.Caching.Distributed;
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.Caching.StackExchangeRedis;
	using Microsoft.Extensions.Options;

	using NHibernate;
	using NHibernate.Cfg;

	using ISession = NHibernate.ISession;
	using Module = Autofac.Module;

	public class DefaultModule : Module
	{
		private class ExtendedMemoryDistributedCacheOptions : MemoryDistributedCacheOptions, IOptions<MemoryDistributedCacheOptions>
		{
			public MemoryDistributedCacheOptions Value => this;
		}
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register<IDistributedLockProvider>(c =>
			{
				var appSettingsProvider = c.Resolve<IAppSettingsProvider>();
				var redisConfiguration = appSettingsProvider.GetValue(MainPlugin.Settings.System.RedisConfiguration);
				if (string.IsNullOrWhiteSpace(redisConfiguration))
				{
					var connectionStringProvider = c.Resolve<Crm.Library.Configuration.IConnectionStringProvider>();
					return new SqlDistributedSynchronizationProvider(connectionStringProvider.DbConnectionString);
				}
				else
				{
					var connection = StackExchange.Redis.ConnectionMultiplexer.Connect(redisConfiguration);
					return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
				}
			}).SingleInstance();
			builder.Register<IDistributedCache>(c =>
			{
				var appSettingsProvider = c.Resolve<IAppSettingsProvider>();
				var redisConfiguration = appSettingsProvider.GetValue(MainPlugin.Settings.System.RedisConfiguration);
				if (string.IsNullOrWhiteSpace(redisConfiguration))
				{
					return new MemoryDistributedCache(new ExtendedMemoryDistributedCacheOptions());
				}
				else
				{
					return new RedisCache(new RedisCacheOptions
					{
						Configuration = redisConfiguration
					});
				}
			}).SingleInstance();
			builder.Register(c => c.Resolve<INHibernateInitializer>().Configuration).As<Configuration>();
			builder.Register(c => c.Resolve<ISessionFactoryProvider>().SessionFactory).As<ISessionFactory>().ExternallyOwned();
			builder.Register(c => c.Resolve<ISessionProvider>().GetSession()).As<ISession>().ExternallyOwned();
			builder.Register(c =>
			{
				var cfg = new MapperConfigurationExpression();
				cfg.AddProfile(c.Resolve<DefaultProfile>());
				cfg.ShouldMapMethod = p => false;
				return new MapperConfiguration(cfg);
			}).As<MapperConfiguration>().SingleInstance();
			builder.Register(c =>
			{
				var context = c.Resolve<IComponentContext>();
				return c.Resolve<MapperConfiguration>().CreateMapper(t => context.ResolveOptional(t) ?? Activator.CreateInstance(t));
			}).As<IMapper>();

			builder.Register(c => c.Resolve<IAppSettingsProvider>().GetValue(MainPlugin.Settings.System.UseActiveDirectoryAuthenticationService)
				? (IAuthenticationService)c.Resolve<ActiveDirectoryAuthenticationService>()
				: (IAuthenticationService)c.Resolve<DefaultAuthenticationService>()).As<IAuthenticationService>();
			builder.RegisterType<CefToPdfConverter>().As<ICefToPdfConverter>().SingleInstance();
			builder.Register(
				c =>
				{
					var appSettingsProvider = c.Resolve<IAppSettingsProvider>();
					var cefToPdfPath = appSettingsProvider.GetValue(MainPlugin.Settings.System.CefToPdfPath);
					if (string.IsNullOrWhiteSpace(cefToPdfPath))
					{
						return (IDeployment)new EmbeddedDeployment();
					}

					if (!Directory.Exists(cefToPdfPath))
					{
						LogManager.GetLogger(GetType()).Error($"CefToPdfPath '{cefToPdfPath}' does not exist, using embedded deployment instead");
						return (IDeployment)new EmbeddedDeployment();
					}

					var fileName = Path.Combine(cefToPdfPath, "CefToPdf.exe");
					var deployedVersion = AssemblyName.GetAssemblyName(fileName).Version;
					var currentVersion = typeof(IDeployment).Assembly.GetName().Version;
					if (deployedVersion < currentVersion)
					{
						LogManager.GetLogger(GetType()).Warn($"CefToPdfPath '{cefToPdfPath}' is outdated (deployed: {deployedVersion}, current: {currentVersion})");
					}

					return (IDeployment)new StaticDeployment(cefToPdfPath);
				}).As<IDeployment>();
			builder.RegisterType<CrmActionResultExecutor>().As<IActionResultExecutor<ViewResult>>();
			builder.RegisterType<CrmPartialViewResultExecutor>().As<IActionResultExecutor<PartialViewResult>>();
			builder.RegisterType<DefaultHttpContextFactory>().As<IHttpContextFactory>().InstancePerDependency();
			builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().SingleInstance();
			builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
		}
	}
}
