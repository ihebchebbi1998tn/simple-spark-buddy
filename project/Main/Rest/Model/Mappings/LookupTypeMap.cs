namespace Main.Rest.Model.Mappings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.BaseModel.Attributes;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;

	using Newtonsoft.Json;

	public class LookupTypeMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Type, LookupType>()
				.ForMember(x => x.IsEditable, m => m.MapFrom(x => x.NotHasAttribute<NotEditable>(false)))
				.ForMember(x => x.LookupProperties, m => m.MapFrom((source, destination, member, context) => GetLookupProperties(context.GetService<IEntityExtensionsProvider>(), source)))
				;
		}

		protected virtual IEnumerable<LookupProperty> GetLookupProperties(IEntityExtensionsProvider entityExtensionsProvider, Type lookupType)
		{
			foreach (var propertyInfo in lookupType.GetPropertiesWith<LookupPropertyAttribute>().Where(p => !p.HasAttribute<JsonIgnoreAttribute>()))
			{
				yield return GetLookupProperty(propertyInfo, false);
			}

			foreach (var propertyInfo in entityExtensionsProvider.GetExtensionTypes(lookupType).SelectMany(entityExtensionsProvider.GetExtensionProperties))
			{
				yield return GetLookupProperty(propertyInfo, true);
			}
		}

		protected virtual LookupProperty GetLookupProperty(PropertyInfo propertyInfo, bool isExtension)
		{
			return new LookupProperty
			{
				Extension = isExtension,
				Hidden = propertyInfo.HasAttribute<UIAttribute>() && ((bool)propertyInfo.GetAttributeValue<UIAttribute>(x => x.Hidden) || (bool)propertyInfo.GetAttributeValue<UIAttribute>(x => x.UIignore)),
				Name = propertyInfo.Name,
				PropertyTypeName = (propertyInfo.PropertyType.IsNullableType() ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType).ToString(),
				Shared = (bool)propertyInfo.GetAttributeValue<LookupPropertyAttribute>(a => a.Shared),
				Required = (bool)propertyInfo.GetAttributeValue<LookupPropertyAttribute>(a => a.Required)
			};
		}
	}
}
