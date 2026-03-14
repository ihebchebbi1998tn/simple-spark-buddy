namespace Main.Services
{
	using System.Reflection;
	using Crm.Library.Api.Model;

	using LMobile.Unicore;

	using Main.Rest.Model;

	using Microsoft.OData.ModelBuilder;
	using ODataConventionModelBuilder = Crm.Library.Api.Model.ODataConventionModelBuilder;

	public class RoleModelConfigurator : IModelConfigurator, IExtensionValueProvider
	{
		private readonly IODataExtensionValueTypeBuilder extensionValueTypeBuilder;
		private readonly ODataModelBuilderHelper modelBuilderHelper;
		public RoleModelConfigurator(IODataExtensionValueTypeBuilder extensionValueTypeBuilder, ODataModelBuilderHelper modelBuilderHelper)
		{
			this.extensionValueTypeBuilder = extensionValueTypeBuilder;
			this.modelBuilderHelper = modelBuilderHelper;
		}
		public virtual void Configure(ODataConventionModelBuilder builder)
		{
			var type = builder.AddEntityType(typeof(PermissionSchemaRoleRest))
				.HasKey(typeof(PermissionSchemaRoleRest).GetProperty(nameof(PermissionSchemaRoleRest.UId)));
			type.Name = modelBuilderHelper.GetEntityTypeName(typeof(PermissionSchemaRole));
			builder.AddEntitySet(modelBuilderHelper.GetEntityTypeName(typeof(PermissionSchemaRole)), type);
		}
		public virtual PropertyInfo Get(ODataConventionModelBuilder builder, EntityTypeConfiguration configuration)
		{
			if (configuration.ClrType == typeof(PermissionSchemaRoleRest))
			{
				return extensionValueTypeBuilder.CreateMergedExtensionValuesProperty(modelBuilderHelper.GetPluginName(typeof(PermissionSchemaRoleRest)), typeof(PermissionSchemaRole), typeof(PermissionSchemaRoleRest));
			}

			return null;
		}
	}
}
