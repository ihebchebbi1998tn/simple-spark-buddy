namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class QuantityRequired : RequiredRule<ServiceOrderMaterial>
	{
		private readonly ILookupManager lookupManager;
		public QuantityRequired(ILookupManager lookupManager)
		{
			this.lookupManager = lookupManager;
			Init(m => m.Quantity);
		}

		protected override bool IsIgnoredFor(ServiceOrderMaterial material)
		{
			return material.ServiceOrderMaterialType == "Invoice" || Helper.OrderClosed(material, lookupManager) || material.ParentServiceOrderMaterialId != null;
		}

		public override bool IsSatisfiedBy(ServiceOrderMaterial material)
		{
			return material.Quantity > 0;
		}
	}

	internal static class Helper
	{
		internal static bool OrderClosed(ServiceOrderMaterial material, ILookupManager lookupManager)
		{
			ServiceOrderStatus orderStatus = null;
			if (material.ServiceOrderHead != null && material.ServiceOrderHead.StatusKey.IsNotNullOrEmpty())
			{
				orderStatus = lookupManager.Get<ServiceOrderStatus>(material.ServiceOrderHead.StatusKey);
			}

			return orderStatus != null && orderStatus.BelongsToClosed();
		}
	}
}
