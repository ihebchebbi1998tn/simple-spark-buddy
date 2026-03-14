namespace Crm.Service.Controllers.OData
{
	using Crm.Library.Api.Model;
	using Crm.Library.Model;
	using Crm.PerDiem.Controllers.OData;
	using Crm.Service.Rest.Model;

	using Main.Rest.Model;

	using ODataConventionModelBuilder = Crm.Library.Api.Model.ODataConventionModelBuilder;

	public class ServiceFunctionConfigurator : IModelConfigurator
	{
		private readonly ODataModelBuilderHelper modelHelper;
		private readonly DistinctDateFunctionConfiguratorHelper distinctDateHelper;
		public ServiceFunctionConfigurator(DistinctDateFunctionConfiguratorHelper distinctDateHelper, ODataModelBuilderHelper modelHelper)
		{
			this.distinctDateHelper = distinctDateHelper;
			this.modelHelper = modelHelper;
		}
		public virtual void Configure(ODataConventionModelBuilder builder)
		{
			var getTechnicians = builder.EntityType<UserRest>()
				.Collection
				.Function(nameof(TechnicianODataController.GetTechnicians))
				.ReturnsCollectionFromEntitySet<UserRest>(modelHelper.GetEntityTypeName(typeof(User)));
			getTechnicians.Parameter<bool>("filterByRole").Optional();
			getTechnicians.Title = $"gets technicians, optionally filters the result by the users being in a role based on the {ServicePlugin.Roles.FieldService} template";

			distinctDateHelper.Configure<ServiceOrderTimePostingRest>(builder);
			distinctDateHelper.Configure<ServiceOrderExpensePostingRest>(builder);
		}
	}
}
