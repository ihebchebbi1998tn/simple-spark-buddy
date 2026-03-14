namespace Sms.Scheduler.Controllers.OData
{
	using System;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;

	using Sms.Scheduler.Model;
	using Sms.Scheduler.Rest.Model;
	using Sms.Scheduler.Services;

	[ControllerName("SmsScheduler_Profile")]
	public class ProfileODataController : ODataControllerEx, IEntityApiController
	{
		private readonly ProfileService profileService;
		public ProfileODataController(ProfileService profileService)
		{
			this.profileService = profileService;
		}

		public Type EntityType => typeof(Profile);
		
		[HttpGet]
		public virtual IActionResult GetDefaultProfileConfig(ODataQueryOptions<ProfileRest> options)
		{
			return new JsonResult(profileService.GetDefaultClientConfig());
		}

		protected virtual string[] GetGroupableResourcePropertiesArray()
		{
			return new string[]{
				"Resource.Stations",
				"Resource.ResourceType",
				"Resource.Assets",
				"Resource.Skills",
			};
		}
		[HttpGet]
		public virtual IActionResult GetGroupableResourceProperties(ODataQueryOptions<ProfileRest> options)
		{
			return Ok(GetGroupableResourcePropertiesArray());
		}
		protected virtual string[] GetResourceTooltipPropertiesArray()
		{
			return new string[]
			{
				"Resource.DisplayName",
				"Resource."+nameof(Main.Rest.Model.UserRest.Email),
				"Resource."+nameof(Main.Rest.Model.UserRest.Remark),
				"Resource."+nameof(Main.Rest.Model.UserRest.PersonnelId),
				"Resource.Capacity",
				"Resource.ValidSkills",
				"Resource.ExpiredSkills",
				"Resource.ValidAssets",
				"Resource.ExpiredAssets",
				"Resource.License",
			};
		}
		[HttpGet]
		public virtual IActionResult GetResourceTooltipProperties(ODataQueryOptions<ProfileRest> options)
		{
			return Ok(GetResourceTooltipPropertiesArray());
		}
	}
}
