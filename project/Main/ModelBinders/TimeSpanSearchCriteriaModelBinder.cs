namespace Main.ModelBinders
{
	using System;
	using System.Linq;

	using Crm.Library.Model;
	using Crm.Library.ModelBinder;

	using Main.SearchCriteria;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ModelBinding;

	[ModelBinderFor(typeof(TimeSpanSearchCriteria))]
	public class TimeSpanSearchCriteriaModelBinder : CrmModelBinder, ICrmModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var model = base.BindModel(controllerContext, bindingContext) as TimeSpanSearchCriteria;

			if (model == null)
			{
				throw new Exception("Only use TimeSpanSearchCriteriaModelBinder if your Model implements TimeSpanSearchCriteria");
			}

			var timeSpanKey = GetValue<string>(bindingContext, "timespan_selector");

			if (string.IsNullOrEmpty(timeSpanKey))
			{
				model.NoDeterminedTimespan = true;
				timeSpanKey = TimeSpanItem.AllKey;
			}
			else
			{
				model.NoDeterminedTimespan = false;
			}

			if (timeSpanKey == TimeSpanItem.FromToKey)
			{
				var fromDateISO = GetValue<string>(bindingContext, "fromDate");
				var toDateISO = GetValue<string>(bindingContext, "toDate");
				try
				{
					model.FromDate = DateTime.Parse(fromDateISO);
					model.ToDate = DateTime.Parse(toDateISO);
				}
				catch
				{
					bindingContext.ModelState.AddModelError("_FILTER", "InvalidTimeSpan");
				}
			}
			else
			{
				var timeSpanItem = TimeSpanSearchCriteria.AllTimeSpanItems.FirstOrDefault(i => i.Key == timeSpanKey);
				model.FromDate = timeSpanItem != null ? timeSpanItem.FromDate : new DateTime();
				model.ToDate = timeSpanItem != null ? timeSpanItem.ToDate : new DateTime();
			}

			model.SelectedTime = TimeSpanSearchCriteria.AllTimeSpanItems.FirstOrDefault(i => i.Key == timeSpanKey);

			if (model.FromDate > model.ToDate)
			{
				bindingContext.ModelState.AddModelError("_FILTER", "InvalidTimeSpan");
			}

			return model;
		}
	}
}
