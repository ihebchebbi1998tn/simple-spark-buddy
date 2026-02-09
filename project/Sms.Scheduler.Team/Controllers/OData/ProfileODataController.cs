namespace Sms.Scheduler.Team.Controllers.OData
{
	using System;
	using System.Linq;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;

	using Sms.Scheduler.Controllers.OData;
	using Sms.Scheduler.Model;
	using Sms.Scheduler.Rest.Model;
	using Sms.Scheduler.Services;

	using BaseProfileODataController = Sms.Scheduler.Controllers.OData.ProfileODataController;

	[ControllerName("SmsScheduler_Profile")]
	public class ProfileODataController : BaseProfileODataController, IReplaceRegistration<BaseProfileODataController>
	{
		public ProfileODataController(ProfileService profileService)
			: base(profileService)
		{
		}
		[HttpGet]
		public override IActionResult GetGroupableResourceProperties(ODataQueryOptions<ProfileRest> options)
		{
			var props = base.GetGroupableResourcePropertiesArray().ToList();
			props.Add("Resource.Teams");

			return Ok(props.ToArray());
		}
		[HttpGet]
		public override IActionResult GetResourceTooltipProperties(ODataQueryOptions<ProfileRest> options)
		{
			var props = base.GetResourceTooltipPropertiesArray().ToList();
			props.Add("Resource.Teams");

			return Ok(props.ToArray());
		}
	}
}
