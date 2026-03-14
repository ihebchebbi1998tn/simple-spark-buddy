namespace Sms.Scheduler.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	using Crm.Article.Rest.Model;
	using Crm.Library.BaseModel;
	using Crm.Library.Model;
	using Crm.Service.Rest.Model;

	using NJsonSchema.Annotations;

	public class Profile : EntityBase<int>, IEqualityComparer<Profile>
	{
		public virtual Guid InternalId { get; set; }
		public virtual string Username { get; set; }
		public virtual string Name { get; set; }
		public virtual bool DefaultProfile { get; set; }
		public virtual string ClientConfig { get; set; }
		public virtual int TemplateKey { get; set; }
		public virtual User User { get; set; }

		public virtual ICollection<string> ResourceKeys { get; set; }

		public Profile()
		{
			InternalId = Guid.NewGuid();
		}
		public virtual int GetHashCode(Profile obj)
		{
			unchecked
			{
				var hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ (Username != null ? Username.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ DefaultProfile.GetHashCode();
				return hashCode;
			}
		}
		public virtual bool Equals(Profile x, Profile y)
		{
			if (ReferenceEquals(x, y))
			{
				return true;
			}

			if (x is null)
			{
				return false;
			}

			if (y is null)
			{
				return false;
			}

			if (x.GetType() != y.GetType())
			{
				return false;
			}

			return base.Equals(y) && string.Equals(Username, y.Username) && DefaultProfile.Equals(y.DefaultProfile);
		}
	}

	public class ClientConfig
	{
		private const string Divider = "#.#";
		[ItemsCanBeNull, DefaultValue(new[]
		{
			"Resource.DisplayName",
			"Resource.Email",
			$"{Divider}",
			"Resource.Skills",
			"Resource.Capacity",
			$"{Divider}",
			"Resource.License"
		})]
		public string[] ResourceTooltip { get; set; }
		[ItemsCanBeNull, DefaultValue(new[]
		{
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.OrderNo)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.Company)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.ErrorMessage)}",
			$"{Divider}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.City)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.ZipCode)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.Street)}",
			"ServiceOrder.Country",
			$"{Divider}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.Dispatches)}",
			$"{Divider}",
			"ServiceOrder.Skills"
		})]
		public string[] ServiceOrderTooltip { get; set; }
		[ItemsCanBeNull, DefaultValue(new[]
		{
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.OrderNo)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.Company)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.ErrorMessage)}",
			$"{Divider}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.City)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.ZipCode)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.Street)}",
			"ServiceOrder.Country",
			$"{Divider}",
			$"ServiceOrderDispatch.{nameof(ServiceOrderDispatchRest.Date)}",
			$"ServiceOrderDispatch.{nameof(ServiceOrderDispatchRest.EndDate)}",
			$"ServiceOrderDispatch.{nameof(ServiceOrderDispatchRest.NetWorkMinutes)}",
			$"{Divider}",
			$"ServiceOrderDispatch.{nameof(ServiceOrderDispatchRest.ModifyDate)}",
			$"ServiceOrderDispatch.{nameof(ServiceOrderDispatchRest.ModifyUser)}"
		})]
		public string[] ServiceOrderDispatchTooltip { get; set; }
		[ItemsCanBeNull, DefaultValue(new[]
		{
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.OrderNo)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.Company)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.ErrorMessage)}",
			$"{Divider}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.City)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.ZipCode)}",
			$"ServiceOrder.{nameof(ServiceOrderHeadRest.Street)}",
			"ServiceOrder.Country",
			$"{Divider}",
			$"ServiceOrderTime.{nameof(ServiceOrderTimeRest.PosNo)}",
			$"ServiceOrderTime.{nameof(ServiceOrderTimeRest.Description)}",
			$"ServiceOrderTime.{nameof(ServiceOrderTimeRest.Article)}.{nameof(ArticleRest.ItemNo)}",
			$"ServiceOrderTime.{nameof(ServiceOrderTimeRest.Article)}.{nameof(ArticleRest.Description)}",
			$"ServiceOrderTime.{nameof(ServiceOrderTimeRest.Installation)}.{nameof(InstallationRest.InstallationNo)}",
			$"ServiceOrderTime.{nameof(ServiceOrderTimeRest.Installation)}.{nameof(InstallationRest.Description)}",
			$"{Divider}",
			$"ServiceOrderDispatch.{nameof(ServiceOrderDispatchRest.ModifyDate)}",
			$"ServiceOrderDispatch.{nameof(ServiceOrderDispatchRest.ModifyUser)}"
		})]
		public string[] ServiceOrderTimeTooltip { get; set; }
		[ItemsCanBeNull]
		public HourSpan[] NonWorkingHours { get; set; }
		[Required, DefaultValue(true)]
		public bool EnablePlanningConfirmations { get; set; }
		[DefaultValue(14)]
		public decimal LowerBound { get; set; }
		[DefaultValue(62)]
		public decimal UpperBound { get; set; }
		[DefaultValue(new string[] { })]
		public string[] PipelineGroup { get; set; }

		[DefaultValue(new string[] { })]
		public string[] ResourceGroup { get; set; }

		[DefaultValue(new[] { "#0000ff", "#ff0000", "#039422", "#ff6f00", "#ffff00" })]
		public string[] RouteColors { get; set; }
		[Required, DefaultValue(false)]
		public bool LoadClosedServiceOrders { get; set; }

		[Required, Range(30, 480), DefaultValue(120)]
		public int ServiceOrderDispatchDefaultDuration { get; set; }

		[CanBeNull]
		public int? ServiceOrderDispatchMaximumDuration { get; set; }
		[DefaultValue(false)]
		public bool ServiceOrderDispatchIgnoreCalculatedDuration { get; set; }
		[DefaultValue(false)]
		public bool ServiceOrderDispatchForceMaximumDuration { get; set; }
		[Required, DefaultValue(true)]
		public bool AllowSchedulingForPast { get; set; }

		[DefaultValue("ServiceOrder.Company")]
		public string PipelineFirstLine { get; set; }
		[DefaultValue("")]
		public string PipelineSecondLine { get; set; }
		[DefaultValue(new string[] { $"ServiceOrder.{nameof(ServiceOrderHeadRest.OrderNo)}" })]
		public string[] DataForFirstRow { get; set; }
		[DefaultValue(new string[] { })]
		public string[] DataForSecondRow { get; set; }
		[DefaultValue(new string[] { })]
		public string[] DataForThirdRow { get; set; }
		[DefaultValue(false)]
		public bool SnapFirstEventOfDayToStartOfDay { get; set; }

		[DefaultValue(false)]
		public bool ServiceOrderTimesScheduling { get; set; }

		[DefaultValue(true)]
		public bool DisplayClosedDispatches { get; set; }
		[DefaultValue(true)]
		public bool DisplayRejectedDispatches { get; set; }
		[DefaultValue(0.5), Range(0, 5)]
		public decimal TooltipDisplayDelay { get; set; }
	}

	public struct HourSpan
	{
		[Range(0, 24), DefaultValue(8)]
		public double From { get; set; }
		[Range(0, 24), DefaultValue(16)]
		public double To { get; set; }
	}
}
