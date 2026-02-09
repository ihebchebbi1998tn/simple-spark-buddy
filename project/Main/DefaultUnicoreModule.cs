namespace Main
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Autofac;
	using Autofac.Core;

	using Crm.Library.AutoFac;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Unicore;

	using LMobile;
	using LMobile.Unicore;
	using LMobile.Unicore.NHibernate;

	using NHibernate.Mapping.ByCode.Conformist;

	public class DefaultUnicoreModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterModule<UnicoreModule>();
			builder.RegisterModule<UnicoreNHibernateModule>();
			builder.Register(c => c.Resolve<DefaultDynamicExtensionRegistry>()).As<DynamicExtensionRegistry>();
			builder.RegisterBuildCallback(RegisterMappingExtensions);
			builder.RegisterBuildCallback(SetupAuthorisation);
			builder.RegisterSource(new ExtensibleObjectRegistrationSource());

			builder.RegisterType<UserStoreWrapper>().InstancePerLifetimeScope();
		}
		protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
		{
			base.AttachToComponentRegistration(componentRegistry, registration);
			var type = registration.Activator.LimitType;
			if (typeof(IUserStore).IsAssignableFrom(type) && typeof(UserStoreWrapper).IsAssignableFrom(type) == false)
			{
				registration.Activating += (s, e) =>
				{
					e.Instance = e.Context.Resolve<UserStoreWrapper>(TypedParameter.From((IUserStore)e.Instance));
				};
			}
		}
		private static void SetupAuthorisation(IContainer container)
		{
			var authorisationSupport = container.Resolve<IAuthorisationConfiguration>();
			authorisationSupport.DefaultQueryOperation = UnicoreOperation.RW;
			authorisationSupport.DefaultQueryDomainAccessOptions = DomainAccessOptions.Normal;
		}
		private static void RegisterMappingExtensions(IContainer container)
		{
			var registry = container.Resolve<UnicoreNHibernateMappingExtensionRegistry>();
			var pluginProvider = container.Resolve<IPluginProvider>();
			var pluginTypes = new HashSet<Type>(pluginProvider.ActivePluginDescriptors
				.SelectMany(x => x.Assembly.GetExportedTypes())
				.Concat(typeof(DefaultDynamicExtensionRegistry).Assembly.GetExportedTypes()));
			var componentMappingTypes = pluginTypes
				.Where(x => x.IsModelExtensionType() && !pluginTypes.Contains(x.GetModelExtensionExtensibleType()))
				.Select(x => typeof(ComponentMapping<>).MakeGenericType(x))
				.ToList();
			var componentMappings = pluginTypes.Where(x => componentMappingTypes.Any(y => y.IsAssignableFrom(x)));
			foreach (var componentMapping in componentMappings)
			{
				registry.AddTypes(componentMapping);
			}
		}

		public class UserStoreWrapper : IUserStore
		{
			private readonly IUserStore store;
			private readonly IUnitOfWorkProvider unitOfWorkProvider;
			private readonly ISessionProvider sessionProvider;
			public UserStoreWrapper(IUserStore store, IUnitOfWorkProvider unitOfWorkProvider, ISessionProvider sessionProvider)
			{
				this.store = store;
				this.unitOfWorkProvider = unitOfWorkProvider;
				this.sessionProvider = sessionProvider;
			}
			private void EnsureUnitOfWork()
			{
				if (unitOfWorkProvider.Current == null)
				{
					unitOfWorkProvider.BeginUnitOfWork();
					sessionProvider.GetSession(); //initialize unit of work
				}
			}
			public void AddInferior(User superior, User inferior)
			{
				EnsureUnitOfWork();
				store.AddInferior(superior, inferior);
			}
			public void RemoveInferior(User superior, User inferior)
			{
				EnsureUnitOfWork();
				store.RemoveInferior(superior, inferior);
			}
			public User Insert(User user)
			{
				EnsureUnitOfWork();
				return store.Insert(user);
			}
			public User Update(User user)
			{
				EnsureUnitOfWork();
				return store.Update(user);
			}
			public void Delete(User user)
			{
				EnsureUnitOfWork();
				store.Delete(user);
			}
			public User SoftDelete(User user)
			{
				EnsureUnitOfWork();
				return store.SoftDelete(user);
			}
			public User CreateDataInstance(Guid domainId, Guid entityTypeId, string userName, string name, string email) => store.CreateDataInstance(domainId, entityTypeId, userName, name, email);
			public User FindById(Guid id) => store.FindById(id);
			public User FindByUserName(string userName) => store.FindByUserName(userName);
			public IList<User> GetInferiors(User superior) => store.GetInferiors(superior);
			public IList<User> GetSuperiors(User inferior) => store.GetSuperiors(inferior);
			public IList<User> ListAll() => store.ListAll();
			public IQueryable<User> Query() => store.Query();
			public IQueryable<WideUser> QueryWide() => store.QueryWide();
			public void SetAccessFailedCount(User user, int accessFailedCount) => store.SetAccessFailedCount(user, accessFailedCount);
			public void SetAuthData(User user, Guid? authDataId) => store.SetAuthData(user, authDataId);
			public void SetDomain(User user, Guid domainId) => store.SetDomain(user, domainId);
			public void SetEmail(User user, string email) => store.SetEmail(user, email);
			public void SetEmailConfirmed(User user, bool confirmed) => store.SetEmailConfirmed(user, confirmed);
			public void SetLastLoginDate(User user, DateTime lastLoginDate) => store.SetLastLoginDate(user, lastLoginDate);
			public void SetLockoutEnabled(User user, bool lockoutEnabled) => store.SetLockoutEnabled(user, lockoutEnabled);
			public void SetLockoutEndDate(User user, DateTime? lockoutStartDate) => store.SetLockoutEndDate(user, lockoutStartDate);
			public void SetLockoutStartDate(User user, DateTime? lockoutStartDate) => store.SetLockoutStartDate(user, lockoutStartDate);
			public void SetName(User user, string name) => store.SetName(user, name);
			public void SetPasswordHash(User user, string passwordHash) => store.SetPasswordHash(user, passwordHash);
			public void SetVersion(User user, long version) => store.SetVersion(user, version);
		}
	}
}
