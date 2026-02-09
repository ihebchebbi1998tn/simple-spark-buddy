namespace Main.Services;

using Crm.Library.Api.Model;

using Main.Rest.Model;

using ODataConventionModelBuilder = Crm.Library.Api.Model.ODataConventionModelBuilder;

public class DocumentGeneratorEntryModelConfigurator : IModelConfigurator
{
	private readonly IODataExtensionValueTypeBuilder extensionValueTypeBuilder;
	private readonly ODataModelBuilderHelper modelBuilderHelper;
	public DocumentGeneratorEntryModelConfigurator(IODataExtensionValueTypeBuilder extensionValueTypeBuilder, ODataModelBuilderHelper modelBuilderHelper)
	{
		this.extensionValueTypeBuilder = extensionValueTypeBuilder;
		this.modelBuilderHelper = modelBuilderHelper;
	}
	public virtual void Configure(ODataConventionModelBuilder builder)
	{
		var type = builder.AddEntityType(typeof(DocumentGeneratorEntry))
			.HasKey(typeof(DocumentGeneratorEntry).GetProperty(nameof(DocumentGeneratorEntry.Id)))
			;
		type.Name = modelBuilderHelper.GetEntityTypeName(typeof(DocumentGeneratorEntry));
		builder.AddEntitySet(modelBuilderHelper.GetEntityTypeName(typeof(DocumentGeneratorEntry)), type);
	}
}
