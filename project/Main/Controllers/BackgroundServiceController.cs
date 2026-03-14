
namespace Main.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;

	using log4net;

	using Main.Model;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using Newtonsoft.Json;

	using Quartz;
	using Quartz.Impl.Matchers;

	[Authorize]
	public class BackgroundServiceController : Controller
	{
		private readonly ILog logger;
		private readonly IScheduler scheduler;
		private readonly IUserService userService;
		public BackgroundServiceController(ILog logger, IScheduler scheduler, IUserService userService)
		{
			this.logger = logger;
			this.scheduler = scheduler;
			this.userService = userService;
		}

		[RenderAction("BackgroundServiceTopMenu")]
		public virtual ActionResult BackgroundServiceTopMenu()
		{
			return PartialView();
		}
		
		[RequiredPermission(PermissionName.RunNow, Group = PermissionGroup.BackgroundService)]
		public virtual ActionResult RunNow(string id, string group)
		{
			var currentUser = userService.CurrentUser;
			logger.InfoFormat("Job {0} in group {1} was triggered by {2}",
				id.RemoveLineBreaks(),
				group.RemoveLineBreaks(),
				currentUser.DisplayName.RemoveLineBreaks());

			var jobKey = new JobKey(id, group);
			scheduler.TriggerJob(jobKey);
			return new EmptyResult();
		}

		[RequiredPermission(PermissionName.Pause, Group = PermissionGroup.BackgroundService)]
		public virtual ActionResult PauseTrigger(string id, string group, string jobName, string jobGroup)
		{
			var currentUser = userService.CurrentUser;
			logger.InfoFormat("Trigger for job {0} in group {1} was paused by {2}", 
				id.RemoveLineBreaks(),
				group.RemoveLineBreaks(),
				currentUser.DisplayName.RemoveLineBreaks());

			var triggerKey = new TriggerKey(id, group);
			scheduler.PauseTrigger(triggerKey);

			JobKey jobKey = new JobKey(jobName, jobGroup);
			List<JobTrigger> Data = scheduler.GetTriggersOfJob(jobKey).Result.Select(x => new JobTrigger(x, scheduler)).ToList();

			return Json(JsonConvert.SerializeObject(Data, Formatting.None));
		}

		[RequiredPermission(PermissionName.Pause, Group = PermissionGroup.BackgroundService)]
		public virtual ActionResult Pause(string id, string group)
		{
			var currentUser = userService.CurrentUser;
			logger.InfoFormat("Trigger for job {0} in group {1} was paused by {2}",
				id.RemoveLineBreaks(),
				group.RemoveLineBreaks(),
				currentUser.DisplayName.RemoveLineBreaks());

			var triggerKey = new TriggerKey(id, group);
			scheduler.PauseTrigger(triggerKey);
			
			return new EmptyResult();
		}

		[RequiredPermission(PermissionName.Pause, Group = PermissionGroup.BackgroundService)]
		public virtual ActionResult PauseAll()
		{
			var currentUser = userService.CurrentUser;
			scheduler.PauseAll();
			logger.InfoFormat("All triggers pause by {0}", currentUser.DisplayName);
			var data = GetAllBackgroundServicesWithTriggers();
			
			return Json(JsonConvert.SerializeObject(data, Formatting.None));
		}

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.BackgroundService)]
		public virtual JsonResult IsRunning(string id, string group)
		{
			var isRunning = scheduler.GetCurrentlyExecutingJobs().Result
				.Any(x => x.JobDetail.Key.Name == id && x.JobDetail.Key.Group == group);
			return new JsonResult(isRunning);
		}

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.BackgroundService)]
		public virtual ActionResult BackgroundServiceListData()
		{
			var data = GetAllBackgroundServicesWithTriggers();
			
			return Json(JsonConvert.SerializeObject(data, Formatting.None));
		}

		protected virtual List<JobTriggerMatch> GetAllBackgroundServicesWithTriggers()
		{
			var jobGroups = scheduler.GetJobGroupNames();
			var allJobs = new List<IJobDetail>();
			foreach (string group in jobGroups.Result)
			{
				var groupMatcher = GroupMatcher<JobKey>.GroupEquals(group);
				var jobKeys = scheduler.GetJobKeys(groupMatcher);
				allJobs.AddRange(jobKeys.Result.Select(jobKey => scheduler.GetJobDetail(jobKey).Result));
			}
			var backgroundServices = allJobs.Where(x => x.JobType.Name != "FileScanJob").OrderBy(x => x.Key.Name);

			List<JobTriggerMatch> data = backgroundServices.Select(x => new JobTriggerMatch(x, scheduler)).ToList();
			return data;
		}

		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.BackgroundService)]
		public virtual ActionResult IndexTemplate() => PartialView();

		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.BackgroundService)]
		public virtual ActionResult FilterTemplate() => PartialView();

		public virtual ActionResult JobDetails() => PartialView();
	}
}
