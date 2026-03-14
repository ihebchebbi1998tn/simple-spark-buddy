namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Extensions;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Api.Model;
	using Crm.Library.BaseModel.Attributes;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;

	using Main.Data.NHibernateProvider;
	using Main.Services.Interfaces;

	using Microsoft.OData.ModelBuilder;

	using NHibernate;
	using NHibernate.Cfg;
	using NHibernate.Mapping;
	using NHibernate.Metadata;

	public class DatabasePropertyConfigurator : IPropertyConfigurator
	{
		private readonly IInformationSchemaCache informationSchemaCache;
		private readonly RestTypeProviderCache restTypeProviderCache;
		private readonly Lazy<ODataMapperFactory> mapperFactory;
		private readonly Lazy<IODataExtensionValueTypeBuilder> extensionValueTypeBuilder;
		private readonly IDictionary<string, IClassMetadata> metadata;
		private readonly Configuration configuration;
		public DatabasePropertyConfigurator(
			IInformationSchemaCache informationSchemaCache,
			ISessionFactory sessionFactory,
			INHibernateInitializer nhibernateInitializer,
			RestTypeProviderCache restTypeProviderCache,
			Lazy<ODataMapperFactory> mapperFactory,
			Lazy<IODataExtensionValueTypeBuilder> extensionValueTypeBuilder)
		{
			this.informationSchemaCache = informationSchemaCache;
			this.restTypeProviderCache = restTypeProviderCache;
			this.mapperFactory = mapperFactory;
			this.extensionValueTypeBuilder = extensionValueTypeBuilder;
			metadata = sessionFactory.GetAllClassMetadata();
			configuration = nhibernateInitializer.Configuration;
		}
		protected virtual InformationSchema GetSchemaFromExtension(Type restType, string propertyName)
		{
			if (extensionValueTypeBuilder.Value.TryGetCachedMergedExtensionTypeInfo(restType.Name, out var info))
			{
				var originalProperty = info.Properties[propertyName].Property;
				var type = LMobile.Unicore.UnicoreExtensions.GetModelExtensionExtensibleType(originalProperty.DeclaringType);
				var attribute = originalProperty.GetCustomAttribute<DatabaseAttribute>();
				if (string.IsNullOrEmpty(attribute?.Formula) == false)
				{
					return null;
				}
				var name = attribute?.Column ?? propertyName;
				var extensions = configuration.GetClassMapping(type)?.GetProperty(nameof(IExtensible.Extensions));
				var column = extensions?.ColumnIterator.OfType<Column>().FirstOrDefault(x => x.Name == name);
				if (column != null)
				{
					return GetInformationSchema(column);
				}
			}
			return null;
		}
		protected virtual InformationSchema GetSchema(Type restType, string propertyName)
		{
			if (typeof(BaseODataExtensionValues).IsAssignableFrom(restType))
			{
				return GetSchemaFromExtension(restType, propertyName);
			}
			var domainTypes = restTypeProviderCache.GetDomainTypes(restType);
			if (domainTypes.Length != 1)
			{
				return null;
			}
			var type = domainTypes[0];
			propertyName = mapperFactory.Value.Configuration.TryFindMappedPropertyNameFor(type, restType, propertyName, out var isIgnored) ?? propertyName;
			if (isIgnored == false
				&& metadata.TryGetValue(type.FullName, out var data)
				&& data.PropertyNames.Contains(propertyName)
				&& configuration.GetClassMapping(type)?.GetProperty(propertyName) is Property property
				&& property.IsComposite == false && property.ColumnIterator.FirstOrDefault() is Column column)
			{
				return GetInformationSchema(column);
			}
			return null;
		}
		protected virtual InformationSchema GetInformationSchema(Column column)
		{
			var informationSchema = informationSchemaCache.GetAll()
				.Where(x => x.TableSchema == column.Value.Table.Schema)
				.Where(x => x.TableName == column.Value.Table.Name)
				.FirstOrDefault(x => x.ColumnName == column.Name);
			return informationSchema;
		}
		public virtual bool Filter( Type type, PropertyInfo property) => false;
		public virtual void Configure(StructuralTypeConfiguration structuralTypeConfiguration, PropertyConfiguration propertyConfiguration)
		{
			if (propertyConfiguration.PropertyInfo.GetCustomAttributes(true).Any(x => x is NavigationPropertyAttribute || x is NotReceivedAttribute))
			{
				return;
			}
			InformationSchema schema = null;
			if (propertyConfiguration is PrimitivePropertyConfiguration primitive)
			{
				schema = GetSchema(structuralTypeConfiguration.ClrType, primitive.Name);
				if (schema?.IsNullable == false && !new[] { nameof(IAuditable.CreateDate), nameof(IAuditable.CreateUser), nameof(IAuditable.ModifyDate), nameof(IAuditable.ModifyUser) }.Contains(primitive.Name))
				{
					primitive.IsRequired();
				}
			}
			if (propertyConfiguration is LengthPropertyConfiguration length)
			{
				schema = schema ?? GetSchema(structuralTypeConfiguration.ClrType, length.Name);
				if (schema?.CharacterMaximumLength.HasValue == true && schema.CharacterMaximumLength > 0)
				{
					length.MaxLength = length.MaxLength.HasValue ? Math.Min(length.MaxLength.Value, schema.CharacterMaximumLength.Value) : schema.CharacterMaximumLength;
				}
			}
		}
	}
}
