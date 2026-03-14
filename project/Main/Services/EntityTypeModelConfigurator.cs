namespace Main.Services
{
	using System.Reflection;
	using Crm.Library.Api.Model;

	using LMobile.Unicore;

	using Main.Rest.Model;

	using Microsoft.OData.ModelBuilder;
	using ODataConventionModelBuilder = Crm.Library.Api.Model.ODataConventionModelBuilder;

	public class EntityTypeModelConfigurator : IModelConfigurator, IExtensionValueProvider
	{
		private readonly IODataExtensionValueTypeBuilder extensionValueTypeBuilder;
		private readonly ODataModelBuilderHelper modelBuilderHelper;
		public EntityTypeModelConfigurator(IODataExtensionValueTypeBuilder extensionValueTypeBuilder, ODataModelBuilderHelper modelBuilderHelper)
		{
			this.extensionValueTypeBuilder = extensionValueTypeBuilder;
			this.modelBuilderHelper = modelBuilderHelper;
		}
		public virtual void Configure(ODataConventionModelBuilder builder)
		{
			var type = builder.AddEntityType(typeof(EntityTypeRest))
				.HasKey(typeof(EntityTypeRest).GetProperty(nameof(EntityTypeRest.UId)));
			type.Name = modelBuilderHelper.GetEntityTypeName(typeof(EntityType));
			builder.AddEntitySet(modelBuilderHelper.GetEntityTypeName(typeof(EntityType)), type);
		}
		public virtual PropertyInfo Get(ODataConventionModelBuilder builder, EntityTypeConfiguration configuration)
		{
			if (configuration.ClrType == typeof(EntityTypeRest))
			{
				return extensionValueTypeBuilder.CreateMergedExtensionValuesProperty(modelBuilderHelper.GetPluginName(typeof(EntityTypeRest)), typeof(EntityType), typeof(EntityTypeRest));
			}

			return null;
		}
	}
}
