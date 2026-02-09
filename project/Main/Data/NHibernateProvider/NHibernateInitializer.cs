namespace Main.Data.NHibernateProvider
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Runtime;
	using System.Text;
	using System.Threading;
	using System.Xml.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Attributes;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Configuration;
	using Crm.Library.Data.Domain;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Data.NHibernateProvider.UserTypes;
	using Crm.Library.Environment.FileSystems.AppDataFolder;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Extensions;
	using Crm.Library.Modularization.Interfaces;

	using HibernatingRhinos.Profiler.Appender.NHibernate;

	using LMobile.Unicore;
	using LMobile.Unicore.NHibernate;

	using NHibernate;
	using NHibernate.Bytecode;
	using NHibernate.Cache;
	using NHibernate.Caches.StackExchangeRedis;
	using NHibernate.Cfg;
	using NHibernate.Cfg.Loquacious;
	using NHibernate.Dialect;
	using NHibernate.Driver;
	using NHibernate.Event;
	using NHibernate.Mapping;
	using NHibernate.Mapping.ByCode;
	using NHibernate.Type;

	public class NHibernateInitializer : INHibernateInitializer
	{
		private readonly IPluginProvider pluginProvider;
		private readonly SoftDeleteEventListener softDeleteEventListener;
		public virtual FileSystemWatcher NHProfConfigWatcher { get; set; }
		private readonly object nhprofLock = new();
		private Timer afterConfigurationChangedTimer;
		private readonly IEnumerable<ISaveOrUpdateEventListener> saveOrUpdateEventListeners = new List<ISaveOrUpdateEventListener>();
		private readonly IEnumerable<IPostInsertEventListener> postInsertEventListeners = new List<IPostInsertEventListener>();
		private readonly IEnumerable<IPostUpdateEventListener> postUpdateEventListeners = new List<IPostUpdateEventListener>();
		private readonly IEnumerable<IPostDeleteEventListener> postDeleteEventListeners = new List<IPostDeleteEventListener>();
		private readonly IEnumerable<IFlushEventListener> flushEventListeners = new List<IFlushEventListener>();
		private readonly IEnumerable<IFlushEntityEventListener> flushEntityEventListeners = new List<IFlushEntityEventListener>();
		private readonly IEnumerable<IMergeEventListener> mergeEventListeners = new List<IMergeEventListener>();
		private readonly IEnumerable<IPreInsertEventListener> preInsertEventListeners = new List<IPreInsertEventListener>();
		private readonly IEnumerable<IPreLoadEventListener> preLoadEventListeners;
		private readonly IEnumerable<IPreUpdateEventListener> preUpdateEventListeners = new List<IPreUpdateEventListener>();
		private readonly IEnumerable<IPreDeleteEventListener> preDeleteEventListeners = new List<IPreDeleteEventListener>();
		private readonly IEntityExtensionsProvider entityExtensionsProvider;
		private readonly IEnumerable<Lazy<ILookup, DependencyMetadata>> lookups;
		private readonly IEnumerable<ISessionFilter> sessionFilters;
		private readonly IEnumerable<IDatabaseMapping> databaseMappings;
		private readonly IConnectionStringProvider connectionStringProvider;
		private readonly IEnumerable<INHibernateAdditionalMappingProvider> additionalMappingProviders;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IAppDataFolder appDataFolder;

		public virtual Configuration Configuration { get; protected set; }

		protected virtual string GetExtensionPropertyColumnName(PropertyInfo propertyInfo)
		{
			var databaseAttribute = propertyInfo.GetFirstOrDefault<DatabaseAttribute>();
			if (databaseAttribute != null && databaseAttribute.Column.IsNotNullOrWhiteSpace())
			{
				return databaseAttribute.Column;
			}
			return propertyInfo.Name;
		}

		protected virtual string AddExtensionDynamicComponentMappings(string oldXml)
		{
			var xDocument = XDocument.Parse(oldXml);
			foreach (Type entityType in entityExtensionsProvider.GetExtendedTypes())
			{
				var element = xDocument.Descendants().FirstOrDefault(e => e.Name.LocalName == "class" && e.Attribute("name").Value == entityType.AssemblyQualifiedName);
				if (element == null)
				{
					var subclassElement = xDocument.Descendants().FirstOrDefault(e => e.Name.LocalName == "subclass" && e.Attribute("name").Value == entityType.AssemblyQualifiedName);
					if (subclassElement != null)
					{
						element = subclassElement.Descendants().FirstOrDefault(e => e.Name.LocalName == "join");
					}
					if (element == null)
					{
						element = xDocument.Descendants().FirstOrDefault(e => e.Name.LocalName == "subclass" && e.Attribute("name").Value == entityType.AssemblyQualifiedName);
					}
				}

				if (element == null)
				{
					continue;
				}

				var xNamespace = element.Name.Namespace;
				var dynamicComponentElement = element.Descendants().FirstOrDefault(x => x.Attribute("name")?.Value == "Extensions");

				if (dynamicComponentElement == null)
				{
					dynamicComponentElement = new XElement(xNamespace + "dynamic-component", new XAttribute("name", "Extensions"));
					element.Add(dynamicComponentElement);
				}

				var componentElements = dynamicComponentElement.Descendants(XName.Get("component", xNamespace.NamespaceName));

				foreach (var extensionType in entityExtensionsProvider.GetExtensionTypesWithoutBaseClass(entityType))
				{
					if (!componentElements.Any(x => x.Attribute("class").Value.Equals(extensionType.AssemblyQualifiedName)))
					{
						var componentElement = new XElement(xNamespace + "component", new XAttribute("class", extensionType.AssemblyQualifiedName), new XAttribute("name", extensionType.GetModelExtensionName()));
						dynamicComponentElement.Descendants().Where(x => x.Attribute("name")?.Value == extensionType.GetModelExtensionName()).ToList().ForEach(x => x.Remove());
						dynamicComponentElement.Add(componentElement);
					}
				}

				foreach (var componentElement in componentElements)
				{
					var hasExplicitMapping = componentElement.Descendants().Any();
					if (hasExplicitMapping)
					{
						continue;
					}

					var componentClass = Type.GetType(componentElement.Attribute("class").Value);
					var extensionPropertyInfos = componentClass.GetExtensionProperties();
					
				foreach (PropertyInfo propInfo in extensionPropertyInfos)
				{
					var columnName = GetExtensionPropertyColumnName(propInfo);

					var databaseAttribute = propInfo.GetFirstOrDefault<DatabaseAttribute>();
					if (databaseAttribute != null && databaseAttribute.Ignore)
					{
						continue;
					}

					var isManyToOne = databaseAttribute != null && databaseAttribute.ManyToOne;

					XElement propertyElement;
					if (isManyToOne)
					{
						var keyProperty = extensionPropertyInfos.FirstOrDefault(x => x.DeclaringType == propInfo.DeclaringType && x.Name.StartsWith(propInfo.Name) && (x.Name.EndsWith("Id") || x.Name.EndsWith("Key")));
						if (keyProperty == null)
						{
							continue;
						}
						var keyColumnName = GetExtensionPropertyColumnName(keyProperty);
						propertyElement = new XElement(xNamespace + "many-to-one",
							new XAttribute("name", propInfo.Name),
							new XAttribute("column", keyColumnName),
							new XAttribute("class", propInfo.PropertyType.AssemblyQualifiedName));
					}
					else
					{
						propertyElement = new XElement(xNamespace + "property",
						new XAttribute("name", propInfo.Name),
						new XAttribute("type", propInfo.PropertyType.FullName),
						new XAttribute("column", columnName));
						if (propInfo.PropertyType == typeof(string))
						{
							propertyElement.Add(new XAttribute("length", Int32.MaxValue));
						}
					}
					if (databaseAttribute != null && databaseAttribute.Formula != null)
					{
						propertyElement.Attribute("column").Remove();
						propertyElement.Add(new XAttribute("formula", databaseAttribute.Formula));
					}
					if (propInfo.HasAttribute<ReadOnlyExtensionPropertyAttribute>() || isManyToOne)
					{
						propertyElement.Add(new XAttribute("insert", false), new XAttribute("update", false));
					}
					if (propInfo.PropertyType == typeof(List<string>))
					{
						propertyElement.SetAttributeValue("type", typeof(DelimitedStringUserType).AssemblyQualifiedName);
					}

						componentElement.Add(propertyElement);
				}
				}
			}

			var newXml = xDocument.ToString();
			return newXml;
		}

		protected virtual string GetTableName(Type lookupType)
		{
			string reflectedTablename = null;
			var attributes = lookupType.GetCustomAttributes(typeof(LookupAttribute), false);
			if (attributes.Length == 1)
			{
				reflectedTablename = ((LookupAttribute)attributes[0]).Tablename;
			}
			return reflectedTablename ?? $"[LU].{lookupType.Name}";
		}
		protected virtual string GetIdColumnName(Type lookupType)
		{
			string reflectedIdColumnName = null;
			var attributes = lookupType.GetCustomAttributes(typeof(LookupAttribute), false);
			if (attributes.Length == 1)
			{
				reflectedIdColumnName = ((LookupAttribute)attributes[0]).IdColumn;
			}
			return reflectedIdColumnName ?? lookupType.Name + "Id";
		}
		protected virtual string AddLookupMappings(string oldXml)
		{
			var xDocument = XDocument.Parse(oldXml);
			var xNamespace = xDocument.Root.Name.Namespace;
			var lookupTypes = lookups.Select(x => x.Metadata.RegisteredType).ToList();
			foreach (Type lookupType in lookupTypes)
			{
				// Use db independent `instead of [], Nhibernate will convert it to the correct escaping
				// http://stackoverflow.com/questions/679279/nhibernate-force-escaping-on-table-names
				var tableName = GetTableName(lookupType).Replace(new[] { "[", "]" }, "`").Split('.');

				var lookupElement = new XElement(xNamespace + "class",
					new XAttribute("name", lookupType.AssemblyQualifiedName),
					new XAttribute("table", tableName[1]),
					new XAttribute("schema", tableName[0]));
				var cacheElement = new XElement(xNamespace + "cache",
					new XAttribute("usage", "read-write"));
				lookupElement.Add(cacheElement);
				var idElement = new XElement(xNamespace + "id",
					new XAttribute("name", "Id"),
					new XAttribute("column", GetIdColumnName(lookupType)),
					new XAttribute("type", "Guid"),
					new XAttribute("unsaved-value", "00000000-0000-0000-0000-000000000000"));
				idElement.Add(new XElement(xNamespace + "generator", new XAttribute("class", LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance.Class)));

				lookupElement.Add(idElement);

				foreach (PropertyInfo lookupProperty in lookupType.GetPropertiesWith<LookupPropertyAttribute>())
				{
					var databaseAttribute = lookupProperty.GetFirstOrDefault<DatabaseAttribute>();
					if (databaseAttribute != null && databaseAttribute.Ignore)
					{
						continue;
					}
					var columnName = lookupProperty.GetFirstOrDefault<LookupPropertyAttribute>().Column ?? lookupProperty.Name;
					var element = new XElement(xNamespace + "property", new XAttribute("name", lookupProperty.Name), new XAttribute("column", columnName));
					if (lookupProperty.PropertyType == typeof(List<string>))
					{
						element.Add(new XAttribute("type", typeof(DelimitedStringUserType).AssemblyQualifiedName));
					}
					if (lookupProperty.PropertyType == typeof(string) || lookupProperty.PropertyType == typeof(byte[]))
					{
						element.Add(new XAttribute("length", Int32.MaxValue));
					}
					if (lookupProperty.PropertyType == typeof(DateTime))
					{
						element.Add(new XAttribute("type", typeof(DateTimeType).AssemblyQualifiedName));
					}
					lookupElement.Add(element);
				}

				if (typeof(IEntityLookup).IsAssignableFrom(lookupType))
				{
					var filterElement = new XElement(xNamespace + "filter", new XAttribute("name", IsActiveFilter.Name), new XAttribute("condition", IsActiveFilter.Condition));
					lookupElement.Add(filterElement);
				} else if (typeof(ISoftDeletableObject).IsAssignableFrom(lookupType))
				{
					var filterElement = new XElement(xNamespace + "filter", new XAttribute("name", SoftDeleteFilter.Name), new XAttribute("condition", SoftDeleteFilter.Condition));
					lookupElement.Add(filterElement);
				}

				xDocument.Root.Add(lookupElement);
			}

			var newXml = xDocument.ToString();
			return newXml;
		}

		protected virtual string MoveChildElementsToTheEndOfParent(string oldXml, string childElementName)
		{
			var xDocument = XDocument.Parse(oldXml);
			var descendants = xDocument.Descendants().Where(x => x.Name.LocalName == childElementName).ToArray();
			foreach (XElement descendant in descendants)
			{
				var parentElement = descendant.Parent;
				descendant.Remove();
				parentElement.Add(descendant);
			}
			var newXml = xDocument.ToString();
			return newXml;
		}

		protected virtual Action<DbIntegrationConfigurationProperties> DataBaseIntegration
		{
			get
			{
				return db =>
				{
					db.Driver<MicrosoftDataSqlClientDriver>();
					db.ConnectionString = connectionStringProvider.DbConnectionString;
					db.Dialect<MsSql2012Dialect>();
#if (DEBUG)
					db.LogSqlInConsole = true;
					db.LogFormattedSql = true;
					db.AutoCommentSql = true;
#endif
				};
			}
		}

		protected virtual Configuration Initialize()
		{
			// This code is not wanted but needed as the InitializeUsingConfiguration method is supposed
			// to watch the file for changes. Unfortunately the implementation was broken as of May 2025
			// and thus we decided to put our own watcher to be able to catch runtime config changes to be
			// able to start or stop profiler during an executing worker process.
			var nhprofConfigPath = appDataFolder.MapPath("config");
			var nhprofConfigFile = Path.Combine(nhprofConfigPath, "nhprof.config");
			FileSystemEventHandler nhprofConfigChanged = (e, args) =>
			{
				lock (nhprofLock)
				{
					NHibernateProfiler.Shutdown();
					if (afterConfigurationChangedTimer != null)
					{
						afterConfigurationChangedTimer.Change(TimeSpan.FromSeconds(1.0), TimeSpan.FromMilliseconds(-1.0));
						return;
					}
					afterConfigurationChangedTimer = new Timer(delegate
					{
						afterConfigurationChangedTimer?.Dispose();
						afterConfigurationChangedTimer = null;
						NHibernateProfiler.InitializeUsingConfiguration(nhprofConfigFile);
					}, null, TimeSpan.FromSeconds(1.0), TimeSpan.FromMilliseconds(-1.0));
				}
			};

			NHProfConfigWatcher = new FileSystemWatcher(nhprofConfigPath, "nhprof.config")
			{
				EnableRaisingEvents = true
			};
			NHProfConfigWatcher.Changed += nhprofConfigChanged;
			NHProfConfigWatcher.Created += nhprofConfigChanged;
			NHibernateProfiler.InitializeUsingConfiguration(nhprofConfigFile);

			var configuration = new Configuration();
			configuration
				.Proxy(p => p.ProxyFactoryFactory<StaticProxyFactoryFactory>())
				.DataBaseIntegration(DataBaseIntegration);
			var redisConfiguration = appSettingsProvider.GetValue(MainPlugin.Settings.System.RedisConfiguration);
			if (!string.IsNullOrWhiteSpace(redisConfiguration))
			{
				configuration.Cache(
						x =>
						{
							x.UseQueryCache = true;
							x.Provider<RedisCacheProvider>();
						})
					.AddProperties(new Dictionary<string, string>
					{
						{ RedisEnvironment.Configuration, redisConfiguration },
						{ RedisEnvironment.Serializer,  typeof(NHibernate.Caches.Util.JsonSerializer.JsonCacheSerializer).AssemblyQualifiedName }
					});
			}
			else
			{
				configuration.Cache(
					x =>
					{
						x.UseQueryCache = true;
						x.Provider<HashtableCacheProvider>();
					});
			}

			var mapper = new ModelMapper(new ExplicitlyDeclaredModel(), new UnicoreNHibernateCandidatePersistentMembersProvider());
			var databaseMappingTypes = databaseMappings.Select(x => x.GetType());
			var pluginMappingTypes = pluginProvider.ActivePluginDescriptors.SelectMany(x => x.Assembly.GetExportedTypes()).Except(databaseMappingTypes).Where(x => typeof (IConformistHoldersProvider).IsAssignableFrom(x) && !x.IsGenericTypeDefinition);
			var pluginMappings = pluginMappingTypes.Select(Activator.CreateInstance).Cast<IConformistHoldersProvider>();
			var mappings = databaseMappings.Concat(pluginMappings);
			var sortedMappings = pluginProvider.SortByPluginDependency(mappings);
			foreach (var mapping in sortedMappings)
			{
				mapper.AddMapping(mapping);
			}

			mapper.CompileMappingForAllExplicitlyAddedEntities();
			foreach (Type entityType in entityExtensionsProvider.GetExtendedTypes())
			{
				var extensionTypes = entityExtensionsProvider.GetExtensionTypesWithoutBaseClass(entityType);
				mapper.AddExtensibleObjectMapping(entityType, extensionTypes);
			}

			var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
			domainMapping.autoimport = false;
			domainMapping.MoveSubclassExtensionsToJoin();

			var mappingXml = domainMapping.AsString();
			mappingXml = AddLookupMappings(mappingXml);
			mappingXml = AddExtensionDynamicComponentMappings(mappingXml);
			foreach (var amp in additionalMappingProviders)
			{
				mappingXml = amp.AddMappings(mappingXml);
			}

			// Due to a bug in NHibernate 3.3 the join element is moved to the last position (see https://nhibernate.jira.com/browse/NHE-73).
			// Since it seems to be fixed in vNext the following line and the method MakeJoinLastElement can probably be removed with the next verion of NHibernate
			mappingXml = MoveChildElementsToTheEndOfParent(mappingXml, "filter");
			mappingXml = MoveChildElementsToTheEndOfParent(mappingXml, "join");

			configuration.AddXmlString(mappingXml);
			configuration
					.ClassMappings
					.SelectMany(x => x.PropertyIterator)
					.Where(x => x.IsComposite)
					.Select(x => x.Value)
					.Cast<Component>()
					.ForEach(x => x.AddTuplizer(EntityMode.Map, typeof(CustomDynamicMapComponentTuplizer).AssemblyQualifiedName));

			// ReSharper disable CoVariantArrayConversion
			// Replace all available Delete event listeners with our SoftDeleteEventListener instance, otherwise
			// we can't keep our promise not to delete records in the database
			configuration.EventListeners.DeleteEventListeners = new[] { softDeleteEventListener };
			configuration.AppendListeners(ListenerType.Flush, flushEventListeners.ToArray());
			configuration.EventListeners.FlushEntityEventListeners = flushEntityEventListeners.ToArray();
			if (mergeEventListeners.Any())
			{
				configuration.EventListeners.MergeEventListeners = mergeEventListeners.ToArray();
			}
			configuration.AppendListeners(ListenerType.PreInsert, preInsertEventListeners.ToArray());
			configuration.AppendListeners(ListenerType.PreLoad, preLoadEventListeners.ToArray());
			configuration.AppendListeners(ListenerType.PreUpdate, preUpdateEventListeners.ToArray());
			configuration.AppendListeners(ListenerType.PreDelete, preDeleteEventListeners.ToArray());
			configuration.AppendListeners(ListenerType.Save, saveOrUpdateEventListeners.ToArray());
			configuration.AppendListeners(ListenerType.PostInsert, postInsertEventListeners.ToArray());
			configuration.AppendListeners(ListenerType.PostUpdate, postUpdateEventListeners.ToArray());
			configuration.AppendListeners(ListenerType.PostDelete, postDeleteEventListeners.ToArray());
			// ReSharper restore CoVariantArrayConversion

			configuration.FilterDefinitions.Clear();
			sessionFilters.ForEach(x => configuration.AddFilterDefinition(x.Definition));

			return configuration;
		}

		public NHibernateInitializer(IPluginProvider pluginProvider,
				IEnumerable<ISaveOrUpdateEventListener> saveOrUpdateEventListeners,
				IEnumerable<IPostInsertEventListener> postInsertEventListeners,
				IEnumerable<IPostUpdateEventListener> postUpdateEventListeners,
				IEnumerable<IPostDeleteEventListener> postDeleteEventListeners,
				SoftDeleteEventListener softDeleteEventListener,
				IEnumerable<IFlushEventListener> flushEventListeners,
				IEnumerable<IFlushEntityEventListener> flushEntityEventListeners,
				IEnumerable<IMergeEventListener> mergeEventListeners,
				IEnumerable<IPreInsertEventListener> preInsertEventListeners,
				IEnumerable<IPreLoadEventListener> preLoadEventListeners,
				IEnumerable<IPreUpdateEventListener> preUpdateEventListeners,
				IEnumerable<IPreDeleteEventListener> preDeleteEventListeners,
				IEntityExtensionsProvider entityExtensionsProvider,
				IEnumerable<Lazy<ILookup, DependencyMetadata>> lookups,
				IEnumerable<ISessionFilter> sessionFilters,
				IEnumerable<IDatabaseMapping> databaseMappings,
				IConnectionStringProvider connectionStringProvider,
				IEnumerable<INHibernateAdditionalMappingProvider> additionalMappingProviders,
				IAppSettingsProvider appSettingsProvider,
				IAppDataFolder appDataFolder)
		{
			this.pluginProvider = pluginProvider;
			this.saveOrUpdateEventListeners = saveOrUpdateEventListeners;
			this.postInsertEventListeners = postInsertEventListeners;
			this.postUpdateEventListeners = postUpdateEventListeners;
			this.postDeleteEventListeners = postDeleteEventListeners;
			this.softDeleteEventListener = softDeleteEventListener;
			this.flushEventListeners = flushEventListeners;
			this.flushEntityEventListeners = flushEntityEventListeners;
			this.mergeEventListeners = mergeEventListeners;
			this.preInsertEventListeners = preInsertEventListeners;
			this.preLoadEventListeners = preLoadEventListeners;
			this.preUpdateEventListeners = preUpdateEventListeners;
			this.preDeleteEventListeners = preDeleteEventListeners;
			this.entityExtensionsProvider = entityExtensionsProvider;
			this.lookups = lookups;
			this.sessionFilters = sessionFilters;
			this.databaseMappings = databaseMappings;
			this.connectionStringProvider = connectionStringProvider;
			this.additionalMappingProviders = additionalMappingProviders;
			this.appSettingsProvider = appSettingsProvider;
			this.appDataFolder = appDataFolder;
			Configuration = Initialize();
		}

#if (DEBUG)
		private class NhibernateDebugWriter : TextWriter
		{
			public override void WriteLine(string value)
			{
				Debug.WriteLine(value);
				base.WriteLine(value);
			}
			public override void Write(string value)
			{
				Debug.Write(value);
				base.Write(value);
			}
			public override Encoding Encoding
			{
				get { return Encoding.UTF8; }
			}
		}
#endif
	}
}
